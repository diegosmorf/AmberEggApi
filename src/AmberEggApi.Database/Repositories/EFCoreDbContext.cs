using AmberEggApi.Database.Mappings;
using Microsoft.EntityFrameworkCore;

namespace AmberEggApi.Database.Repositories
{
    public class EfCoreDbContext(DbContextOptions<EfCoreDbContext> options) : DbContext(options)
    {
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //domain
            modelBuilder.ApplyConfiguration(new PersonaMapConfig());
        }
    }
}