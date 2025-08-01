﻿using AmberEggApi.ApplicationService.InjectionModules;
using AmberEggApi.Database.Repositories;
using AmberEggApi.Infrastructure.InjectionModules;

using Autofac;

using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

using System.Reflection;

namespace AmberEggApi.IntegrationTests.Server;

public class StartupIntegrationTest
{
    protected StartupIntegrationTest()
    {
    }

    public static void ConfigureContainer(ContainerBuilder builder)
    {
        // IoC Container Module Registration
        builder.RegisterModule(new IoCModuleApplicationService());
        builder.RegisterModule(new IoCModuleInfrastructure());                

        var opt = new DbContextOptionsBuilder<EfCoreDbContext>();
        opt.UseInMemoryDatabase(databaseName: "AmberEgg-API-DomainTests");

        builder.RegisterInstance(new EfCoreDbContext(opt.Options)).As<DbContext>();
    }

    // This method gets called by the runtime. Use this method to add services to the container.
    public static void ConfigureServices(IServiceCollection services)
    {
        services
            .AddControllers()
            .AddApplicationPart(Assembly.Load("AmberEggApi.WebApi"));

        services.AddMemoryCache();
    }

    public static void Configure(IApplicationBuilder app)
    {
        app.UseHttpsRedirection();
        app.UseRouting();
        app.UseAuthorization();
        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();
        });
    }
}