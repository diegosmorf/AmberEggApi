using AmberEggApi.ApplicationService.ViewModels;
using AmberEggApi.Domain.Commands;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace AmberEggApi.Integration.Tests.Factories
{
    public interface IIntegrationFactoryTest
    {
        Task<PersonaViewModel> Create();
        Task<PersonaViewModel> Create(CreatePersonaCommand command);
        Task<HttpResponseMessage> Delete(Guid id);
        Task<PersonaViewModel> Get(Guid id);
        Task<IEnumerable<PersonaViewModel>> GetAll();
        Task<HttpResponseMessage> UpdateRequest(PersonaViewModel viewModel);
        Task<PersonaViewModel> Update(PersonaViewModel viewModel);
        Task DeleteAll();
    }
}