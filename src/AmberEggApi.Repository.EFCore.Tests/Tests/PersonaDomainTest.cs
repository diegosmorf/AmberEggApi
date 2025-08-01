﻿using AmberEggApi.Contracts.Repositories;
using AmberEggApi.Domain.Commands;
using AmberEggApi.Domain.Models;
using AmberEggApi.Repository.EFCoreTests.Factories;

using Autofac;

using FluentAssertions;

using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using Xunit;

namespace AmberEggApi.Repository.EFCoreTests.Tests;

[Collection("Repository.Tests.Global.Setup")]
public class PersonaDomainTest
{
    private readonly IRepository<Persona> repository;
    private readonly PersonaRepositoryFactory factory;
    private int index = 1;

    public PersonaDomainTest()
    {
        repository = SetupTests.Container.Resolve<IRepository<Persona>>();
        factory = SetupTests.Container.Resolve<PersonaRepositoryFactory>();
    }

    [Theory()]
    [InlineData("P")]
    [InlineData("Persona-Test 1")]
    [InlineData("Persona-Test 10")]
    [InlineData("Persona-Test 100")]
    [InlineData("Persona-Test 1000")]
    public async Task WhenCreate_Then_FindItById(string name)
    {
        // arrange            
        var token = new CancellationToken();
        var expectedName = $"{name}-{index++}";
        // act
        var resultCreate = await factory.Create(expectedName, token);
        var resultGet = await repository.SearchById(resultCreate.Id, token);

        // assert
        resultGet.Id.Should().Be(resultCreate.Id);
        resultGet.Name.Should().Be(resultCreate.Name);
    }

    [Theory()]
    [InlineData("P")]
    [InlineData("Persona")]
    [InlineData("Persona-Test")]
    [InlineData("Persona-Test 1")]
    public async Task WhenCreateAndUpdate_Then_FindItById(string name)
    {
        // arrange
        var token = new CancellationToken();
        var expectedName = $"{name}-{index++}";
        var expectedNameAfterUpdate = $"{expectedName}-v2";

        // act
        var resultCreate = await factory.Create(expectedName, token);
        var commandUpdate = new UpdatePersonaCommand(resultCreate.Id, expectedNameAfterUpdate);

        var responseUpdate = await factory.Update(commandUpdate, token);
        var resultGet = await repository.Search(x => x.Id == resultCreate.Id, token);

        // assert
        resultGet.Id.Should().Be(responseUpdate.Id);
        resultGet.Name.Should().Be(responseUpdate.Name);
    }

    [Theory()]
    [InlineData("P")]
    [InlineData("Persona")]
    [InlineData("Persona-Test")]
    [InlineData("Persona-Test 1")]
    public async Task WhenCreateAndUpdateAndDelete_Then_Success(string name)
    {
        // arrange
        var token = new CancellationToken();
        var expectedName = $"{name}-{index++}";
        var expectedNameAfterUpdate = $"{expectedName}-v2";

        // act
        var resultCreate = await factory.Create(expectedName, token);
        var commandUpdate = new UpdatePersonaCommand(
            resultCreate.Id,
            expectedNameAfterUpdate);

        await factory.Update(commandUpdate, token);
        await repository.Delete(resultCreate.Id, token);

        var resultGet = await repository.SearchById(resultCreate.Id, token);

        // assert
        resultGet.Should().BeNull();
    }

    [Theory()]
    [InlineData("P")]
    [InlineData("Persona-Test")]
    [InlineData("Persona-Test 1")]
    [InlineData("Persona-Test 10")]
    public async Task WhenCreateMultiples_Then_DeleteMultiples(string name)
    {
        // arrange
        var token = new CancellationToken();
        var expectedName = $"{name}-{index++}";
        var expectedInserted = 4;
        var finalResult = 0;

        // act
        await factory.DeleteAll(token);
        await factory.Create(expectedName, token);
        await factory.Create(expectedName, token);
        await factory.Create(expectedName, token);
        await factory.Create(expectedName, token);

        var currentInserted = (await repository.SearchList(x => x.Name.Contains(expectedName), token)).Count();
        await factory.DeleteAll(token);
        var currentResult = (await repository.ListAll(token)).Count();
        // assert
        expectedInserted.Should().Be(currentInserted);
        finalResult.Should().Be(currentResult);
    }

    [Theory()]
    [InlineData("P")]
    [InlineData("Persona-Test")]
    [InlineData("Persona-Test 1")]
    [InlineData("Persona-Test 10")]
    public async Task WhenCreateList_Then_DeleteMultiples(string name)
    {
        // arrange
        var token = new CancellationToken();
        var expectedName = $"{name}-{index++}";
        var expectedInserted = 4;
        var listToCreate = new[]
        {
            new CreatePersonaCommand(expectedName),
            new CreatePersonaCommand(expectedName),
            new CreatePersonaCommand(expectedName),
            new CreatePersonaCommand(expectedName)
        };
        var finalResult = 0;

        // act
        await factory.DeleteAll(token);
        await factory.Create(listToCreate, token);
        var currentInserted = (await repository.SearchList(x => x.Name.Contains(expectedName), token)).Count();
        await factory.DeleteAll(token);
        var currentResult = (await repository.ListAll(token)).Count();
        // assert
        expectedInserted.Should().Be(currentInserted);
        finalResult.Should().Be(currentResult);
    }

    [Theory()]
    [InlineData("P")]
    [InlineData("Persona-Test")]
    [InlineData("Persona-Test 1")]
    [InlineData("Persona-Test 10")]
    public async Task WhenUpdateMultiples_Then_KeepMultiples(string name)
    {
        // arrange
        var token = new CancellationToken();
        var expectedInserted = 4;
        var finalResult = 4;
        var expectedName = $"{name}-{index++}";

        // act
        await factory.DeleteAll(token);
        var persona1 = await factory.Create(expectedName, token);
        var persona2 = await factory.Create(expectedName, token);
        var persona3 = await factory.Create(expectedName, token);
        var persona4 = await factory.Create(expectedName, token);

        var list = new[] { persona1, persona2, persona3, persona4 };
        var currentInserted = (await factory.GetListByName(expectedName, token)).Count();
        await repository.Update(list, token);
        var currentResult = (await factory.GetListByName(expectedName, token)).Count();

        // assert
        expectedInserted.Should().Be(currentInserted);
        finalResult.Should().Be(currentResult);
    }
}