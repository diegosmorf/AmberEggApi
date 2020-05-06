using AmberEggApi.ApplicationService.Interfaces;
using AmberEggApi.Domain.Commands;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace AmberEggApi.WebApi.Controllers
{
    public class CompanyController : BaseController
    {
        private readonly ICompanyAppService appService;

        public CompanyController(ICompanyAppService appService)
        {
            this.appService = appService;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var result = await appService.GetAll();

            if (result.Any())
            {
                return NotFound();
            }

            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get([FromRoute] Guid id)
        {
            var result = await appService.Get(id);

            if (result == null)
            {
                return NotFound();
            }

            return Ok(result);
        }

        [HttpGet("name/{name}")]
        public async Task<IActionResult> Get([FromRoute] string name)
        {
            var result = await appService.GetListByName(name);

            if (result == null)
            {
                return NotFound();
            }

            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateCompanyCommand command)
        {
            var result = await appService.Create(command);
            return Created(result.Id.ToString(),result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete([FromRoute] Guid id)
        {
            await appService.Delete(new DeleteCompanyCommand(id));
            return NoContent();
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update([FromRoute] Guid id, [FromBody] UpdateCompanyCommand command)
        {
            var result = await appService.Get(id);

            if (result == null)
            {
                return NotFound();
            }

            return Ok(await appService.Update(command));
        }
    }
}