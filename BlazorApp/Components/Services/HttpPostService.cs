using System.Text.Json;
using ApiContracts.DTO;
using BlazorApp.Components.Services;

namespace BlazorApp.Components.Services;

public class HttpPostService : IPostService
{
    private readonly IHttpClientFactory _httpClientFactory;

    public HttpPostService(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }

    public async Task<PostDto> AddPostAsync(CreatePostDto request)
    {
        try
        {
            var client = _httpClientFactory.CreateClient("ForumApi");
            HttpResponseMessage httpResponse = await client.PostAsJsonAsync("api/posts", request);
            string response = await httpResponse.Content.ReadAsStringAsync();
            
            if (!httpResponse.IsSuccessStatusCode)
            {
                throw new Exception($"Error: {httpResponse.StatusCode} - {response}");
            }
            
            return JsonSerializer.Deserialize<PostDto>(response, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            })!;
        }
        catch (Exception ex)
        {
            throw new Exception($"Failed to create post: {ex.Message}");
        }
    }

    public async Task<PostDto> GetPostAsync(int id)
    {
        try
        {
            var client = _httpClientFactory.CreateClient("ForumApi");
            HttpResponseMessage httpResponse = await client.GetAsync($"api/posts/{id}");
            string response = await httpResponse.Content.ReadAsStringAsync();
            
            if (!httpResponse.IsSuccessStatusCode)
            {
                throw new Exception($"Error: {httpResponse.StatusCode} - {response}");
            }
            
            return JsonSerializer.Deserialize<PostDto>(response, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            })!;
        }
        catch (Exception ex)
        {
            throw new Exception($"Failed to load post: {ex.Message}");
        }
    }

    public async Task<List<PostDto>> GetPostsAsync()
    {
        try
        {
            var client = _httpClientFactory.CreateClient("ForumApi");
            HttpResponseMessage httpResponse = await client.GetAsync("api/posts");
            string response = await httpResponse.Content.ReadAsStringAsync();
            
            if (!httpResponse.IsSuccessStatusCode)
            {
                throw new Exception($"Error: {httpResponse.StatusCode} - {response}");
            }
            
            return JsonSerializer.Deserialize<List<PostDto>>(response, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            }) ?? new List<PostDto>();
        }
        catch (Exception ex)
        {
            throw new Exception($"Failed to load posts: {ex.Message}");
        }
    }
}