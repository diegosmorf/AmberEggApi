using System;
using Api.Common.Repository.Entities;

namespace AmberEggApi.Domain.QueryModels
{
    public class BaseQueryModel : AggregateRootBase
    {
        public Guid CorrelationId { get; set; }
    }
}