using Api.Common.Repository.Repositories;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace Api.Common.Repository.EFCore
{
    public class EFCoreUnitOfWork : IUnitOfWork
    {
        private readonly DbContext context;

        public EFCoreUnitOfWork(DbContext context)
        {
            this.context = context;
        }


        public async Task Commit()
        {
            await context.SaveChangesAsync();            
        }

        public void Dispose()
        {
            // Cleanup            
            Dispose(true);            
        }

        protected virtual void Dispose(bool disposing)
        {
            // Cleanup              
            context?.Dispose();
        }        
    }
}