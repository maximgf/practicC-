using WebApi.Models.Enums;

public record PlaceRequest(
    float Longitude,
    float Latitude,
    long UserId,
    FeatureTag[] Tags);