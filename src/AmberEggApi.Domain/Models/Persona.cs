using AmberEggApi.Domain.Commands;
using AmberEggApi.Domain.Events;
using Api.Common.Repository.Entities;
using System.ComponentModel.DataAnnotations;

namespace AmberEggApi.Domain.Models
{
    public class Persona : AggregateRoot
    {
        [MinLength(2)]
        [MaxLength(20)]
        [Required]
        public string Name { get; private set; }

        public void Create(CreatePersonaCommand command)
        {
            Name = command.Name;
            Version++;
            AppliedEvents.Add(new PersonaCreatedEvent(this, command.MessageId));
        }

        public void Delete(DeletePersonaCommand command)
        {
            AppliedEvents.Add(new PersonaDeletedEvent(this, command.MessageId));
        }

        public void Update(UpdatePersonaCommand command)
        {
            Name = command.Name;
            Version++;
            AppliedEvents.Add(new PersonaUpdatedEvent(this, command.MessageId));
        }
    }
}