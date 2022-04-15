using AmberEggApi.ApplicationService.Interfaces;
using AmberEggApi.ApplicationService.ViewModels;
using AmberEggApi.Domain.Commands;
using Api.Common.Repository.Validations;
using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AmberEggApi.Domain.Tests.Factories
{
    public class PersonaAppServiceFactory
    {
        private readonly IPersonaAppService appService;

        public PersonaAppServiceFactory(IPersonaAppService appService)
        {
            this.appService = appService;
        }

        public async Task<PersonaViewModel> Create()
        {
            var name = "Persona 0001";
            var command = new CreatePersonaCommand(name);
            return await Create(command);
        }

        public async Task<PersonaViewModel> Create(CreatePersonaCommand command)
        {
            //arrange
            const int expectedNumberOfErrors = 0;

            //act
            var response = await appService.Create(command);

            //assert
            command.ValidateModelAnnotations().Count.Should().Be(expectedNumberOfErrors);
            response.Id.Should().NotBe(Guid.Empty);
            response.Name.Should().Be(command.Name);

            return response;
        }

        public async Task Delete(Guid id)
        {
            var commandDelete = new DeletePersonaCommand(id);
            await appService.Delete(commandDelete);
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
            //arrange
            const int expectedNumberOfErrors = 0;

            //act
            var response = await appService.Update(command);

            //assert
            command.ValidateModelAnnotations().Count.Should().Be(expectedNumberOfErrors);
            response.Id.Should().Be(command.Id);
            response.Name.Should().Be(command.Name);

            return response;
        }
    }
}