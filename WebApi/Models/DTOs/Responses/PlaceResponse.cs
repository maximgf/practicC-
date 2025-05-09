/// <summary>
/// Represents the response after adding a new place.
/// </summary>
/// <remarks>
/// This record contains the operation result message,
/// the created place details, and information about its author.
/// </remarks>
using WebApi.Models.Entities;

public record PlaceResponse(
    /// <summary>
    /// A message describing the operation result.
    /// </summary>
    string Message,
    
    /// <summary>
    /// The created place details.
    /// </summary>
    Place Place,
    
    /// <summary>
    /// Information about the user who created the place.
    /// </summary>
    User Author);