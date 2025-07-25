using AmberEggApi.ApplicationService.ViewModels;
using AmberEggApi.Domain.Commands;
using AmberEggApi.Domain.Models;

using AutoMapper;

namespace AmberEggApi.ApplicationService.Mappings;
public class DomainMapping : Profile
{
    public DomainMapping()
    {
        CreateMap<Persona, PersonaViewModel>();
        CreateMap<CreatePersonaCommand, Persona>();
        CreateMap<UpdatePersonaCommand, Persona>();
    }
}
