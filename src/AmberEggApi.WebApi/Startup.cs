using AmberEggApi.ApplicationService.InjectionModules;
using AmberEggApi.Database.Repositories;
using AmberEggApi.Infrastructure.InjectionModules;
using Autofac;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace AmberEggApi.WebApi;

public class Startup(IWebHostEnvironment environment)
{
    private readonly IConfiguration configuration = new ConfigurationBuilder()
            .SetBasePath(environment.ContentRootPath)
            .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
            .AddJsonFile($"appsettings.{environment.EnvironmentName}.json", optional: true)
            .AddEnvironmentVariables()
            .Build();

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

        var connectionStringApp = configuration.GetConnectionString("ApiDbConnection");
        services.AddDbContext<EfCoreDbContext>(options => { options.UseSqlServer(connectionStringApp); });

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

        var serviceScope = app.ApplicationServices.GetService<IServiceScopeFactory>().CreateScope();
        serviceScope
            .ServiceProvider
            .GetRequiredService<EfCoreDbContext>()
            .Database
            .Migrate();

    }
}
