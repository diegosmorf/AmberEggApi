using AmberEggApi.ApplicationService.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace AmberEggApi.WebApi.Controllers
{
    public class HealthCheckController : BaseController
    {
        private readonly ICompanyAppService appService;
        private readonly ILogger<HealthCheckController> logger;

        public HealthCheckController(ICompanyAppService appService,
            ILogger<HealthCheckController> logger)
        {
            this.appService = appService;
            this.logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            const string message = "HealthCheck Status OK";

            await appService.GetAll();
            logger.LogInformation(message);
            return Ok(message);

        }
    }
}