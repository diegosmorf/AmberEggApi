using Api.Common.Repository.Entities;
using Api.Common.Repository.Repositories;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Api.Common.Repository.MongoDb
{
    public class MongoDbRepository<TEntity> : IRepository<TEntity> where TEntity : AggregateRootBase
    {
        protected readonly IMongoDbContext context;
        protected readonly IMongoCollection<TEntity> dbSet;

        public MongoDbRepository(IMongoDbContext context)
        {
            this.context = context;
            dbSet = context.GetCollection<TEntity>(typeof(TEntity).Name);
        }

        public void Dispose()
        {
            // Cleanup
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public async Task<IEnumerable<TEntity>> All()
        {
            var list = await dbSet.FindAsync(Builders<TEntity>.Filter.Empty);
            return list.ToList();
        }

        public async Task Delete(IEnumerable<Guid> ids)
        {
            foreach (var id in ids)
            {
                await DeleteInstance(id);
            }
        }

        public async Task Delete(Guid id)
        {
            await DeleteInstance(id);
        }

        public async Task Delete(Expression<Func<TEntity, bool>> expression)
        {
            await context.AddCommand(() => dbSet.DeleteManyAsync(expression));
        }

        public async Task<TEntity> Find(Expression<Func<TEntity, bool>> expression)
        {
            var list = await dbSet.FindAsync(expression);
            return list.FirstOrDefault();
        }

        public async Task<TEntity> FindById(Guid id)
        {
            var list = await dbSet.FindAsync(Builders<TEntity>.Filter.Eq("Id", id));
            return list.FirstOrDefault();
        }

        public async Task<IEnumerable<TEntity>> FindList(Expression<Func<TEntity, bool>> expression)
        {
            var list = await dbSet.FindAsync(expression);
            return list.ToList();
        }

        public async Task Insert(TEntity instance)
        {
            if (instance.Id == Guid.Empty)
            {
                instance.Id = Guid.NewGuid();
            }

            if (instance.Version == 0)
            {
                instance.Version = 1;
            }

            if (instance.CreateDate == DateTime.MinValue)
            {
                instance.CreateDate = DateTime.UtcNow;
            }

            await context.AddCommand(() => dbSet.InsertOneAsync(instance));
        }

        public async Task Insert(IEnumerable<TEntity> instances)
        {
            foreach (var instance in instances)
            {
                await Insert(instance);
            }
        }

        public async Task Update(TEntity instance)
        {
            await UpdateInstance(instance);
        }

        public async Task Update(IEnumerable<TEntity> instances)
        {
            foreach (var instance in instances)
            {
                await UpdateInstance(instance);
            }
        }

        protected virtual void Dispose(bool disposing)
        {
            // Cleanup
            context.Dispose();
        }

        private async Task DeleteInstance(Guid id)
        {
            await context.AddCommand(() => dbSet.DeleteOneAsync(Builders<TEntity>.Filter.Eq("Id", id)));
        }

        private async Task UpdateInstance(TEntity instance)
        {
            instance.Version++;
            instance.ModifiedDate = DateTime.UtcNow;
            await context.AddCommand(() =>
                dbSet.ReplaceOneAsync(Builders<TEntity>.Filter.Eq("Id", instance.Id), instance));
        }
    }
}