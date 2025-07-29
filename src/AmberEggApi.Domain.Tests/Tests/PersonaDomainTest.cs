using AmberEggApi.Contracts.Exceptions;
using AmberEggApi.Domain.Commands;
using AmberEggApi.DomainTests.Factories;

using Autofac;

using FluentAssertions;

using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using Xunit;

namespace AmberEggApi.DomainTests.Tests;

[Collection("Domain.Tests.Global.Setup")]
public class PersonaDomainTest
{
    private readonly PersonaAppServiceFactory factory;
    private int index = 1;

    public PersonaDomainTest()
    {
        factory = SetupTests.Container.Resolve<PersonaAppServiceFactory>();
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
        var responseCreate = await factory.Create(expectedName, token);
        var responseSearchById = await factory.Get(responseCreate.Id, token);

        // assert
        responseSearchById.Id.Should().Be(responseCreate.Id);
        responseSearchById.Name.Should().Be(responseCreate.Name);
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
        var responseCreate = await factory.Create(expectedName, token);
        var commandUpdate = new UpdatePersonaCommand(responseCreate.Id, expectedNameAfterUpdate);

        var responseUpdate = await factory.Update(commandUpdate, token);
        var responseSearchById = await factory.Get(responseCreate.Id, token);

        // assert
        responseSearchById.Id.Should().Be(responseUpdate.Id);
        responseSearchById.Name.Should().Be(responseUpdate.Name);
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
        var responseCreate = await factory.Create(expectedName, token);
        var commandUpdate = new UpdatePersonaCommand(responseCreate.Id, expectedNameAfterUpdate);
        await factory.Update(commandUpdate, token);
        await factory.Delete(responseCreate.Id, token);
        var responseSearchById = await factory.Get(responseCreate.Id, token);
        // assert
        responseSearchById.Should().BeNull();
    }

    [Theory()]
    [InlineData("")]
    [InlineData(null)]
    [InlineData(" ")]
    [InlineData("1")]
    [InlineData("Persona-Test-Invalid-Name-1234567890")]
    public void WhenCreateNotValidEntity_Then_Error(string name)
    {
        // arrange        
        var expectedMessage = "This object instance is not valid based on DataAnnotation definitions. See more details on Errors list.";
        var command = new CreatePersonaCommand(name);
        // act
        Func<Task> action = async () => { await factory.Create(command, new CancellationToken()); };

        // assert                    
        action.Should().ThrowAsync<DomainModelException>().WithMessage(expectedMessage);
    }

    [Fact]
    public async Task WhenGetNewGuid_Then_Null()
    {
        var viewModel = await factory.Get(Guid.NewGuid(), new CancellationToken());
        // assert
        viewModel.Should().BeNull();
    }

    [Fact]
    public async Task WhenGetEmptyGuid_Then_Null()
    {
        var viewModel = await factory.Get(Guid.Empty, new CancellationToken());
        // assert
        viewModel.Should().BeNull();
    }

    [Fact]
    public async Task WhenGetListByNameEmptyString_Then_NotFound()
    {
        var expectedNumber = 0;
        //act
        var viewModel = await factory.GetListByName("", new CancellationToken());
        // assert
        viewModel.Count().Should().Be(expectedNumber);
    }

    [Fact]
    public async Task WhenGetListByNameNewGuidString_Then_NotFound()
    {
        var expectedNumber = 0;
        //act
        var viewModel = await factory.GetListByName(Guid.NewGuid().ToString(), new CancellationToken());
        // assert
        viewModel.Count().Should().Be(expectedNumber);
    }

    [Fact]
    public async Task When_Update_NewGuid_Then_Null()
    {
        // arrange
        var command = new UpdatePersonaCommand(Guid.NewGuid(), "123");
        // act
        var viewModel = await factory.Update(command, new CancellationToken());
        // assert
        viewModel.Should().BeNull();
    }

    [Fact]
    public async Task When_Update_EmptyGuid_Then_Null()
    {
        // arrange
        var command = new UpdatePersonaCommand(Guid.Empty, "123");
        // act
        Func<Task> action = async () => { await factory.Update(command, new CancellationToken()); };
        // assert
        await action.Should().ThrowAsync<DomainModelException>();
    }

    [Fact]
    public async Task When_Update_NewGuid_Then_NotThrowException()
    {
        // arrange
        var command = new UpdatePersonaCommand(Guid.NewGuid(), "123");
        // act
        Func<Task> action = async () => { await factory.Update(command, new CancellationToken()); };
        // assert
        await action.Should().NotThrowAsync<DomainModelException>();
    }

    [Fact]
    public async Task When_Update_EmptyGuid_Then_ThrowException()
    {
        // arrange
        var command = new UpdatePersonaCommand(Guid.Empty, "123");
        // act
        Func<Task> action = async () => { await factory.Update(command, new CancellationToken()); };
        // assert
        await action.Should().ThrowAsync<DomainModelException>();
    }

    [Fact]
    public async Task When_Delete_NewGuid_Then_NotThrowException()
    {
        // act
        Func<Task> action = async () => { await factory.Delete(Guid.NewGuid(), new CancellationToken()); };
        // assert
        await action.Should().NotThrowAsync<DomainModelException>();
    }

    [Fact]
    public async Task When_Delete_EmptyGuid_Then_NotThrowException()
    {
        // act
        Func<Task> action = async () => { await factory.Delete(Guid.Empty, new CancellationToken()); };
        // assert
        await action.Should().ThrowAsync<DomainModelException>();
    }    
}