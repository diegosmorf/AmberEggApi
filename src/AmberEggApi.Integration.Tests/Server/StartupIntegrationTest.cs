using AmberEggApi.ApplicationService.InjectionModules;
using AmberEggApi.Database.InjectionModules;
using AmberEggApi.Domain.InjectionModules;
using AmberEggApi.Infrastructure.InjectionModules;
using AmberEggApi.Integration.Tests.IntegrationTests;
using Api.Common.Repository.MongoDb;
using Api.Common.Repository.Repositories;
using Api.Common.WebServer.Server;
using Autofac;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System.Reflection;

namespace AmberEggApi.Integration.Tests.Server
{
    public class StartupIntegrationTest
    {
        public StartupIntegrationTest(IHostingEnvironment environment)
        {
            new ConfigurationBuilder()
                .SetBasePath(environment.ContentRootPath)
                .AddJsonFile("appsettings.json", true, true)
                .AddEnvironmentVariables()
                .Build();
        }

        public void ConfigureContainer(ContainerBuilder builder)
        {
            // IoC Container Module Registration
            builder.RegisterModule(new IoCModuleApplicationService());
            builder.RegisterModule(new IoCModuleInfrastructure());
            builder.RegisterModule(new IoCModuleDomain());
            builder.RegisterModule(new IoCModuleAutoMapper());
            builder.RegisterModule(new IoCModuleDatabase());
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services
                .AddMvc(opt => { opt.Filters.Add(new ValidateModelAttribute()); })
                .AddApplicationPart(Assembly.Load("AmberEggApi.WebApi"))
                .AddJsonOptions(opt =>
                {
                    opt.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
                    opt.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
                    opt.SerializerSettings.DateTimeZoneHandling = DateTimeZoneHandling.Utc;
                });

            var settings = new MongoSettings
            {
                ConnectionString = BaseIntegrationTest.MongoDbServer.ConnectionString,
                DatabaseName = "Database-Integration-Tests"
            };

            services.AddSingleton(settings);
            services.AddMemoryCache();
        }

        public void Configure(IApplicationBuilder app)
        {
            app.UseMiddleware<ApiResponseMiddleware>();
            app.UseMvc();
            ApplyDbMigrations(app);
        }

        private void ApplyDbMigrations(IApplicationBuilder app)
        {
            app.ApplicationServices.GetService<IDatabaseMigrator>().ApplyMigrations();
        }
    }
}