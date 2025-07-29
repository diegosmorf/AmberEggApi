using AmberEggApi.Contracts.Commands;

using System.ComponentModel.DataAnnotations;

namespace AmberEggApi.Domain.Commands;
public class CreatePersonaCommand(string name) : ICommand
{
    [MinLength(2)]
    [MaxLength(20)]
    [Required]
    public string Name { get; set; } = name;
}