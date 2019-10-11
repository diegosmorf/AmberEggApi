using Api.Common.Repository.Validations;
using AmberEggApi.ApplicationService.Interfaces;
using AmberEggApi.ApplicationService.ViewModels;
using AmberEggApi.Domain.Commands;
using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AmberEggApi.Domain.Tests.Factories
{
    public class CompanyFactoryTest : IDomainFactoryTest
    {
        private readonly ICompanyAppService appService;

        public CompanyFactoryTest(ICompanyAppService appService)
        {
            this.appService = appService;
        }

        public async Task<CompanyViewModel> Create()
        {
            var name = "Company 0001";
            var command = new CreateCompanyCommand(name);
            return await Create(command);
        }

        public async Task Delete(Guid id)
        {
            var commandDelete = new DeleteCompanyCommand(id);
            await appService.Delete(commandDelete);
        }

        public async Task<CompanyViewModel> Get(Guid id)
        {
            return await appService.Get(id);
        }

        public async Task<IEnumerable<CompanyViewModel>> GetAll()
        {
            return await appService.GetAll();
        }

        public async Task<IEnumerable<CompanyViewModel>> GetListByName(string name)
        {
            return await appService.GetListByName(name);
        }

        public async Task<CompanyViewModel> Create(CreateCompanyCommand command)
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

        public async Task<CompanyViewModel> Update(UpdateCompanyCommand command)
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