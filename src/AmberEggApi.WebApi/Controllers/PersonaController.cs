using AmberEggApi.ApplicationService.Interfaces;
using AmberEggApi.Domain.Commands;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace AmberEggApi.WebApi.Controllers
{
    public class PersonaController : BaseController
    {
        private readonly IPersonaAppService appService;

        public PersonaController(IPersonaAppService appService)
        {
            this.appService = appService;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var result = await appService.GetAll();

            if (!result.Any())
            {
                return NoContent();
            }

            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get([FromRoute] Guid id)
        {
            var result = await appService.Get(id);

            if (result == null)
            {
                return NoContent();
            }

            return Ok(result);
        }

        [HttpGet("name/{name}")]
        public async Task<IActionResult> Get([FromRoute] string name)
        {
            var result = await appService.GetListByName(name);

            if (!result.Any())
            {
                return NoContent();
            }

            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreatePersonaCommand command)
        {
            var result = await appService.Create(command);
            return Created(result.Id.ToString(), result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete([FromRoute] Guid id)
        {
            await appService.Delete(new DeletePersonaCommand(id));
            return NoContent();
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update([FromRoute] Guid id, [FromBody] UpdatePersonaCommand command)
        {
            var result = await appService.Get(id);

            if (result == null)
            {
                return NoContent();
            }

            return Ok(await appService.Update(command));
        }
    }
}