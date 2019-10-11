using System.Threading.Tasks;
using Api.Common.Cqrs.Core.CommandHandlers;
using Api.Common.Repository.Repositories;
using AmberEggApi.Domain.Commands;
using AmberEggApi.Domain.Models;

namespace AmberEggApi.Domain.CommandHandlers
{
    public class UpdateCompanyCommandHandler :
        ICommandHandler<UpdateCompanyCommand, Company>
    {
        private readonly IRepository<Company> repository;
        private readonly IUnitOfWork unitOfWork;

        public UpdateCompanyCommandHandler(IRepository<Company> repository, IUnitOfWork unitOfWork)
        {
            this.repository = repository;
            this.unitOfWork = unitOfWork;
        }

        public async Task<Company> Handle(UpdateCompanyCommand command)
        {
            //Domain
            var instance = await repository.FindById(command.Id);
            instance.Update(command);

            //Persistence
            await repository.Update(instance);
            await unitOfWork.Commit();

            return instance;
        }
    }
}