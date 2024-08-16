using AmberEggApi.ApplicationService.ViewModels;
using AmberEggApi.Domain.Commands;
using AmberEggApi.Integration.Tests.Factories;
using FluentAssertions;
using NUnit.Framework;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace AmberEggApi.Integration.Tests.IntegrationTests
{
    public class PersonaControllerTest
    {
        private readonly PersonaControllerFactoryTest factory;

        public PersonaControllerTest()
        {
            factory = new PersonaControllerFactoryTest(BaseIntegrationTest.Client);
        }

        [Test]
        public async Task WhenCreate_Then_FindItById()
        {
            // Act
            var viewModelCreate = await factory.Create();
            var viewModelGet = await factory.Get(viewModelCreate.Id);

            // Assert            
            viewModelGet.Should().BeOfType<PersonaViewModel>();
            viewModelGet.Id.Should().NotBeEmpty();
            viewModelGet.Id.Should().Be(viewModelCreate.Id);
            viewModelGet.Name.Should().Be(viewModelCreate.Name);
        }

        [Test]
        public async Task WhenCreate_Then_GetAll()
        {
            // Act
            var viewModelCreate = await factory.Create();
            var viewModelGetAll = await factory.GetAll();
            var viewModelGet = viewModelGetAll.FirstOrDefault(x => x.Id == viewModelCreate.Id);

            // Assert            
            viewModelGet.Should().BeOfType<PersonaViewModel>();
            viewModelGet.Id.Should().NotBeEmpty();
            viewModelGet.Id.Should().Be(viewModelCreate.Id);
            viewModelGet.Name.Should().Be(viewModelCreate.Name);
        }

        [Test]
        public async Task WhenEmptyDatabase_Then_NoContent()
        {
            // Act            
            await factory.DeleteAll();
            var viewModelGetAll = await factory.GetAll();

            // Assert            
            viewModelGetAll.Should().BeNull();
        }

        [Test]
        public async Task WhenCreateInvalidModel_Then_BadRequest()
        {
            // Act   
            var response = await factory.CreateRequest(new CreatePersonaCommand(null));

            // Assert            
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Test]
        public async Task WhenCreateAndUpdateInvalidData_Then_BadRequest()
        {
            // Act
            var viewModelCreate = await factory.Create();
            viewModelCreate.Name = null;
            var response = await factory.UpdateRequest(viewModelCreate);

            // Assert            
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Test]
        public async Task WhenCreateAndDelete_Then_Deleted()
        {
            // Act
            var viewModelCreate = await factory.Create();
            await factory.Delete(viewModelCreate.Id);

            var responseGet = await factory.Get(viewModelCreate.Id);

            // Assert
            responseGet.Should().BeNull();
        }

        [Test]
        public async Task WhenCreateAndUpdate_Then_FindItById()
        {
            // Act
            var viewModelCreate = await factory.Create();
            var viewModelUpdate = await factory.Update(viewModelCreate);

            var viewModelGet = await factory.Get(viewModelCreate.Id);

            // Assert            
            viewModelGet.Should().BeOfType<PersonaViewModel>();
            viewModelGet.Id.Should().Be(viewModelUpdate.Id);
            viewModelGet.Name.Should().Be(viewModelUpdate.Name);
        }

        [Test]
        public async Task WhenCreateAndUpdateAndDelete_Then_Success()
        {
            // Act
            var viewModelCreate = await factory.Create();
            var viewModelUpdate = await factory.Update(viewModelCreate);

            var viewModelGet = await factory.Get(viewModelCreate.Id);

            await factory.Delete(viewModelCreate.Id);
            var responseGetAfterDelete = await factory.Get(viewModelCreate.Id);

            // Assert
            viewModelGet.Id.Should().Be(viewModelUpdate.Id);
            viewModelGet.Name.Should().Be(viewModelUpdate.Name);

            responseGetAfterDelete.Should().BeNull();
        }

    }
}