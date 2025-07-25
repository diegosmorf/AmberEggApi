using System;

namespace AmberEggApi.Contracts.Commands;

public abstract class Command : ICommand
{
    protected Command() : this(Guid.NewGuid()) { }

    protected Command(Guid messageId)
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