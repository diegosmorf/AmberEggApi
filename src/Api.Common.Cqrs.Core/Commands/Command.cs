using Api.Common.Cqrs.Core.Messages;

namespace Api.Common.Cqrs.Core.Commands
{
    public abstract class Command : Message, ICommand
    {
    }
}