using Api.Common.Repository.Entities;
using Api.Common.Repository.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Api.Common.Repository.EFCore
{
    public class EfCoreRepository<TEntity> : IRepository<TEntity> where TEntity : DomainEntity
    {
        protected readonly DbContext context;
        protected readonly DbSet<TEntity> dbSet;

        public void Dispose()
        {
            // Cleanup
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            //Commit
            context.SaveChanges();

            // Cleanup
            context.Dispose();
        }

        public EfCoreRepository(DbContext context)
        {
            this.context = context;
            this.context.ChangeTracker.AutoDetectChangesEnabled = false;
            this.context.ChangeTracker.LazyLoadingEnabled = false;
            this.context.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
            dbSet = this.context.Set<TEntity>();
        }

        public async Task<IEnumerable<TEntity>> All()
        {
            return await dbSet.ToArrayAsync();
        }

        public async Task Delete(IEnumerable<Guid> ids)
        {
            foreach (var id in ids)
            {
                await DeleteInstance(id);
            }

            await context.SaveChangesAsync();
        }

        public async Task Delete(Guid id)
        {
            await DeleteInstance(id);
            await context.SaveChangesAsync();
        }

        public async Task Delete(Expression<Func<TEntity, bool>> expression)
        {
            var instances = await dbSet.Where(expression).ToArrayAsync();
            foreach (var instance in instances)
            {
                await DeleteInstance(instance);
            }

            if (instances.Any())
            {
                await context.SaveChangesAsync();
            }
        }

        public async Task<TEntity> Find(Expression<Func<TEntity, bool>> expression)
        {
            return await dbSet.FirstOrDefaultAsync(expression);
        }

        public async Task<TEntity> FindById(Guid id)
        {
            return await dbSet.FindAsync(id);
        }

        public async Task<IEnumerable<TEntity>> FindList(Expression<Func<TEntity, bool>> expression)
        {
            return await dbSet.Where(expression).ToArrayAsync();
        }

        public async Task Insert(TEntity instance)
        {
            await InsertInstance(instance);
            await context.SaveChangesAsync();
        }

        public async Task Insert(IEnumerable<TEntity> instances)
        {
            foreach (var instance in instances)
            {
                await InsertInstance(instance);
            }

            if (instances.Any())
            {
                await context.SaveChangesAsync();
            }
        }

        private async Task InsertInstance(TEntity instance)
        {
            instance.Id = Guid.NewGuid();
            instance.CreateDate = DateTime.UtcNow;

            await dbSet.AddAsync(instance);
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

            if (instances.Any())
            {
                await context.SaveChangesAsync();
            }
        }

        private async Task DeleteInstance(Guid id)
        {
            var instance = await FindById(id);

            if (instance != null)
                await DeleteInstance(instance);
        }

        private async Task DeleteInstance(TEntity instance)
        {
            dbSet.Remove(instance);
            await Task.CompletedTask;
        }

        private async Task UpdateInstance(TEntity instance)
        {
            instance.ModifiedDate = DateTime.UtcNow;
            dbSet.Update(instance);
            await Task.CompletedTask;
        }
    }
}