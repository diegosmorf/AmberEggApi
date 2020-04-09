using AmberEggApi.ApplicationService.ViewModels;
using AmberEggApi.Integration.Tests.Factories;
using FluentAssertions;
using Newtonsoft.Json;
using NUnit.Framework;
using System.Net;
using System.Threading.Tasks;

namespace AmberEggApi.Integration.Tests.IntegrationTests
{
    public class CompanyControllerTest
    {
        private readonly CompanyControllerFactoryTest companyFactory;

        public CompanyControllerTest()
        {
            companyFactory = new CompanyControllerFactoryTest(SetupIntegrationTests.Client);
        }

        [Test]
        public async Task WhenCreate_Then_ICanFindItById()
        {
            // Act
            var viewModelCreate = await companyFactory.Create();
            var responseGet = await companyFactory.Get(viewModelCreate.Id);

            var viewModelGet =
                JsonConvert.DeserializeObject<CompanyViewModel>(responseGet.Result.ToString());

            // Assert
            responseGet.StatusCode.Should().Be((int)HttpStatusCode.OK);
            viewModelGet.Should().BeOfType<CompanyViewModel>();
            viewModelGet.Id.Should().NotBeEmpty();
            viewModelGet.Id.Should().Be(viewModelCreate.Id);
            viewModelGet.Name.Should().Be(viewModelCreate.Name);
        }

        [Test]
        public async Task WhenCreateAndDelete_Then_Deleted()
        {
            // Act
            var viewModelCreate = await companyFactory.Create();
            await companyFactory.Delete(viewModelCreate.Id);
            var responseGet = await companyFactory.Get(viewModelCreate.Id);

            // Assert
            responseGet.StatusCode.Should().Be((int)HttpStatusCode.NoContent);
        }

        [Test]
        public async Task WhenCreateAndUpdate_Then_ICanFindItById()
        {
            // Act
            var viewModelCreate = await companyFactory.Create();
            var viewModelUpdate = await companyFactory.Update(viewModelCreate);
            var responseGet = await companyFactory.Get(viewModelCreate.Id);
            var viewModelGet =
                JsonConvert.DeserializeObject<CompanyViewModel>(responseGet.Result.ToString());

            // Assert
            responseGet.StatusCode.Should().Be((int)HttpStatusCode.OK);
            viewModelGet.Should().BeOfType<CompanyViewModel>();
            viewModelGet.Id.Should().Be(viewModelUpdate.Id);
            viewModelGet.Name.Should().Be(viewModelUpdate.Name);
        }

        [Test]
        public async Task WhenCreateAndUpdateAndDelete_Then_Success()
        {
            // Act
            var viewModelCreate = await companyFactory.Create();
            var viewModelUpdate = await companyFactory.Update(viewModelCreate);

            var responseGet = await companyFactory.Get(viewModelCreate.Id);
            var viewModelGet =
                JsonConvert.DeserializeObject<CompanyViewModel>(responseGet.Result.ToString());

            await companyFactory.Delete(viewModelCreate.Id);
            var responseGetAfterDelete = await companyFactory.Get(viewModelCreate.Id);

            // Assert
            viewModelGet.Id.Should().Be(viewModelUpdate.Id);
            viewModelGet.Name.Should().Be(viewModelUpdate.Name);

            responseGetAfterDelete.StatusCode.Should().Be((int)HttpStatusCode.NoContent);
        }
    }
}