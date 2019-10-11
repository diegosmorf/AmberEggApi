using System;

namespace Api.Common.Cqrs.Core.Messages
{
    public interface IMessage
    {
        Guid MessageId { get; }
        Guid AuditUserId { get; set; }
        string MessageType { get; }
        DateTime MessageCreatedDate { get; }
    }
}