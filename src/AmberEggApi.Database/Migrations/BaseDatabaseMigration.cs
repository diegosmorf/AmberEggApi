using Api.Common.Repository.Repositories;
using System.Threading.Tasks;

namespace AmberEggApi.Database.Migrations
{
    public abstract class BaseDatabaseMigration : IDatabaseMigration
    {
        public string Name => GetType().Name;
        public abstract Task Up();
    }
}