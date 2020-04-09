using Autofac.Extensions.DependencyInjection;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Serilog;
using System;
using System.IO;

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
                                
                var host = WebHost.CreateDefaultBuilder(args)
                    .UseConfiguration(config)
                    .ConfigureServices(s => s.AddAutofac())
                    .UseContentRoot(pathToContentRoot)
                    .UseStartup<Startup>()
                    .UseSerilog()
                    .Build();

                host.RunAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex}");
            }
        }
    }
}