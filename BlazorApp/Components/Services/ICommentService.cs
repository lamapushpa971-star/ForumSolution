using ApiContracts.DTO;

namespace BlazorApp.Components.Services;

public interface ICommentService
{
    Task<CommentDto> AddCommentAsync(CreateCommentDto request);
}