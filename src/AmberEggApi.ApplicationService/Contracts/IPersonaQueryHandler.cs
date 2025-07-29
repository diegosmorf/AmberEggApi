using AmberEggApi.ApplicationService.ViewModels;
using AmberEggApi.Domain.Models;

using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace AmberEggApi.ApplicationService.Contracts;

public interface IPersonaQueryHandler:    
    IQueryHandler<Persona, PersonaViewModel>        
{
    Task<IEnumerable<PersonaViewModel>> GetListByName(string name, CancellationToken cancellationToken);
}