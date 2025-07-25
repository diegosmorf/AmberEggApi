using AmberEggApi.Domain.Models;
using System.Threading.Tasks;
using AmberEggApi.Contracts.CommandHandlers;
using AmberEggApi.Contracts.Repositories;
using AmberEggApi.Domain.Commands.Persona;

namespace AmberEggApi.Domain.CommandHandlers;
public class DeletePersonaCommandHandler(IRepository<Persona> repository, IUnitOfWork unitOfWork) :
    ICommandHandler<DeletePersonaCommand, Persona>
{
    public async Task<Persona> Handle(DeletePersonaCommand command)
    {
        //Domain
        var instance = await repository.SearchById(command.Id);
        //Persistence
        await repository.Delete(command.Id);

        await unitOfWork.Commit();

        return instance;
    }
}