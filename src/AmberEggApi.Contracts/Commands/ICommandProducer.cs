using AmberEggApi.Contracts.Entities;
using System.Threading.Tasks;

namespace AmberEggApi.Cqrs.Core.Commands;

public interface ICommandProducer
{
    Task<TEntity> Send<TCommand, TEntity>(TCommand command)
        where TCommand : ICommand
        where TEntity : IDomainEntity;
}