/* S-CODE ATTRIBUTION
TITLE: Configuring DbContext and managing schema snapshots
AUTHOR: Microsoft Corporation
DATE: 3 June 2026
VERSION: No version specified
AVAILABLE: https://learn.microsoft.com/en-us/ef/core/dbcontext-configuration/
*/

using Event_Ease2.Models;
using Microsoft.EntityFrameworkCore;

namespace Event_Ease2.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<Venue> Venues { get; set; }
        public DbSet<Event> Events { get; set; }
        public DbSet<Booking> Bookings { get; set; }

        // --- PART 3 ENHANCEMENT: EVENT TYPE LOOKUP DBSET ---
        public DbSet<EventType> EventTypes { get; set; }

        // Seeding predefined lookup categories as required by Part 3 Section A
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<EventType>().HasData(
                new EventType { EventTypeID = 1, EventTypeName = "Conference & Seminar" },
                new EventType { EventTypeID = 2, EventTypeName = "Wedding & Reception" },
                new EventType { EventTypeID = 3, EventTypeName = "Concert & Live Show" },
                new EventType { EventTypeID = 4, EventTypeName = "Corporate Banquet" },
                new EventType { EventTypeID = 5, EventTypeName = "Social Gathering / Party" }
            );
        }
    }
}