using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using LibraryManagementSystem.Models;

namespace LibraryManagementSystem.Data;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
    : IdentityDbContext<ApplicationUser>(options)
{
    public DbSet<Location> Location { get; set; } = default!;

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        builder.Entity<Book>()
            .HasMany(e => e.Locations)
            .WithMany(e => e.Books)
            .UsingEntity<Store>(
                 r => r.HasOne(e => e.Location).WithMany(e => e.Stores)
                     .HasForeignKey(e => new {e.LocationLevel, e.LocationId}),
                 l => l.HasOne(e => e.Book).WithMany(e => e.Stores)
                     .HasForeignKey(e => new {e.BookSortCallNumber, e.BookFormCallNumber}));
    }
}