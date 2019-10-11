using System.ComponentModel.DataAnnotations;
using Api.Common.Repository.Entities;

namespace AmberEggApi.Database.Models
{
    public class DatabaseVersion : AggregateRootBase
    {
        [MinLength(2)]
        [MaxLength(255)]
        [Required]
        public string Name { get; set; }

    }
}