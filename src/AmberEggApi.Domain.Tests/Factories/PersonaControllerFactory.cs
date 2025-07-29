using AmberEggApi.ApplicationService.ViewModels;
using AmberEggApi.Domain.Commands;
using AmberEggApi.WebApi.Controllers;

using FluentAssertions;

using Microsoft.AspNetCore.Mvc;

using System;
using System.Collections.Generic;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace AmberEggApi.DomainTests.Factories;

public class PersonaControllerFactory(PersonaController controller)
{
    private readonly PersonaController controller = controller;

    public async Task<PersonaViewModel> Create(string name)
    {
        var command = new CreatePersonaCommand(name);
        return await Create(command);
    }

    public async Task<PersonaViewModel> Create(CreatePersonaCommand command)
    {
        var response = await controller.Create(command, new CancellationToken()) as CreatedResult;
        var viewModel = response.Value as PersonaViewModel;
        return viewModel;
    }

    public async Task Delete(Guid id)
    {
        var response = await controller.Delete(id, new CancellationToken()) as NoContentResult;

        response.StatusCode.Should().Be((int)HttpStatusCode.NoContent);
    }

    public async Task<PersonaViewModel> Get(Guid id)
    {
        if (await controller.Get(id, new CancellationToken()) is not OkObjectResult response)
        {
            return null;
        }

        var viewModel = response.Value as PersonaViewModel;
        return viewModel;
    }

    public async Task<NoContentResult> GetNotExistent()
    {
        return await controller.Get(Guid.NewGuid(), new CancellationToken()) as NoContentResult;
    }
    public async Task<NoContentResult> GetByNameNotExistent()
    {
        return await controller.Get(Guid.NewGuid().ToString(), new CancellationToken()) as NoContentResult;
    }

    public async Task<NoContentResult> UpdateNotExistent()
    {
        var id = Guid.NewGuid();
        return await controller.Update(new UpdatePersonaCommand(id, "test1"), new CancellationToken()) as NoContentResult;
    }

    public async Task<IEnumerable<PersonaViewModel>> GetAll()
    {
        var response = await controller.Get(new CancellationToken()) as OkObjectResult;
        var viewModel = response.Value as IEnumerable<PersonaViewModel>;
        return viewModel;
    }

    public async Task<IEnumerable<PersonaViewModel>> GetListByName(string name)
    {
        var response = await controller.Get(name, new CancellationToken()) as OkObjectResult;
        var viewModel = response.Value as IEnumerable<PersonaViewModel>;
        return viewModel;
    }

    public async Task<PersonaViewModel> Update(UpdatePersonaCommand command)
    {
        var response = await controller.Update(command, new CancellationToken()) as OkObjectResult;
        var viewModel = response.Value as PersonaViewModel;
        return viewModel;
    }
}