using Api.Common.Cqrs.Core.Commands;
using Api.Common.Repository.Validations;
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

        [NotEmpty]
        [Required] 
        public Guid Id { get; set; }
    }
}