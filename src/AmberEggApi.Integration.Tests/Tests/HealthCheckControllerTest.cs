using AmberEggApi.IntegrationTests.Factories;
using FluentAssertions;
using System.Net;
using System.Threading.Tasks;
using Xunit;

namespace AmberEggApi.IntegrationTests.Tests
{
    [Collection("Integration.Tests.Global.Setup")]
    public class HealthCheckControllerTest
    {
        private readonly HealthCheckControllerFactoryTest factory;
        public HealthCheckControllerTest()
        {
            factory = new HealthCheckControllerFactoryTest(SetupTests.Client);
        }

        [Fact]
        public async Task WhenCheck_Then_Success()
        {
            // arrange
            var expectedMessage = "HealthCheck Status OK";
            // act            
            var response = await factory.Get();
            var message = await response.Content.ReadAsStringAsync();
            message = message.Replace("\"", string.Empty);
            // assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            message.Should().NotBeNullOrEmpty();
            message.Should().Be(expectedMessage);
        }
    }
}