using AmberEggApi.ApplicationService.ViewModels;
using AmberEggApi.Domain.Commands;
using AmberEggApi.WebApi.Controllers;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
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
            await controller.Delete(id);
        }

        public async Task<CompanyViewModel> Get(Guid id)
        {
            var response = await controller.Get(id) as OkObjectResult;

            if (response == null)
            {
                return null; 
            }

            var viewmodel = response.Value as CompanyViewModel;
            return viewmodel;
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