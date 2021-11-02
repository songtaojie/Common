using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hx.Sdk.EventBus
{
    public interface IEventBus
    {
        /// <summary>
        /// 发布事件消息
        /// </summary>
        /// <param name="name"></param>
        /// <param name="event"></param>
        /// <returns></returns>
        Task PublishAsync<T>(string name, T @event);

        /// <summary>
        /// 发布事件消息
        /// </summary>
        /// <param name="name"></param>
        /// <param name="event"></param>
        /// <returns></returns>
        void Publish<T>(string name, T @event);

        /// <summary>
        /// 发布事件消息
        /// </summary>
        /// <param name="event"></param>
        /// <returns></returns>
        Task PublishAsync<T>(T @event);

        /// <summary>
        /// 发布事件消息
        /// </summary>
        /// <param name="event"></param>
        /// <returns></returns>
        void Publish<T>(T @event);

        void Subscribe<T, TH>(string routeingKey) where T : IEventBusHandler<TH>, new() where TH : BaseDomainEvent;

        void Subscribe<T, TH>() where T : IEventBusHandler<TH>, new() where TH : BaseDomainEvent;
    }
}
