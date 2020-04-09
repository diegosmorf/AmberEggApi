using AmberEggApi.Domain.Events;
using AmberEggApi.Domain.QueryModels;
using Api.Common.Cqrs.Core.Events;
using Api.Common.Repository.Repositories;
using System.Threading.Tasks;

namespace AmberEggApi.Domain.EventHandlers
{
    public class CompanyDeletedEventHandler : IEventHandler<CompanyDeletedEvent>
    {
        private readonly IRepository<CompanyQueryModel> repository;
        private readonly IUnitOfWork unitOfWork;

        public CompanyDeletedEventHandler(IRepository<CompanyQueryModel> repository, IUnitOfWork unitOfWork)
        {
            this.repository = repository;
            this.unitOfWork = unitOfWork;
        }

        public async Task Handle(CompanyDeletedEvent @event)
        {
            //Domain Changes
            var instance = await repository.FindById(@event.Company.Id);

            //Persistence
            await repository.Delete(instance.Id);
            await unitOfWork.Commit();
        }
    }
}