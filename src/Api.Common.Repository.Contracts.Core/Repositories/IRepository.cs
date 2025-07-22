using Api.Common.Contracts.Entities;

namespace Api.Common.Repository.Repositories;

public interface IRepository<TEntity> :
    IPersistenceService<TEntity>,
    IQueryService<TEntity> where TEntity : IDomainEntity
{
}