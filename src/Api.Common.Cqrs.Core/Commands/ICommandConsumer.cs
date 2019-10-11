using System.Threading.Tasks;
using Api.Common.Cqrs.Core.Entities;

namespace Api.Common.Cqrs.Core.Commands
{
    public interface ICommandConsumer
    {
        Task<TEntity> Receive<TCommand, TEntity>(TCommand command)
            where TCommand : ICommand
            where TEntity : IAggregateRoot;
    }
}