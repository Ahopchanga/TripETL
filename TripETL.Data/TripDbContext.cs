using Microsoft.EntityFrameworkCore;
using TripETL.Domain.Entities;

namespace TripETL.Data;

public class TripDbContext : DbContext
{
    public DbSet<Trip> Trips { get; set; }

    public TripDbContext(DbContextOptions<TripDbContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Trip>()
            .HasIndex(t => new { t.TpepPickupDatetime, t.TpepDropoffDatetime, t.PassengerCount })
            .IsUnique(true);

        modelBuilder.Entity<Trip>()
            .Property(t => t.StoreAndFwdFlag)
            .HasConversion(
                v => v == "Y" ? "Yes" : "No",
                v => v == "Yes" ? "Y" : "N");
    }
}