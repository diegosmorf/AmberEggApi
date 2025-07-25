using AmberEggApi.Contracts.CommandHandlers;
using AmberEggApi.Contracts.Repositories;
using AmberEggApi.Domain.Commands;
using AmberEggApi.Domain.Models;

namespace AmberEggApi.Domain.CommandHandlers;
public class DeletePersonaCommandHandler(IRepository<Persona> repository, IUnitOfWork unitOfWork) :
    GenericDeleteCommandHandle<DeletePersonaCommand, Persona>(repository, unitOfWork)
{    
}