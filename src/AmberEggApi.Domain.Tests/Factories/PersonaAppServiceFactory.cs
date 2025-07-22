using AmberEggApi.ApplicationService.Interfaces;
using AmberEggApi.ApplicationService.ViewModels;
using AmberEggApi.Domain.Commands;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AmberEggApi.DomainTests.Factories;

public class PersonaAppServiceFactory(IPersonaAppService appService)
{
    private readonly IPersonaAppService appService = appService;

    public async Task<PersonaViewModel> Create(string name)
    {
        return await Create(new CreatePersonaCommand(name));
    }

    public async Task<PersonaViewModel> Create(CreatePersonaCommand command)
    {
        return await appService.Create(command);
    }

    public async Task Delete(Guid id)
    {
        await appService.Delete(new DeletePersonaCommand(id));
    }

    public async Task<PersonaViewModel> Get(Guid id)
    {
        return await appService.Get(id);
    }

    public async Task<IEnumerable<PersonaViewModel>> GetAll()
    {
        return await appService.GetAll();
    }

    public async Task<IEnumerable<PersonaViewModel>> GetListByName(string name)
    {
        return await appService.GetListByName(name);
    }

    public async Task<PersonaViewModel> Update(UpdatePersonaCommand command)
    {
        return await appService.Update(command);
    }
}