using System;
using System.Threading.Tasks;

namespace Hx.Sdk.EventBus
{
    public interface IEventBusHandler<in TIntegrationEvent> : IEventBusHandler
       where TIntegrationEvent : BaseDomainEvent
    {
        /// <summary>
        /// 处理具有详情信息的事件
        /// </summary>
        /// <param name="event"></param>
        /// <returns></returns>
        Task Handle(TIntegrationEvent @event);
    }

    public interface IEventBusHandler
    {
    }
}
