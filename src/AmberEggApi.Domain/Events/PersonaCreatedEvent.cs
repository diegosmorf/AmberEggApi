using AmberEggApi.Domain.Models;
using Api.Common.Cqrs.Core.Events;
using System;

namespace AmberEggApi.Domain.Events
{
    public class PersonaCreatedEvent : Event
    {
        public PersonaCreatedEvent(Persona persona, Guid commandMessageId) : base(commandMessageId)
        {
            Persona = persona;
        }

        public Persona Persona { get; }
    }
}