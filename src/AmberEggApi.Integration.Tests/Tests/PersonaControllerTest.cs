using AmberEggApi.ApplicationService.ViewModels;
using AmberEggApi.IntegrationTests.Factories;
using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Xunit;

namespace AmberEggApi.IntegrationTests.Tests;

[Collection("Integration.Tests.Global.Setup")]
public class PersonaControllerTest
{
    private readonly PersonaControllerFactoryTest factory;
    private int index = 1;

    public PersonaControllerTest()
    {
        factory = new PersonaControllerFactoryTest(SetupTests.Client);
    }

    [Theory()]
    [InlineData("")]
    [InlineData(null)]
    [InlineData(" ")]
    [InlineData("1")]
    [InlineData("Persona-Test-Invalid-Name-1234567890")]
    public async Task When_Empty_Create_Then_BadRequest(string name)
    {
        // act
        var response = await factory.Create(name);
        // assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Theory()]
    [InlineData("")]
    [InlineData(null)]
    [InlineData(" ")]    
    public async Task When_Invalid_GetByName_Then_BadRequest(string name)
    {
        // act
        var response = await factory.Get(name);
        // assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Theory()]
    [InlineData("P")]
    [InlineData("Persona-Test 1")]
    [InlineData("Persona-Test 10")]
    [InlineData("Persona-Test 100")]
    [InlineData("Persona-Test 1000")]
    public async Task When_Valid_Create_Then_Success(string name)
    {
        // arrange
        var expectedName = $"{name}-{index++}";
        // act            
        var response = await factory.Create(expectedName);
        var viewModel = await PersonaControllerFactoryTest.GetViewModel<PersonaViewModel>(response);
        // assert
        response.StatusCode.Should().Be(HttpStatusCode.Created);
        viewModel.Should().BeOfType<PersonaViewModel>();
        viewModel.Id.Should().NotBeEmpty();
        viewModel.Name.Should().NotBeNullOrEmpty();
        viewModel.Name.Should().Be(expectedName);
    }

    [Theory()]
    [InlineData("P")]
    [InlineData("Persona-Test 1")]
    [InlineData("Persona-Test 10")]
    [InlineData("Persona-Test 100")]
    [InlineData("Persona-Test 1000")]
    public async Task When_Valid_Create_GetById_Then_Success(string name)
    {
        // arrange
        var expectedName = $"{name}-{index++}";
        // act
        var responseCreate = await factory.Create(expectedName);
        var viewModelCreate = await PersonaControllerFactoryTest.GetViewModel<PersonaViewModel>(responseCreate);

        var responseGet = await factory.Get(viewModelCreate.Id);
        var viewModelGet = await PersonaControllerFactoryTest.GetViewModel<PersonaViewModel>(responseGet);
        // assert
        responseCreate.StatusCode.Should().Be(HttpStatusCode.Created);
        responseGet.StatusCode.Should().Be(HttpStatusCode.OK);
        viewModelGet.Should().BeOfType<PersonaViewModel>();
        viewModelGet.Id.Should().NotBeEmpty();
        viewModelGet.Name.Should().NotBeNullOrEmpty();
        viewModelGet.Name.Should().Be(expectedName);
        viewModelCreate.Id.Should().Be(viewModelGet.Id);
        viewModelCreate.Name.Should().Be(viewModelGet.Name);
        viewModelCreate.Name.Should().Be(expectedName);
    }

    [Theory()]
    [InlineData("P")]
    [InlineData("Persona-Test 1")]
    [InlineData("Persona-Test 10")]
    [InlineData("Persona-Test 100")]
    [InlineData("Persona-Test 1000")]
    public async Task When_Valid_Create_GetAll_Then_Success(string name)
    {
        // arrange
        var expectedName = $"{name}-{index++}";
        // act
        var responseCreate = await factory.Create(expectedName);
        var viewModelCreate = await PersonaControllerFactoryTest.GetViewModel<PersonaViewModel>(responseCreate);

        var responseGet = await factory.GetAll();
        var viewModelGet = (await PersonaControllerFactoryTest.GetViewModel<IEnumerable<PersonaViewModel>>(responseGet)).FirstOrDefault(x => x.Id == viewModelCreate.Id);

        // assert
        responseCreate.StatusCode.Should().Be(HttpStatusCode.Created);
        responseGet.StatusCode.Should().Be(HttpStatusCode.OK);
        viewModelGet.Should().BeOfType<PersonaViewModel>();
        viewModelGet.Id.Should().NotBeEmpty();
        viewModelGet.Name.Should().NotBeNullOrEmpty();
        viewModelGet.Name.Should().Be(expectedName);
        viewModelCreate.Id.Should().Be(viewModelGet.Id);
        viewModelCreate.Name.Should().Be(viewModelGet.Name);
        viewModelCreate.Name.Should().Be(expectedName);
    }

    [Fact]
    public async Task When_EmptyDatabase_GetAll_Then_NoContent()
    {
        // act            
        await factory.DeleteAll();
        var response = await factory.GetAll();

        // assert            
        response.StatusCode.Should().Be(HttpStatusCode.NoContent);
    }

    [Theory()]
    [InlineData("P2")]
    [InlineData("Persona-Test 2")]
    [InlineData("Persona-Test 20")]
    [InlineData("Persona-Test 200")]
    [InlineData("Persona-Test 2000")]
    public async Task When_Valid_Create_GetByName_Then_Success(string name)
    {
        // arrange
        var expectedName = $"{name}-{index++}";
        // act
        var responseCreate = await factory.Create(expectedName);
        var viewModelCreate = await PersonaControllerFactoryTest.GetViewModel<PersonaViewModel>(responseCreate);

        var responseGet = await factory.Get(viewModelCreate.Name);
        var viewModelGet = (await PersonaControllerFactoryTest.GetViewModel<IEnumerable<PersonaViewModel>>(responseGet)).FirstOrDefault();

        // assert
        responseCreate.StatusCode.Should().Be(HttpStatusCode.Created);
        responseGet.StatusCode.Should().Be(HttpStatusCode.OK);
        viewModelGet.Should().BeOfType<PersonaViewModel>();
        viewModelGet.Id.Should().NotBeEmpty();
        viewModelGet.Name.Should().NotBeNullOrEmpty();
        viewModelGet.Name.Should().Be(expectedName);
        viewModelCreate.Id.Should().Be(viewModelGet.Id);
        viewModelCreate.Name.Should().Be(viewModelGet.Name);
        viewModelCreate.Name.Should().Be(expectedName);
    }

    [Theory()]
    [InlineData("P")]
    [InlineData("Persona-Test 1")]
    public async Task When_Valid_Create_Update_Then_Success(string name)
    {
        // arrange
        var expectedName = $"{name}-{index++}";
        // act
        var responseCreate = await factory.Create(expectedName);
        var viewModelCreate = await PersonaControllerFactoryTest.GetViewModel<PersonaViewModel>(responseCreate);
        var responseUpdate = await factory.Update(viewModelCreate.Id, $"{expectedName}-v2");
        var viewModelUpdate = await PersonaControllerFactoryTest.GetViewModel<PersonaViewModel>(responseUpdate);
        var responseGet = await factory.Get(viewModelCreate.Id);
        var viewModelGet = await PersonaControllerFactoryTest.GetViewModel<PersonaViewModel>(responseGet);
        // assert
        responseCreate.StatusCode.Should().Be(HttpStatusCode.Created);
        responseGet.StatusCode.Should().Be(HttpStatusCode.OK);
        responseUpdate.StatusCode.Should().Be(HttpStatusCode.OK);
        viewModelGet.Should().BeOfType<PersonaViewModel>();
        viewModelGet.Id.Should().NotBeEmpty();
        viewModelGet.Name.Should().NotBeNullOrEmpty();
        viewModelUpdate.Id.Should().Be(viewModelGet.Id);
        viewModelUpdate.Name.Should().Be(viewModelGet.Name);
    }

    [Theory()]
    [InlineData("P")]
    [InlineData("Persona-Test 1")]
    public async Task When_Valid_Create_Update_Delete_Then_Success(string name)
    {
        // arrange
        var expectedName = $"{name}-{index++}";
        // act
        var responseCreate = await factory.Create(expectedName);
        var viewModelCreate = await PersonaControllerFactoryTest.GetViewModel<PersonaViewModel>(responseCreate);
        var responseUpdate = await factory.Update(viewModelCreate.Id, $"{expectedName}-v2");
        var responseDelete = await factory.Delete(viewModelCreate.Id);
        var responseGet = await factory.Get(viewModelCreate.Id);

        // assert
        responseCreate.StatusCode.Should().Be(HttpStatusCode.Created);
        responseUpdate.StatusCode.Should().Be(HttpStatusCode.OK);
        responseDelete.StatusCode.Should().Be(HttpStatusCode.NoContent);
        responseGet.StatusCode.Should().Be(HttpStatusCode.NoContent);
    }

    [Theory()]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData(null)]
    [InlineData("Persona-Test-Invalid-Name-1234567890")]
    public async Task When_Invalid_Create_Update_Then_BadRequest(string invalidName)
    {
        // arrange
        var name = $"Persona-Test-{index++}";
        // act
        var responseCreate = await factory.Create(name);
        var viewModelCreate = await PersonaControllerFactoryTest.GetViewModel<PersonaViewModel>(responseCreate);
        var responseUpdate = await factory.Update(viewModelCreate.Id, invalidName);
        // assert
        responseCreate.StatusCode.Should().Be(HttpStatusCode.Created);
        responseUpdate.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        viewModelCreate.Should().BeOfType<PersonaViewModel>();
        viewModelCreate.Id.Should().NotBeEmpty();
        viewModelCreate.Name.Should().NotBeNullOrEmpty();
    }

    [Fact]
    public async Task When_Create_Update_EmptyId_Then_BadRequest()
    {
        // arrange
        var name = $"Persona-Test-{index++}";
        // act
        var responseCreate = await factory.Create(name);
        var viewModelCreate = await PersonaControllerFactoryTest.GetViewModel<PersonaViewModel>(responseCreate);
        var responseUpdate = await factory.Update(Guid.Empty, name);
        // assert
        responseCreate.StatusCode.Should().Be(HttpStatusCode.Created);
        responseUpdate.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        viewModelCreate.Should().BeOfType<PersonaViewModel>();
        viewModelCreate.Id.Should().NotBeEmpty();
        viewModelCreate.Name.Should().NotBeNullOrEmpty();
    }

    [Fact]
    public async Task When_Create_Update_NewId_Then_NoContent()
    {
        // arrange
        var name = $"Persona-Test-{index++}";
        // act
        var responseCreate = await factory.Create(name);
        await PersonaControllerFactoryTest.GetViewModel<PersonaViewModel>(responseCreate);
        var responseUpdate = await factory.Update(Guid.NewGuid(), name);
        // assert
        responseCreate.StatusCode.Should().Be(HttpStatusCode.Created);
        responseUpdate.StatusCode.Should().Be(HttpStatusCode.NoContent);
    }

    [Fact]
    public async Task When_Delete_NewId_Then_NoContent()
    {
        // act
        var responseUpdate = await factory.Delete(Guid.NewGuid());
        // assert            
        responseUpdate.StatusCode.Should().Be(HttpStatusCode.NoContent);
    }

    [Fact]
    public async Task When_Delete_EmptyId_Then_BadRequest()
    {
        // act
        var responseUpdate = await factory.Delete(Guid.Empty);
        // assert            
        responseUpdate.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }
}