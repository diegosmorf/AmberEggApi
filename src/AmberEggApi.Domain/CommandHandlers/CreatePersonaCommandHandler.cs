using AmberEggApi.Domain.Models;
using AutoMapper;
using System.Threading.Tasks;
using AmberEggApi.Contracts.CommandHandlers;
using AmberEggApi.Contracts.Repositories;
using AmberEggApi.Domain.Commands.Persona;

namespace AmberEggApi.Domain.CommandHandlers;
public class CreatePersonaCommandHandler(IRepository<Persona> repository, IMapper mapper, IUnitOfWork unitOfWork) :
    ICommandHandler<CreatePersonaCommand, Persona>
{
    private readonly IRepository<Persona> repository = repository;
    private readonly IMapper mapper = mapper;
    private readonly IUnitOfWork unitOfWork = unitOfWork;

    public async Task<Persona> Handle(CreatePersonaCommand command)
    {
        //Domain
        var instance = mapper.Map<Persona>(command);
        //Persistence
        await repository.Insert(instance);
        //Commit
        await unitOfWork.Commit();
        return instance;
    }
}