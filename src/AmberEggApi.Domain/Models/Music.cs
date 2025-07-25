using AmberEggApi.Contracts.Entities;

using System;
using System.ComponentModel.DataAnnotations;

namespace AmberEggApi.Domain.Models;

public class Music : DomainEntity
{
    [MinLength(2)]
    [MaxLength(20)]
    [Required]
    public required string Title { get; set; }
    [Required]
    public required TimeSpan Duration { get; set; }    
    [Required]
    public required int TrackNumber { get; set; }
    [Required]
    // Foreign key
    public required int AlbumId { get; set; }
    [Required]
    // Navigation properties
    public required virtual Album Album { get; set; }    
}
