using Hx.Sdk.EventBus.RabbitMq;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Polly;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using RabbitMQ.Client.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Hx.Sdk.EventBus.Internal
{
    internal class EventBusRabbitMQ : IEventBus, IDisposable
    {
        private const string Exchange_Name = "hx_exchagne"; //交换机名称
        private const string DlxExchange_Name = "hx_dlx_exchagne"; //死信队列交换机名称
        private const string Queue_Name = "hx_queue"; //死信队列队列名称
        private readonly IRabbitMQPersistentConnection _persistentConnection;
        private readonly ILogger<EventBusRabbitMQ> _logger;
        private readonly int _retryCount;

        private IModel _consumerChannel;


        public EventBusRabbitMQ(IRabbitMQPersistentConnection persistentConnection, ILogger<EventBusRabbitMQ> logger,
            int retryCount = 5)
        {
            _persistentConnection = persistentConnection ?? throw new ArgumentNullException(nameof(persistentConnection));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _consumerChannel = CreateConsumerChannel();
            _retryCount = retryCount;
        }

        /// <summary>
        /// 创建消费者并投递消息
        /// </summary>
        /// <returns></returns>
        private IModel CreateConsumerChannel()
        {
            if (!_persistentConnection.IsConnected)
            {
                _persistentConnection.TryConnect();
            }

            var channel = _persistentConnection.CreateModel();
            //声明交换机
            channel.ExchangeDeclare(Exchange_Name, ExchangeType.Direct);
            //声明一个死信队列的交换机
            channel.ExchangeDeclare(DlxExchange_Name, ExchangeType.Direct);
            channel.QueueDeclare(Queue_Name, true, false, false, null);
            //var consumer = new EventingBasicConsumer(channel);
            //consumer.Received += async (model, ea) =>
            //{
            //    var message = Encoding.UTF8.GetString(ea.Body);

            //    await ProcessEvent(ea.RoutingKey, message);

            //    channel.BasicAck(ea.DeliveryTag, multiple: false);
            //};

            //channel.BasicConsume(Queue_Name, false, consumer);
            channel.CallbackException += (sender, ea) =>
            {
                _consumerChannel.Dispose();
                _consumerChannel = CreateConsumerChannel();
            };
            return channel;
        }

        public Task PublishAsync<T>(string routingKey, T @event)
        {
            Publish(routingKey, @event);
            return Task.CompletedTask;
        }

        /// <summary>
        /// 发布消息
        /// </summary>
        public void Publish<T>(string routingKey, T @event)
        {
            if (!_persistentConnection.IsConnected)
            {
                _persistentConnection.TryConnect();
            }

            var policy = Policy.Handle<BrokerUnreachableException>()
                .Or<SocketException>()
                .WaitAndRetry(_retryCount, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)),
                    (ex, time) =>
                    {
                        _logger.LogWarning(ex, "Could not publish event:  after {Timeout}s ({ExceptionMessage})", $"{time.TotalSeconds:n1}", ex.Message);
                    });

            using var channel = _persistentConnection.CreateModel();

            var message = JsonConvert.SerializeObject(@event);
            var body = Encoding.UTF8.GetBytes(message);

            policy.Execute(() =>
            {
                var properties = channel.CreateBasicProperties();
                properties.DeliveryMode = 2; // persistent

                channel.BasicPublish(
                    exchange: Exchange_Name,
                    routingKey: routingKey.ToLower(),
                    mandatory: true,
                    basicProperties: properties,
                    body: body);
            });
        }

        public async Task PublishAsync<T>(T @event)
        {
            var eventName = @event.GetType().Name;
            await PublishAsync(eventName, @event);
        }

        public void Publish<T>(T @event)
        {
            var eventName = @event.GetType().Name;
            Publish(eventName, @event);
        }

        #region Subscribe
        public void Subscribe<T, TH>(string routeingKey) where T : IEventBusHandler<TH>, new() where TH : BaseDomainEvent
        {
            var eventName = typeof(T).Name;

            DoInternalSubscription(eventName, routeingKey);
            StartBasicConsume<T, TH>(eventName);
        }

        public void Subscribe<T, TH>() where T : IEventBusHandler<TH>, new() where TH : BaseDomainEvent
        {
            var routeingKey = typeof(TH).Name;
            Subscribe<T, TH>(routeingKey);
        }
        /// <summary>
        /// 事件订阅逻辑
        /// </summary>
        /// <param name="eventName"></param>
        /// <param name="routeingKey"></param>
        private void DoInternalSubscription(string eventName, string routeingKey)
        {
            if (!_persistentConnection.IsConnected)
            {
                _persistentConnection.TryConnect();
            }

            using var channel = _persistentConnection.CreateModel();
            routeingKey = routeingKey.ToLower();
            //死信队列参数
            var args = new Dictionary<string, object>
            {
                {"x-message-ttl", 10000}, //TTL
                {"x-dead-letter-exchange", DlxExchange_Name}, //DLX
                {"x-dead-letter-routing-key", routeingKey}
            };
            channel.QueueDeclare(queue: eventName,
                durable: false,
                exclusive: false,
                autoDelete: false,
                arguments: args);

            //交换机和队列绑定
            channel.QueueBind(queue: eventName,
                exchange: Exchange_Name,
                routingKey: routeingKey);

            //死信队列和交换机绑定
            channel.QueueBind(queue: Queue_Name,
                exchange: DlxExchange_Name,
                routingKey: routeingKey);
        }

        /// <summary>
        /// 消费者开始等待接收消息
        /// </summary>
        /// <param name="queueName"></param>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TH"></typeparam>
        private void StartBasicConsume<T, TH>(string queueName) where T : IEventBusHandler<TH>, new() where TH : BaseDomainEvent
        {
            _logger.LogTrace("Starting RabbitMQ basic consume");

            if (_consumerChannel != null)
            {
                var consumer = new AsyncEventingBasicConsumer(_consumerChannel);
                var handler = new T();

                consumer.Received += async (model, ea) =>
                {
                    var message = Encoding.UTF8.GetString(ea.Body.ToArray());
                    var mesObj = JsonConvert.DeserializeObject<TH>(message);
                    await handler.Handle(mesObj);

                    _consumerChannel.BasicAck(ea.DeliveryTag, multiple: false);
                    // Console.WriteLine($"收到消息{message}{DateTime.Now}");
                };

                _consumerChannel.BasicConsume(
                    queue: queueName,
                    autoAck: false,
                    consumer: consumer);
            }
            else
            {
                _logger.LogError("StartBasicConsume can't call on _consumerChannel == null");
            }
        }
        #endregion


        public void Dispose()
        {
            _consumerChannel?.Dispose();
        }
    }
}
