/// <summary>
/// Service for handling place-related operations.
/// </summary>
/// <remarks>
/// This service manages all business logic related to places,
/// including creation, retrieval, and geographical queries.
/// It coordinates between the database, geo service, and user service.
/// </remarks>
using WebApi.Data;
using WebApi.Models.Entities;
using WebApi.Models.DTOs.Requests;
using WebApi.Models.DTOs.Responses;

public class PlaceService
{
    private readonly ApplicationContext _db;
    private readonly GeoService _geoService;
    private readonly UserService _userService;

    /// <summary>
    /// Initializes a new instance of the PlaceService.
    /// </summary>
    /// <param name="db">The database context.</param>
    /// <param name="geoService">The geo service for location calculations.</param>
    /// <param name="userService">The service for user-related operations.</param>
    public PlaceService(
        ApplicationContext db, 
        GeoService geoService,
        UserService userService)
    {
        _db = db;
        _geoService = geoService;
        _userService = userService;
    }

    /// <summary>
    /// Adds a new place to the system.
    /// </summary>
    /// <param name="request">The place creation request.</param>
    /// <returns>A response containing the created place and author information.</returns>
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

    /// <summary>
    /// Retrieves a place by its ID.
    /// </summary>
    /// <param name="id">The unique identifier of the place.</param>
    /// <returns>The place with author information, or null if not found.</returns>
    public async Task<PlaceWithAuthorResponse?> GetPlace(Guid id)
    {
        var place = await _db.Places.FindAsync(id);
        if (place is null) return null;

        var user = await _userService.GetUser(place.AddedBy);
        return new PlaceWithAuthorResponse(place, user);
    }

    /// <summary>
    /// Retrieves places near a specified location.
    /// </summary>
    /// <param name="request">The nearby places request parameters.</param>
    /// <returns>A response containing nearby places with their authors.</returns>
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