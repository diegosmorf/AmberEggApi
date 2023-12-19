using Api.Common.Repository.Repositories;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace Api.Common.Repository.EFCore
{
    public class EFCoreUnitOfWork(DbContext context) : IUnitOfWork
    {
        private readonly DbContext context = context;

        public async Task Commit()
        {
            await context.SaveChangesAsync();
        }
    }
}