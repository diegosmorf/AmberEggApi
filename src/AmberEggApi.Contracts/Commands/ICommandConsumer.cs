using AmberEggApi.Contracts.Entities;

using System.Threading.Tasks;

namespace AmberEggApi.Contracts.Commands;

public interface ICommandConsumer
{
    Task<TEntity> Receive<TCommand, TEntity>(TCommand command)
        where TCommand : ICommand
        where TEntity : IDomainEntity;
}