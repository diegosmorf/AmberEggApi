using System.Threading.Tasks;
using Api.Common.Cqrs.Core.Events;
using Api.Common.Repository.Repositories;
using AmberEggApi.Domain.Events;
using AmberEggApi.Domain.QueryModels;

namespace AmberEggApi.Domain.EventHandlers
{
    public class CompanyCreatedEventHandler : IEventHandler<CompanyCreatedEvent>
    {
        private readonly IRepository<CompanyQueryModel> repository;
        private readonly IUnitOfWork unitOfWork;

        public CompanyCreatedEventHandler(IRepository<CompanyQueryModel> repository, IUnitOfWork unitOfWork)
        {
            this.repository = repository;
            this.unitOfWork = unitOfWork;
        }

        public async Task Handle(CompanyCreatedEvent @event)
        {
            //Domain Changes
            var instance = new CompanyQueryModel();
            instance.Create(@event);

            //Persistence
            await repository.Insert(instance);
            await unitOfWork.Commit();
        }
    }
}