using AmberEggApi.ApplicationService.Contracts;
using AmberEggApi.ApplicationService.Mapping;
using AmberEggApi.ApplicationService.ViewModels;
using AmberEggApi.Contracts.Repositories;
using AmberEggApi.Domain.Commands;
using AmberEggApi.Domain.Models;
using AmberEggApi.Contracts.Validations;
using System.Threading;
using System.Threading.Tasks;

namespace AmberEggApi.ApplicationService.CommandHandlers;

public class PersonaCommanderHandler(IRepository<Persona> repository, IMapper mapper, IUnitOfWork unitOfWork) : IPersonaCommandHandler
{
    public async Task<PersonaViewModel> Handle(CreatePersonaCommand command, CancellationToken cancellationToken)
    {
        command.Validate();

        //Domain
        var instance = mapper.Map<CreatePersonaCommand, Persona>(command);
        //Persistence
        await repository.Insert(instance, cancellationToken);
        //Commit
        await unitOfWork.Commit(cancellationToken);
        return mapper.Map<Persona, PersonaViewModel>(instance);
    }

    public async Task<PersonaViewModel> Handle(UpdatePersonaCommand command, CancellationToken cancellationToken)
    {
        command.Validate();

        //Domain
        var instance = await repository.SearchById(command.Id, cancellationToken);

        if (instance == null)
            return null;

        mapper.Map(command, instance);
        instance.Update();

        //Persistence
        await repository.Update(instance, cancellationToken);
        //Commit
        await unitOfWork.Commit(cancellationToken);

        return mapper.Map<Persona, PersonaViewModel>(instance);

    }

    public async Task<PersonaViewModel> Handle(DeletePersonaCommand command, CancellationToken cancellationToken)
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

        return mapper.Map<Persona, PersonaViewModel>(instance);
    }
}