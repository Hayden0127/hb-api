using System.Threading.Tasks;

namespace Strateq.Core.API.Infrastructures.EventBus.Abstractions
{
    public interface IDynamicIntegrationEventHandler
    {
        Task Handle(dynamic eventData);
    }
}
