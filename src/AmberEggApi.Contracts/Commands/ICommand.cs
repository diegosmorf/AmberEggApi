using System;

namespace AmberEggApi.Contracts.Commands;

public interface ICommand
{
    Guid MessageId { get; }
    string MessageType { get; }
    DateTime MessageCreatedDate { get; }
}