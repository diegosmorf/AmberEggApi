using System;

namespace AmberEggApi.Contracts.Commands;

public interface IUpdateCommand:ICommand
{
    Guid Id { get; }    
}
