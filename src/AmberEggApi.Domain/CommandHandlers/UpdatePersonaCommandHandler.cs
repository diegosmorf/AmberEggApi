using AmberEggApi.Contracts.CommandHandlers;
using AmberEggApi.Contracts.Repositories;
using AmberEggApi.Domain.Commands;
using AmberEggApi.Domain.Models;

using AutoMapper;

namespace AmberEggApi.Domain.CommandHandlers;

public class UpdatePersonaCommandHandler(IRepository<Persona> repository, IMapper mapper, IUnitOfWork unitOfWork) 
    : GenericUpdateCommandHandle<UpdatePersonaCommand, Persona>(repository, mapper, unitOfWork)
{
}