using AmberEggApi.ApplicationService.Contracts;
using AmberEggApi.ApplicationService.Mapping;
using AmberEggApi.Contracts.Commands;
using AmberEggApi.Contracts.Entities;
using AmberEggApi.Contracts.Repositories;
using AmberEggApi.Contracts.Validations;

using System.Threading;
using System.Threading.Tasks;

namespace AmberEggApi.ApplicationService.CommandHandlers;

public class GenericCommandHandler<TDomainEntity, TViewlModel, TCreateCommand, TUpdateCommand, TDeleteCommand>(IRepository<TDomainEntity> repository, IMapper mapper, IUnitOfWork unitOfWork)
    : ICommandHandler<TDomainEntity, TViewlModel, TCreateCommand, TUpdateCommand, TDeleteCommand>
        where TDomainEntity : class, IDomainEntity, new()
        where TViewlModel : class, IViewModel, new()
        where TCreateCommand : class, ICommand
        where TUpdateCommand : class, IUpdateCommand
        where TDeleteCommand : class, IDeleteCommand
{
    private readonly IMapper mapper = mapper;
    public async Task<TViewlModel> Handle(TCreateCommand command, CancellationToken cancellationToken)
    {
        command.Validate();

        //Domain
        var instance = mapper.Map<TCreateCommand, TDomainEntity>(command);
        //Persistence
        await repository.Insert(instance, cancellationToken);
        //Commit
        await unitOfWork.Commit(cancellationToken);
        return mapper.Map<TDomainEntity, TViewlModel>(instance);
    }

    public async Task<TViewlModel> Handle(TUpdateCommand command, CancellationToken cancellationToken)
    {
        command.Validate();

        //Domain
        var instance = await repository.SearchById(command.Id, cancellationToken);

        if (instance == null)
            return null;

        mapper.Map<TUpdateCommand, TDomainEntity>(command, instance);
        instance.Update();

        //Persistence
        await repository.Update(instance, cancellationToken);
        //Commit
        await unitOfWork.Commit(cancellationToken);

        return mapper.Map<TDomainEntity, TViewlModel>(instance);

    }

    public async Task<TViewlModel> Handle(TDeleteCommand command, CancellationToken cancellationToken)
    {
        command.Validate();        

        //Domain
        var instance = await repository.SearchById(command.Id, cancellationToken);

        if (instance == null)
            return null;

        //Persistence
        await repository.Delete(command.Id, cancellationToken);
        //Commit
        await unitOfWork.Commit(cancellationToken);

        return mapper.Map<TDomainEntity, TViewlModel>(instance);
    }
}