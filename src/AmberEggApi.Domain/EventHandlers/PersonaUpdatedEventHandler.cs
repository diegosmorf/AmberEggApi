using AmberEggApi.Domain.Events;
using AmberEggApi.Domain.QueryModels;
using Api.Common.Cqrs.Core.Events;
using Api.Common.Repository.Repositories;

namespace AmberEggApi.Domain.EventHandlers
{
    public class PersonaUpdatedEventHandler : IEventHandler<PersonaUpdatedEvent>
    {
        private readonly IRepository<PersonaQueryModel> repository;
        private readonly IUnitOfWork unitOfWork;

        public PersonaUpdatedEventHandler(IRepository<PersonaQueryModel> repository, IUnitOfWork unitOfWork)
        {
            this.repository = repository;
            this.unitOfWork = unitOfWork;
        }

        public async Task Handle(PersonaUpdatedEvent @event)
        {
            //Domain Changes
            var instance = await repository.SearchById(@event.Persona.Id);
            instance.Update(@event);

            //Persistence
            await repository.Update(instance);
            await unitOfWork.Commit();
        }
    }
}