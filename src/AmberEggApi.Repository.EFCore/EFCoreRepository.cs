using AmberEggApi.Repository.Entities;
using AmberEggApi.Repository.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
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

    public async Task<IEnumerable<TEntity>> ListAll()
    {
        return await dbSet.ToArrayAsync();
    }

    public async Task Delete(Guid id)
    {
        await DeleteInstance(id);
        await context.SaveChangesAsync();
    }

    public async Task<TEntity> Search(Expression<Func<TEntity, bool>> expression)
    {
        return await dbSet.FirstOrDefaultAsync(expression);
    }

    public async Task<TEntity> SearchById(Guid id)
    {
        return await dbSet.FindAsync(id);
    }

    public async Task<IEnumerable<TEntity>> SearchList(Expression<Func<TEntity, bool>> expression)
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
        instance.Create();            

        await dbSet.AddAsync(instance);
    }

    public async Task Update(TEntity instance)
    {   
        UpdateInstance(instance);
        await context.SaveChangesAsync();
    }

    public async Task Update(IEnumerable<TEntity> instances)
    {
        foreach (var instance in instances)
        {
            UpdateInstance(instance);
        }

        if (instances.Any())
        {
            await context.SaveChangesAsync();
        }
    }

    private async Task DeleteInstance(Guid id)
    {
        var instance = await SearchById(id);

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