using AmberEggApi.Domain.QueryModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AmberEggApi.Database.Mappings
{
    public class PersonaQueryMapConfig : IEntityTypeConfiguration<PersonaQueryModel>
    {
        public void Configure(EntityTypeBuilder<PersonaQueryModel> builder)
        {
            builder.ToTable("PersonaQuery");
        }

    }
}