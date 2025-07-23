using AmberEggApi.Contracts.Entities;
using System.Threading.Tasks;

namespace AmberEggApi.Cqrs.Core.Commands;

public interface ICommandConsumer
{
    Task<TEntity> Receive<TCommand, TEntity>(TCommand command)
        where TCommand : ICommand
        where TEntity : IDomainEntity;
}