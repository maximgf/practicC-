/// <summary>
/// Represents a request to find nearby places.
/// </summary>
/// <remarks>
/// This record contains the search parameters for finding places
/// near a specific location within a certain radius.
/// </remarks>
public record NearbyPlacesRequest(
    /// <summary>
    /// The latitude coordinate of the center point for the search.
    /// </summary>
    float Latitude,
    
    /// <summary>
    /// The longitude coordinate of the center point for the search.
    /// </summary>
    float Longitude,
    
    /// <summary>
    /// The search radius in meters.
    /// </summary>
    float Radius,
    
    /// <summary>
    /// The maximum number of places to return.
    /// </summary>
    int Count);