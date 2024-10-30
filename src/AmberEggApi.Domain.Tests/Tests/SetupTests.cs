using AmberEggApi.ApplicationService.InjectionModules;
using AmberEggApi.Database.Repositories;
using AmberEggApi.DomainTests.InjectionModules;
using AmberEggApi.Infrastructure.InjectionModules;
using Autofac;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;

namespace AmberEggApi.DomainTests.Tests
{
    [SetUpFixture]
    public class SetupTests
    {
        public static IContainer Container { get; protected set; }

        [OneTimeSetUp]
        public void RunBeforeAllTests()
        {
            // Setup IoC Container
            var builder = new ContainerBuilder();
            builder.RegisterModule(new IoCModuleApplicationService());
            builder.RegisterModule(new IoCModuleInfrastructure());
            builder.RegisterModule(new IoCModuleAutoMapper());
            builder.RegisterModule(new IoCModuleDomainTest());

            var opt = new DbContextOptionsBuilder<EfCoreDbContext>();
            opt.UseInMemoryDatabase(databaseName: "AmberEgg-API-DomainTests");

            builder.RegisterInstance(new EfCoreDbContext(opt.Options)).As<DbContext>();

            Container = builder.Build();
        }

        [OneTimeTearDown]
        public void RunAfterAllTests()
        {
            Container.Dispose();
        }
    }
}