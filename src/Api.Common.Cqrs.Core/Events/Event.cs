using Api.Common.Cqrs.Core.Messages;
using System;

namespace Api.Common.Cqrs.Core.Events
{
    public abstract class Event(Guid messageId) : Message(messageId), IEvent
    {
    }
}