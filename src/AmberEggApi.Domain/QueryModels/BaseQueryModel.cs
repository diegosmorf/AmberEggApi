using Api.Common.Repository.Entities;
using System;

namespace AmberEggApi.Domain.QueryModels
{
    public class BaseQueryModel : AggregateRoot
    {
        public Guid CorrelationId { get; set; }
    }
}