using AmberEggApi.DomainTests.Factories;
using Autofac;
using FluentAssertions;
using NUnit.Framework;
using System.Threading.Tasks;

namespace AmberEggApi.DomainTests.Tests
{
    public class HealthCheckControllerTest
    {
        private readonly HealthCheckControllerFactory factory;

        public HealthCheckControllerTest()
        {
            factory = SetupTests.Container.Resolve<HealthCheckControllerFactory>();
        }

        [Test]
        public async Task WhenCheck_Then_Success()
        {
            // arrange
            var message = "HealthCheck Status OK";
            // act            
            var response = await factory.Get();
            // assert
            response.Should().Be(message);
        }
    }
}