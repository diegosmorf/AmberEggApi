using AmberEggApi.ApplicationService.InjectionModules;
using AmberEggApi.Database.Repositories;
using AmberEggApi.Infrastructure.InjectionModules;
using AmberEggApi.Infrastructure.Loggers;
using Autofac;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Reflection;
using System.Threading.Tasks;

namespace AmberEggApi.WebApi
{
    public class Startup
    {
        private readonly IConfiguration configuration;

        private readonly ConsoleLogger logger = new();

        public Startup(IWebHostEnvironment environment)
        {
            configuration = new ConfigurationBuilder()
                .SetBasePath(environment.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{environment.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables()
                .Build();

            Task.Run(async () => { 
                await logger.Information($"Starting up: {Assembly.GetEntryAssembly().GetName()}");
                await logger.Information($"Environment: {environment.EnvironmentName}");

                if (environment.IsDevelopment())
                {
                    foreach (var item in configuration.AsEnumerable())
                    {
                       await  logger.Information($"Key:'{item.Key}' - Value: '{item.Value}'");
                    }
                }
            }).Wait();
        }

        public static void ConfigureContainer(ContainerBuilder builder)
        {
            // IoC Container Module Registration
            builder.RegisterModule(new IoCModuleApplicationService());
            builder.RegisterModule(new IoCModuleInfrastructure());
            builder.RegisterModule(new IoCModuleAutoMapper());
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {

            services.AddControllers();
            services.AddEndpointsApiExplorer();
            services.AddOpenApiDocument(document =>
            {
                document.Title = "AmberEggApi";
                document.Version = "v1";
            });

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

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseOpenApi();
                app.UseSwaggerUi(settings =>
                {
                    settings.Path = "/swagger";
                    settings.DocumentPath = "/swagger/v1/swagger.json";
                });
            }

            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseAuthorization();
            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });

            Task.Run(async () =>
            {
                await UpdateDatabaseUsingEfCore(app);
            }).Wait();
        }

        private async Task UpdateDatabaseUsingEfCore(IApplicationBuilder app)
        {
            await logger.Information("Starting: Database Migration");

            using (var serviceScope = app.ApplicationServices.GetService<IServiceScopeFactory>().CreateScope())
            {
                await serviceScope
                    .ServiceProvider
                    .GetRequiredService<EfCoreDbContext>()
                    .Database
                    .MigrateAsync();
            }

            await logger.Information("Ending: Database Migration");
        }
    }
}
