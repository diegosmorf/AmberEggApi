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
    public class PersonaControllerFactoryTest : IIntegrationFactoryTest
    {
        private const string url = "/api/v1/Persona";
        private readonly HttpClient client;

        public PersonaControllerFactoryTest(HttpClient client)
        {
            this.client = client;
        }

        public async Task<PersonaViewModel> Create()
        {
            var name = "Persona Test";

            //Act
            // var responseModel = await Create(new CreatePersonaCommand(name));
            // var viewModel =
            //     JsonConvert.DeserializeObject<PersonaViewModel>(responseModel.Result.ToString());

             var viewModel = await Create(new CreatePersonaCommand(name));

            // Assert
            //responseModel.StatusCode.Should().Be((int)HttpStatusCode.Created);
            viewModel.Should().BeOfType<PersonaViewModel>();

            viewModel.Id.Should().NotBeEmpty();
            viewModel.Name.Should().Be(name);

            return viewModel;
        }

        public async Task Delete(Guid id)
        {
            // Act
            var response = await client.DeleteAsync($"{url}/{id}");
            //var responseModel = JsonConvert.DeserializeObject<ApiResponse>(await response.Content.ReadAsStringAsync());

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.NoContent);
            //responseModel.StatusCode.Should().Be((int)HttpStatusCode.NoContent);
            //responseModel.Result.Should().Be("");
        }

        public async Task<PersonaViewModel> Get(Guid id)
        {
            await Task.Delay(2);
            // Act
            var response = await client.GetAsync($"{url}/{id}");

            if(response.StatusCode == HttpStatusCode.NoContent)
            {
                return null;
            }

            var responseModel = JsonConvert.DeserializeObject<PersonaViewModel>(await response.Content.ReadAsStringAsync());

            return responseModel;
        }

        private async Task<PersonaViewModel> Create(CreatePersonaCommand command)
        {
            // Arrange
            var requestBody =
                new StringContent(JsonConvert.SerializeObject(command), Encoding.UTF8, "application/json");

            // Act
            var response = await client.PostAsync(url, requestBody);

            // Assert
            var apiResponse = JsonConvert.DeserializeObject<PersonaViewModel>(await response.Content.ReadAsStringAsync());

            response.StatusCode.Should().Be((int)HttpStatusCode.Created);
            //apiResponse.IsSuccessRequest.Should().BeTrue();
            //apiResponse.Message.Should().Be(HttpStatusCode.Created.ToString());

            return apiResponse;
        }

        public async Task<PersonaViewModel> Update(PersonaViewModel viewModel)
        {
            // Arrange
            viewModel.Name = $"{viewModel.Name}-{DateTime.UtcNow.ToLongTimeString()}";

            var command = new UpdatePersonaCommand(viewModel.Id, viewModel.Name);
            var requestBody =
                new StringContent(JsonConvert.SerializeObject(command), Encoding.UTF8, "application/json");

            // Act
            var response = await client.PutAsync($"{url}/{viewModel.Id}", requestBody);
            // var apiResponse = JsonConvert.DeserializeObject<ApiResponse>(await response.Content.ReadAsStringAsync());
            // var viewModelResponse =
            //     JsonConvert.DeserializeObject<PersonaViewModel>(apiResponse.Result.ToString());

            var viewModelResponse = JsonConvert.DeserializeObject<PersonaViewModel>(await response.Content.ReadAsStringAsync());
            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            //apiResponse.StatusCode.Should().Be((int)HttpStatusCode.OK);
            viewModelResponse.Should().BeOfType<PersonaViewModel>();

            viewModelResponse.Id.Should().NotBeEmpty();
            viewModelResponse.Id.Should().Be(viewModel.Id);
            viewModelResponse.Name.Should().Be(viewModel.Name);

            return viewModelResponse;
        }
    }
}