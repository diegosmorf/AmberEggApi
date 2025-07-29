using AmberEggApi.ApplicationService.ViewModels;
using AmberEggApi.Domain.Commands;
using AmberEggApi.Domain.Models;

namespace AmberEggApi.ApplicationService.Contracts;

public interface IPersonaCommandHandler :
    ICommandHandler<Persona, PersonaViewModel, CreatePersonaCommand, UpdatePersonaCommand, DeletePersonaCommand>

{ 
}