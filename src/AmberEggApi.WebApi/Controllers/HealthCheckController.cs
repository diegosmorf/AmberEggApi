using AmberEggApi.ApplicationService.Contracts;

using Microsoft.AspNetCore.Mvc;

using System.Threading;
using System.Threading.Tasks;

namespace AmberEggApi.WebApi.Controllers;

public class HealthCheckController(IPersonaQueryHandler queryHandler) : BaseController
{
    private readonly IPersonaQueryHandler queryHandler = queryHandler;

    [HttpGet]
    public async Task<IActionResult> Get(CancellationToken cancellationToken)
    {
        const string message = "HealthCheck Status OK";
        await queryHandler.GetAll(cancellationToken);
        return Ok(message);

    }
}