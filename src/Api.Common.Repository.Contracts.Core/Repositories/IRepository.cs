using System;
using Api.Common.Contracts.Entities;

namespace Api.Common.Repository.Repositories
{
    public interface IRepository<TEntity> :
        IPersistenceService<TEntity>,
        IQueryService<TEntity>, IDisposable where TEntity : IDomainEntity
    {
    }
}