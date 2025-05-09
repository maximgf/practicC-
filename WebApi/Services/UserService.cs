using System.Net.Http.Json;
using WebApi.Models.Entities;

public class UserService
{
    private readonly HttpClient _httpClient;

    public UserService(IHttpClientFactory httpClientFactory)
    {
        _httpClient = httpClientFactory.CreateClient("UsersClient");
    }

    public async Task<User> GetUser(long userId)
    {
        return await _httpClient.GetFromJsonAsync<User>($"/users/{userId}") 
               ?? throw new InvalidOperationException("User not found");
    }
}