using AmberEggApi.Domain.Commands;
using Api.Common.Repository.Entities;
using System.ComponentModel.DataAnnotations;

namespace AmberEggApi.Domain.Models;

public class Persona : DomainEntity
{
    [MinLength(2)]
    [MaxLength(20)]
    [Required]
    public string Name { get; private set; }

    public void Create(CreatePersonaCommand command)
    {
        Name = command.Name;
        Version++;
    }

    public void Update(UpdatePersonaCommand command)
    {
        Name = command.Name;
        Version++;
    }
}