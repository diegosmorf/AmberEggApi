using Autofac.Extensions.DependencyInjection;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Serilog;
using System;
using System.IO;

namespace AmberEggApi.WebApi
{
    public static class Program
    {
        public static void Main()
        {
            try
            {   
                WebHost.CreateDefaultBuilder()
                    .ConfigureServices(s => s.AddAutofac())
                    .UseContentRoot(Directory.GetCurrentDirectory())
                    .UseStartup<Startup>()
                    .UseSerilog()
                    .Build()
                    .Run();

            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex}");
            }
        }
    }
}