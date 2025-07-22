using Microsoft.AspNetCore.Authorization;

namespace AmberEggApi.WebApi.Controllers;

[Authorize("Bearer")]
public class BaseAuthorizedController : BaseController
{
}