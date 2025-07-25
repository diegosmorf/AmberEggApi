using AmberEggApi.Contracts.Commands;
using AmberEggApi.Contracts.Entities;

using System.Threading.Tasks;

namespace AmberEggApi.Contracts.CommandHandlers;

public interface ICommandHandler<in TCommand, TDomainEntity>
    where TCommand : ICommand
    where TDomainEntity : IDomainEntity
{
    Task<TDomainEntity> Handle(TCommand command);
}