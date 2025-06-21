using AmberEggApi.DomainTests.Factories;
using Autofac;
using FluentAssertions;
using System.Threading.Tasks;
using Xunit;

namespace AmberEggApi.DomainTests.Tests
{
    [Collection("Domain.Tests.Global.Setup")]
    public class HealthCheckControllerTest
    {
        private readonly HealthCheckControllerFactory factory;

        public HealthCheckControllerTest()
        {
            factory = SetupTests.Container.Resolve<HealthCheckControllerFactory>();
        }

        [Fact]
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