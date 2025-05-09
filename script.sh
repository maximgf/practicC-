#!/bin/bash

# Создаем основную директорию проекта
mkdir -p WebApi

# Создаем все необходимые поддиректории
mkdir -p WebApi/Controllers
mkdir -p WebApi/Data
mkdir -p WebApi/Services
mkdir -p WebApi/Models/Entities
mkdir -p WebApi/Models/DTOs/Requests
mkdir -p WebApi/Models/DTOs/Responses
mkdir -p WebApi/Models/Enums
mkdir -p WebApi/Extensions
mkdir -p WebApi/Properties

# Создаем основные файлы
touch WebApi/Program.cs
touch WebApi/appsettings.json
touch WebApi/appsettings.Development.json
touch WebApi/Properties/launchSettings.json
touch WebApi/WebApi.csproj

# Создаем файлы контроллеров
touch WebApi/Controllers/PlacesController.cs

# Создаем файлы сервисов
touch WebApi/Services/PlaceService.cs
touch WebApi/Services/UserService.cs
touch WebApi/Services/GeoService.cs

# Создаем файлы данных
touch WebApi/Data/ApplicationContext.cs

# Создаем файлы моделей
touch WebApi/Models/Entities/Place.cs
touch WebApi/Models/Entities/User.cs
touch WebApi/Models/Enums/FeatureTag.cs
touch WebApi/Models/DTOs/Requests/PlaceRequest.cs
touch WebApi/Models/DTOs/Requests/NearbyPlacesRequest.cs
touch WebApi/Models/DTOs/Responses/PlaceResponse.cs
touch WebApi/Models/DTOs/Responses/NearbyPlacesResponse.cs

# Создаем файлы расширений
touch WebApi/Extensions/ServiceCollectionExtensions.cs

# Заполняем Program.cs
cat > WebApi/Program.cs << 'EOL'
using WebApi.Data;
using WebApi.Extensions;
using JwtUserAuth;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Конфигурация сервисов
builder.Services
    .AddAntiforgery(options => options.SuppressXFrameOptionsHeader = true)
    .AddJwtAuth<User>()
    .AddDatabase(builder.Configuration)
    .AddApplicationServices();

var app = builder.Build();

// Конфигурация middleware
app.UseAntiforgery()
   .UseJwtAuth<User>()
   .UseDefaultFiles()
   .UseStaticFiles();

// Регистрация endpoints
app.MapPlacesEndpoints();

app.Run();
EOL

# Заполняем ServiceCollectionExtensions.cs
cat > WebApi/Extensions/ServiceCollectionExtensions.cs << 'EOL'
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
EOL

# Заполняем appsettings.json
cat > WebApi/appsettings.json << 'EOL'
{
  "ConnectionStrings": {
    "DefaultConnection": "Data Source=PlaceDB/place.db"
  },
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  },
  "AllowedHosts": "*"
}
EOL

# Заполняем appsettings.Development.json
cat > WebApi/appsettings.Development.json << 'EOL'
{
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  },
  "Jwt": {
    "Issuer": "example.com",
    "Audience": "example.com",
    "Symkey": "your-256-bit-secure-key-here"
  }
}
EOL

# Заполняем WebApi.csproj
cat > WebApi/WebApi.csproj << 'EOL'
<Project Sdk="Microsoft.NET.Sdk.Web">

  <PropertyGroup>
    <TargetFramework>net9.0</TargetFramework>
    <Nullable>enable</Nullable>
    <ImplicitUsings>enable</ImplicitUsings>
  </PropertyGroup>

  <ItemGroup>
    <PackageReference Include="JwtUserAuth.AspNet" Version="1.0.9" />
    <PackageReference Include="Microsoft.EntityFrameworkCore" Version="8.0.6" />
    <PackageReference Include="Microsoft.EntityFrameworkCore.Sqlite" Version="8.0.6" />
    <PackageReference Include="Swashbuckle.AspNetCore" Version="6.5.0" />
  </ItemGroup>

</Project>
EOL

echo "Структура проекта успешно создана в директории WebApi!"
