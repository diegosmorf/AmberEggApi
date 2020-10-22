using AmberEggApi.Domain.Commands;
using Api.Common.Repository.MongoDb.Tests.Factories;
using Autofac;
using FluentAssertions;
using NUnit.Framework;
using System;
using System.Linq;
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
            var commandUpdate = new UpdatePersonaCommand(
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
            var commandUpdate = new UpdatePersonaCommand(
                objCreate.Id,
                expectedNameAfterUpdate);

            await factory.Update(commandUpdate);
            await factory.Delete(objCreate.Id);

            var objGet = await factory.Get(objCreate.Id);

            //assert
            objGet.Should().BeNull();
        }

        [Test]
        public async Task WhenCreateMultiples_Then_DeleteMultiples()
        {
            //arrange
            var expectedInserted = 4;
            var finalResult = 0;

            //act
            await factory.DeleteAll();
            var company1 = await factory.Create();
            var company2 = await factory.Create();
            var company3 = await factory.Create();
            var company4 = await factory.Create();
            
            var list = new [] { company1.Id, company2.Id, company3.Id, company4.Id };
            var currentInserted = (await factory.GetAll()).Count();
            await factory.Delete(list);
            var currentResult = (await factory.GetAll()).Count();
            //assert
            expectedInserted.Should().Be(currentInserted);
            finalResult.Should().Be(currentResult);
        }

        [Test]
        public async Task WhenUpdateMultiples_Then_KeepMultiples()
        {
            //arrange
            var expectedInserted = 4;
            var finalResult = 4;
            var name = "Company Test";

            //act
            await factory.DeleteAll();
            var company1 = await factory.Create();
            var company2 = await factory.Create();
            var company3 = await factory.Create();
            var company4 = await factory.Create();

            var list = new[] { company1, company2, company3, company4 };
            var currentInserted = (await factory.GetListByName(name)).Count();
            await factory.Update(list);
            var currentResult = (await factory.GetListByName(name)).Count();
            //assert
            expectedInserted.Should().Be(currentInserted);
            finalResult.Should().Be(currentResult);
        }
    }
}