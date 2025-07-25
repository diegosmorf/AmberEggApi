using AmberEggApi.Domain.Commands;
using AmberEggApi.Domain.Models;
using AutoMapper;
using System.Threading.Tasks;
using AmberEggApi.Contracts.CommandHandlers;
using AmberEggApi.Contracts.Repositories;

namespace AmberEggApi.Domain.CommandHandlers;
public class UpdatePersonaCommandHandler(IRepository<Persona> repository, IMapper mapper, IUnitOfWork unitOfWork) :
    ICommandHandler<UpdatePersonaCommand, Persona>
{
    private readonly IMapper mapper = mapper;
    public async Task<Persona> Handle(UpdatePersonaCommand command)
    {
        //Domain
        var instance = await repository.SearchById(command.Id);            
        mapper.Map(command, instance);
        instance.Update();

        //Persistence
        await repository.Update(instance);
        await unitOfWork.Commit();

        return instance;
    }
}