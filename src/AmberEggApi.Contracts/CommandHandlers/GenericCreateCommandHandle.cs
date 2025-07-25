using AmberEggApi.Contracts.Commands;
using AmberEggApi.Contracts.Entities;
using AmberEggApi.Contracts.Repositories;
using AutoMapper;
using System.Threading.Tasks;

namespace AmberEggApi.Contracts.CommandHandlers;

public class GenericCreateCommandHandle<TCommand, TDomainEntity>(IRepository<TDomainEntity> repository, IMapper mapper, IUnitOfWork unitOfWork)
    : ICommandHandler<TCommand, TDomainEntity>
        where TCommand : ICommand
        where TDomainEntity : IDomainEntity
{
    private readonly IMapper mapper = mapper;
    public async Task<TDomainEntity> Handle(TCommand command)
    {
        //Domain
        var instance = mapper.Map<TDomainEntity>(command);
        //Persistence
        await repository.Insert(instance);
        //Commit
        await unitOfWork.Commit();
        return instance;
    }
}