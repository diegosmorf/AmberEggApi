using System.Threading;
using System.Threading.Tasks;

namespace AmberEggApi.Contracts.Repositories;

public interface IUnitOfWork
{
    Task Commit(CancellationToken cancellationToken);
}