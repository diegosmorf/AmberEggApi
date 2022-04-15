using AmberEggApi.Domain.Events;
using AmberEggApi.Domain.QueryModels;
using Api.Common.Cqrs.Core.Events;
using Api.Common.Repository.Repositories;
using System.Threading.Tasks;

namespace AmberEggApi.Domain.EventHandlers
{
    public class PersonaCreatedEventHandler : IEventHandler<PersonaCreatedEvent>
    {
        private readonly IRepository<PersonaQueryModel> repository;
        private readonly IUnitOfWork unitOfWork;

        public PersonaCreatedEventHandler(IRepository<PersonaQueryModel> repository, IUnitOfWork unitOfWork)
        {
            this.repository = repository;
            this.unitOfWork = unitOfWork;
        }

        public async Task Handle(PersonaCreatedEvent @event)
        {
            //Domain Changes
            var instance = new PersonaQueryModel();
            instance.Create(@event);

            //Persistence
            await repository.Insert(instance);
            await unitOfWork.Commit();
        }
    }
}