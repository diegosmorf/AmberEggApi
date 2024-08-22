using AmberEggApi.ApplicationService.ViewModels;
using AmberEggApi.Domain.Commands;
using FluentAssertions;
using Newtonsoft.Json;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace AmberEggApi.Integration.Tests.IntegrationTests
{
    public class PersonaControllerTest222
    {
        private readonly HttpClient client;
        private const string url = "/api/v1/Persona";
        private int index = 1;

        public PersonaControllerTest222()
        {
            client = BaseIntegrationTest.Client;
        }
        
        [TestCase("")]  
        [TestCase(null)]
        [TestCase(" ")]      
        [TestCase("1")]
        [TestCase("Persona-Test-Invalid-Name-1234567890")] 
        public async Task When_Empty_Create_Then_BadRequest(string name)
        {
            // act
            var response = await Create(name);
            // assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [TestCase("P")]
        [TestCase("Persona-Test 1")]
        [TestCase("Persona-Test 10")]
        [TestCase("Persona-Test 100")]
        [TestCase("Persona-Test 1000")]        
        public async Task When_Valid_Create_Then_Success(string name)
        {
            // arrange
            var expectedName = $"{name}-{index++}";
            // act            
            var response = await Create(expectedName);
            var viewModel = await GetViewModel<PersonaViewModel>(response);
            // assert
            response.StatusCode.Should().Be(HttpStatusCode.Created);
            viewModel.Should().BeOfType<PersonaViewModel>();
            viewModel.Id.Should().NotBeEmpty();
            viewModel.Name.Should().NotBeNullOrEmpty();
            viewModel.Name.Should().Be(expectedName);
        }

        [TestCase("P")]
        [TestCase("Persona-Test 1")]
        [TestCase("Persona-Test 10")]
        [TestCase("Persona-Test 100")]
        [TestCase("Persona-Test 1000")]          
        public async Task When_Valid_Create_GetById_Then_Success(string name)
        {
            // arrange
            var expectedName = $"{name}-{index++}";
            // act
            var responseCreate = await Create(expectedName);
            var viewModelCreate = await GetViewModel<PersonaViewModel>(responseCreate);

            var responseGet = await Get(viewModelCreate.Id);
            var viewModelGet = await GetViewModel<PersonaViewModel>(responseGet);
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

        [TestCase("P")]
        [TestCase("Persona-Test 1")]
        [TestCase("Persona-Test 10")]
        [TestCase("Persona-Test 100")]
        [TestCase("Persona-Test 1000")]           
        public async Task When_Valid_Create_GetAll_Then_Success(string name)
        {
            // arrange
            var expectedName = $"{name}-{index++}";
            // act
            var responseCreate = await Create(expectedName);
            var viewModelCreate = await GetViewModel<PersonaViewModel>(responseCreate);

            var responseGet = await GetAll();
            var viewModelGet = (await GetViewModel<IEnumerable<PersonaViewModel>>(responseGet)).FirstOrDefault(x=> x.Id == viewModelCreate.Id);

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
        
        [Test]
        public async Task When_EmptyDatabase_GetAll_Then_NoContent()
        {
            // act            
            await DeleteAll();
            var response = await GetAll();            

            // assert            
            response.StatusCode.Should().Be(HttpStatusCode.NoContent);
        }

        [TestCase("P")]
        [TestCase("Persona-Test 1")]
        [TestCase("Persona-Test 10")]
        [TestCase("Persona-Test 100")]
        [TestCase("Persona-Test 1000")]  
        public async Task When_Valid_Create_GetByName_Then_Success(string name)
        {
            // arrange
            var expectedName = $"{name}-{index++}";
            // act
            var responseCreate = await Create(expectedName);
            var viewModelCreate = await GetViewModel<PersonaViewModel>(responseCreate);

            var responseGet = await Get(viewModelCreate.Name);
            var viewModelGet = (await GetViewModel<IEnumerable<PersonaViewModel>>(responseGet)).FirstOrDefault();

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

        [TestCase("P")]
        [TestCase("Persona-Test 1")]
        public async Task When_Valid_Create_Update_Then_Success(string name)
        {
            // arrange
            var expectedName = $"{name}-{index++}";
            // act
            var responseCreate = await Create(expectedName);
            var viewModelCreate = await GetViewModel<PersonaViewModel>(responseCreate);            
            var responseUpdate = await Update(viewModelCreate.Id,  $"{expectedName}-v2");
            var viewModelUpdate = await GetViewModel<PersonaViewModel>(responseUpdate);
            var responseGet = await Get(viewModelCreate.Id);
            var viewModelGet = await GetViewModel<PersonaViewModel>(responseGet);
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

        [TestCase("P")]
        [TestCase("Persona-Test 1")]
        public async Task When_Valid_Create_Update_Delete_Then_Success(string name)
        {
            // arrange
            var expectedName = $"{name}-{index++}";
            // act
            var responseCreate = await Create(expectedName);
            var viewModelCreate = await GetViewModel<PersonaViewModel>(responseCreate);            
            var responseUpdate = await Update(viewModelCreate.Id,  $"{expectedName}-v2");            
            var responseDelete = await Delete(viewModelCreate.Id);
            var responseGet = await Get(viewModelCreate.Id);
            
            // assert
            responseCreate.StatusCode.Should().Be(HttpStatusCode.Created);            
            responseUpdate.StatusCode.Should().Be(HttpStatusCode.OK);
            responseDelete.StatusCode.Should().Be(HttpStatusCode.NoContent);
            responseGet.StatusCode.Should().Be(HttpStatusCode.NoContent);                                
        }

        [TestCase("")]
        [TestCase(" ")]
        [TestCase(null)]        
        [TestCase("Persona-Test-Invalid-Name-1234567890")]                  
        public async Task When_Invalid_Create_Update_Then_BadRequest(string invalidName)
        {
            // arrange
            var name = $"Persona-Test-{index++}";
            // act
            var responseCreate = await Create(name);
            var viewModelCreate = await GetViewModel<PersonaViewModel>(responseCreate);            
            var responseUpdate = await Update(viewModelCreate.Id, invalidName);            
            // assert
            responseCreate.StatusCode.Should().Be(HttpStatusCode.Created);            
            responseUpdate.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            viewModelCreate.Should().BeOfType<PersonaViewModel>();
            viewModelCreate.Id.Should().NotBeEmpty();
            viewModelCreate.Name.Should().NotBeNullOrEmpty();                                            
        }

        [Test]
        public async Task When_Create_Update_EmptyId_Then_BadRequest()
        {
            // arrange
            var name = $"Persona-Test-{index++}";
            // act
            var responseCreate = await Create(name);
            var viewModelCreate = await GetViewModel<PersonaViewModel>(responseCreate);            
            var responseUpdate = await Update(Guid.Empty, name);            
            // assert
            responseCreate.StatusCode.Should().Be(HttpStatusCode.Created);            
            responseUpdate.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            viewModelCreate.Should().BeOfType<PersonaViewModel>();
            viewModelCreate.Id.Should().NotBeEmpty();
            viewModelCreate.Name.Should().NotBeNullOrEmpty();                                            
        }

        [Test]
        public async Task When_Create_Update_NewId_Then_NoContent()
        {
            // arrange
            var name = $"Persona-Test-{index++}";
            // act
            var responseCreate = await Create(name);
            var viewModelCreate = await GetViewModel<PersonaViewModel>(responseCreate);            
            var responseUpdate = await Update(Guid.NewGuid(), name);            
            // assert
            responseCreate.StatusCode.Should().Be(HttpStatusCode.Created);            
            responseUpdate.StatusCode.Should().Be(HttpStatusCode.NoContent);
            viewModelCreate.Should().BeOfType<PersonaViewModel>();
            viewModelCreate.Id.Should().NotBeEmpty();
            viewModelCreate.Name.Should().NotBeNullOrEmpty();                                            
        }

        private async Task<HttpResponseMessage> Create(string name)
        {
            var requestBody = ParseToJson(new CreatePersonaCommand(name));
            return await client.PostAsync(url, requestBody);
        }

        private static async Task<TViewModel> GetViewModel<TViewModel>(HttpResponseMessage response)
        {
            return JsonConvert.DeserializeObject<TViewModel>(await response.Content.ReadAsStringAsync());
        }

        private static StringContent ParseToJson(object command)
        {
            return new StringContent(JsonConvert.SerializeObject(command), Encoding.UTF8, "application/json");
        }
        private async Task<HttpResponseMessage> Get(Guid id)
        {
            return await client.GetAsync($"{url}/{id}");
        }

        private async Task<HttpResponseMessage> GetAll()
        {            
            return await client.GetAsync($"{url}");
        }

        private async Task<HttpResponseMessage> Get(string name)
        {
            return await client.GetAsync($"{url}/name/{name}");
        }

        private async Task<HttpResponseMessage> Delete(Guid id)
        {
            return await client.DeleteAsync($"{url}/{id}");            
        }

        public async Task<HttpResponseMessage> Update(Guid id, string name)
        {
            var requestBody = ParseToJson(new UpdatePersonaCommand(id,name));            
            return await client.PutAsync($"{url}/{id}", requestBody);            
        }

        private async Task DeleteAll()
        {
            var responseGet = await GetAll();

            if(responseGet.StatusCode == HttpStatusCode.OK)
            {
                var listViewGet = await GetViewModel<IEnumerable<PersonaViewModel>>(responseGet);

                foreach (var viewModel in listViewGet)
                {
                    await Delete(viewModel.Id);
                }
            }
        }
    }
}