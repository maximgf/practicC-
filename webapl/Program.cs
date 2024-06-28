using System.IO;
using System.Text.Json;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.EntityFrameworkCore;
using JwtUserAuth;
using JwtUserAuth.Types;
using System.Security.Claims;
using JwtUserAuth.Attributes;




var builder = WebApplication.CreateBuilder();
builder.Services.AddAntiforgery(options => { options.SuppressXFrameOptionsHeader = true; }); 
builder.AddJwtAuth<User>();
string? connection = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<ApplicationContext>(options => options.UseSqlite(connection));


var app = builder.Build();
app.UseAntiforgery();
app.UseJwtAuth<User>();
app.UseDefaultFiles();
app.UseStaticFiles();


async Task<User> GetUser(Guid id)
{
    using var http = new HttpClient();

    return await http.GetFromJsonAsync<User>($"/{id}");
}





app.MapPost("/add", async (float longitude, float latitude, User user,FeatureTag[] tags, ApplicationContext dbContext) =>
{
    var userID = user.Id;
    
    var placeID = Guid.NewGuid();

    var place = new Place
    {
        ID = placeID,
        AddedBy = userID,
        AddedAt = DateTime.UtcNow,
        Tags = tags,
        Verified = false,
        Longitude = longitude,
        Latitude = latitude,
    };

    await dbContext.Places.AddAsync(place);
    await dbContext.SaveChangesAsync();

    var user = await GetUser(userID);

    return Results.Ok(new
    {
        Message = "Place added successfully",
        Object = place, user
    });
}).RequireAuthorization().DisableAntiforgery();


app.MapGet("/", async (Guid ID, ApplicationContext dbContext) =>
{
    var entity = await dbContext.Places.FindAsync(ID);
    if (entity == null)
    {
        return Results.NotFound();
    }

    var user = await GetUser(entity.AddedBy);

    return Results.Ok(new
    {
        Place = entity,user
    });
});

app.MapGet("/get", async (float latitude, float longitude, float radius, int count, ApplicationContext dbContext) =>
{
    var places = await dbContext.Places.ToListAsync();
    float R = 6371F;
    var placesInRadius = places.Where(place =>
    {
        var pointLat = latitude;
        var pointLon = longitude;
        var placeLat = place.Latitude;
        var placeLon = place.Longitude;

        var deltaLat = (pointLat - placeLat) * Math.PI / 180;
        var deltaLong = (pointLon - placeLon) * Math.PI / 180;

        var a = Math.Sin(deltaLat / 2) * Math.Sin(deltaLat / 2) + Math.Cos(pointLat * Math.PI / 180) * Math.Cos(placeLat * Math.PI / 180) * Math.Sin(deltaLong / 2) * Math.Sin(deltaLong / 2);
        var c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));

        var distance = R * c;

        return distance <= radius;
    }).Take(count).ToList();

    var result = new List<object>();
    foreach (var place in placesInRadius)
    {
        var user = await GetUser(place.AddedBy);
        result.Add(new
        {
            place = place,
            author = user
        });
    }

    return Results.Ok(new
    {
        message = "ok",
        places = result
    });



});


app.Run();

public class ApplicationContext : DbContext
{

    public DbSet<Place> Places { get; set; } = null!;
 

    public ApplicationContext(DbContextOptions<ApplicationContext> options)
        : base(options)
    {
        Database.EnsureCreated();   // создаем базу данных при первом обращении
    }
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            optionsBuilder.UseSqlite("Data Source=PlaceDB/place.db");
        }
    }
 

}

public class Place
{
    public Guid ID { get; set; }
    public long AddedBy { get; set; }
    public DateTime AddedAt { get; set; }
    public float Longitude { get; set; }
    public float Latitude { get; set; }
    public FeatureTag[]? Tags { get; set; }
    public bool Verified { get; set; }
}


public enum FeatureTag
{
    DrinkingWater,
    MobileCommunication ,
    CampfireSite,
    Toilet,
    Shore,
    Fishing,
    Bike,
    Paid,
    Sand,
    Stone,
    Ground
}

public class User : JwtUser
{
    [JwtClaim("firstname")]
    public string? FirstName{get;set;}
    
    [JwtClaim("lastname")]
    public string? LastName{get;set;}
    
    [JwtClaim(ClaimTypes.Name)]
    public string? Username{get;set;}
    
    [JwtClaim("id")]
    public long Id{get;set;}
    
    [JwtClaim("photourl")]
    public string? PhotoUrl{get;set;}
}
