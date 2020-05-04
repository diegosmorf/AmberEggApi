using AmberEggApi.Domain.Commands;
using AmberEggApi.Domain.Models;
using Api.Common.Repository.Repositories;
using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Api.Common.Repository.MongoDb.Tests.Factories
{
    public class CompanyRepositoryFactory
    {
        private readonly IRepository<Company> repository;
        private readonly IUnitOfWork unitOfWork;

        public CompanyRepositoryFactory(IRepository<Company> repository, IUnitOfWork unitOfWork)
        {
            this.repository = repository;
            this.unitOfWork = unitOfWork;
        }

        public async Task<Company> Create()
        {
            var name = "Company 0001";
            var command = new CreateCompanyCommand(name);
           
            return await Create(command);
        }

        public async Task<Company> Create(CreateCompanyCommand command)
        {
            var company = new Company();
            company.Create(command);

            //act
            await repository.Insert(company);
            await unitOfWork.Commit();

            //assert
            company.Id.Should().NotBe(Guid.Empty);
            company.Name.Should().Be(command.Name);

            return company;
        }

        public async Task Delete(Guid id)
        {
            await repository.Delete(id);
            await unitOfWork.Commit();
        }

        public async Task<Company> Get(Guid id)
        {
            return await repository.FindById(id);
        }

        public async Task<IEnumerable<Company>> GetAll()
        {
            return await repository.All();
        }

        public async Task<IEnumerable<Company>> GetListByName(string name)
        {
            return await repository.FindList(x=>x.Name == name);
        }

        public async Task<Company> Update(UpdateCompanyCommand command)
        {
            var company = await Get(command.Id);
            company.Update(command);

            //act
            await repository.Update(company);
            await unitOfWork.Commit();

            //assert
            company.Id.Should().Be(command.Id);
            company.Name.Should().Be(command.Name);

            return company;
        }
    }
}