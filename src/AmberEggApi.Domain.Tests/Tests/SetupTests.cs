using AmberEggApi.ApplicationService.InjectionModules;
using AmberEggApi.Database.Repositories;
using AmberEggApi.DomainTests.InjectionModules;
using AmberEggApi.Infrastructure.InjectionModules;
using Autofac;
using Microsoft.EntityFrameworkCore;
using System;

namespace AmberEggApi.DomainTests.Tests;

public class SetupTests : IDisposable
{
    public static IContainer Container { get; protected set; }
    protected bool _disposed = false;

    public SetupTests()
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

    protected virtual void Dispose(bool disposing)
    {
        if (!_disposed && disposing)
        {
            Container?.Dispose();
            _disposed = true;
        }
    }
    public void Dispose()
    {
        Dispose(disposing: true);
        GC.SuppressFinalize(this);
    }
    ~SetupTests()
    {
        Dispose(disposing: false);
    }
}