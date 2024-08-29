using AmberEggApi.WebApi;
using Autofac.Extensions.DependencyInjection;
using FluentAssertions;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.AspNetCore.TestHost;
using NUnit.Framework;
using System.IO;
using System.Net;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace AmberEggApi.IntegrationTests.Tests
{
    public class ProgramTest 
    {
        [Test]
        public async Task WhenStartupHost_Then_Success()
        {
            // Arrange            
            var server = new TestServer(new WebHostBuilder()
                .UseEnvironment("Development")
                .ConfigureServices(s => s.AddAutofac())
                .UseContentRoot(Directory.GetCurrentDirectory())
                .UseStartup<Startup>());

            var client = server.CreateClient();
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            
            // Act
            var response = await client.GetAsync("/api/v1/HealthCheck");
            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }        

        [Test]
        public async Task WhenStartupHostViaProgram_Then_Success()
        {
            // act
            var host = Program.CreteWebHost();
            await host.StartAsync();
            host.Services.GetService(typeof(IServer)).Should().NotBeNull();
            await host.StopAsync();
            // assert            
            host.Should().NotBeNull();
        }
    }
}