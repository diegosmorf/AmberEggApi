using AmberEggApi.Contracts.Commands;
using AmberEggApi.Contracts.Entities;
using AmberEggApi.Contracts.Repositories;

using AutoMapper;

using System.Threading.Tasks;

namespace AmberEggApi.Contracts.CommandHandlers;

public class GenericUpdateCommandHandle<TCommand, TDomainEntity>(IRepository<TDomainEntity> repository, IMapper mapper, IUnitOfWork unitOfWork) 
    :ICommandHandler<TCommand, TDomainEntity>
        where TCommand : IUpdateCommand
        where TDomainEntity : IDomainEntity
{
    private readonly IMapper mapper = mapper;
    public async Task<TDomainEntity> Handle(TCommand command)
    {
        //Domain
        var instance = await repository.SearchById(command.Id);

        if (instance != null)
        {
            mapper.Map(command, instance);
            instance.Update();

            //Persistence
            await repository.Update(instance);
            await unitOfWork.Commit();
        }

        return instance;
    }
}