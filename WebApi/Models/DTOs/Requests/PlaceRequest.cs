/// <summary>
/// Represents a request to add a new place.
/// </summary>
/// <remarks>
/// This record contains all necessary information to create a new place,
/// including its geographic coordinates, associated user, and feature tags.
/// </remarks>
using WebApi.Models.Enums;

public record PlaceRequest(
    /// <summary>
    /// The longitude coordinate of the place.
    /// </summary>
    float Longitude,
    
    /// <summary>
    /// The latitude coordinate of the place.
    /// </summary>
    float Latitude,
    
    /// <summary>
    /// The unique identifier of the user creating the place.
    /// </summary>
    long UserId,
    
    /// <summary>
    /// The feature tags associated with the place.
    /// </summary>
    FeatureTag[] Tags);