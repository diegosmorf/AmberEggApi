using System;

namespace Api.Common.Cqrs.Core.Messages
{
    public interface IMessage
    {
        Guid MessageId { get; }        
        string MessageType { get; }
        DateTime MessageCreatedDate { get; }
    }
}