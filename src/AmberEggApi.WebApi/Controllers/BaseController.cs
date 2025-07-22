using Microsoft.AspNetCore.Mvc;

namespace AmberEggApi.WebApi.Controllers;

[Produces("application/json")]
[Route("api/v1/[controller]")]
public class BaseController : Controller
{
}