using System;

namespace AmberEggApi.Cqrs.Core.Commands;

public interface ICommand
{
    Guid MessageId { get; }
    string MessageType { get; }
    DateTime MessageCreatedDate { get; }
}