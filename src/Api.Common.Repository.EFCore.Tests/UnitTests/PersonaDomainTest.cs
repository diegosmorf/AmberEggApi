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
        public async Task WhenCreate_Then_FindItById()
        {
            //act
            var resultCreate = await factory.Create();
            var resultGet = await repository.SearchById(resultCreate.Id);

            //assert
            resultGet.Id.Should().Be(resultCreate.Id);
            resultGet.Name.Should().Be(resultCreate.Name);
        }

        [Test]
        public async Task WhenCreateAndUpdate_Then_FindItById()
        {
            //arrange
            var expectedNameAfterUpdate =
                $"AfterUpdate-Persona-Test-{DateTime.UtcNow.ToLongTimeString()}";

            //act
            var resultCreate = await factory.Create();
            var commandUpdate = new UpdatePersonaCommand(
                resultCreate.Id,
                expectedNameAfterUpdate);

            var responseUpdate = await factory.Update(commandUpdate);
            var resultGet = await repository.Search(x => x.Id == resultCreate.Id);

            //assert
            resultGet.Id.Should().Be(responseUpdate.Id);
            resultGet.Name.Should().Be(responseUpdate.Name);
        }

        [Test]
        public async Task WhenCreateAndUpdateAndDelete_Then_Success()
        {
            //arrange
            var expectedNameAfterUpdate =
                $"AfterUpdate-Persona-Test-{DateTime.UtcNow.ToLongTimeString()}";

            //act
            var resultCreate = await factory.Create();
            var commandUpdate = new UpdatePersonaCommand(
                resultCreate.Id,
                expectedNameAfterUpdate);

            await factory.Update(commandUpdate);
            await repository.Delete(resultCreate.Id);

            var resultGet = await repository.SearchById(resultCreate.Id);

            //assert
            resultGet.Should().BeNull();
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