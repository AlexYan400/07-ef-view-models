using Microsoft.EntityFrameworkCore;
using LuckySpin.Models; // Entity models are in this namespace    

namespace LuckySpin.Services
{
    public class LuckySpinContext : DbContext
    {
        public LuckySpinContext(DbContextOptions<LuckySpinContext> options) : base(options)
        {
        }

        // DbSet properties for  entities
         public DbSet<Player> Players { get; set; }
            public DbSet<Game> Games { get; set; }
            public DbSet<Spin> Spins { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            
            // Configure your entities here
            modelBuilder.Entity<Game>()
            .Navigation(g => g.Player)
            .AutoInclude();

            modelBuilder.Entity<Game>()
            .Navigation(g => g.Spins)
            .AutoInclude();

            modelBuilder.Entity<Spin>()
            .Navigation(s => s.Game)
            .AutoInclude();
        }
    }
}