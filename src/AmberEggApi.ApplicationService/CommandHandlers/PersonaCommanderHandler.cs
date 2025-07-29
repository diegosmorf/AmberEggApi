using AmberEggApi.ApplicationService.Contracts;
using AmberEggApi.ApplicationService.Mapping;
using AmberEggApi.ApplicationService.ViewModels;
using AmberEggApi.Contracts.Repositories;
using AmberEggApi.Domain.Commands;
using AmberEggApi.Domain.Models;

namespace AmberEggApi.ApplicationService.CommandHandlers;

public class PersonaCommanderHandler(IRepository<Persona> repository, IMapper mapper, IUnitOfWork unitOfWork) : 
    GenericCommandHandler<Persona, PersonaViewModel, CreatePersonaCommand, UpdatePersonaCommand, DeletePersonaCommand>(repository, mapper, unitOfWork), 
    IPersonaCommandHandler
{
}