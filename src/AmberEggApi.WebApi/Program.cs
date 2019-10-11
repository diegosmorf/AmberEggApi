using System;
using System.IO;
using System.Linq;
using Autofac.Extensions.DependencyInjection;
using AmberEggApi.WebApi.Server;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Serilog;

namespace AmberEggApi.WebApi
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            try
            {
                var builder = new ConfigurationBuilder();
                builder.AddCommandLine(args);

                var config = builder.Build();
                var pathToContentRoot = Directory.GetCurrentDirectory();

                var webHostArgs = args.Where(arg => arg != "--console").ToArray();

                var host = WebHost.CreateDefaultBuilder(webHostArgs)
                    .UseConfiguration(config)
                    .ConfigureServices(s => s.AddAutofac())
                    .UseContentRoot(pathToContentRoot)
                    .UseStartup<Startup>()
                    .UseSerilog()
                    .Build();

                host.Run();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex}");
            }
        }
    }
}