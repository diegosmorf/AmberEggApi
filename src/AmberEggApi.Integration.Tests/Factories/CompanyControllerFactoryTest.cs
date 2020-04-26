using AmberEggApi.ApplicationService.ViewModels;
using AmberEggApi.Domain.Commands;
using Api.Common.WebServer.Server;
using FluentAssertions;
using Newtonsoft.Json;
using System;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace AmberEggApi.Integration.Tests.Factories
{
    public class CompanyControllerFactoryTest : IIntegrationFactoryTest
    {
        private const string url = "/api/v1/Company";
        private readonly HttpClient client;

        public CompanyControllerFactoryTest(HttpClient client)
        {
            this.client = client;
        }

        public async Task<CompanyViewModel> Create()
        {
            var name = "Company Test";

            //Act
            var responseModel = await Create(new CreateCompanyCommand(name));
            var viewModel =
                JsonConvert.DeserializeObject<CompanyViewModel>(responseModel.Result.ToString());

            // Assert
            responseModel.StatusCode.Should().Be((int)HttpStatusCode.OK);
            viewModel.Should().BeOfType<CompanyViewModel>();

            viewModel.Id.Should().NotBeEmpty();
            viewModel.Name.Should().Be(name);

            return viewModel;
        }

        public async Task Delete(Guid id)
        {
            // Act
            var response = await client.DeleteAsync($"{url}/{id}");
            var responseModel = JsonConvert.DeserializeObject<ApiResponse>(await response.Content.ReadAsStringAsync());

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            responseModel.StatusCode.Should().Be((int)HttpStatusCode.OK);
            responseModel.Result.Should().Be("");
        }

        public async Task<ApiResponse> Get(Guid id)
        {
            // Act
            var response = await client.GetAsync($"{url}/{id}");
            var responseModel = JsonConvert.DeserializeObject<ApiResponse>(await response.Content.ReadAsStringAsync());

            return responseModel;
        }

        private async Task<ApiResponse> Create(CreateCompanyCommand command)
        {
            // Arrange
            var requestBody =
                new StringContent(JsonConvert.SerializeObject(command), Encoding.UTF8, "application/json");

            // Act
            var response = await client.PostAsync(url, requestBody);

            // Assert
            var apiResponse = JsonConvert.DeserializeObject<ApiResponse>(await response.Content.ReadAsStringAsync());

            apiResponse.StatusCode.Should().Be((int)HttpStatusCode.OK);
            apiResponse.IsOk.Should().BeTrue();
            apiResponse.Message.Should().Be(HttpStatusCode.OK.ToString());

            return apiResponse;
        }

        public async Task<CompanyViewModel> Update(CompanyViewModel viewModel)
        {
            // Arrange
            viewModel.Name = $"{viewModel.Name}-{DateTime.UtcNow.ToLongTimeString()}";

            var command = new UpdateCompanyCommand(viewModel.Id, viewModel.Name);
            var requestBody =
                new StringContent(JsonConvert.SerializeObject(command), Encoding.UTF8, "application/json");

            // Act
            var response = await client.PutAsync($"{url}/{viewModel.Id}", requestBody);
            var apiResponse = JsonConvert.DeserializeObject<ApiResponse>(await response.Content.ReadAsStringAsync());
            var viewModelResponse =
                JsonConvert.DeserializeObject<CompanyViewModel>(apiResponse.Result.ToString());

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            apiResponse.StatusCode.Should().Be((int)HttpStatusCode.OK);
            viewModelResponse.Should().BeOfType<CompanyViewModel>();

            viewModelResponse.Id.Should().NotBeEmpty();
            viewModelResponse.Id.Should().Be(viewModel.Id);
            viewModelResponse.Name.Should().Be(viewModel.Name);

            return viewModelResponse;
        }
    }
}