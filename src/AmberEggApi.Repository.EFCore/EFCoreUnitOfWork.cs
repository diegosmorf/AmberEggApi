using AmberEggApi.Contracts.Repositories;

using Microsoft.EntityFrameworkCore;

using System.Threading;
using System.Threading.Tasks;

namespace AmberEggApi.Repository.EFCore
{
    public class EFCoreUnitOfWork(DbContext context) : IUnitOfWork
    {
        private readonly DbContext context = context;

        public async Task Commit(CancellationToken cancellationToken)
        {
            await context.SaveChangesAsync(cancellationToken);
        }
    }
}