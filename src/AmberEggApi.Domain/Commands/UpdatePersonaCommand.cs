using Api.Common.Cqrs.Core.Commands;
using Api.Common.Repository.Validations;
using System;
using System.ComponentModel.DataAnnotations;

namespace AmberEggApi.Domain.Commands
{
    public class UpdatePersonaCommand(Guid id, string name) : Command
    {
        [Required]
        [NotEmpty]
        public Guid Id { get; } = id;

        [MinLength(2)]
        [MaxLength(20)]
        [Required]
        [NotEmpty]
        public string Name { get; set; } = name;
    }
}