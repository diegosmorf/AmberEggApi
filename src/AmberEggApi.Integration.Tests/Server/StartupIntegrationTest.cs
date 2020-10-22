using AmberEggApi.ApplicationService.InjectionModules;
using AmberEggApi.Database.Repositories;
using AmberEggApi.Domain.InjectionModules;
using AmberEggApi.Infrastructure.InjectionModules;
using Api.Common.WebServer.Server;
using Autofac;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace AmberEggApi.Integration.Tests.Server
{

    public class StartupIntegrationTest
    {

        private readonly IConfiguration configuration;
        public StartupIntegrationTest(IWebHostEnvironment environment)
        {
            configuration = new ConfigurationBuilder()
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
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services
                .AddControllers(opt => { opt.Filters.Add(new ValidateModelAttribute()); })
                .AddApplicationPart(Assembly.Load("AmberEggApi.WebApi"))
                .AddNewtonsoftJson();

            var connectionStringApp = configuration.GetConnectionString("ApiDbConnection");
            services.AddDbContext<EfCoreDbContext>(options => { options.UseSqlServer(connectionStringApp); });

            services.AddMemoryCache();
        }

        public void Configure(IApplicationBuilder app)
        {

            app.UseMiddleware<ApiResponseMiddleware>();

            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            

            UpdateDatabaseUsingEfCore(app);
        }

        private void UpdateDatabaseUsingEfCore(IApplicationBuilder app)
        {
            using var serviceScope = app.ApplicationServices.GetService<IServiceScopeFactory>().CreateScope();
            serviceScope
                .ServiceProvider
                .GetRequiredService<EfCoreDbContext>()
                .Database
                .Migrate();
        }
    }
}