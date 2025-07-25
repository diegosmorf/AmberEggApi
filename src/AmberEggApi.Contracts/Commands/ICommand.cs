using System;

namespace AmberEggApi.Contracts.Commands;

public interface ICommand
{    
    Guid MessageId { get; }
}

public interface IUpdateCommand:ICommand
{
    Guid Id { get; }    
}

public interface IDeleteCommand : ICommand
{
    Guid Id { get; }
}