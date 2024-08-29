using Api.Common.Cqrs.Core.Commands;
using Api.Common.Repository.Validations;
using System;
using System.ComponentModel.DataAnnotations;

namespace AmberEggApi.Domain.Commands
{
    public class UpdatePersonaCommand : Command
    {
        public UpdatePersonaCommand(Guid id, string name)
        {
            Id = id;
            Name = name;
        }

        [Required]
        [NotEmpty]
        public Guid Id { get; }

        [MinLength(2)]
        [MaxLength(20)]
        [Required]
        [NotEmpty]
        public string Name { get; set; }
    }
}