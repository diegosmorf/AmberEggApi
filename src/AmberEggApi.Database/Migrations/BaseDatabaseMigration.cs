using System.Threading.Tasks;
using Api.Common.Repository.Repositories;

namespace AmberEggApi.Database.Migrations
{
    public abstract class BaseDatabaseMigration : IDatabaseMigration
    {
        public string Name => GetType().FullName;
        public abstract Task Up();
    }
}