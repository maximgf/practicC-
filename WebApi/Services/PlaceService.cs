using WebApi.Data;
using WebApi.Models.Entities;
using WebApi.Models.DTOs.Requests;
using WebApi.Models.DTOs.Responses;

public class PlaceService
{
    private readonly ApplicationContext _db;
    private readonly GeoService _geoService;
    private readonly UserService _userService;

    public PlaceService(
        ApplicationContext db, 
        GeoService geoService,
        UserService userService)
    {
        _db = db;
        _geoService = geoService;
        _userService = userService;
    }

    public async Task<PlaceResponse> AddPlace(PlaceRequest request)
    {
        var place = new Place
        {
            ID = Guid.NewGuid(),
            AddedBy = request.UserId,
            AddedAt = DateTime.UtcNow,
            Tags = request.Tags,
            Verified = false,
            Longitude = request.Longitude,
            Latitude = request.Latitude,
        };

        await _db.Places.AddAsync(place);
        await _db.SaveChangesAsync();

        var user = await _userService.GetUser(request.UserId);

        return new PlaceResponse
        {
            Message = "Place added successfully",
            Place = place,
            Author = user
        };
    }

    public async Task<PlaceWithAuthorResponse?> GetPlace(Guid id)
    {
        var place = await _db.Places.FindAsync(id);
        if (place is null) return null;

        var user = await _userService.GetUser(place.AddedBy);
        return new PlaceWithAuthorResponse(place, user);
    }

    public async Task<NearbyPlacesResponse> GetNearbyPlaces(NearbyPlacesRequest request)
    {
        var places = await _db.Places.ToListAsync();
        var nearbyPlaces = _geoService.GetPlacesWithinRadius(
            places, 
            request.Latitude, 
            request.Longitude, 
            request.Radius, 
            request.Count);

        var results = new List<PlaceWithAuthorResponse>();
        foreach (var place in nearbyPlaces)
        {
            var user = await _userService.GetUser(place.AddedBy);
            results.Add(new PlaceWithAuthorResponse(place, user));
        }

        return new NearbyPlacesResponse("OK", results);
    }
}