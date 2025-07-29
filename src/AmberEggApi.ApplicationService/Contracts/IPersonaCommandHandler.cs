using AmberEggApi.ApplicationService.ViewModels;
using AmberEggApi.Domain.Commands;
using System.Threading;
using System.Threading.Tasks;

namespace AmberEggApi.ApplicationService.Contracts;

public interface IPersonaCommandHandler
{
    Task<PersonaViewModel> Handle(CreatePersonaCommand command, CancellationToken cancellationToken);
    Task<PersonaViewModel> Handle(UpdatePersonaCommand command, CancellationToken cancellationToken);
    Task<PersonaViewModel> Handle(DeletePersonaCommand command, CancellationToken cancellationToken);
}