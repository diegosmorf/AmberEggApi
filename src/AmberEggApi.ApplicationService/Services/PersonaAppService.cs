using AmberEggApi.ApplicationService.Interfaces;
using AmberEggApi.ApplicationService.ViewModels;
using AmberEggApi.Domain.Commands;
using AmberEggApi.Domain.Models;
using Api.Common.Cqrs.Core.Commands;
using Api.Common.Repository.Repositories;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AmberEggApi.ApplicationService.Services
{
    public class PersonaAppService(
        ICommandProducer producer,
        IMapper mapper,
        IRepository<Persona> repository) : IPersonaAppService
    {
        private readonly IRepository<Persona> repository = repository;
        private readonly ICommandProducer producer = producer;
        private readonly IMapper mapper = mapper;

        public async Task<IEnumerable<PersonaViewModel>> GetAll()
        {
            var list = await repository.ListAll();

            return mapper.Map<IEnumerable<PersonaViewModel>>(list);
        }

        public async Task<IEnumerable<PersonaViewModel>> GetListByName(string name)
        {
            var list = await repository.SearchList(x => x.Name.Contains(name));

            return mapper.Map<IEnumerable<PersonaViewModel>>(list).OrderBy(x => x.Name);
        }

        public async Task<PersonaViewModel> Get(Guid id)
        {
            var instance = await repository.SearchById(id);

            return mapper.Map<PersonaViewModel>(instance);
        }

        public async Task<PersonaViewModel> Create(CreatePersonaCommand command)
        {
            //send command to broker
            var result = await producer.Send<CreatePersonaCommand, Persona>(command);

            return mapper.Map<PersonaViewModel>(result);
        }

        public async Task<PersonaViewModel> Update(UpdatePersonaCommand command)
        {
            //send command to broker
            var result = await producer.Send<UpdatePersonaCommand, Persona>(command);

            return mapper.Map<PersonaViewModel>(result);
        }

        public async Task Delete(DeletePersonaCommand commandDelete)
        {
            //send command to broker
            await producer.Send<DeletePersonaCommand, Persona>(commandDelete);
        }
    }
}