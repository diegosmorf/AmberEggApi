using AmberEggApi.ApplicationService.InjectionModules;
using AmberEggApi.Database.Repositories;
using AmberEggApi.Domain.Tests.InjectionModules;
using AmberEggApi.Infrastructure.InjectionModules;
using Api.Common.Repository.EFCoreTests.InjectionModules;
using Autofac;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;

namespace Api.Common.Repository.EFCoreTests.Tests
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
            builder.RegisterModule(new IoCModuleDatabaseTest());

            var opt = new DbContextOptionsBuilder<EfCoreDbContext>();
            opt.UseInMemoryDatabase(databaseName: "AmberEgg-API-EFCore-Db");

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