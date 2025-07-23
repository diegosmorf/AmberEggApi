using Api.Common.Contracts.Entities;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Api.Common.Repository.Repositories;

public interface IQueryService<TEntity> where TEntity : IDomainEntity
{
    Task<TEntity> SearchById(Guid id);

    Task<TEntity> Search(Expression<Func<TEntity, bool>> expression);

    Task<IEnumerable<TEntity>> ListAll();

    Task<IEnumerable<TEntity>> SearchList(Expression<Func<TEntity, bool>> expression);
}