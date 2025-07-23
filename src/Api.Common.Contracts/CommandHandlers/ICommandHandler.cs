using Api.Common.Contracts.Entities;
using Api.Common.Cqrs.Core.Commands;
using System.Threading.Tasks;

namespace Api.Common.Cqrs.Core.CommandHandlers;

public interface ICommandHandler<in TCommand, TDomainEntity>
    where TCommand : ICommand
    where TDomainEntity : IDomainEntity
{
    Task<TDomainEntity> Handle(TCommand command);
}