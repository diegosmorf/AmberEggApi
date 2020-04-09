using AmberEggApi.Domain.Models;
using System;
using System.Threading.Tasks;

namespace AmberEggApi.Domain.Interfaces
{
    public interface ICompanyRepository
    {
        Task<Company> FindByIdAsync(Guid id);
    }
}