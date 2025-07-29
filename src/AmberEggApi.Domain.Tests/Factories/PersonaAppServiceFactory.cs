using AmberEggApi.ApplicationService.Contracts;
using AmberEggApi.ApplicationService.ViewModels;
using AmberEggApi.Domain.Commands;

using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace AmberEggApi.DomainTests.Factories;

public class PersonaAppServiceFactory(IPersonaQueryHandler queryHandler, IPersonaCommandHandler commandHandler)
{
    private readonly IPersonaQueryHandler queryHandler = queryHandler;
    private readonly IPersonaCommandHandler commandHandler = commandHandler;

    public async Task<PersonaViewModel> Create(string name , CancellationToken cancellationToken)
    {
        return await Create(new CreatePersonaCommand(name), cancellationToken);
    }

    public async Task<PersonaViewModel> Create(CreatePersonaCommand command, CancellationToken cancellationToken)
    {
        return await commandHandler.Handle(command, cancellationToken);
    }

    public async Task Delete(Guid id, CancellationToken cancellationToken)
    {
        await commandHandler.Handle(new DeletePersonaCommand(id), cancellationToken);
    }

    public async Task<PersonaViewModel> Get(Guid id, CancellationToken cancellationToken)
    {
        return await queryHandler.Get(id, cancellationToken);
    }

    public async Task<IEnumerable<PersonaViewModel>> GetAll(CancellationToken cancellationToken)
    {
        return await queryHandler.GetAll(cancellationToken);
    }

    public async Task<IEnumerable<PersonaViewModel>> GetListByName(string name, CancellationToken cancellationToken)
    {
        return await queryHandler.GetListByName(name, cancellationToken);
    }

    public async Task<PersonaViewModel> Update(UpdatePersonaCommand command, CancellationToken cancellationToken)
    {
        return await commandHandler.Handle(command, cancellationToken);
    }
}