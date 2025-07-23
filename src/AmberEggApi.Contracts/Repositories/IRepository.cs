using AmberEggApi.Contracts.Entities;

namespace AmberEggApi.Repository.Repositories;

public interface IRepository<TEntity> :
    IPersistenceService<TEntity>,
    IQueryService<TEntity> where TEntity : IDomainEntity
{
}