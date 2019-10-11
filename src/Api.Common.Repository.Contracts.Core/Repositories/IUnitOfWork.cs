using System;
using System.Threading.Tasks;

namespace Api.Common.Repository.Repositories
{
    public interface IUnitOfWork : IDisposable
    {
        Task Commit();
    }
}