using Api.Common.Contracts.Entities;

namespace Api.Common.Repository.Repositories
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