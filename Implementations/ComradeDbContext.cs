using Microsoft.EntityFrameworkCore;
using Dsa.RapidResponse.Services;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace Dsa.RapidResponse.Implementations
{
    public class ComradeDbContext : IdentityDbContext
    {
        /*public ComradeDbContext(DbContextOptions<ComradeDbContext> options) : base(options)
        {
        }*/
        public ComradeDbContext(DbContextOptions options) : base(options)
        {
        }

        //public DbSet<Comrade> Comrades { get; set; }
        public DbSet<Availability> Availabilities { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            //modelBuilder.Entity<Comrade>().ToTable("Comrade");
            modelBuilder.Entity<Availability>().ToTable("Availability");
        }
    }
}