using AmberEggApi.IntegrationTests.Factories;
using FluentAssertions;
using NUnit.Framework;
using System.Net;
using System.Threading.Tasks;

namespace AmberEggApi.IntegrationTests.Tests
{
    public class HealthCheckControllerTest
    {
        private readonly HealthCheckControllerFactoryTest factory;
        public HealthCheckControllerTest()
        {
            factory = new HealthCheckControllerFactoryTest(BaseIntegrationTest.Client);
        }

        [Test]
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