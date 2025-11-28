using ApiContracts.DTO;

namespace BlazorApp.Components.Services;

public interface IPostService
{
    Task<PostDto> AddPostAsync(CreatePostDto request);
    Task<PostDto> GetPostAsync(int id); // Keep this as PostDto
    Task<List<PostDto>> GetPostsAsync();
}