using AmberEggApi.WebApi.Controllers;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace AmberEggApi.Domain.Tests.Factories
{
    public class HealthcheckControllerFactory(HealthCheckController controller)
    {
        private readonly HealthCheckController controller = controller;

        public async Task<string> Get()
        {
            var response = await controller.Get() as OkObjectResult;
            var viewModel = response.Value as string;
            return viewModel;
        }
    }
}