using Api.Common.Cqrs.Core.Commands;
using System;
using System.ComponentModel.DataAnnotations;

namespace AmberEggApi.Domain.Commands
{
    public class DeletePersonaCommand : Command
    {
        public DeletePersonaCommand(Guid id)
        {
            Id = id;
        }

        [Required] public Guid Id { get; protected set; }
    }
}