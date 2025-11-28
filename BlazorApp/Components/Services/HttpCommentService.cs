using System.Text.Json;
using ApiContracts.DTO;
using BlazorApp.Components.Services;

namespace BlazorApp.Components.Services;

public class HttpCommentService : ICommentService
{
    private readonly IHttpClientFactory _httpClientFactory;

    public HttpCommentService(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }

    public async Task<CommentDto> AddCommentAsync(CreateCommentDto request)
    {
        try
        {
            var client = _httpClientFactory.CreateClient("ForumApi");
            HttpResponseMessage httpResponse = await client.PostAsJsonAsync("api/comments", request);
            string response = await httpResponse.Content.ReadAsStringAsync();
            
            if (!httpResponse.IsSuccessStatusCode)
            {
                throw new Exception($"Error: {httpResponse.StatusCode} - {response}");
            }
            
            var comment = JsonSerializer.Deserialize<CommentDto>(response, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });
            
            return comment!;
        }
        catch (Exception ex)
        {
            throw new Exception($"Failed to add comment: {ex.Message}");
        }
    }
}