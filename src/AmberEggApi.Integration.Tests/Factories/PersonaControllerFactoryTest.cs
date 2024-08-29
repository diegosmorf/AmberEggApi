using AmberEggApi.ApplicationService.ViewModels;
using AmberEggApi.Domain.Commands;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace AmberEggApi.IntegrationTests.Factories
{
    public class PersonaControllerFactoryTest(HttpClient client)
    {
        private const string url = "/api/v1/Persona";

        public async Task<HttpResponseMessage> Create(string name)
        {
            var requestBody = ParseToJson(new CreatePersonaCommand(name));
            return await client.PostAsync(url, requestBody);
        }

        public static async Task<TViewModel> GetViewModel<TViewModel>(HttpResponseMessage response)
        {
            return JsonConvert.DeserializeObject<TViewModel>(await response.Content.ReadAsStringAsync());
        }
        public static StringContent ParseToJson(object command)
        {
            return new StringContent(JsonConvert.SerializeObject(command), Encoding.UTF8, "application/json");
        }
        public async Task<HttpResponseMessage> Get(string name)
        {
            return await client.GetAsync($"{url}/name/{name}");
        }

        public async Task<HttpResponseMessage> Get(Guid id)
        {
            return await client.GetAsync($"{url}/{id}");
        }
        public async Task<HttpResponseMessage> GetAll()
        {
            return await client.GetAsync($"{url}");
        }

        public async Task<HttpResponseMessage> Delete(Guid id)
        {
            return await client.DeleteAsync($"{url}/{id}");
        }
        public async Task<HttpResponseMessage> Update(Guid id, string name)
        {
            var requestBody = ParseToJson(new UpdatePersonaCommand(id, name));
            return await client.PutAsync($"{url}", requestBody);
        }
        public async Task DeleteAll()
        {
            var responseGet = await GetAll();

            if (responseGet.StatusCode == HttpStatusCode.OK)
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