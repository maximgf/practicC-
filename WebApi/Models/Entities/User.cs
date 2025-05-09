/// <summary>
/// Represents a user entity in the system.
/// </summary>
/// <remarks>
/// This class extends JwtUser to provide JWT authentication capabilities.
/// It includes user profile information and is decorated with JwtClaim attributes
/// to specify how claims are mapped to properties.
/// </remarks>
using JwtUserAuth;
using System.Security.Claims;
using JwtUserAuth.Attributes;

public class User : JwtUser
{
    /// <summary>
    /// The user's first name.
    /// </summary>
    [JwtClaim("firstname")] public string? FirstName { get; set; }
    
    /// <summary>
    /// The user's last name.
    /// </summary>
    [JwtClaim("lastname")] public string? LastName { get; set; }
    
    /// <summary>
    /// The user's username.
    /// </summary>
    [JwtClaim(ClaimTypes.Name)] public string? Username { get; set; }
    
    /// <summary>
    /// The user's unique identifier.
    /// </summary>
    [JwtClaim("id")] public long Id { get; set; }
    
    /// <summary>
    /// URL to the user's profile photo.
    /// </summary>
    [JwtClaim("photourl")] public string? PhotoUrl { get; set; }
}