using AmberEggApi.Contracts.Entities;

using System.Threading.Tasks;

namespace AmberEggApi.Contracts.Commands;

public interface ICommandProducer
{
    Task<TEntity> Send<TCommand, TEntity>(TCommand command)
        where TCommand : ICommand
        where TEntity : IDomainEntity;
}