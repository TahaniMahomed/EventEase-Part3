namespace Event_Ease2.Data
{
    using Event_Ease2.Models;
    using Microsoft.EntityFrameworkCore;
    using Event_Ease2.Models; // Change to your project name

    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<Venue> Venues { get; set; }
        public DbSet<Event> Events { get; set; }
        public DbSet<Booking> Bookings { get; set; }
    }
}
