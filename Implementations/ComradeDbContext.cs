using Microsoft.EntityFrameworkCore;
using Dsa.RapidResponse.Services;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace Dsa.RapidResponse.Implementations
{
    public class ComradeDbContext : IdentityDbContext<ApplicationUser>
    {
        public ComradeDbContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<Availability> Availabilities { get; set; }
        public DbSet<Event> Events { get; set; }
        public DbSet<ApplicationUser> AppUsers { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Availability>().ToTable("Availability");

            modelBuilder.Entity<EventUser>()
                .HasKey(t => new { t.EventId, t.UserId });

            modelBuilder.Entity<EventUser>()
                .HasOne(pt => pt.Event)
                .WithMany(p => p.EventUsers)
                .HasForeignKey(pt => pt.EventId);

            modelBuilder.Entity<EventUser>()
                .HasOne(pt => pt.User)
                .WithMany(t => t.EventUsers)
                .HasForeignKey(pt => pt.UserId);
        }
    }

    public class EventUser
    {
        public string UserId { get; set; }
        public ApplicationUser User { get; set; }
        public int EventId { get; set; }
        public Event Event { get; set; }
    }
}