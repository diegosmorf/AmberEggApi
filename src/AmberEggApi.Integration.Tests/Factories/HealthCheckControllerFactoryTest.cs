using System.Net.Http;
using System.Threading.Tasks;

namespace AmberEggApi.IntegrationTests.Factories
{
    public class HealthCheckControllerFactoryTest(HttpClient client)
    {
        private const string url = "/api/v1/HealthCheck";
        
        public async Task<HttpResponseMessage> Get()
        {
            return await client.GetAsync($"{url}");
        }
    }
}