using Api.Common.Repository.Repositories;
using AmberEggApi.Database.Models;
using Autofac;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AmberEggApi.Database.Migrators
{
    public class MongoDbMigrator : IDatabaseMigrator
    {
        private readonly IComponentContext container;
        private readonly IRepository<DatabaseVersion> repository;
        private readonly IUnitOfWork unitOfWork;

        public MongoDbMigrator(IComponentContext container, IRepository<DatabaseVersion> repository, IUnitOfWork unitOfWork)
        {
            this.container = container;
            this.repository = repository;
            this.unitOfWork = unitOfWork;
        }

        public async Task ApplyMigrations()
        {
            var migrations = container.Resolve<IEnumerable<IDatabaseMigration>>();
            var appliedMigrations = await repository.All();

            foreach (var migration in migrations)
            {
                if (appliedMigrations.Any(x => x.Name == migration.Name))
                {
                    continue;
                }

                await migration.Up();
                await unitOfWork.Commit();
            }
        }
    }
}