using AmberEggApi.Domain.EventHandlers;
using AmberEggApi.Domain.Events;
using AmberEggApi.Domain.Models;
using AmberEggApi.Domain.QueryModels;
using Api.Common.Repository.Repositories;
using Autofac;
using FluentAssertions;
using NUnit.Framework;
using System;
using System.Threading.Tasks;

namespace AmberEggApi.DomainTests.Tests
{
    public class PersonaDeletedEventHandlerTest
    {


        [TestCase("")]
        [TestCase(" ")]
        [TestCase("Test")]
        [TestCase("Test Message")]
        [TestCase("Test Message 1231231232145654654 78979789 465465456 21321321231")]
        public async Task WhenPersonaDeletedEventHandler_NewGuid_Then_Success(string expectedMessage)
        {
            // arrange
            var repository = SetupTests.Container.Resolve<IRepository<PersonaQueryModel>>();
            var unitOfWork = SetupTests.Container.Resolve<IUnitOfWork>();
            var handler = new PersonaDeletedEventHandler(repository, unitOfWork);

            // act
            var persona = new Persona();
            persona.Create(new Domain.Commands.CreatePersonaCommand(expectedMessage));
            var personaDeleted = new PersonaDeletedEvent(persona, Guid.NewGuid());
            await handler.Handle(personaDeleted);
            //assert
            personaDeleted.Persona.Should().NotBeNull();
        }
    }
}