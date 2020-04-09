using Api.Common.Cqrs.Core.Events;
using System.Collections.Generic;

namespace Api.Common.Cqrs.Core.Entities
{
    public interface IAggregateRoot
    {
        int Version { get; }
        List<IEvent> AppliedEvents { get; }
    }
}