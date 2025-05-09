/// <summary>
/// Service for handling user-related operations.
/// </summary>
/// <remarks>
/// This service communicates with the user management system
/// to retrieve user information. It uses HttpClient to make HTTP requests.
/// </remarks>
using System.Net.Http.Json;
using WebApi.Models.Entities;

public class UserService
{
    private readonly HttpClient _httpClient;

    /// <summary>
    /// Initializes a new instance of the UserService.
    /// </summary>
    /// <param name="httpClientFactory">The factory for creating HttpClient instances.</param>
    public UserService(IHttpClientFactory httpClientFactory)
    {
        _httpClient = httpClientFactory.CreateClient("UsersClient");
    }

    /// <summary>
    /// Retrieves a user by their ID.
    /// </summary>
    /// <param name="userId">The ID of the user to retrieve.</param>
    /// <returns>The user information.</returns>
    /// <exception cref="InvalidOperationException">Thrown when user is not found.</exception>
    public async Task<User> GetUser(long userId)
    {
        return await _httpClient.GetFromJsonAsync<User>($"/users/{userId}") 
               ?? throw new InvalidOperationException("User not found");
    }
}