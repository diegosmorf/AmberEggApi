using AmberEggApi.ApplicationService.ViewModels;
using AmberEggApi.Domain.Models;
using AutoMapper;

namespace AmberEggApi.ApplicationService.Mappings
{
    public class DomainToViewModelMapping : Profile
    {
        public DomainToViewModelMapping()
        {
            CreateMap<Company, CompanyViewModel>();
        }
    }
}