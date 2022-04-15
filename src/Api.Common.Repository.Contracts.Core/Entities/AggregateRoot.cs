using Api.Common.Cqrs.Core.Entities;
using Api.Common.Cqrs.Core.Events;
using System.Collections.Generic;

namespace Api.Common.Repository.Entities
{
    public abstract class AggregateRoot : DomainEntity, IAggregateRoot
    {
        protected AggregateRoot()
        {
            AppliedEvents = new List<IEvent>();
        }

        public int Version { get; set; }


        public List<IEvent> AppliedEvents { get; }
    }
}