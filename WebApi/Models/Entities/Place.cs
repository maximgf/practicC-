/// <summary>
/// Represents a geographical place in the system.
/// </summary>
/// <remarks>
/// This class contains all information about a specific location,
/// including its coordinates, tags, and metadata about its creation.
/// </remarks>
using WebApi.Models.Enums;

public class Place
{
    /// <summary>
    /// Unique identifier for the place.
    /// </summary>
    public Guid ID { get; set; }
    
    /// <summary>
    /// ID of the user who added this place.
    /// </summary>
    public long AddedBy { get; set; }
    
    /// <summary>
    /// Date and time when the place was added.
    /// </summary>
    public DateTime AddedAt { get; set; }
    
    /// <summary>
    /// Longitude coordinate of the place.
    /// </summary>
    public float Longitude { get; set; }
    
    /// <summary>
    /// Latitude coordinate of the place.
    /// </summary>
    public float Latitude { get; set; }
    
    /// <summary>
    /// Array of feature tags describing the place.
    /// </summary>
    public FeatureTag[] Tags { get; set; }
    
    /// <summary>
    /// Indicates whether the place has been verified by administrators.
    /// </summary>
    public bool Verified { get; set; }
}