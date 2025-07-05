using Autofac.Extensions.DependencyInjection;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using System;
using System.IO;
using System.Reflection;
using Microsoft.Extensions.Logging;

namespace AmberEggApi.WebApi
{
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
}