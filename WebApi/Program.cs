/// <summary>
/// The main entry point for the application.
/// </summary>
/// <remarks>
/// This class configures the web application host and services.
/// It sets up authentication, database, and application services,
/// and configures the middleware pipeline.
/// </remarks>
using WebApi.Data;
using WebApi.Extensions;
using JwtUserAuth;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Configure services
builder.Services
    .AddAntiforgery(options => options.SuppressXFrameOptionsHeader = true)
    .AddJwtAuth<User>()
    .AddDatabase(builder.Configuration)
    .AddApplicationServices();

var app = builder.Build();

// Configure middleware pipeline
app.UseAntiforgery()
   .UseJwtAuth<User>()
   .UseDefaultFiles()
   .UseStaticFiles();

// Register endpoints
app.MapPlacesEndpoints();

app.Run();