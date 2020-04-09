using AmberEggApi.Domain.Commands;
using AmberEggApi.Domain.Models;
using Api.Common.Cqrs.Core.CommandHandlers;
using Api.Common.Repository.Repositories;
using System.Threading.Tasks;

namespace AmberEggApi.Domain.CommandHandlers
{
    public class DeleteCompanyCommandHandler :
        ICommandHandler<DeleteCompanyCommand, Company>
    {
        private readonly IRepository<Company> repository;
        private readonly IUnitOfWork unitOfWork;

        public DeleteCompanyCommandHandler(IRepository<Company> repository, IUnitOfWork unitOfWork)
        {
            this.repository = repository;
            this.unitOfWork = unitOfWork;
        }

        public async Task<Company> Handle(DeleteCompanyCommand command)
        {
            //Domain
            var instance = await repository.FindById(command.Id);
            instance.Delete(command);

            //Persistence
            await repository.Delete(command.Id);

            await unitOfWork.Commit();

            return instance;
        }
    }
}