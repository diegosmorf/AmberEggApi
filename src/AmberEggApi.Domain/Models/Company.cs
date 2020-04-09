using AmberEggApi.Domain.Commands;
using AmberEggApi.Domain.Events;
using Api.Common.Repository.Entities;
using System.ComponentModel.DataAnnotations;

namespace AmberEggApi.Domain.Models
{
    public class Company : AggregateRootBase
    {
        [MinLength(2)]
        [MaxLength(255)]
        [Required]
        public string Name { get; private set; }

        public void Create(CreateCompanyCommand command)
        {
            Name = command.Name;

            AppliedEvents.Add(new CompanyCreatedEvent(this, command.MessageId));
        }

        public void Delete(DeleteCompanyCommand command)
        {
            AppliedEvents.Add(new CompanyDeletedEvent(this, command.MessageId));
        }

        public void Update(UpdateCompanyCommand command)
        {
            Name = command.Name;

            AppliedEvents.Add(new CompanyUpdatedEvent(this, command.MessageId));
        }
    }
}