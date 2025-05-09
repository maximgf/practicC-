/// <summary>
/// The database context for the application.
/// </summary>
/// <remarks>
/// This context manages all database interactions for the application.
/// It includes a DbSet for Places and handles the conversion of FeatureTag arrays to/from strings for storage.
/// The context ensures the database is created when initialized.
/// </remarks>
using Microsoft.EntityFrameworkCore;
using WebApi.Models.Entities;

public class ApplicationContext : DbContext
{
    /// <summary>
    /// Gets or sets the Places DbSet for database operations.
    /// </summary>
    public DbSet<Place> Places { get; set; }

    /// <summary>
    /// Initializes a new instance of the ApplicationContext.
    /// </summary>
    /// <param name="options">The options for this context.</param>
    public ApplicationContext(DbContextOptions<ApplicationContext> options) : base(options)
    {
        Database.EnsureCreated();
    }

    /// <summary>
    /// Configures the model creation for the context.
    /// </summary>
    /// <param name="modelBuilder">The builder used to construct the model.</param>
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