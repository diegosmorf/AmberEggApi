using Newtonsoft.Json;
using System;

namespace Api.Common.Cqrs.Core.Messages
{
    public abstract class Message : IMessage
    {
        protected Message() : this(Guid.NewGuid())
        {

        }

        protected Message(Guid messageId)
        {
            MessageId = messageId;
            MessageType = GetType().Name;
            MessageCreatedDate = DateTime.UtcNow;
        }

        [JsonIgnore] public Guid MessageId { get; }

        [JsonIgnore] public Guid AuditUserId { get; set; }

        [JsonIgnore] public string MessageType { get; }

        [JsonIgnore] public DateTime MessageCreatedDate { get; }

        public override string ToString()
        {
            return $"MessageId:{MessageId} - MessageType:{MessageType} - TimeStamp:{MessageCreatedDate}";
        }
    }
}