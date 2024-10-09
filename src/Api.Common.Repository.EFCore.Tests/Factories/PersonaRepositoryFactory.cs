using AmberEggApi.Domain.Commands;
using AmberEggApi.Domain.Models;
using Api.Common.Repository.Repositories;
using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Api.Common.Repository.EFCoreTests.Factories
{
    public class PersonaRepositoryFactory(IRepository<Persona> repository, IUnitOfWork unitOfWork)
    {
        private readonly IRepository<Persona> repository = repository;
        private readonly IUnitOfWork unitOfWork = unitOfWork;

        public async Task<Persona> Create(string name)
        {
            var command = new CreatePersonaCommand(name);
            return await Create(command);
        }

        public async Task<Persona> Create(CreatePersonaCommand command)
        {
            var datetime = DateTime.Now;
            var persona = new Persona();
            persona.Create(command);

            // act
            await repository.Insert(persona);
            await unitOfWork.Commit();

            // assert
            persona.Id.Should().NotBe(Guid.Empty);
            persona.Name.Should().Be(command.Name);
            persona.CreateDate.ToShortDateString().Should().Be(datetime.ToShortDateString());
            persona.ModifiedDate.Should().BeNull();
            persona.ToString().Should().Be($"Type:{persona.GetType().Name} - Id:{persona.Id}");

            return persona;
        }


        public async Task<Persona> Get(Guid id)
        {
            return await repository.SearchById(id);
        }

        public async Task<IEnumerable<Persona>> GetAll()
        {
            return await repository.ListAll();
        }

        public async Task<IEnumerable<Persona>> GetList(string name)
        {
            return await repository.SearchList(x => x.Name == name);
        }

        public async Task<IEnumerable<Persona>> GetListByName(string name)
        {
            return await repository.SearchList(x => x.Name == name);
        }

        public async Task DeleteAll()
        {
            var list = await repository.ListAll();

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
            foreach (var id in list)
            {
                await repository.Delete(id);
            }

            await unitOfWork.Commit();
        }

        public async Task Update(IEnumerable<Persona> list)
        {
            await repository.Update(list);
            await unitOfWork.Commit();
        }

        public async Task<Persona> Update(UpdatePersonaCommand command)
        {
            var persona = await Get(command.Id);
            persona.Update(command);

            // act
            await repository.Update(persona);
            await unitOfWork.Commit();

            // assert
            persona.Id.Should().Be(command.Id);
            persona.Name.Should().Be(command.Name);

            return persona;
        }
    }
}