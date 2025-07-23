using AmberEggApi.Contracts.Entities;
using AmberEggApi.Cqrs.Core.Commands;
using System.Threading.Tasks;

namespace AmberEggApi.Cqrs.Core.CommandHandlers;

public interface ICommandHandler<in TCommand, TDomainEntity>
    where TCommand : ICommand
    where TDomainEntity : IDomainEntity
{
    Task<TDomainEntity> Handle(TCommand command);
}