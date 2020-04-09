using AmberEggApi.ApplicationService.InjectionModules;
using AmberEggApi.Database.InjectionModules;
using AmberEggApi.Domain.Tests.InjectionModules;
using AmberEggApi.Infrastructure.InjectionModules;
using Api.Common.Repository.MongoDb;
using Api.Common.Repository.Repositories;
using Autofac;
using Mongo2Go;
using NUnit.Framework;
using System.Threading.Tasks;

namespace AmberEggApi.Domain.Tests.UnitTests
{
    [SetUpFixture]
    public class SetupDomainTests
    {
        public static IContainer Container { get; protected set; }
        public static MongoDbRunner MongoDbServer { get; protected set; }


        [OneTimeSetUp]
        public async Task RunBeforeAllTests()
        {
            //Setup MongoDB InMemory
            MongoDbServer = MongoDbRunner.Start();

            // Setup IoC Container
            var builder = new ContainerBuilder();
            builder.RegisterModule(new IoCModuleApplicationService());
            builder.RegisterModule(new IoCModuleInfrastructure());
            builder.RegisterModule(new IoCModuleAutoMapper());
            builder.RegisterModule(new IoCModuleDomainTest());
            builder.RegisterModule(new IoCModuleDatabase());

            var settings = new MongoSettings
            {
                ConnectionString = MongoDbServer.ConnectionString,
                DatabaseName = "Database-Domain-Tests"
            };

            builder.RegisterInstance(settings);
            Container = builder.Build();

            //Apply Db Migrations
            var migrator = Container.Resolve<IDatabaseMigrator>();
            await migrator.ApplyMigrations();
        }

        [OneTimeTearDown]
        public void RunAfterAllTests()
        {
            Container.Dispose();
            MongoDbServer.Dispose();
        }
    }
}