using AmberEggApi.ApplicationService.ViewModels;
using AmberEggApi.Domain.Commands;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AmberEggApi.ApplicationService.Interfaces
{
    public interface IPersonaAppService
    {
        Task<IEnumerable<PersonaViewModel>> GetAll();

        Task Delete(DeletePersonaCommand commandDelete);

        Task<PersonaViewModel> Get(Guid id);

        Task<IEnumerable<PersonaViewModel>> GetListByName(string name);

        Task<PersonaViewModel> Create(CreatePersonaCommand command);

        Task<PersonaViewModel> Update(UpdatePersonaCommand command);
    }
}