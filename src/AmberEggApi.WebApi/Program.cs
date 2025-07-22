using Autofac.Extensions.DependencyInjection;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Logging;
using System.IO;

namespace AmberEggApi.WebApi;

public static class Program
{
    public static void Main()
    {
        CreteWebHost().Run();
    }

    public static IWebHost CreteWebHost()
    {
        return WebHost.CreateDefaultBuilder()
                .ConfigureServices(s => s.AddAutofac())
                .UseContentRoot(Directory.GetCurrentDirectory())
                .ConfigureLogging(logging =>
                {
                    logging.AddConsole();
                })
                .UseStartup<Startup>()
                .Build();
    }
}