using Api.Common.Repository.Entities;
using System.ComponentModel.DataAnnotations;

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