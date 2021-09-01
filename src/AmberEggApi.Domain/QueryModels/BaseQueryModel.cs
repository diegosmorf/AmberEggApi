using Api.Common.Repository.Entities;

namespace AmberEggApi.Domain.QueryModels
{
    public class BaseQueryModel : AggregateRoot
    {
        public Guid CorrelationId { get; set; }
    }
}