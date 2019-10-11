using AutoMapper;
using AmberEggApi.ApplicationService.ViewModels;
using AmberEggApi.Domain.Models;

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