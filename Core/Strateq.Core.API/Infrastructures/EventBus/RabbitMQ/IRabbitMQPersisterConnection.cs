using RabbitMQ.Client;
using System;

namespace Strateq.Core.API.Infrastructures.EventBus.RabbitMQ
{
    public interface IRabbitMQPersisterConnection
        : IDisposable
    {
        bool IsConnected { get; }

        bool TryConnect();

        IModel CreateModel();
    }
}
