using AmberEggApi.ApplicationService.Interfaces;
using AmberEggApi.ApplicationService.ViewModels;
using AmberEggApi.Domain.Commands;
using AmberEggApi.Domain.Models;
using Api.Common.Cqrs.Core.Commands;
using Api.Common.Repository.Repositories;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AmberEggApi.ApplicationService.Services
{
    public class CompanyAppService : BaseAppService, ICompanyAppService
    {
        private readonly IRepository<Company> repository;

        public CompanyAppService(
            ICommandProducer producer,
            IMapper mapper,
            IRepository<Company> repository) : base(producer, mapper)
        {
            this.repository = repository;
        }

        public async Task<IEnumerable<CompanyViewModel>> GetAll()
        {
            var list = await repository.All();

            return mapper.Map<IEnumerable<CompanyViewModel>>(list);
        }

        public async Task<IEnumerable<CompanyViewModel>> GetListByName(string name)
        {
            var list = await repository.FindList(x => x.Name.Contains(name));

            return mapper.Map<IEnumerable<CompanyViewModel>>(list).OrderBy(x => x.Name);
        }

        public async Task<CompanyViewModel> Get(Guid id)
        {
            var instance = await repository.FindById(id);

            return mapper.Map<CompanyViewModel>(instance);
        }

        public async Task<CompanyViewModel> Create(CreateCompanyCommand command)
        {
            //send command to broker
            var result = await producer.Send<CreateCompanyCommand, Company>(command);

            return mapper.Map<CompanyViewModel>(result);
        }

        public async Task<CompanyViewModel> Update(UpdateCompanyCommand command)
        {
            //send command to broker
            var result = await producer.Send<UpdateCompanyCommand, Company>(command);

            return mapper.Map<CompanyViewModel>(result);
        }

        public async Task Delete(DeleteCompanyCommand commandDelete)
        {
            //send command to broker
            await producer.Send<DeleteCompanyCommand, Company>(commandDelete);
        }
    }
}