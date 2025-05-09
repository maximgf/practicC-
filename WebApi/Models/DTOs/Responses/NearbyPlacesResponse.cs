using WebApi.Models.Entities;

public record NearbyPlacesResponse(
    string Message,
    List<PlaceWithAuthorResponse> Places);

public record PlaceWithAuthorResponse(
    Place Place,
    User Author);