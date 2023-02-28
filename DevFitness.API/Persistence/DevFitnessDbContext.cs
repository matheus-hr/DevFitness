using DevFitness.API.Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace DevFitness.API.Persistence
{
    public class DevFitnessDbContext : DbContext
    {
        public DevFitnessDbContext(DbContextOptions<DevFitnessDbContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<Meal> Meals { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>(e =>
            {
                e.ToTable("Users", (string)null);
                e.HasKey(u => u.Id).HasAnnotation("Sqlite:Autoincrement", "Autoincrement");

                e.HasMany(u => u.Meals)
                    .WithOne()
                    .HasForeignKey(m => m.UserId)
                    .OnDelete(DeleteBehavior.Restrict); 
            });

            modelBuilder.Entity<Meal>(e =>
            {
                e.ToTable("Meals", (string)null);
                e.HasKey(m => m.Id).HasAnnotation("Sqlite:Autoincrement", "Autoincrement"); ;
            });
        }
    }
}
