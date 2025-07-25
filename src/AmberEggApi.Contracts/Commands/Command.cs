using System;

namespace AmberEggApi.Contracts.Commands;

public abstract class Command : ICommand
{
    protected Command() : this(Guid.NewGuid()) { }

    protected Command(Guid messageId)
    {
        this.MessageId = messageId;
    }    
    public Guid MessageId { get; }
    public override string ToString()
    {
        return $"MessageId:{this.MessageId}";
    }
}