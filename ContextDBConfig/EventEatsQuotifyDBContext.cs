using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using EventEatsQuotify.Models;
using Microsoft.AspNetCore.Identity;

namespace EventEatsQuotify.ContextDBConfig
{
    public class EventEatsQuotifyDBContext : IdentityDbContext<ApplicationUser>
    {
        public DbSet<FoodItem> FoodItems { get; set; }
        public EventEatsQuotifyDBContext(DbContextOptions<EventEatsQuotifyDBContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // Seed roles during database creation
            builder.Entity<IdentityRole>().HasData(new IdentityRole { Name = "Vendor", NormalizedName = "VENDOR" });
            builder.Entity<IdentityRole>().HasData(new IdentityRole { Name = "Customer", NormalizedName = "CUSTOMER" });

            // Other configurations...
        }
    }
}
