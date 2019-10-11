using System.Threading.Tasks;

namespace Api.Common.Repository.Repositories
{
    public interface IDatabaseMigration
    {
        Task Up();
        string Name { get; }
    }
}