using AmberEggApi.WebApi.Controllers;
using Microsoft.AspNetCore.Mvc;

using System.Threading;
using System.Threading.Tasks;

namespace AmberEggApi.DomainTests.Factories;

public class HealthCheckControllerFactory(HealthCheckController controller)
{
    private readonly HealthCheckController controller = controller;

    public async Task<string> Get()
    {
        var response = await controller.Get(new CancellationToken()) as OkObjectResult;
        var viewModel = response.Value as string;
        return viewModel;
    }
}