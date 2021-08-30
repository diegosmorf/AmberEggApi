using AmberEggApi.Domain.Events;
using AmberEggApi.Domain.QueryModels;
using Api.Common.Cqrs.Core.Events;
using Api.Common.Repository.Repositories;

namespace AmberEggApi.Domain.EventHandlers
{
    public class PersonaDeletedEventHandler : IEventHandler<PersonaDeletedEvent>
    {
        private readonly IRepository<PersonaQueryModel> repository;
        private readonly IUnitOfWork unitOfWork;

        public PersonaDeletedEventHandler(IRepository<PersonaQueryModel> repository, IUnitOfWork unitOfWork)
        {
            this.repository = repository;
            this.unitOfWork = unitOfWork;
        }

        public async Task Handle(PersonaDeletedEvent @event)
        {
            //Domain Changes
            var instance = await repository.SearchById(@event.Persona.Id);


            if (instance == null)
            {
                await Task.CompletedTask;
                return;
            }

            //Persistence
            await repository.Delete(instance.Id);
            await unitOfWork.Commit();
        }
    }
}