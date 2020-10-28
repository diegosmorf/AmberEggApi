using AmberEggApi.Domain.Tests.Factories;
using AmberEggApi.WebApi;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using FluentAssertions;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using NUnit.Framework;
using System.IO;
using System.Threading.Tasks;

namespace AmberEggApi.Domain.Tests.UnitTests
{
    [TestFixture]
    public class HealthcheckControllerTest
    {
        private readonly HealthcheckControllerFactory factory;

        public HealthcheckControllerTest()
        {
            factory = SetupTests.Container.Resolve<HealthcheckControllerFactory>();
        }

        [Test]
        public async Task WhenCheck_Then_Success()
        {
            //arrange
            var message = "HealthCheck Status OK";
            //act            
            var response = await factory.Get();
            //assert
            response.Should().Be(message);            
        }        

        [Test]
        public void WhenStartupHost_Then_Success()
        {
            //arrange
            var host = WebHost.CreateDefaultBuilder()
                                .ConfigureServices(s => s.AddAutofac())
                                .UseContentRoot(Directory.GetCurrentDirectory())
                                .UseStartup<Startup>()                                
                                .Build();

            //assert
            host.Should().NotBeNull();
        }

        [Test]
        public void WhenStartupHostViaProgram_Then_Success()
        {
            //act
            var host = Program.CreteWebHost();
            
            //assert
            host.Should().NotBeNull();
        }
    }
}