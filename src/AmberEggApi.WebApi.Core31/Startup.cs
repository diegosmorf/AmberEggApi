using AmberEggApi.ApplicationService.InjectionModules;
using AmberEggApi.Database.InjectionModules;
using AmberEggApi.Infrastructure.InjectionModules;
using Api.Common.Repository.MongoDb;
using Api.Common.Repository.Repositories;
using Api.Common.WebServer.Server;
using Autofac;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using Serilog;
using System.Reflection;

namespace AmberEggApi.WebApi.Core31
{
    public class Startup
    {
        public Startup(IWebHostEnvironment environment)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(environment.ContentRootPath)
                .AddJsonFile("appsettings.json", true, true)
                .AddJsonFile($"appsettings.{environment.EnvironmentName}.json", true)
                .AddEnvironmentVariables();

            Configuration = builder.Build();

            Log.Logger = new LoggerConfiguration()
                .ReadFrom.Configuration(Configuration)
                .CreateLogger();

            Log.Information($"Starting up: {Assembly.GetEntryAssembly().GetName()}");
            Log.Information($"Environment: {environment.EnvironmentName}");
        }

        public IConfiguration Configuration { get; }

        public void ConfigureContainer(ContainerBuilder builder)
        {
            // IoC Container Module Registration
            builder.RegisterModule(new IoCModuleApplicationService());
            builder.RegisterModule(new IoCModuleInfrastructure());
            builder.RegisterModule(new IoCModuleAutoMapper());
            builder.RegisterModule(new IoCModuleDatabase());
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();

            //Config Swagger
            services.AddSwaggerGen(c => { c.SwaggerDoc("v1", new OpenApiInfo { Title = "AmberEggApi", Version = "v1" }); });

            var settings = Configuration.GetSection("MongoSettings").Get<MongoSettings>();
            services.AddSingleton(settings);

            services.AddMemoryCache();
        }

        private void ApplyDbMigrations(IApplicationBuilder app)
        {
            app.ApplicationServices.GetService<IDatabaseMigrator>().ApplyMigrations();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILoggerFactory loggerFactory)
        {
            app.UseMiddleware<ApiResponseMiddleware>();
            app.UseMiddleware<SerilogMiddleware>();
            loggerFactory.AddSerilog();

            app.UseSwagger(c => c.RouteTemplate = "swagger/{documentName}/swagger.json");
            app.UseSwaggerUI(x => x.SwaggerEndpoint("/swagger/v1/swagger.json", "AmberEggApi V1"));
            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseAuthorization();
            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });

            ApplyDbMigrations(app);
        }
    }
}
