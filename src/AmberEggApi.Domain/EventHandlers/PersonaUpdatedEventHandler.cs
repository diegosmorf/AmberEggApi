using AmberEggApi.Domain.Events;
using AmberEggApi.Domain.QueryModels;
using Api.Common.Cqrs.Core.Events;
using Api.Common.Repository.Repositories;
using System.Threading.Tasks;

namespace AmberEggApi.Domain.EventHandlers
{
    public class PersonaUpdatedEventHandler(IRepository<PersonaQueryModel> repository, IUnitOfWork unitOfWork) : IEventHandler<PersonaUpdatedEvent>
    {
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