using Api.Common.Contracts.Entities;
using Api.Common.Cqrs.Core.Entities;
using Api.Common.Cqrs.Core.Events;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;

namespace Api.Common.Repository.Entities
{
    public abstract class AggregateRootBase : IAggregateRoot, IDomainEntity
    {
        protected AggregateRootBase()
        {
            AppliedEvents = new List<IEvent>();
        }

        public int Version { get; set; }

        [BsonIgnore]
        public List<IEvent> AppliedEvents { get; }

        [BsonId]
        public virtual Guid Id { get; set; }

        [BsonRequired]
        public DateTime CreateDate { get; set; }

        public DateTime? ModifiedDate { get; set; }

        public override string ToString()
        {
            return $"Type:{GetType().Name} - Id:{Id}";
        }
    }
}