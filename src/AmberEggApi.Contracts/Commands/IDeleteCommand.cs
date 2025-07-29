using System;

namespace AmberEggApi.Contracts.Commands;

public interface IDeleteCommand : ICommand
{
    Guid Id { get; }
}