using Api.Common.Repository.Repositories;
using System.Threading.Tasks;

namespace Api.Common.Repository.MongoDb
{
    public sealed class MongoDbUnitOfWork : IUnitOfWork
    {
        private readonly IMongoDbContext context;

        public MongoDbUnitOfWork(IMongoDbContext context)
        {
            this.context = context;
        }

        public async Task Commit()
        {
            await context.SaveChanges();
        }

        public void Dispose()
        {
            // Cleanup
            context.Dispose();
        }
    }
}