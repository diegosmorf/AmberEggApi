using AmberEggApi.Domain.Commands;
using AmberEggApi.Domain.Models;
using Api.Common.Cqrs.Core.CommandHandlers;
using Api.Common.Repository.Repositories;

namespace AmberEggApi.Domain.CommandHandlers
{
    public class UpdatePersonaCommandHandler :
        ICommandHandler<UpdatePersonaCommand, Persona>
    {
        private readonly IRepository<Persona> repository;
        private readonly IUnitOfWork unitOfWork;

        public UpdatePersonaCommandHandler(IRepository<Persona> repository, IUnitOfWork unitOfWork)
        {
            this.repository = repository;
            this.unitOfWork = unitOfWork;
        }

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