/// <summary>
/// Enumeration of possible feature tags for places.
/// </summary>
/// <remarks>
/// These tags describe the characteristics and amenities available at a place.
/// They are used for categorizing and filtering places in the system.
/// </remarks>
public enum FeatureTag
{
    /// <summary>Location has drinking water available</summary>
    DrinkingWater,
    /// <summary>Location has mobile communication coverage</summary>
    MobileCommunication,
    /// <summary>Location has a campfire site</summary>
    CampfireSite,
    /// <summary>Location has toilet facilities</summary>
    Toilet,
    /// <summary>Location is near a shore/water body</summary>
    Shore,
    /// <summary>Location is suitable for fishing</summary>
    Fishing,
    /// <summary>Location is suitable for biking</summary>
    Bike,
    /// <summary>Location requires payment for access</summary>
    Paid,
    /// <summary>Location has sandy terrain</summary>
    Sand,
    /// <summary>Location has stony terrain</summary>
    Stone,
    /// <summary>Location has regular ground terrain</summary>
    Ground
}