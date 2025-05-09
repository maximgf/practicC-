using WebApi.Models.Enums;

public class Place
{
    public Guid ID { get; set; }
    public long AddedBy { get; set; }
    public DateTime AddedAt { get; set; }
    public float Longitude { get; set; }
    public float Latitude { get; set; }
    public FeatureTag[] Tags { get; set; }
    public bool Verified { get; set; }
}