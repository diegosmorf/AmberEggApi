using AmberEggApi.Domain.Events;
using System.ComponentModel.DataAnnotations;

namespace AmberEggApi.Domain.QueryModels
{
    public class PersonaQueryModel : BaseQueryModel
    {
        [MinLength(2)]
        [MaxLength(255)]
        [Required]
        public string Name { get; private set; }

        public void Create(PersonaCreatedEvent @event)
        {
            Id = @event.Persona.Id;
            Name = @event.Persona.Name;
            CorrelationId = @event.MessageId;
        }

        public void Update(PersonaUpdatedEvent @event)
        {
            Id = @event.Persona.Id;
            Name = @event.Persona.Name;
            CorrelationId = @event.MessageId;
        }
    }
}