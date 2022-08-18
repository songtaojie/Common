using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hx.Sdk.EventBus.Internal
{
    /// <summary>
    /// 无效的事件总线
    /// </summary>
    internal class EmptyEventBus : IEventBus
    {
        private readonly ILogger<EmptyEventBus> _logger;
        public EmptyEventBus(ILogger<EmptyEventBus> logger)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        /// <inheritdoc cref="IEventBus.PublishAsync{T}(string,T)"/>
        public virtual async Task PublishAsync<T>(string name, T @event)
        {
            _logger.LogInformation($"EmptyEventBus PublishAsync name:{name}");
            await Task.CompletedTask;
        }

        /// <inheritdoc cref="IEventBus.Publish{T}(string,T)"/>
        public virtual void Publish<T>(string name, T @event) => _logger.LogInformation($"EmptyEventBus Publish name:{name}");

        /// <inheritdoc cref="IEventBus.PublishAsync{T}(T)"/>
        public virtual async Task PublishAsync<T>(T @event)
        {
            _logger.LogInformation($"EmptyEventBus PublishAsync name:{@event?.GetType().FullName}");
            await Task.CompletedTask;
        }

        /// <inheritdoc cref="IEventBus.Publish{T}(T)"/>
        public virtual void Publish<T>(T @event) => _logger.LogInformation($"EmptyEventBus Publish name:{@event?.GetType().FullName}");

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TH"></typeparam>
        /// <param name="routeingKey"></param>
        /// <exception cref="System.NotImplementedException"></exception>
        public void Subscribe<T, TH>(string routeingKey) where T : IEventBusHandler<TH>, new() where TH : BaseDomainEvent
        {
            _logger.LogInformation($"EmptyEventBus Subscribe routeingKey:{routeingKey}");
        }
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TH"></typeparam>
        /// <exception cref="System.NotImplementedException"></exception>
        public void Subscribe<T, TH>() where T : IEventBusHandler<TH>, new() where TH : BaseDomainEvent
        {
            _logger.LogInformation($"EmptyEventBus Subscribe ");
        }
    }
}
