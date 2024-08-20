using Api.Common.Cqrs.Core.Commands;
using System.ComponentModel.DataAnnotations;

namespace AmberEggApi.Domain.Commands
{
    public class CreatePersonaCommand(string name) : Command
    {
        [MinLength(2)]
        [MaxLength(255)]
        [Required(AllowEmptyStrings = false)]
        public string Name { get; set; } = name;
    }
}