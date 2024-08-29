using AmberEggApi.IntegrationTests.Server;
using Autofac.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using NUnit.Framework;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;

namespace AmberEggApi.IntegrationTests.Tests
{
    [SetUpFixture]
    internal class BaseIntegrationTest
    {
        private static TestServer apiServer;
        public static HttpClient Client { get; private set; }

        [OneTimeSetUp]
        public void RunBeforeAllTests()
        {
            // Setup API SERVER
            apiServer = new TestServer(new WebHostBuilder()
                .ConfigureServices(s => s.AddAutofac())
                .UseContentRoot(Directory.GetCurrentDirectory())
                .UseStartup<StartupIntegrationTest>());

            Client = apiServer.CreateClient();
            Client.DefaultRequestHeaders.Accept.Clear();
            Client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        [OneTimeTearDown]
        public void RunAfterAllTests()
        {
            Client.Dispose();
            apiServer.Dispose();
        }
    }
}