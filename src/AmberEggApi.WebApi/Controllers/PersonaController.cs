using AmberEggApi.ApplicationService.Contracts;
using AmberEggApi.Domain.Commands;

using Microsoft.AspNetCore.Mvc;

using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace AmberEggApi.WebApi.Controllers;

public class PersonaController(IPersonaQueryHandler queryHandler, IPersonaCommandHandler commandHandler) : BaseController
{
    [HttpGet]
    public async Task<IActionResult> Get(CancellationToken cancellationToken)
    {
        var result = await queryHandler.GetAll(cancellationToken);

        return !result.Any() ? NoContent() : Ok(result);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> Get([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        try
        {
            var result = await queryHandler.Get(id, cancellationToken);

            return result == null ? NoContent() : Ok(result);
        }
        catch (Exception ex)
        {
            return BadRequest(ex);
        }
    }

    [HttpGet("name/{name}")]
    public async Task<IActionResult> Get([FromRoute] string name, CancellationToken cancellationToken)
    {
        try
        {
            var result = await queryHandler.GetListByName(name, cancellationToken);

            return !result.Any() ? NoContent() : Ok(result);
        }
        catch (Exception ex)
        {
            return BadRequest(ex);
        }
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreatePersonaCommand command, CancellationToken cancellationToken)
    {
        try
        {
            var result = await commandHandler.Handle(command, cancellationToken);
            return Created(result.Id.ToString(), result);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        try
        {
            await commandHandler.Handle(new DeletePersonaCommand(id), cancellationToken);

            return NoContent();
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
    [HttpPut()]
    public async Task<IActionResult> Update([FromBody] UpdatePersonaCommand command, CancellationToken cancellationToken)
    {
        try
        {
            var result = await commandHandler.Handle(command, cancellationToken);

            return result == null ? NoContent() : Ok(result);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
}