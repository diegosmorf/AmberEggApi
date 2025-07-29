using AmberEggApi.Contracts.Entities;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace AmberEggApi.Contracts.Repositories
{
    public interface IPersistenceService<in TEntity> where TEntity : IDomainEntity
    {
        Task Insert(TEntity instance, CancellationToken cancellationToken);

        Task Insert(IEnumerable<TEntity> instances, CancellationToken cancellationToken);

        Task Delete(Guid id, CancellationToken cancellationToken);

        Task Update(TEntity instance, CancellationToken cancellationToken);

        Task Update(IEnumerable<TEntity> instances, CancellationToken cancellationToken);
    }
}