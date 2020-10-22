using AmberEggApi.ApplicationService.ViewModels;
using AmberEggApi.Domain.QueryModels;
using AutoMapper;

namespace AmberEggApi.ApplicationService.Mappings
{
    public class QueryModelToViewModelMapping : Profile
    {
        public QueryModelToViewModelMapping()
        {
            CreateMap<PersonaQueryModel, PersonaViewModel>();
        }
    }
}