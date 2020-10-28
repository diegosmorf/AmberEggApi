using AmberEggApi.Domain.Commands;
using AmberEggApi.Domain.Models;
using Api.Common.Cqrs.Core.CommandHandlers;
using Api.Common.Repository.Repositories;
using System.Threading.Tasks;

namespace AmberEggApi.Domain.CommandHandlers
{
    public class DeletePersonaCommandHandler :
        ICommandHandler<DeletePersonaCommand, Persona>
    {
        private readonly IRepository<Persona> repository;
        private readonly IUnitOfWork unitOfWork;

        public DeletePersonaCommandHandler(IRepository<Persona> repository, IUnitOfWork unitOfWork)
        {
            this.repository = repository;
            this.unitOfWork = unitOfWork;
        }

        public async Task<Persona> Handle(DeletePersonaCommand command)
        {
            //Domain
            var instance = await repository.SearchById(command.Id);
            instance.Delete(command);

            //Persistence
            await repository.Delete(command.Id);

            await unitOfWork.Commit();

            return instance;
        }
    }
}