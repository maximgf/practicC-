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


app.MapPost("/add", async (float longitude, float latitude, IFormFileCollection? files, User user, ApplicationContext dbContext) =>
{
    // AddedBy
    var userID = user.Id;
    Console.WriteLine(userID);
    // PlaceID
    var placeID = Guid.NewGuid();

    //Сoordinates
    var Longitude = longitude;
    var Latitude = latitude;

    var place = new Place
    {
        ID = placeID,
        AddedBy = userID,
        AddedAt = DateTime.UtcNow,
        Photos = [],
        Tags = [ FeatureTag.Tag1 ], // Provide a default value for Tags
        Verified = false, // Provide a default value for Verified
        Longitude = longitude,
        Latitude = latitude,
    };

    // Photos save
    var photosPath = Path.Combine(Directory.GetCurrentDirectory(), "Photos");

    var folderPath = Path.Combine(photosPath, placeID.ToString());
    Directory.CreateDirectory(folderPath);
    if (files != null || files.Count() > 0)
    {
        foreach (var file in files)
        {
            if (file.Length == 0) continue;
            var fileName = Guid.NewGuid().ToString();
            var filePath = Path.Combine(folderPath, fileName + Path.GetExtension(file.FileName));

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            var photo = new PlacePhoto
            {
                Id = Guid.NewGuid(),
                AddedAt = DateTime.UtcNow,
                AddedBy = userID,
                PlaceId = placeID
            };
            await dbContext.PlacePhotos.AddAsync(photo);
            place.Photos.Add(photo);
        }
    }
    await dbContext.Places.AddAsync(place);
    await dbContext.SaveChangesAsync();
    
    return Results.Ok(new
    {
        Message = "Place added successfully",
        Object = place
    });
}).RequireAuthorization().DisableAntiforgery();

app.MapGet("/", async (Guid ID,ApplicationContext dbContext)=>
    {
        var entity = await dbContext.Places.FindAsync(ID);
        if (entity == null)
        {
            return Results.NotFound(); // Возвращает статус 404, если объект не найден
        }
        return Results.Ok(entity); // Возвращает объект с соответствующим статусом 200
    });

app.MapGet("/get", async (float latitude, float longitude, float radius, ApplicationContext dbContext) =>
{
    // Вычисление расстояния между двумя точками на сфере (в километрах)
    double R = 6371; // радиус Земли в километрах

    // Получаем все места из базы данных
    var places = await dbContext.Places.ToListAsync();

    // Фильтруем места, находящиеся в пределах заданного радиуса(радиус в км)
    var placesInRadius = places.Where(place =>
    {
        //
        var pointLat =  latitude;
        var pointLon =  longitude;
        var placeLat = place.Latitude;
        var placeLon = place.Longitude;

        var deltaLat = (pointLat - placeLat)*Math.PI / 180;
        var deltaLong = (pointLon - placeLon)*Math.PI / 180;

        var a = Math.Sin(deltaLat/2)*Math.Sin(deltaLat/2) + Math.Cos(pointLat*Math.PI / 180)*Math.Cos(placeLat*Math.PI / 180)*Math.Sin(deltaLong/2)*Math.Sin(deltaLong/2);
        var c = 2 * Math.Atan2(Math.Sqrt(a),Math.Sqrt(1-a));

        var distance = R * c;

        return distance <= radius;
    }).ToList();

    return Results.Ok(placesInRadius);
});


app.Run();

public class ApplicationContext : DbContext
{

    public DbSet<Place> Places { get; set; } = null!;
    public DbSet<PlacePhoto> PlacePhotos { get; set; } = null!;

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
    public List<PlacePhoto> Photos { get; set; } = new List<PlacePhoto>();
    public float Longitude { get; set; }
    public float Latitude { get; set; }
    public FeatureTag[]? Tags { get; set; }
    public bool Verified { get; set; }
}

public class PlacePhoto
{
    public Guid Id { get; set; }
    public DateTime AddedAt { get; set; }
    public long AddedBy { get; set; }
    public Guid PlaceId { get; set; }
}

public enum FeatureTag
{
    Tag1,
    Tag2,
    Tag3
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