using Api.Common.Cqrs.Core.Commands;
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

        [Required] public Guid Id { get; }

        [MinLength(2)]
        [MaxLength(20)]
        [Required(AllowEmptyStrings = false)]
        public string Name { get; set; }
    }
}