using AmberEggApi.Domain.Commands;
using AmberEggApi.Domain.Models;
using Api.Common.Cqrs.Core.CommandHandlers;
using Api.Common.Repository.Repositories;
using System.Threading.Tasks;

namespace AmberEggApi.Domain.CommandHandlers
{
    public class UpdatePersonaCommandHandler(IRepository<Persona> repository, IUnitOfWork unitOfWork) :
        ICommandHandler<UpdatePersonaCommand, Persona>
    {
        public async Task<Persona> Handle(UpdatePersonaCommand command)
        {
            //Domain
            var instance = await repository.SearchById(command.Id);
            instance.Update(command);

            //Persistence
            await repository.Update(instance);
            await unitOfWork.Commit();

            return instance;
        }
    }
}