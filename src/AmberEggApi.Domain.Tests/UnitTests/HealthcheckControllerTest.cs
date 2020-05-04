using AmberEggApi.Domain.Tests.Factories;
using Autofac;
using FluentAssertions;
using NUnit.Framework;
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
        public async Task WhenCreate_Then_ICanFindItById()
        {
            //arrange
            var message = "HealthCheck Status OK";
            //act            
            var response = await factory.Get();
            //assert
            response.Should().Be(message);            
        }
    }
}