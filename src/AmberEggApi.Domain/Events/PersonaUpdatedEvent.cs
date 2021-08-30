using AmberEggApi.Domain.Models;
using Api.Common.Cqrs.Core.Events;

namespace AmberEggApi.Domain.Events
{
    public class PersonaUpdatedEvent : Event
    {
        public PersonaUpdatedEvent(Persona persona, Guid commandMessageId) : base(commandMessageId)
        {
            Persona = persona;
        }

        public Persona Persona { get; }
    }
}