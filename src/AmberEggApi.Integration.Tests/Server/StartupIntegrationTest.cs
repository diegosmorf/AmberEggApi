using AmberEggApi.ApplicationService.InjectionModules;
using AmberEggApi.Database.Repositories;
using AmberEggApi.Domain.InjectionModules;
using AmberEggApi.Infrastructure.InjectionModules;
using Api.Common.WebServer.Server;
using Autofac;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace AmberEggApi.Integration.Tests.Server
{
    public class StartupIntegrationTest
    {
        public void ConfigureContainer(ContainerBuilder builder)
        {
            // IoC Container Module Registration
            builder.RegisterModule(new IoCModuleApplicationService());
            builder.RegisterModule(new IoCModuleInfrastructure());
            builder.RegisterModule(new IoCModuleDomain());
            builder.RegisterModule(new IoCModuleAutoMapper());

            var opt = new DbContextOptionsBuilder<EfCoreDbContext>();
            opt.UseInMemoryDatabase(databaseName: "AmberEgg-API-DomainTests");

            builder.RegisterInstance(new EfCoreDbContext(opt.Options)).As<DbContext>();
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services
                .AddControllers(opt => { opt.Filters.Add(new ValidateModelAttribute()); })
                .AddApplicationPart(Assembly.Load("AmberEggApi.WebApi"))
                .AddNewtonsoftJson();

            services.AddMemoryCache();
        }

        public void Configure(IApplicationBuilder app)
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
}