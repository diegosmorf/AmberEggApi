using AmberEggApi.ApplicationService.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace AmberEggApi.WebApi.Controllers
{
    public class HealthCheckController : BaseController
    {
        private readonly ICompanyAppService appService;        

        public HealthCheckController(ICompanyAppService appService)
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