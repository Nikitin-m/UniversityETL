using Microsoft.EntityFrameworkCore;
using UniversityService.Domain;

namespace UniversityService.Infrastructure.DAL;

public sealed class UniversityDbContext : DbContext
{
    public DbSet<University> Universities { get; set; }

    public UniversityDbContext(DbContextOptions options) : base(options)
    {
        Database.EnsureCreated();
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.Entity<University>()
            .HasMany(x=> x.Sites);
    }
}