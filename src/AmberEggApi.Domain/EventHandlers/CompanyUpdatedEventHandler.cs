using AmberEggApi.Domain.Events;
using AmberEggApi.Domain.QueryModels;
using Api.Common.Cqrs.Core.Events;
using Api.Common.Repository.Repositories;
using System.Threading.Tasks;

namespace AmberEggApi.Domain.EventHandlers
{
    public class CompanyUpdatedEventHandler : IEventHandler<CompanyUpdatedEvent>
    {
        private readonly IRepository<CompanyQueryModel> repository;
        private readonly IUnitOfWork unitOfWork;

        public CompanyUpdatedEventHandler(IRepository<CompanyQueryModel> repository, IUnitOfWork unitOfWork)
        {
            this.repository = repository;
            this.unitOfWork = unitOfWork;
        }

        public async Task Handle(CompanyUpdatedEvent @event)
        {
            //Domain Changes
            var instance = await repository.FindById(@event.Company.Id);
            instance.Update(@event);

            //Persistence
            await repository.Update(instance);
            await unitOfWork.Commit();
        }
    }
}