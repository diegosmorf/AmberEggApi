using AmberEggApi.Contracts.Commands;
using AmberEggApi.Contracts.Entities;
using AmberEggApi.Contracts.Repositories;

using System.Threading.Tasks;

namespace AmberEggApi.Contracts.CommandHandlers;

public class GenericDeleteCommandHandle<TCommand, TDomainEntity>(IRepository<TDomainEntity> repository, IUnitOfWork unitOfWork)
    : ICommandHandler<TCommand, TDomainEntity>
        where TCommand : IDeleteCommand
        where TDomainEntity : IDomainEntity
{    
    public async Task<TDomainEntity> Handle(TCommand command)
    {
        //Domain
        var instance = await repository.SearchById(command.Id);
        //Persistence
        await repository.Delete(command.Id);

        await unitOfWork.Commit();

        return instance;
    }
}