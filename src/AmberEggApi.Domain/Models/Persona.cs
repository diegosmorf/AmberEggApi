using AmberEggApi.Contracts.Entities;
using AmberEggApi.Domain.Commands;

using System.ComponentModel.DataAnnotations;

namespace AmberEggApi.Domain.Models;

public class Persona : DomainEntity
{
    [MinLength(2)]
    [MaxLength(20)]
    [Required]
    public string Name { get; private set; }
}