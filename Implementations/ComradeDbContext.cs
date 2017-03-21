using Microsoft.EntityFrameworkCore;
using Dsa.RapidResponse.Services;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace Dsa.RapidResponse.Implementations
{
    public class ComradeDbContext : IdentityDbContext
    {
        public ComradeDbContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<Availability> Availabilities { get; set; }
        public DbSet<Event> Events { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Availability>().ToTable("Availability");
        }
    }
}