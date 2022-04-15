using AmberEggApi.ApplicationService.ViewModels;
using AmberEggApi.Domain.Commands;
using AmberEggApi.WebApi.Controllers;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

namespace AmberEggApi.Domain.Tests.Factories
{
    public class PersonaControllerFactory
    {
        private readonly PersonaController controller;

        public PersonaControllerFactory(PersonaController controller)
        {
            this.controller = controller;
        }

        public async Task<PersonaViewModel> Create()
        {
            var name = "Persona 0001";
            var command = new CreatePersonaCommand(name);
            return await Create(command);
        }

        public async Task<PersonaViewModel> Create(CreatePersonaCommand command)
        {
            var response = await controller.Create(command) as CreatedResult;
            var viewmodel = response.Value as PersonaViewModel;
            return viewmodel;
        }

        public async Task Delete(Guid id)
        {
            var response = await controller.Delete(id) as NoContentResult;

            response.StatusCode.Should().Be((int)HttpStatusCode.NoContent);
        }

        public async Task<PersonaViewModel> Get(Guid id)
        {
            if (await controller.Get(id) is not OkObjectResult response)
            {
                return null;
            }

            var viewmodel = response.Value as PersonaViewModel;
            return viewmodel;
        }

        public async Task<NoContentResult> GetNotExistent()
        {
            return await controller.Get(Guid.NewGuid()) as NoContentResult;
        }
        public async Task<NoContentResult> GetByNameNotExistent()
        {
            return await controller.Get(Guid.NewGuid().ToString()) as NoContentResult;
        }

        public async Task<NoContentResult> UpdateNotExistent()
        {
            var id = Guid.NewGuid();
            return await controller.Update(id, new UpdatePersonaCommand(id, "")) as NoContentResult;
        }

        public async Task<IEnumerable<PersonaViewModel>> GetAll()
        {
            var response = await controller.Get() as OkObjectResult;
            var viewmodel = response.Value as IEnumerable<PersonaViewModel>;
            return viewmodel;
        }

        public async Task<IEnumerable<PersonaViewModel>> GetListByName(string name)
        {
            var response = await controller.Get(name) as OkObjectResult;
            var viewmodel = response.Value as IEnumerable<PersonaViewModel>;
            return viewmodel;
        }

        public async Task<PersonaViewModel> Update(UpdatePersonaCommand command)
        {
            var response = await controller.Update(command.Id, command) as OkObjectResult;
            var viewmodel = response.Value as PersonaViewModel;
            return viewmodel;
        }
    }
}