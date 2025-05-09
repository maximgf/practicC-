using WebApi.Data;
using WebApi.Services;
using Microsoft.EntityFrameworkCore;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddDatabase(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<ApplicationContext>(options => 
            options.UseSqlite(configuration.GetConnectionString("DefaultConnection")));
        
        return services;
    }

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
