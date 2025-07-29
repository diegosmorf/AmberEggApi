using AmberEggApi.Contracts.Entities;
using AmberEggApi.Contracts.Repositories;

using Microsoft.EntityFrameworkCore;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace AmberEggApi.Repository.EFCore;
public class EfCoreRepository<TEntity> : IRepository<TEntity> where TEntity : DomainEntity
{
    protected readonly DbContext context;
    protected readonly DbSet<TEntity> dbSet;

    public EfCoreRepository(DbContext context)
    {
        this.context = context;
        this.context.ChangeTracker.AutoDetectChangesEnabled = false;
        this.context.ChangeTracker.LazyLoadingEnabled = false;
        this.context.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
        dbSet = this.context.Set<TEntity>();
    }

    public async Task<IEnumerable<TEntity>> ListAll(CancellationToken cancellationToken)
    {
        return await dbSet.ToArrayAsync(cancellationToken);
    }

    public async Task Delete(Guid id, CancellationToken cancellationToken)
    {
        await DeleteInstance(id, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);
    }

    public async Task<TEntity> Search(Expression<Func<TEntity, bool>> expression, CancellationToken cancellationToken)
    {
        return await dbSet.FirstOrDefaultAsync(expression, cancellationToken);
    }

    public async Task<TEntity> SearchById(Guid id, CancellationToken cancellationToken)
    {
        return await dbSet.FindAsync(keyValues: [id], cancellationToken: cancellationToken);
    }

    public async Task<IEnumerable<TEntity>> SearchList(Expression<Func<TEntity, bool>> expression, CancellationToken cancellationToken)
    {
        return await dbSet.Where(expression).ToArrayAsync(cancellationToken);
    }

    public async Task Insert(TEntity instance, CancellationToken cancellationToken)
    {
        await InsertInstance(instance, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);
    }

    public async Task Insert(IEnumerable<TEntity> instances, CancellationToken cancellationToken)
    {
        foreach (var instance in instances)
        {
            await InsertInstance(instance, cancellationToken);
        }

        if (instances.Any())
        {
            await context.SaveChangesAsync(cancellationToken);
        }
    }

    private async Task InsertInstance(TEntity instance, CancellationToken cancellationToken)
    {
        instance.Create();            

        await dbSet.AddAsync(instance, cancellationToken);
    }

    public async Task Update(TEntity instance, CancellationToken cancellationToken)
    {   
        UpdateInstance(instance);
        await context.SaveChangesAsync(cancellationToken);
    }

    public async Task Update(IEnumerable<TEntity> instances, CancellationToken cancellationToken)
    {
        foreach (var instance in instances)
        {
            UpdateInstance(instance);
        }

        if (instances.Any())
        {
            await context.SaveChangesAsync(cancellationToken);
        }
    }

    private async Task DeleteInstance(Guid id, CancellationToken cancellationToken)
    {
        var instance = await SearchById(id, cancellationToken);

        if (instance != null)
            DeleteInstance(instance);
    }

    private void DeleteInstance(TEntity instance)
    {
        dbSet.Remove(instance);            
    }

    private void UpdateInstance(TEntity instance)
    {
        instance.Update();
        dbSet.Update(instance);            
    }
}