using AmberEggApi.ApplicationService.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace AmberEggApi.WebApi.Controllers
{
    public class HealthCheckController : BaseController
    {
        private readonly IPersonaAppService appService;

        public HealthCheckController(IPersonaAppService appService)
        {
            this.appService = appService;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            const string message = "HealthCheck Status OK";
            await appService.GetAll();
            return Ok(message);

        }
    }
}