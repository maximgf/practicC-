/// <summary>
/// Extension methods for configuring services in the dependency injection container.
/// </summary>
/// <remarks>
/// These extensions provide a clean way to configure database and application services.
/// They are typically called during application startup in Program.cs.
/// </remarks>
using WebApi.Data;
using WebApi.Services;
using Microsoft.EntityFrameworkCore;

public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Adds and configures the database services for the application.
    /// </summary>
    /// <param name="services">The service collection to extend.</param>
    /// <param name="configuration">The application configuration.</param>
    /// <returns>The service collection for chaining.</returns>
    public static IServiceCollection AddDatabase(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<ApplicationContext>(options => 
            options.UseSqlite(configuration.GetConnectionString("DefaultConnection")));
        
        return services;
    }

    /// <summary>
    /// Adds application-specific services to the dependency injection container.
    /// </summary>
    /// <param name="services">The service collection to extend.</param>
    /// <returns>The service collection for chaining.</returns>
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services
            .AddHttpClient()
            .AddScoped<GeoService>()
            .AddScoped<PlaceService>()
            .AddScoped<UserService>();
        
        return services;
    }
}