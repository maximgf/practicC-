using System.IO;
using System.Text.Json;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.EntityFrameworkCore;
//using JwtUserAuth;
//using JwtUserAuth.Types;



var builder = WebApplication.CreateBuilder();
string? connection = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<ApplicationContext>(options => options.UseSqlite(connection));

var app = builder.Build();

app.UseDefaultFiles();
app.UseStaticFiles();

app.MapPost("/", async (HttpRequest request, ApplicationContext dbContext) =>
{
    // AddedBy
    string head = request.Headers["Authorization"].ToString();
    if (head is null) throw new ArgumentNullException(nameof(head));
    string userID = JWTtoID(head);

    // PlaceID
    var placeID = Guid.NewGuid();
    
    var place = new Place
    {
        ID = placeID,
        AddedBy = long.Parse(userID),
        AddedAt = DateTime.UtcNow,
        Photos = new List<PlacePhoto>(),
        Tags = new FeatureTag[] { FeatureTag.Tag1 }, // Provide a default value for Tags
        Verified = false, // Provide a default value for Verified
        Longitude = 10.0f,
        Latitude = 10.0f,
    };
    
    // Photos save
    var photosPath = Path.Combine(Directory.GetCurrentDirectory(), "Photos");
    if (!Directory.Exists(photosPath))
    {
        Directory.CreateDirectory(photosPath);
    }

    var folderPath = Path.Combine(photosPath, placeID.ToString());
    Directory.CreateDirectory(folderPath);

    foreach (var file in request.Form.Files)
    {
        if (file.Length > 0)
        {
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
                AddedBy = long.Parse(userID),
                PlaceId = placeID
            };
            dbContext.PlacePhotos.Add(photo);
            place.Photos.Add(photo);
        }
    }
    
    dbContext.Places.Add(place);
    await dbContext.SaveChangesAsync();

    return Results.Ok(new { message = "Photos uploaded successfully" });
});

string JWTtoID(string head)
{
    var token = head.Substring("Bearer ".Length).Trim();
    var handler = new JwtSecurityTokenHandler();
    var jsonToken = handler.ReadToken(token) as JwtSecurityToken;

    if (jsonToken is null) throw new ArgumentNullException(nameof(jsonToken));

    string id = jsonToken.Claims.First(claim => claim.Type == "sub").Value;
    return id;
}

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







//public class User:JwtUser;