using AmberEggApi.ApplicationService.InjectionModules;
using AmberEggApi.Database.Repositories;
using AmberEggApi.Infrastructure.InjectionModules;
using AmberEggApi.Infrastructure.Loggers;
using Api.Common.WebServer.Server;
using Autofac;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using System.Reflection;

namespace AmberEggApi.WebApi
{
    public class Startup
    {
        private readonly IConfiguration configuration;

        private readonly ConsoleLogger looger = new ConsoleLogger();

        public Startup(IWebHostEnvironment environment)
        {
            configuration = new ConfigurationBuilder()
                .SetBasePath(environment.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{environment.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables()
                .Build();

            looger.Information($"Starting up: {Assembly.GetEntryAssembly().GetName()}");
            looger.Information($"Environment: {environment.EnvironmentName}");

            if (environment.IsDevelopment())
            {
                foreach (var item in configuration.AsEnumerable())
                {
                    looger.Information($"Key:'{item.Key}' - Value: '{item.Value}'");
                }
            }
        }

        public void ConfigureContainer(ContainerBuilder builder)
        {
            // IoC Container Module Registration
            builder.RegisterModule(new IoCModuleApplicationService());
            builder.RegisterModule(new IoCModuleInfrastructure());
            builder.RegisterModule(new IoCModuleAutoMapper());            
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services
                .AddControllers(opt => { opt.Filters.Add(new ValidateModelAttribute()); })
                .AddNewtonsoftJson();
                

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

            var connectionStringApp = configuration.GetConnectionString("ApiDbConnection");
            services.AddDbContext<EfCoreDbContext>(options => { options.UseSqlServer(connectionStringApp); });

            services.AddMemoryCache();
        }

        public void Configure(IApplicationBuilder app)
        {
            app.UseMiddleware<ApiResponseMiddleware>();
            app.UseMiddleware<LoggerMiddleware>();
            
            app.UseRouting();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });            

            app.UseSwagger(c => c.RouteTemplate = "swagger/{documentName}/swagger.json");
            app.UseSwaggerUI(x => x.SwaggerEndpoint("/swagger/v1/swagger.json", "AmberEggApi Documentation"));

            UpdateDatabaseUsingEfCore(app);
        }

        private void UpdateDatabaseUsingEfCore(IApplicationBuilder app)
        {
            looger.Information("Starting: Database Migration");

            using (var serviceScope = app.ApplicationServices.GetService<IServiceScopeFactory>().CreateScope())
            {
                serviceScope
                    .ServiceProvider
                    .GetRequiredService<EfCoreDbContext>()
                    .Database
                    .Migrate();
            }

            looger.Information("Ending: Database Migration");
        }
    }
}