using System;
using System.ComponentModel.DataAnnotations;
using Api.Common.Cqrs.Core.Commands;

namespace AmberEggApi.Domain.Commands
{
    public class UpdateCompanyCommand : Command
    {
        public UpdateCompanyCommand(Guid id, string name)
        {
            Id = id;
            Name = name;
        }

        [Required] public Guid Id { get; }

        [MinLength(2)]
        [MaxLength(255)]
        [Required]
        public string Name { get; protected set; }
    }
}