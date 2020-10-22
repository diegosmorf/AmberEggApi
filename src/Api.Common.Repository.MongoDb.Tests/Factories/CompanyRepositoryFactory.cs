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
        private readonly IRepository<Persona> repository;
        private readonly IUnitOfWork unitOfWork;

        public CompanyRepositoryFactory(IRepository<Persona> repository, IUnitOfWork unitOfWork)
        {
            this.repository = repository;
            this.unitOfWork = unitOfWork;
        }

        public async Task<Persona> Create()
        {
            var name = "Company Test";
            var command = new CreatePersonaCommand(name);
           
            return await Create(command);
        }

        public async Task<Persona> Create(CreatePersonaCommand command)
        {
            var datetime = DateTime.Now;
            var company = new Persona();
            company.Create(command);

            //act
            await repository.Insert(company);
            await unitOfWork.Commit();

            //assert
            company.Id.Should().NotBe(Guid.Empty);
            company.Name.Should().Be(command.Name);            
            company.CreateDate.ToShortDateString().Should().Be(datetime.ToShortDateString());
            company.ModifiedDate.Should().BeNull();
            company.ToString().Should().Be($"Type:{company.GetType().Name} - Id:{company.Id}");

            return company;
        }


        public async Task<Persona> Get(Guid id)
        {
            return await repository.FindById(id);
        }

        public async Task<IEnumerable<Persona>> GetAll()
        {
            return await repository.All();
        }

        public async Task<IEnumerable<Persona>> GetList(string name)
        {
            return await repository.FindList(x=>x.Name == name);
        }

        public async Task<IEnumerable<Persona>> GetListByName(string name)
        {
            return await repository.FindList(x=>x.Name == name);
        }

        public async Task<Persona> Update(UpdatePersonaCommand command)
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

        public async Task DeleteAll()
        {
            var list = await repository.All();

            foreach (var item in list)
            {
                await repository.Delete(item.Id);
            }

            await unitOfWork.Commit();
        }

        public async Task Delete(Guid id)
        {
            await repository.Delete(id);
            await unitOfWork.Commit();
        }

        public async Task Delete(IEnumerable<Guid> list)
        {
            await repository.Delete(list);
            await unitOfWork.Commit();
        }

        public async Task Update(IEnumerable<Persona> list)
        {
            await repository.Update(list);
            await unitOfWork.Commit();
        }
    }
}