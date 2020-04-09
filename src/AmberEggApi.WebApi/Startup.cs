using AmberEggApi.ApplicationService.InjectionModules;
using AmberEggApi.Database.InjectionModules;
using AmberEggApi.Infrastructure.InjectionModules;
using Api.Common.Repository.MongoDb;
using Api.Common.Repository.Repositories;
using Api.Common.WebServer.Server;
using Autofac;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using Mongo2Go;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Serilog;
using System.Reflection;

namespace AmberEggApi.WebApi
{
    public class Startup
    {
        public Startup(IHostingEnvironment environment)
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(environment.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{environment.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables()
                .Build();


            Log.Logger = new LoggerConfiguration()
                .ReadFrom.Configuration(configuration)
                .CreateLogger();

            Log.Information($"Starting up: {Assembly.GetEntryAssembly().GetName()}");
            Log.Information($"Environment: {environment.EnvironmentName}");
        }

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
            services
                .AddMvc(opt => { opt.Filters.Add(new ValidateModelAttribute()); })
                .AddJsonOptions(opt =>
                {
                    opt.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
                    opt.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
                    opt.SerializerSettings.DateTimeZoneHandling = DateTimeZoneHandling.Utc;
                });

            //Config Swagger
            services.AddSwaggerGen(c => { c.SwaggerDoc("v1", new OpenApiInfo { Title = "AmberEggApi", Version = "v1" }); });

            services.AddCors(config =>
            {
                var policy = new CorsPolicy();
                policy.Headers.Add("*");
                policy.Methods.Add("*");
                policy.Origins.Add("*");
                policy.SupportsCredentials = true;
                config.AddPolicy("policy", policy);
            });

            //Setup MongoDB InMemory
            var mongoDbServer = MongoDbRunner.Start();

            var settings = new MongoSettings
            {
                ConnectionString = mongoDbServer.ConnectionString,
                DatabaseName = "Companies"
            };

            /// Setup MongoDB ConnectioString
            /// var settings = Configuration.GetSection("MongoSettings").Get<MongoSettings>();
            services.AddSingleton(settings);

            services.AddMemoryCache();
        }

        public void Configure(IApplicationBuilder app, ILoggerFactory loggerFactory)
        {
            app.UseMiddleware<ApiResponseMiddleware>();
            app.UseMiddleware<SerilogMiddleware>();
            loggerFactory.AddSerilog();

            app.UseSwagger(c => c.RouteTemplate = "swagger/{documentName}/swagger.json");
            app.UseSwaggerUI(x => x.SwaggerEndpoint("/swagger/v1/swagger.json", "AmberEggApi Documentation"));
            app.UseMvc();
            ApplyDbMigrations(app);
        }

        private void ApplyDbMigrations(IApplicationBuilder app)
        {
            app.ApplicationServices.GetService<IDatabaseMigrator>().ApplyMigrations();
        }
    }
}