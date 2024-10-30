﻿using AmberEggApi.Domain.Models;
using Api.Common.Cqrs.Core.Events;
using System;

namespace AmberEggApi.Domain.Events
{
    public class PersonaDeletedEvent(Persona persona, Guid commandMessageId) : Event(commandMessageId)
    {
        public Persona Persona { get; } = persona;
    }
}