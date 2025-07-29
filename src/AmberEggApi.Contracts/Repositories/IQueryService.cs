using AmberEggApi.Contracts.Entities;

using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace AmberEggApi.Contracts.Repositories;

public interface IQueryService<TEntity> where TEntity : IDomainEntity
{
    Task<TEntity> SearchById(Guid id, CancellationToken cancellationToken);

    Task<TEntity> Search(Expression<Func<TEntity, bool>> expression, CancellationToken cancellationToken);

    Task<IEnumerable<TEntity>> ListAll(CancellationToken cancellationToken);

    Task<IEnumerable<TEntity>> SearchList(Expression<Func<TEntity, bool>> expression, CancellationToken cancellationToken);
}