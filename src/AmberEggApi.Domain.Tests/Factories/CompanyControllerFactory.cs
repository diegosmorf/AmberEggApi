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
    public class CompanyControllerFactory
    {
        private readonly CompanyController controller;

        public CompanyControllerFactory(CompanyController controller)
        {
            this.controller = controller;
        }

        public async Task<CompanyViewModel> Create()
        {
            var name = "Company 0001";
            var command = new CreateCompanyCommand(name);
            return await Create(command);
        }

        public async Task<CompanyViewModel> Create(CreateCompanyCommand command)
        {
            var response = await controller.Create(command) as CreatedResult;
            var viewmodel = response.Value as CompanyViewModel;
            return viewmodel;
        }

        public async Task Delete(Guid id)
        {
            var response = await controller.Delete(id) as NoContentResult;

            response.StatusCode.Should().Be((int)HttpStatusCode.NoContent);
        }

        public async Task<CompanyViewModel> Get(Guid id)
        {
            var response = await controller.Get(id) as OkObjectResult;
            
            if (response == null)
            {
                return null; 
            }

            var viewmodel = response.Value  as CompanyViewModel;
            return viewmodel;
        }

        public async Task<NotFoundResult> GetNotExistent()
        {
            return await controller.Get(Guid.NewGuid()) as NotFoundResult;
        }
        public async Task<NotFoundResult> GetByNameNotExistent()
        {
            return await controller.Get(Guid.NewGuid().ToString()) as NotFoundResult;
        }

        public async Task<NotFoundResult> UpdateNotExistent()
        {
            var id = Guid.NewGuid();
            return await controller.Update(id, new UpdateCompanyCommand(id,"")) as NotFoundResult;
        }

        public async Task<IEnumerable<CompanyViewModel>> GetAll()
        {
            var response = await controller.Get() as OkObjectResult;
            var viewmodel = response.Value as IEnumerable<CompanyViewModel>;
            return viewmodel;
        }

        public async Task<IEnumerable<CompanyViewModel>> GetListByName(string name)
        {
            var response = await controller.Get(name) as OkObjectResult;
            var viewmodel = response.Value as IEnumerable<CompanyViewModel>;
            return viewmodel;
        }

        public async Task<CompanyViewModel> Update(UpdateCompanyCommand command)
        {
            var response = await controller.Update(command.Id, command) as OkObjectResult;
            var viewmodel = response.Value as CompanyViewModel;
            return viewmodel;
        }
    }
}