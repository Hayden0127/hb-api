using Azure.Messaging.ServiceBus;
using Azure.Messaging.ServiceBus.Administration;
using System;

namespace Strateq.Core.API.Infrastructures.EventBus.AzureServiceBus
{
    public interface IServiceBusPersisterConnection : IDisposable
    {
        ServiceBusClient TopicClient { get; }
        ServiceBusAdministrationClient AdministrationClient { get; }
    }
}