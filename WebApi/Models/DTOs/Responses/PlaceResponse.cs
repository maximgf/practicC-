using WebApi.Models.Entities;

public record PlaceResponse(
    string Message,
    Place Place,
    User Author);