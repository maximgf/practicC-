using JwtUserAuth;
using System.Security.Claims;
using JwtUserAuth.Attributes;

public class User : JwtUser
{
    [JwtClaim("firstname")] public string? FirstName { get; set; }
    [JwtClaim("lastname")] public string? LastName { get; set; }
    [JwtClaim(ClaimTypes.Name)] public string? Username { get; set; }
    [JwtClaim("id")] public long Id { get; set; }
    [JwtClaim("photourl")] public string? PhotoUrl { get; set; }
}