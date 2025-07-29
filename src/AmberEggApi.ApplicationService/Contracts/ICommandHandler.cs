using AmberEggApi.Contracts.Commands;
using AmberEggApi.Contracts.Entities;
using System.Threading;
using System.Threading.Tasks;

namespace AmberEggApi.ApplicationService.Contracts;

public interface ICommandHandler<TDomainEntity, TViewlModel, TCreateCommand, TUpdateCommand, TDeleteCommand>
        where TDomainEntity : class, IDomainEntity
        where TViewlModel : class, IViewModel
        where TCreateCommand : class, ICommand
        where TUpdateCommand : class, IUpdateCommand
        where TDeleteCommand : class, IDeleteCommand
{
    Task<TViewlModel> Handle(TCreateCommand command, CancellationToken cancellationToken);
    Task<TViewlModel> Handle(TUpdateCommand command, CancellationToken cancellationToken);
    Task<TViewlModel> Handle(TDeleteCommand command, CancellationToken cancellationToken);
}
