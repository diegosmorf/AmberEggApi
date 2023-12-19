using AmberEggApi.Domain.Events;
using AmberEggApi.Domain.QueryModels;
using Api.Common.Cqrs.Core.Events;
using Api.Common.Repository.Repositories;
using System.Threading.Tasks;

namespace AmberEggApi.Domain.EventHandlers
{
    public class PersonaDeletedEventHandler(IRepository<PersonaQueryModel> repository, IUnitOfWork unitOfWork) : IEventHandler<PersonaDeletedEvent>
    {
        private readonly IRepository<PersonaQueryModel> repository = repository;
        private readonly IUnitOfWork unitOfWork = unitOfWork;

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