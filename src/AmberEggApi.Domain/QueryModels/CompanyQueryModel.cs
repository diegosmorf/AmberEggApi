using AmberEggApi.Domain.Events;
using System.ComponentModel.DataAnnotations;

namespace AmberEggApi.Domain.QueryModels
{
    public class CompanyQueryModel : BaseQueryModel
    {
        [MinLength(2)]
        [MaxLength(255)]
        [Required]
        public string Name { get; private set; }

        public void Create(CompanyCreatedEvent @event)
        {
            Id = @event.Company.Id;
            Name = @event.Company.Name;
            CorrelationId = @event.MessageId;
        }

        public void Update(CompanyUpdatedEvent @event)
        {
            Id = @event.Company.Id;
            Name = @event.Company.Name;
            CorrelationId = @event.MessageId;
        }
    }
}