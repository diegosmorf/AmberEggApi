using AmberEggApi.Contracts.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AmberEggApi.Repository.Repositories
{
    public interface IPersistenceService<in TEntity> where TEntity : IDomainEntity
    {
        Task Insert(TEntity instance);

        Task Insert(IEnumerable<TEntity> instances);

        Task Delete(Guid id);

        Task Update(TEntity instance);

        Task Update(IEnumerable<TEntity> instances);
    }
}