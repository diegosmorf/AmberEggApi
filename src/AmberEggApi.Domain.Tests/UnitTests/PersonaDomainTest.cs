using AmberEggApi.Domain.Commands;
using AmberEggApi.Domain.Tests.Factories;
using Api.Common.Repository.Exceptions;
using Autofac;
using FluentAssertions;
using NUnit.Framework;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace AmberEggApi.Domain.Tests.UnitTests
{
    [TestFixture]
    public class PersonaDomainTest
    {
        private readonly PersonaAppServiceFactory factory;
        private int index = 1;

        public PersonaDomainTest()
        {
            factory = SetupTests.Container.Resolve<PersonaAppServiceFactory>();
        }

        [TestCase("P")]
        [TestCase("Persona-Test 1")]
        [TestCase("Persona-Test 10")]
        [TestCase("Persona-Test 100")]
        [TestCase("Persona-Test 1000")]
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

        [TestCase("P")]
        [TestCase("Persona")]
        [TestCase("Persona-Test")]
        [TestCase("Persona-Test 1")]
        public async Task WhenCreateAndUpdate_Then_FindItById(string name)
        {
            // arrange
            var expectedName = $"{name}-{index++}";
            var expectedNameAfterUpdate = $"{expectedName}-v2";
            // act
            var responseCreate = await factory.Create(expectedName);
            var commandUpdate = new UpdatePersonaCommand(responseCreate.Id,expectedNameAfterUpdate);

            var responseUpdate = await factory.Update(commandUpdate);
            var responseSearchById = await factory.Get(responseCreate.Id);

            // assert
            responseSearchById.Id.Should().Be(responseUpdate.Id);
            responseSearchById.Name.Should().Be(responseUpdate.Name);
        }

        [TestCase("P")]
        [TestCase("Persona")]
        [TestCase("Persona-Test")]
        [TestCase("Persona-Test 1")]
        public async Task WhenCreateAndUpdateAndDelete_Then_Success(string name)
        {
            // arrange
            var expectedName = $"{name}-{index++}";
            var expectedNameAfterUpdate = $"{expectedName}-v2";
            // act
            var responseCreate = await factory.Create(expectedName);
            var commandUpdate = new UpdatePersonaCommand(responseCreate.Id, expectedNameAfterUpdate);
            await factory.Update(commandUpdate);
            await factory.Delete(responseCreate.Id);
            var responseSearchById = await factory.Get(responseCreate.Id);
            // assert
            responseSearchById.Should().BeNull();
        }

        [TestCase("")]
        [TestCase(null)]
        [TestCase(" ")]
        [TestCase("1")]
        [TestCase("Persona-Test-Invalid-Name-1234567890")]
        public void WhenCreateNotValidEntity_Then_Error(string name)
        {
            // arrange
            var expectedNumberOfErrors = 1;
            var expectedMessage = "This object instance is not valid based on DataAnnotation definitions. See more details on Errors list.";
            var command = new CreatePersonaCommand(name);
            // act
            Func<Task> action = async () => { await factory.Create(command); };
            // assert
            action.Should().ThrowAsync<ModelException>().Where(x => x.Errors.Count() == expectedNumberOfErrors);            
            action.Should().ThrowAsync<ModelException>().WithMessage(expectedMessage);
        }
    }
}