using System.Threading.Tasks;
using Api.Common.Cqrs.Core.CommandHandlers;
using Api.Common.Repository.Repositories;
using AmberEggApi.Domain.Commands;
using AmberEggApi.Domain.Models;

namespace AmberEggApi.Domain.CommandHandlers
{
    public class CreateCompanyCommandHandler :
        ICommandHandler<CreateCompanyCommand, Company>
    {
        private readonly IRepository<Company> repository;
        private readonly IUnitOfWork unitOfWork;

        public CreateCompanyCommandHandler(IRepository<Company> repository, IUnitOfWork unitOfWork)
        {
            this.repository = repository;
            this.unitOfWork = unitOfWork;
        }

        public async Task<Company> Handle(CreateCompanyCommand command)
        {
            //Domain
            var instance = new Company();
            instance.Create(command);

            //Persistence
            await repository.Insert(instance);

            //Commit
            await unitOfWork.Commit();

            return instance;
        }
    }
}