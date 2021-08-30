using Api.Common.Cqrs.Core.Events;

namespace Api.Common.Cqrs.Core.Entities
{
    public interface IAggregateRoot
    {
        int Version { get; }
        List<IEvent> AppliedEvents { get; }
    }
}