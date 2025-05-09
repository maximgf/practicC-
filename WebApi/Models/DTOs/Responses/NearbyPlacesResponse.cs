/// <summary>
/// Represents the response containing nearby places.
/// </summary>
/// <remarks>
/// This record contains the operation result message and a list
/// of places with their respective authors that are near the requested location.
/// </remarks>
using WebApi.Models.Entities;

public record NearbyPlacesResponse(
    /// <summary>
    /// A message describing the operation result.
    /// </summary>
    string Message,
    
    /// <summary>
    /// The list of nearby places with author information.
    /// </summary>
    List<PlaceWithAuthorResponse> Places);

/// <summary>
/// Represents a place with its author information.
/// </summary>
public record PlaceWithAuthorResponse(
    /// <summary>
    /// The place details.
    /// </summary>
    Place Place,
    
    /// <summary>
    /// Information about the user who created the place.
    /// </summary>
    User Author);