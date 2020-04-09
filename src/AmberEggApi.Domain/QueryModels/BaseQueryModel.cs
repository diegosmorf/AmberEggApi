using Api.Common.Repository.Entities;
using System;

namespace AmberEggApi.Domain.QueryModels
{
    public class BaseQueryModel : AggregateRootBase
    {
        public Guid CorrelationId { get; set; }
    }
}