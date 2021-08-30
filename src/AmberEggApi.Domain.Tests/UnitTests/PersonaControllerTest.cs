using AmberEggApi.Domain.Commands;
using AmberEggApi.Domain.Tests.Factories;
using Api.Common.Repository.Exceptions;
using Autofac;
using FluentAssertions;
using NUnit.Framework;
using System.Net;

namespace AmberEggApi.Domain.Tests.UnitTests
{
    [TestFixture]
    public class PersonaControllerTest
    {
        private readonly PersonaControllerFactory factory;

        public PersonaControllerTest()
        {
            factory = SetupTests.Container.Resolve<PersonaControllerFactory>();
        }

        [Test]
        public async Task WhenCreate_Then_ICanFindItById()
        {
            //act
            var responseCreate = await factory.Create();
            var responseSearchById = await factory.Get(responseCreate.Id);

            //assert
            responseSearchById.Id.Should().Be(responseCreate.Id);
            responseSearchById.Name.Should().Be(responseCreate.Name);
        }

        [Test]
        public async Task WhenCreateAndUpdate_Then_ICanFindItById()
        {
            //arrange
            var expectedNameAfterUpdate =
                $"AfterUpdate-Persona-Test-{DateTime.UtcNow.ToLongTimeString()}";

            //act
            var responseCreate = await factory.Create();
            var commandUpdate = new UpdatePersonaCommand(
                responseCreate.Id,
                expectedNameAfterUpdate);

            var responseUpdate = await factory.Update(commandUpdate);
            var responseSearchById = await factory.Get(responseCreate.Id);

            //assert
            responseSearchById.Id.Should().Be(responseUpdate.Id);
            responseSearchById.Name.Should().Be(responseUpdate.Name);
        }

        [Test]
        public async Task WhenCreateAndUpdateAndDelete_Then_Success()
        {
            //arrange
            var expectedNameAfterUpdate =
                $"AfterUpdate-Persona-Test-{DateTime.UtcNow.ToLongTimeString()}";

            //act
            var responseCreate = await factory.Create();
            var commandUpdate = new UpdatePersonaCommand(
                responseCreate.Id,
                expectedNameAfterUpdate);

            await factory.Update(commandUpdate);
            await factory.Delete(responseCreate.Id);

            var responseSearchById = await factory.Get(responseCreate.Id);

            //assert
            responseSearchById.Should().BeNull();
        }

        [Test]
        public void WhenCreateNotValidEntity_Then_Error()
        {
            //arrange
            var expectedNumberOfErrors = 1;

            var name = string.Empty;
            var command = new CreatePersonaCommand(name);

            //act
            Func<Task> action = async () => { await factory.Create(command); };

            //assert
            action.Should()
                .ThrowAsync<ModelException>()
                .Where(x => x.Errors.Count() == expectedNumberOfErrors);

            action.Should()
                .ThrowAsync<ModelException>()
                .WithMessage(
                    "This object instance is not valid based on DataAnnotation definitions. See more details on Errors list.");
        }

        [Test]
        public async Task WhenGetNotExistent_Then_NotFound()
        {
            var response = await factory.GetNotExistent();

            //assert
            response.StatusCode.Should().Be((int)HttpStatusCode.NoContent);
        }

        [Test]
        public async Task WhenGetNameNotExistent_Then_NotFound()
        {
            var response = await factory.GetByNameNotExistent();

            //assert
            response.StatusCode.Should().Be((int)HttpStatusCode.NoContent);
        }

        [Test]
        public async Task WhenUpdateNotExistent_Then_NotFound()
        {
            var response = await factory.UpdateNotExistent();

            //assert
            response.StatusCode.Should().Be((int)HttpStatusCode.NoContent);
        }
    }
}