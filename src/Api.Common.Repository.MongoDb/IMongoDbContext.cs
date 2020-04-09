using MongoDB.Driver;
using System;
using System.Threading.Tasks;

namespace Api.Common.Repository.MongoDb
{
    public interface IMongoDbContext : IDisposable
    {
        Task AddCommand(Func<Task> func);
        Task SaveChanges();
        IMongoCollection<T> GetCollection<T>(string name);
    }
}