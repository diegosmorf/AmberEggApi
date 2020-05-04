using AmberEggApi.Domain.Commands;
using Api.Common.Repository.MongoDb.Tests.Factories;
using Autofac;
using FluentAssertions;
using NUnit.Framework;
using System;
using System.Threading.Tasks;

namespace Api.Common.Repository.MongoDb.Tests.UnitTests
{
    [TestFixture]
    public class CompanyDomainTest
    {
        private readonly CompanyRepositoryFactory factory;

        public CompanyDomainTest()
        {
            factory = SetupTests.Container.Resolve<CompanyRepositoryFactory>();
        }

        [Test]
        public async Task WhenCreate_Then_ICanFindItById()
        {
            //act
            var objCreate = await factory.Create();
            var objGet = await factory.Get(objCreate.Id);

            //assert
            objGet.Id.Should().Be(objCreate.Id);
            objGet.Name.Should().Be(objCreate.Name);
        }

        [Test]
        public async Task WhenCreateAndUpdate_Then_ICanFindItById()
        {
            //arrange
            var expectedNameAfterUpdate =
                $"AfterUpdate-Company-Test-{DateTime.UtcNow.ToLongTimeString()}";

            //act
            var objCreate = await factory.Create();
            var commandUpdate = new UpdateCompanyCommand(
                objCreate.Id,
                expectedNameAfterUpdate);

            var responseUpdate = await factory.Update(commandUpdate);
            var objGet = await factory.Get(objCreate.Id);

            //assert
            objGet.Id.Should().Be(responseUpdate.Id);
            objGet.Name.Should().Be(responseUpdate.Name);
        }

        [Test]
        public async Task WhenCreateAndUpdateAndDelete_Then_Success()
        {
            //arrange
            var expectedNameAfterUpdate =
                $"AfterUpdate-Company-Test-{DateTime.UtcNow.ToLongTimeString()}";

            //act
            var objCreate = await factory.Create();
            var commandUpdate = new UpdateCompanyCommand(
                objCreate.Id,
                expectedNameAfterUpdate);

            await factory.Update(commandUpdate);
            await factory.Delete(objCreate.Id);

            var objGet = await factory.Get(objCreate.Id);

            //assert
            objGet.Should().BeNull();
        }
    }
}