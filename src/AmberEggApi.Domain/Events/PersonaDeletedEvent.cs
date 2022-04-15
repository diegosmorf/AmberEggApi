using AmberEggApi.Domain.Models;
using Api.Common.Cqrs.Core.Events;
using System;

namespace AmberEggApi.Domain.Events
{
    public class PersonaDeletedEvent : Event
    {
        public PersonaDeletedEvent(Persona persona, Guid commandMessageId) : base(commandMessageId)
        {
            Persona = persona;
        }

        public Persona Persona { get; }
    }
}