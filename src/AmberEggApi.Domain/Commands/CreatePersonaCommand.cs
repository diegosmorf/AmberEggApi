using Api.Common.Cqrs.Core.Commands;
using Api.Common.Repository.Validations;
using System.ComponentModel.DataAnnotations;

namespace AmberEggApi.Domain.Commands
{
    public class CreatePersonaCommand(string name) : Command
    {
        [MinLength(2)]
        [MaxLength(20)]
        [Required]
        [NotEmpty]
        public string Name { get; set; } = name;
    }
}