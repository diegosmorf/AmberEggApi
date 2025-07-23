using System.Threading.Tasks;

namespace AmberEggApi.Repository.Repositories;

public interface IUnitOfWork
{
    Task Commit();
}