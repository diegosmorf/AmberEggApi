using System;
using System.Net;
using System.Threading.Tasks;
using AmberEggApi.ApplicationService.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

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

        /// <summary>
        ///     Verify system's conectivity
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            return await Task.Run(() =>
            {
                try
                {
                    const string message = "HealthCheck Status OK";
                    //validate Database Connection (read method)
                    appService.GetAll();

                    logger.LogInformation(message);

                    return Ok(message);
                }
                catch (Exception ex)
                {
                    return StatusCode((int) HttpStatusCode.InternalServerError, new {ex.Message});
                }
            });
        }
    }
}