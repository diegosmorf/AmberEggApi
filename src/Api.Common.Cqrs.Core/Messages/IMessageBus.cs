using Api.Common.Cqrs.Core.Commands;
using Api.Common.Cqrs.Core.Events;

namespace Api.Common.Cqrs.Core.Messages
{
    public interface IMessageBus : ICommandProducer, IEventProducer
    {
    }
}