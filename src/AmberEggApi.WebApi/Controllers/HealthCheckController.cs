using AmberEggApi.ApplicationService.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace AmberEggApi.WebApi.Controllers;

public class HealthCheckController(IPersonaAppService appService) : BaseController
{
    private readonly IPersonaAppService appService = appService;

    [HttpGet]
    public async Task<IActionResult> Get()
    {
        const string message = "HealthCheck Status OK";
        await appService.GetAll();
        return Ok(message);

    }
}