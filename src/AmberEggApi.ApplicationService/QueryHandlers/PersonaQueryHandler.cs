using AmberEggApi.ApplicationService.Contracts;
using AmberEggApi.ApplicationService.Mapping;
using AmberEggApi.ApplicationService.ViewModels;
using AmberEggApi.Contracts.Repositories;
using AmberEggApi.Domain.Models;

using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace AmberEggApi.ApplicationService.QueryHandlers;
public class PersonaQueryHandler(IMapper mapper, IRepository<Persona> repository) : 
    GenericQueryHandler<Persona, PersonaViewModel>(mapper, repository), IPersonaQueryHandler
{
    public async Task<IEnumerable<PersonaViewModel>> GetListByName(string name, CancellationToken cancellationToken)
    {
        if (string.IsNullOrEmpty(name) || string.IsNullOrWhiteSpace(name) || name.Length <= 2)
            return [];

        var list = await repository.SearchList(x => x.Name.Contains(name), cancellationToken);

        return mapper.MapList<Persona, PersonaViewModel>(list).OrderBy(x => x.Name);
    }    
}