using AmberEggApi.Contracts.Entities;

namespace AmberEggApi.Contracts.Repositories;

public interface IRepository<TEntity> :
    IPersistenceService<TEntity>,
    IQueryService<TEntity> where TEntity : IDomainEntity
{
}