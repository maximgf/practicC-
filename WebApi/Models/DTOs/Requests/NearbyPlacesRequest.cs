public record NearbyPlacesRequest(
    float Latitude,
    float Longitude,
    float Radius,
    int Count);