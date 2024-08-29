using AmberEggApi.Domain.Commands;
using AmberEggApi.Domain.Models;
using Api.Common.Cqrs.Core.CommandHandlers;
using Api.Common.Repository.Repositories;
using System.Threading.Tasks;

namespace AmberEggApi.Domain.CommandHandlers
{
    public class CreatePersonaCommandHandler(IRepository<Persona> repository, IUnitOfWork unitOfWork) :
        ICommandHandler<CreatePersonaCommand, Persona>
    {
        private readonly IRepository<Persona> repository = repository;
        private readonly IUnitOfWork unitOfWork = unitOfWork;

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