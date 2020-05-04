using AmberEggApi.Database.Models;
using Api.Common.Repository.Repositories;
using Autofac;
using FluentAssertions;
using NUnit.Framework;
using System.Linq;
using System.Threading.Tasks;

namespace Api.Common.Repository.MongoDb.Tests.UnitTests
{
    [TestFixture]
    public class CompanyMigrationTest
    {
        private readonly IDatabaseMigrator migrator;
        private readonly IRepository<DatabaseVersion> repository;        

        public CompanyMigrationTest()
        {
            migrator = SetupTests.Container.Resolve<IDatabaseMigrator>();
            repository = SetupTests.Container.Resolve<IRepository<DatabaseVersion>>();            
        }

        [Test]
        public async Task WhenCreate_Then_ICanFindItById()
        {
            //arrange
            var numberOfMigrations = 1;
            var version = 1;
            var name = "V20191001001InitialState";

            //act
            await migrator.ApplyMigrations();            
            var list = await repository.All();
            
            //assert
            list.Count().Should().Be(numberOfMigrations);
            list.FirstOrDefault().Name.Should().Be(name);            
        }
    }
}