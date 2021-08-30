namespace Api.Common.Repository.Repositories
{
    public interface IDatabaseMigrator
    {
        Task ApplyMigrations();
    }
}