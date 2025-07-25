using AmberEggApi.ApplicationService.Interfaces;
using AmberEggApi.Domain.Commands;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace AmberEggApi.WebApi.Controllers;

public class PersonaController(IPersonaAppService appService) : BaseController
{
    [HttpGet]
    public async Task<IActionResult> Get()
    {
        var result = await appService.GetAll();

        return !result.Any() ? NoContent() : Ok(result);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> Get([FromRoute] Guid id)
    {
        if (!this.ModelState.IsValid)
        {
            return BadRequest(this.ModelState);
        }

        var result = await appService.Get(id);

        return result == null ? NoContent() : Ok(result);
    }

    [HttpGet("name/{name}")]
    public async Task<IActionResult> Get([FromRoute] string name)
    {
        if (!this.ModelState.IsValid)
        {
            return BadRequest(this.ModelState);
        }        

        var result = await appService.GetListByName(name);

        return !result.Any() ? NoContent() : Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreatePersonaCommand command)
    {
        if (!this.ModelState.IsValid)
        {
            return BadRequest(this.ModelState);
        }

        var result = await appService.Create(command);
        return Created(result.Id.ToString(), result);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete([FromRoute] Guid id)
    {
        if (id == Guid.Empty)
        {
            return BadRequest("Id parameter is required");
        }

        var result = await appService.Get(id);

        if (result == null)
        {
            return NoContent();
        }

        await appService.Delete(new DeletePersonaCommand(id));

        return NoContent();
    }

    [HttpPut()]
    public async Task<IActionResult> Update([FromBody] UpdatePersonaCommand command)
    {
        if (!this.ModelState.IsValid)
        {
            return BadRequest(this.ModelState);
        }

        if (command.Id == Guid.Empty)
        {
            return BadRequest("Id parameter is required");
        }

        var result = await appService.Get(command.Id);

        return result == null ? NoContent() : Ok(await appService.Update(command));
    }
}