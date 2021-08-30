using AmberEggApi.WebApi.Controllers;
using Microsoft.AspNetCore.Mvc;

namespace AmberEggApi.Domain.Tests.Factories
{
    public class HealthcheckControllerFactory
    {
        private readonly HealthCheckController controller;

        public HealthcheckControllerFactory(HealthCheckController controller)
        {
            this.controller = controller;
        }

        public async Task<string> Get()
        {
            var response = await controller.Get() as OkObjectResult;
            var viewmodel = response.Value as string;
            return viewmodel;
        }
    }
}