using Api.Common.Cqrs.Core.Commands;
using System.ComponentModel.DataAnnotations;

namespace AmberEggApi.Domain.Commands
{
    public class CreateCompanyCommand : Command
    {
        public CreateCompanyCommand(string name)
        {
            Name = name;
        }

        [MinLength(2)]
        [MaxLength(255)]
        [Required]
        public string Name { get; }
    }
}