using AmberEggApi.Contracts.Exceptions;
using AmberEggApi.Domain.Commands;
using AmberEggApi.DomainTests.Factories;

using Autofac;

using FluentAssertions;

using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

using Xunit;

namespace AmberEggApi.DomainTests.Tests;

[Collection("Domain.Tests.Global.Setup")]
public class PersonaControllerTest
{
    private readonly PersonaControllerFactory factory;
    private int index = 1;

    public PersonaControllerTest()
    {
        factory = SetupTests.Container.Resolve<PersonaControllerFactory>();
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
        var expectedName = $"{name}-{index++}";
        // act
        var responseCreate = await factory.Create(expectedName);
        var responseSearchById = await factory.Get(responseCreate.Id);

        // assert
        responseSearchById.Id.Should().Be(responseCreate.Id);
        responseSearchById.Name.Should().Be(responseCreate.Name);
    }

    [Theory()]
    [InlineData("P")]
    [InlineData("Test")]
    [InlineData("Persona")]
    [InlineData("Persona-Test")]
    [InlineData("Persona-Test 1")]
    public async Task WhenCreateAndUpdate_Then_FindItById(string name)
    {
        // arrange
        var expectedName = $"{name}-{index++}";
        var expectedNameAfterUpdate = $"{expectedName}-v2";

        // act
        var responseCreate = await factory.Create(expectedName);
        var commandUpdate = new UpdatePersonaCommand(
            responseCreate.Id,
            expectedNameAfterUpdate);

        var responseUpdate = await factory.Update(commandUpdate);
        var responseSearchById = await factory.Get(responseCreate.Id);

        // assert
        responseSearchById.Id.Should().Be(responseUpdate.Id);
        responseSearchById.Name.Should().Be(responseUpdate.Name);
    }

    [Theory()]
    [InlineData("P")]
    [InlineData("Test")]
    [InlineData("Persona")]
    [InlineData("Persona-Test")]
    [InlineData("Persona-Test 1")]
    public async Task WhenCreateAndUpdateAndDelete_Then_Success(string name)
    {
        // arrange
        var expectedName = $"{name}-{index++}";
        var expectedNameAfterUpdate = $"{expectedName}-v2";

        // act
        var responseCreate = await factory.Create(expectedName);
        var commandUpdate = new UpdatePersonaCommand(
            responseCreate.Id,
            expectedNameAfterUpdate);

        await factory.Update(commandUpdate);
        await factory.Delete(responseCreate.Id);

        var responseSearchById = await factory.Get(responseCreate.Id);

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
        var expectedNumberOfErrors = 1;
        var expectedMessage = "This object instance is not valid based on DataAnnotation definitions. See more details on Errors list.";
        var command = new CreatePersonaCommand(name);

        // act
        Func<Task> action = async () => await factory.Create(command);

        // assert
        action.Should()
            .ThrowAsync<ModelException>()
            .Where(x => x.Errors.Count() == expectedNumberOfErrors);

        action.Should()
            .ThrowAsync<ModelException>()
            .WithMessage(expectedMessage);
    }

    [Fact]
    public async Task WhenGetNotExistent_Then_NotFound()
    {
        var response = await factory.GetNotExistent();

        // assert
        response.StatusCode.Should().Be((int)HttpStatusCode.NoContent);
    }

    [Fact]
    public async Task WhenGetNameNotExistent_Then_NotFound()
    {
        var response = await factory.GetByNameNotExistent();

        // assert
        response.StatusCode.Should().Be((int)HttpStatusCode.NoContent);
    }

    [Fact]
    public async Task WhenUpdateNotExistent_Then_NotFound()
    {
        var response = await factory.UpdateNotExistent();

        // assert
        response.StatusCode.Should().Be((int)HttpStatusCode.NoContent);
    }
}