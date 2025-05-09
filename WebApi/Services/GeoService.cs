/// <summary>
/// Service for handling geographical calculations.
/// </summary>
/// <remarks>
/// This service provides methods for working with geographical coordinates,
/// including distance calculations and location-based queries.
/// </remarks>
using WebApi.Models.Entities;

public class GeoService
{
    private const float EarthRadiusKm = 6371f;

    /// <summary>
    /// Filters places within a specified radius from a center point.
    /// </summary>
    /// <param name="places">The list of places to filter.</param>
    /// <param name="centerLat">Latitude of the center point.</param>
    /// <param name="centerLon">Longitude of the center point.</param>
    /// <param name="radiusKm">Search radius in kilometers.</param>
    /// <param name="maxCount">Maximum number of places to return.</param>
    /// <returns>List of places within the specified radius.</returns>
    public List<Place> GetPlacesWithinRadius(
        List<Place> places, 
        float centerLat, 
        float centerLon, 
        float radiusKm, 
        int maxCount)
    {
        return places
            .Where(place => CalculateDistance(centerLat, centerLon, place.Latitude, place.Longitude) <= radiusKm)
            .Take(maxCount)
            .ToList();
    }

    /// <summary>
    /// Calculates the distance between two geographical points using the Haversine formula.
    /// </summary>
    /// <param name="lat1">Latitude of point 1.</param>
    /// <param name="lon1">Longitude of point 1.</param>
    /// <param name="lat2">Latitude of point 2.</param>
    /// <param name="lon2">Longitude of point 2.</param>
    /// <returns>Distance between the points in kilometers.</returns>
    private double CalculateDistance(float lat1, float lon1, float lat2, float lon2)
    {
        var dLat = (lat2 - lat1) * Math.PI / 180;
        var dLon = (lon2 - lon1) * Math.PI / 180;

        var a = Math.Sin(dLat / 2) * Math.Sin(dLat / 2) +
                Math.Cos(lat1 * Math.PI / 180) * Math.Cos(lat2 * Math.PI / 180) *
                Math.Sin(dLon / 2) * Math.Sin(dLon / 2);

        var c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));
        return EarthRadiusKm * c;
    }
}