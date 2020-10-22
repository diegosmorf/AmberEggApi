using AmberEggApi.Database.Mappings;
using Microsoft.EntityFrameworkCore;

namespace AmberEggApi.Database.Repositories
{
    public class EfCoreDbContext : DbContext
    {
        public EfCoreDbContext(DbContextOptions<EfCoreDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //domain
            modelBuilder.ApplyConfiguration(new PersonaMapConfig());
        }
    }
}