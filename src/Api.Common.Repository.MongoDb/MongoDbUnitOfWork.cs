using System;
using System.Threading.Tasks;
using Api.Common.Repository.Repositories;

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