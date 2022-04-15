using AmberEggApi.Domain.Commands;
using AmberEggApi.Domain.Models;
using Api.Common.Repository.EFCore.Tests.Factories;
using Api.Common.Repository.Repositories;
using Autofac;
using FluentAssertions;
using NUnit.Framework;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Api.Common.Repository.EFCore.Tests.UnitTests
{
    [TestFixture]
    public class PersonaDomainTest
    {
        private readonly IRepository<Persona> repository;
        private readonly PersonaRepositoryFactory factory;

        public PersonaDomainTest()
        {
            repository = SetupTests.Container.Resolve<IRepository<Persona>>();
            factory = SetupTests.Container.Resolve<PersonaRepositoryFactory>();
        }

        [Test]
        public async Task WhenCreate_Then_ICanFindItById()
        {
            //act
            var objCreate = await factory.Create();
            var objGet = await repository.SearchById(objCreate.Id);

            //assert
            objGet.Id.Should().Be(objCreate.Id);
            objGet.Name.Should().Be(objCreate.Name);
        }

        [Test]
        public async Task WhenCreateAndUpdate_Then_ICanFindItById()
        {
            //arrange
            var expectedNameAfterUpdate =
                $"AfterUpdate-Persona-Test-{DateTime.UtcNow.ToLongTimeString()}";

            //act
            var objCreate = await factory.Create();
            var commandUpdate = new UpdatePersonaCommand(
                objCreate.Id,
                expectedNameAfterUpdate);

            var responseUpdate = await factory.Update(commandUpdate);
            var objGet = await repository.Search(x => x.Id == objCreate.Id);

            //assert
            objGet.Id.Should().Be(responseUpdate.Id);
            objGet.Name.Should().Be(responseUpdate.Name);
        }

        [Test]
        public async Task WhenCreateAndUpdateAndDelete_Then_Success()
        {
            //arrange
            var expectedNameAfterUpdate =
                $"AfterUpdate-Persona-Test-{DateTime.UtcNow.ToLongTimeString()}";

            //act
            var objCreate = await factory.Create();
            var commandUpdate = new UpdatePersonaCommand(
                objCreate.Id,
                expectedNameAfterUpdate);

            await factory.Update(commandUpdate);
            await repository.Delete(objCreate.Id);

            var objGet = await repository.SearchById(objCreate.Id);

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
            await factory.Create();
            await factory.Create();
            await factory.Create();
            await factory.Create();

            var currentInserted = (await repository.SearchList(x => x.Name.Contains("Test"))).Count();
            await factory.DeleteAll();
            var currentResult = (await repository.ListAll()).Count();
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
            var name = "Persona Test";

            //act
            await factory.DeleteAll();
            var company1 = await factory.Create();
            var company2 = await factory.Create();
            var company3 = await factory.Create();
            var company4 = await factory.Create();

            var list = new[] { company1, company2, company3, company4 };
            var currentInserted = (await factory.GetListByName(name)).Count();
            await repository.Update(list);
            var currentResult = (await factory.GetListByName(name)).Count();

            //assert
            expectedInserted.Should().Be(currentInserted);
            finalResult.Should().Be(currentResult);
        }
    }
}