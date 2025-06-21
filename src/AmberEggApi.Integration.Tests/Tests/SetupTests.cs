using AmberEggApi.IntegrationTests.Server;
using Autofac.Core;
using Autofac.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using System;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;

namespace AmberEggApi.IntegrationTests.Tests;
internal class SetupTests : IDisposable
{
    private static TestServer apiServer;
    public static HttpClient Client { get; private set; }
    private bool _disposed = false;

    public SetupTests()
    {
        // Setup API SERVER
        apiServer = new TestServer(new WebHostBuilder()
            .ConfigureServices(s => s.AddAutofac())
            .UseContentRoot(Directory.GetCurrentDirectory())
            .UseStartup<StartupIntegrationTest>());

        Client = apiServer.CreateClient();
        Client.DefaultRequestHeaders.Accept.Clear();
        Client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
    }
    
    protected virtual void Dispose(bool disposing)
    {
        if (!_disposed && disposing)
        {
            Client?.Dispose();
            apiServer?.Dispose();
            _disposed = true;
        }
    }
    public void Dispose()
    {
        Dispose(disposing: true);
        GC.SuppressFinalize(this);
    }
    ~SetupTests()
    {
        Dispose(disposing: false);
    }
}