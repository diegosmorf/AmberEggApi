using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AmberEggApi.ApplicationService.ViewModels;
using AmberEggApi.Domain.Commands;

namespace AmberEggApi.ApplicationService.Interfaces
{
    public interface ICompanyAppService
    {
        Task<IEnumerable<CompanyViewModel>> GetAll();

        Task Delete(DeleteCompanyCommand commandDelete);

        Task<CompanyViewModel> Get(Guid id);

        Task<IEnumerable<CompanyViewModel>> GetListByName(string name);

        Task<CompanyViewModel> Create(CreateCompanyCommand command);

        Task<CompanyViewModel> Update(UpdateCompanyCommand command);
    }
}