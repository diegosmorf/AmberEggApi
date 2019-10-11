using System.Threading.Tasks;
using Api.Common.Cqrs.Core.Commands;
using Api.Common.Cqrs.Core.Entities;

namespace Api.Common.Cqrs.Core.CommandHandlers
{
    public interface ICommandHandler<in TCommand, TDomainEntity>
        where TCommand : ICommand
        where TDomainEntity : IAggregateRoot
    {
        Task<TDomainEntity> Handle(TCommand command);
    }
}