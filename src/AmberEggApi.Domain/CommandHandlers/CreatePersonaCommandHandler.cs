using AmberEggApi.Domain.Commands;
using AmberEggApi.Domain.Models;
using Api.Common.Cqrs.Core.CommandHandlers;
using Api.Common.Repository.Repositories;
using System.Threading.Tasks;

namespace AmberEggApi.Domain.CommandHandlers
{
    public class CreatePersonaCommandHandler :
        ICommandHandler<CreatePersonaCommand, Persona>
    {
        private readonly IRepository<Persona> repository;
        private readonly IUnitOfWork unitOfWork;

        public CreatePersonaCommandHandler(IRepository<Persona> repository, IUnitOfWork unitOfWork)
        {
            this.repository = repository;
            this.unitOfWork = unitOfWork;
        }

        public async Task<Persona> Handle(CreatePersonaCommand command)
        {
            //Domain
            var instance = new Persona();
            instance.Create(command);

            //Persistence
            await repository.Insert(instance);

            //Commit
            await unitOfWork.Commit();

            return instance;
        }
    }
}