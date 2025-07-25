using AmberEggApi.Contracts.Entities;

using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace AmberEggApi.Domain.Models;

public class Artist : DomainEntity
{
    [MinLength(2)]
    [MaxLength(20)]
    [Required]
    public required string Name { get; set; }
    [MinLength(2)]
    [MaxLength(20)]
    [Required]
    public required string Biography { get; set; }
    [Required]
    public required int TrackNumber { get; set; }    
    [Required]
    // Navigation properties
    public required virtual ICollection<Album> Albums { get; set; }
}