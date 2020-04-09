using Api.Common.Contracts.Entities;
using System;

namespace Api.Common.Repository.Repositories
{
    public interface IRepository<TEntity> :
        IPersistenceService<TEntity>,
        IQueryService<TEntity>, IDisposable where TEntity : IDomainEntity
    {
    }
}