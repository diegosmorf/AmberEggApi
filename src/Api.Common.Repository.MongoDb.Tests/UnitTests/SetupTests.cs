using AmberEggApi.ApplicationService.InjectionModules;
using AmberEggApi.Database.InjectionModules;
using AmberEggApi.Infrastructure.InjectionModules;
using Api.Common.Repository.MongoDb.Tests.InjectionModules;
using Autofac;
using Mongo2Go;
using NUnit.Framework;

namespace Api.Common.Repository.MongoDb.Tests.UnitTests
{
    [SetUpFixture]
    public class SetupTests
    {
        public static IContainer Container { get; protected set; }
        public static MongoDbRunner MongoDbServer { get; protected set; }

        [OneTimeSetUp]
        public void RunBeforeAllTests()
        {
            //Setup MongoDB InMemory
            MongoDbServer = MongoDbRunner.Start();

            // Setup IoC Container
            var builder = new ContainerBuilder();
            builder.RegisterModule(new IoCModuleApplicationService());
            builder.RegisterModule(new IoCModuleInfrastructure());
            builder.RegisterModule(new IoCModuleAutoMapper());
            builder.RegisterModule(new IoCModuleDatabaseTest());
            builder.RegisterModule(new IoCModuleDatabase());

            var settings = new MongoSettings
            {
                ConnectionString = MongoDbServer.ConnectionString,
                DatabaseName = "Database-Database-Tests"
            };

            builder.RegisterInstance(settings);
            Container = builder.Build();            
        }

        [OneTimeTearDown]
        public void RunAfterAllTests()
        {
            Container.Dispose();
            MongoDbServer.Dispose();
        }
    }
}