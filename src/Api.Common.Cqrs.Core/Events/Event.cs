﻿using Api.Common.Cqrs.Core.Messages;
using System;

namespace Api.Common.Cqrs.Core.Events
{
    public abstract class Event : Message, IEvent
    {
        protected Event(Guid messageId) : base(messageId) { }
    }
}