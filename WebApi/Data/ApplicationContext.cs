using Microsoft.EntityFrameworkCore;
using WebApi.Models.Entities;

public class ApplicationContext : DbContext
{
    public DbSet<Place> Places { get; set; }

    public ApplicationContext(DbContextOptions<ApplicationContext> options) : base(options)
    {
        Database.EnsureCreated();
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Place>()
            .Property(p => p.Tags)
            .HasConversion(
                v => string.Join(',', v.Select(t => t.ToString())),
                v => v.Split(',', StringSplitOptions.RemoveEmptyEntries)
                    .Select(t => Enum.Parse<FeatureTag>(t))
                    .ToArray());
    }
}