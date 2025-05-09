using WebApi.Models.Entities;

public class GeoService
{
    private const float EarthRadiusKm = 6371f;

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