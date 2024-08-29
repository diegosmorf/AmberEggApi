using AmberEggApi.Domain.Events;
using AmberEggApi.Domain.QueryModels;
using Api.Common.Cqrs.Core.Events;
using Api.Common.Repository.Repositories;
using System.Threading.Tasks;

namespace AmberEggApi.Domain.EventHandlers
{
    public class PersonaCreatedEventHandler(IRepository<PersonaQueryModel> repository, IUnitOfWork unitOfWork) : IEventHandler<PersonaCreatedEvent>
    {
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