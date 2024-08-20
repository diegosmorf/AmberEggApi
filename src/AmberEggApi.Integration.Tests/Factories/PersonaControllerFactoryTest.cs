using AmberEggApi.ApplicationService.ViewModels;
using AmberEggApi.Domain.Commands;
using FluentAssertions;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace AmberEggApi.Integration.Tests.Factories
{
    public class PersonaControllerFactoryTest(HttpClient client) : IIntegrationFactoryTest
    {
        private const string url = "/api/v1/Persona";
        private int index = 1;

        public async Task<PersonaViewModel> Create()
        {
            var name = $"Persona Test {index++}";

            //Act
            var viewModel = await Create(new CreatePersonaCommand(name));

            // Assert
            viewModel.Should().BeOfType<PersonaViewModel>();

            viewModel.Id.Should().NotBeEmpty();
            viewModel.Name.Should().Be(name);

            return viewModel;
        }

        public async Task<PersonaViewModel> Create(CreatePersonaCommand command)
        {
            // Act
            var response = await CreateRequest(command);

            // Assert
            var apiResponse = JsonConvert.DeserializeObject<PersonaViewModel>(await response.Content.ReadAsStringAsync());

            response.StatusCode.Should().Be(HttpStatusCode.Created);

            return apiResponse;
        }

        public async Task<HttpResponseMessage> CreateRequest(CreatePersonaCommand command)
        {
            // Arrange
            var requestBody =
                new StringContent(JsonConvert.SerializeObject(command), Encoding.UTF8, "application/json");

            // Act
            return await client.PostAsync(url, requestBody);
        }

        public async Task<HttpResponseMessage> Delete(Guid id)
        {
            return await client.DeleteAsync($"{url}/{id}");            
        }

        public async Task<PersonaViewModel> Get(Guid id)
        {
            await Task.Delay(2);
            // Act
            var response = await client.GetAsync($"{url}/{id}");

            if (response.StatusCode == HttpStatusCode.NoContent)
            {
                return null;
            }

            var responseModel = JsonConvert.DeserializeObject<PersonaViewModel>(await response.Content.ReadAsStringAsync());

            return responseModel;
        }

        public async Task<IEnumerable<PersonaViewModel>> GetAll()
        {
            await Task.Delay(2);
            // Act
            var response = await client.GetAsync($"{url}");

            if (response.StatusCode == HttpStatusCode.NoContent)
            {
                return null;
            }

            var responseModel = JsonConvert.DeserializeObject<IEnumerable<PersonaViewModel>>(await response.Content.ReadAsStringAsync());

            return responseModel;
        }

        public async Task<HttpResponseMessage> UpdateRequest(PersonaViewModel viewModel)
        {
            // Arrange

            var command = new UpdatePersonaCommand(viewModel.Id, viewModel.Name);
            var requestBody =
                new StringContent(JsonConvert.SerializeObject(command), Encoding.UTF8, "application/json");

            // Act
            var response = await client.PutAsync($"{url}/{viewModel.Id}", requestBody);
            return response;
        }

        public async Task<PersonaViewModel> Update(PersonaViewModel viewModel)
        {
            // Arrange
            viewModel.Name = $"{viewModel.Name}-{DateTime.UtcNow.ToLongTimeString()}";


            // Act
            var response = await UpdateRequest(viewModel);
            var viewModelResponse = JsonConvert.DeserializeObject<PersonaViewModel>(await response.Content.ReadAsStringAsync());

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            viewModelResponse.Should().BeOfType<PersonaViewModel>();

            viewModelResponse.Id.Should().NotBeEmpty();
            viewModelResponse.Id.Should().Be(viewModel.Id);
            viewModelResponse.Name.Should().Be(viewModel.Name);

            return viewModelResponse;
        }
        public async Task DeleteAll()
        {
            var viewModelGetAll = await GetAll();

            foreach (var viewModel in viewModelGetAll)
            {
                await Delete(viewModel.Id);
            }
        }
    }
}