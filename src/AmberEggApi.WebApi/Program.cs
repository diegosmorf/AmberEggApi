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
            try
            {
                Console.WriteLine($"Starting up: {Assembly.GetEntryAssembly().GetName()}");

                CreteWebHost().Run();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex}");
            }
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