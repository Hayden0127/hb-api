using Strateq.Core.API.Infrastructures.EventBus;
using System.Threading.Tasks;

namespace Strateq.Core.API.Infrastructures.EventBus.Abstractions
{
    public interface IIntegrationEventHandler<in TIntegrationEvent> : IIntegrationEventHandler
        where TIntegrationEvent : IntegrationEvent
    {
        Task Handle(TIntegrationEvent @event);
    }

    public interface IIntegrationEventHandler
    {
    }
}
