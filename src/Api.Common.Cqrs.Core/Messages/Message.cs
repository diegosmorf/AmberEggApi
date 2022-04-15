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

        
        public Guid MessageId { get; }

        
        public string MessageType { get; }

        
        public DateTime MessageCreatedDate { get; }

        public override string ToString()
        {
            return $"MessageId:{MessageId} - MessageType:{MessageType} - TimeStamp:{MessageCreatedDate}";
        }
    }
}