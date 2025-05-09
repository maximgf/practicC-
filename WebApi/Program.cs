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
