using System;
using System.Threading.Tasks;
using AmberEggApi.Domain.Models;

namespace AmberEggApi.Domain.Interfaces
{
    public interface ICompanyRepository
    {
        Task<Company> FindByIdAsync(Guid id);
    }
}