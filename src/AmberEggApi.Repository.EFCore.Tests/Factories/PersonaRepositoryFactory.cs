using AmberEggApi.ApplicationService.Mapping;
using AmberEggApi.Contracts.Repositories;
using AmberEggApi.Domain.Commands;
using AmberEggApi.Domain.Models;
using FluentAssertions;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace AmberEggApi.Repository.EFCoreTests.Factories;

public class PersonaRepositoryFactory(IRepository<Persona> repository, IUnitOfWork unitOfWork, IMapper mapper)
{
    private readonly IRepository<Persona> repository = repository;
    private readonly IUnitOfWork unitOfWork = unitOfWork;
    private readonly IMapper mapper = mapper;

    public async Task<Persona> Create(string name, CancellationToken cancellationToken)
    {
        var command = new CreatePersonaCommand(name);
        return await Create(command, cancellationToken);
    }

    public async Task<Persona> Create(CreatePersonaCommand command, CancellationToken cancellationToken)
    {
        var datetime = DateTime.UtcNow;
        var entity = mapper.Map<CreatePersonaCommand, Persona>(command);
        // act
        await repository.Insert(entity, cancellationToken);
        await unitOfWork.Commit(cancellationToken);
        // assert
        AssertEntityCommand(datetime, entity, command);
        return entity;
    }

    public async Task<IEnumerable<Persona>> Create(IEnumerable<CreatePersonaCommand> commands, CancellationToken cancellationToken)
    {
        var datetime = DateTime.UtcNow;
        var entities = mapper.MapList<CreatePersonaCommand, Persona>(commands);
        // act
        await repository.Insert(entities, cancellationToken);
        await unitOfWork.Commit(cancellationToken);

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

    public async Task<Persona> Get(Guid id, CancellationToken cancellationToken)
    {
        return await repository.SearchById(id, cancellationToken);
    }

    public async Task<IEnumerable<Persona>> GetAll(CancellationToken cancellationToken)
    {
        return await repository.ListAll(cancellationToken);
    }

    public async Task<IEnumerable<Persona>> GetList(string name, CancellationToken cancellationToken)
    {
        return await repository.SearchList(x => x.Name == name, cancellationToken);
    }

    public async Task<IEnumerable<Persona>> GetListByName(string name, CancellationToken cancellationToken)
    {
        return await repository.SearchList(x => x.Name == name, cancellationToken);
    }

    public async Task DeleteAll(CancellationToken cancellationToken)
    {
        var list = await repository.ListAll(cancellationToken);

        foreach (var item in list)
        {
            await repository.Delete(item.Id, cancellationToken);
        }

        await unitOfWork.Commit(cancellationToken);
    }

    public async Task Delete(Guid id, CancellationToken cancellationToken)
    {
        await repository.Delete(id, cancellationToken);
        await unitOfWork.Commit(cancellationToken);
    }

    public async Task Delete(IEnumerable<Guid> list, CancellationToken cancellationToken)
    {
        foreach (var id in list)
        {
            await repository.Delete(id, cancellationToken);
        }

        await unitOfWork.Commit(cancellationToken);
    }

    public async Task Update(IEnumerable<Persona> list, CancellationToken cancellationToken)
    {
        await repository.Update(list,cancellationToken);
        await unitOfWork.Commit(cancellationToken);
    }

    public async Task<Persona> Update(UpdatePersonaCommand command, CancellationToken cancellationToken)
    {
        var persona = await Get(command.Id, cancellationToken);
        mapper.Map<UpdatePersonaCommand, Persona>(command, persona);
        // act
        await repository.Update(persona, cancellationToken);
        await unitOfWork.Commit(cancellationToken);
        // assert
        persona.Id.Should().Be(command.Id);
        persona.Name.Should().Be(command.Name);

        return persona;
    }
}