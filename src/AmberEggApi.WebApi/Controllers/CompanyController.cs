using AmberEggApi.ApplicationService.Interfaces;
using AmberEggApi.Domain.Commands;
using Microsoft.AspNetCore.Mvc;
using System;
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
            return Ok(await appService.GetAll());
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get([FromRoute] Guid id)
        {
            return Ok(await appService.Get(id));
        }

        [HttpGet("name/{name}")]
        public async Task<IActionResult> Get([FromRoute] string name)
        {
            return Ok(await appService.GetListByName(name));
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] CreateCompanyCommand command)
        {
            return Ok(await appService.Create(command));
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete([FromRoute] Guid id)
        {
            await appService.Delete(new DeleteCompanyCommand(id));
            return Ok();
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update([FromRoute] Guid id, [FromBody] UpdateCompanyCommand command)
        {
            if (!command.Id.Equals(id))
                return BadRequest("Invalid Id passed from route");

            return Ok(await appService.Update(command));
        }
    }
}