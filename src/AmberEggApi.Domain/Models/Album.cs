using AmberEggApi.Contracts.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace AmberEggApi.Domain.Models;

public class Album : DomainEntity
{
    [MinLength(2)]
    [MaxLength(20)]
    [Required]
    public required string Title { get; set; }
    [Required]
    public required DateTime ReleaseDate { get; set; }
    [MinLength(2)]
    [MaxLength(20)]
    [Required]
    public required string Genre { get; set; } 
    [Required]
    public required decimal Price { get; set; }
    [Required]
    // Foreign key
    public required int ArtistId { get; set; }
    [Required]
    // Navigation properties
    public required virtual Artist Artist { get; set; }
    [Required]
    public required virtual ICollection<Music> Musics { get; set; }
}
