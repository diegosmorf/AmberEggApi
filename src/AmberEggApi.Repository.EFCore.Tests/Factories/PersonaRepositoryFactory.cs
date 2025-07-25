using AmberEggApi.Contracts.Repositories;
using AmberEggApi.Domain.Commands;
using AmberEggApi.Domain.Models;

using AutoMapper;

using FluentAssertions;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AmberEggApi.Repository.EFCoreTests.Factories;

public class PersonaRepositoryFactory(IRepository<Persona> repository, IUnitOfWork unitOfWork, IMapper mapper)
{
    private readonly IRepository<Persona> repository = repository;
    private readonly IUnitOfWork unitOfWork = unitOfWork;
    private readonly IMapper mapper = mapper;

    public async Task<Persona> Create(string name)
    {
        var command = new CreatePersonaCommand(name);
        return await Create(command);
    }

    public async Task<Persona> Create(CreatePersonaCommand command)
    {
        var datetime = DateTime.UtcNow;
        var entity = mapper.Map<Persona>(command);
        // act
        await repository.Insert(entity);
        await unitOfWork.Commit();
        // assert
        AssertEntityCommand(datetime, entity, command);
        return entity;
    }

    public async Task<IEnumerable<Persona>> Create(IEnumerable<CreatePersonaCommand> commands)
    {
        var datetime = DateTime.UtcNow;
        var entities = mapper.Map<IEnumerable<Persona>>(commands);
        // act
        await repository.Insert(entities);
        await unitOfWork.Commit();

        // assert
        foreach (var entity in entities)
        {
            var command = commands.FirstOrDefault(x => x.Name == entity.Name);
            AssertEntityCommand(datetime, entity, command);
        }

        return entities;
    }

    private static void AssertEntityCommand(DateTime datetime, Persona entity, CreatePersonaCommand command)
    {
        entity.Should().NotBeNull();
        entity.Id.Should().NotBe(Guid.Empty);
        entity.Name.Should().Be(command.Name);
        entity.CreateDate.ToShortDateString().Should().Be(datetime.ToShortDateString());
        entity.ModifiedDate.Should().BeNull();
        entity.ModifiedDate?.ToShortDateString().Should().Be(datetime.ToShortDateString());
        entity.ToString().Should().Be($"Type:{entity.GetType().Name} - Id:{entity.Id}");
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
        mapper.Map(command, persona);
        // act
        await repository.Update(persona);
        await unitOfWork.Commit();
        // assert
        persona.Id.Should().Be(command.Id);
        persona.Name.Should().Be(command.Name);

        return persona;
    }
}