using System.Text.Json;
using ApiContracts.DTO;
using BlazorApp.Components.Services;

namespace BlazorApp.Components.Services;

public class HttpUserService : IUserService
{
    private readonly IHttpClientFactory _httpClientFactory;

    public HttpUserService(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }

    public async Task<UserDto> AddUserAsync(CreateUserDto request)
    {
        try
        {
            var client = _httpClientFactory.CreateClient("ForumApi");
            HttpResponseMessage httpResponse = await client.PostAsJsonAsync("api/users", request);
            string response = await httpResponse.Content.ReadAsStringAsync();
            
            if (!httpResponse.IsSuccessStatusCode)
            {
                throw new Exception($"Error: {httpResponse.StatusCode} - {response}");
            }
            
            return JsonSerializer.Deserialize<UserDto>(response, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            })!;
        }
        catch (Exception ex)
        {
            throw new Exception($"Failed to add user: {ex.Message}");
        }
    }

    public async Task<List<UserDto>> GetUsersAsync()
    {
        try
        {
            var client = _httpClientFactory.CreateClient("ForumApi");
            HttpResponseMessage httpResponse = await client.GetAsync("api/users");
            string response = await httpResponse.Content.ReadAsStringAsync();
            
            if (!httpResponse.IsSuccessStatusCode)
            {
                throw new Exception($"Error: {httpResponse.StatusCode} - {response}");
            }
            
            return JsonSerializer.Deserialize<List<UserDto>>(response, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            }) ?? new List<UserDto>();
        }
        catch (Exception ex)
        {
            throw new Exception($"Failed to load users: {ex.Message}");
        }
    }

    public async Task<UserDto> GetUserAsync(int id)
    {
        try
        {
            var client = _httpClientFactory.CreateClient("ForumApi");
            HttpResponseMessage httpResponse = await client.GetAsync($"api/users/{id}");
            string response = await httpResponse.Content.ReadAsStringAsync();
            
            if (!httpResponse.IsSuccessStatusCode)
            {
                throw new Exception($"Error: {httpResponse.StatusCode} - {response}");
            }
            
            return JsonSerializer.Deserialize<UserDto>(response, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            })!;
        }
        catch (Exception ex)
        {
            throw new Exception($"Failed to load user: {ex.Message}");
        }
    }
}