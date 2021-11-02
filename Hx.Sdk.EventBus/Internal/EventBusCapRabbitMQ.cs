using DotNetCore.CAP;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hx.Sdk.EventBus.Internal
{
    internal class EventBusCapRabbitMQ : IEventBus
    {
        private readonly ICapPublisher _capPublisher;

        public EventBusCapRabbitMQ(ICapPublisher capPublisher)
        {
            _capPublisher = capPublisher;
        }

        /// <inheritdoc cref="IEventBus.PublishAsync{T}(string,T)"/>
        public virtual async Task PublishAsync<T>(string name, T @event) =>
            await _capPublisher.PublishAsync(name, @event);

        /// <inheritdoc cref="IEventBus.Publish{T}(string,T)"/>
        public virtual void Publish<T>(string name, T @event) => _capPublisher.Publish(name, @event);

        /// <inheritdoc cref="IEventBus.PublishAsync{T}(T)"/>
        public virtual async Task PublishAsync<T>(T @event) =>
            await _capPublisher.PublishAsync($"{@event.GetType().FullName}", @event);

        /// <inheritdoc cref="IEventBus.Publish{T}(T)"/>
        public virtual void Publish<T>(T @event) => _capPublisher.Publish($"{@event.GetType().FullName}", @event);

        public void Subscribe<T, TH>(string routeingKey) where T : IEventBusHandler<TH>, new() where TH : BaseDomainEvent
        {
            throw new System.NotImplementedException();
        }

        public void Subscribe<T, TH>() where T : IEventBusHandler<TH>, new() where TH : BaseDomainEvent
        {
            throw new System.NotImplementedException();
        }
    }
}
