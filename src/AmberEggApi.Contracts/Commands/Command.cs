using AmberEggApi.Cqrs.Core.Messages;

namespace AmberEggApi.Cqrs.Core.Commands;

public abstract class Command : Message, ICommand
{
}