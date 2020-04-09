using Api.Common.Contracts.Entities;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Api.Common.Repository.Repositories
{
    public interface IQueryService<TEntity> where TEntity : IDomainEntity
    {
        Task<TEntity> FindById(Guid id);

        Task<TEntity> Find(Expression<Func<TEntity, bool>> expression);

        Task<IEnumerable<TEntity>> All();

        Task<IEnumerable<TEntity>> FindList(Expression<Func<TEntity, bool>> expression);
    }
}