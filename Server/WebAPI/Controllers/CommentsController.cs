using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CommentsController : ControllerBase
{
    private static List<CommentDto> _comments = new();
    private static int _nextId = 1;

    [HttpGet("post/{postId}")]
    public IActionResult GetCommentsByPost(int postId)
    {
        var comments = _comments.Where(c => c.PostId == postId).ToList();
        return Ok(comments);
    }

    [HttpPost]
    public IActionResult CreateComment([FromBody] CreateCommentDto createCommentDto)
    {
        var comment = new CommentDto
        {
            Id = _nextId++,
            Content = createCommentDto.Content ?? string.Empty,
            PostId = createCommentDto.PostId,
            UserId = createCommentDto.UserId,
            Username = "user",
            CreatedAt = DateTime.Now
        };
        _comments.Add(comment);
        return Ok(comment);
    }
}

public class CreateCommentDto
{
    public string Content { get; set; } = string.Empty;
    public int PostId { get; set; }
    public int UserId { get; set; }
}

public class CommentDto
{
    public int Id { get; set; }
    public string Content { get; set; } = string.Empty;
    public int PostId { get; set; }
    public int UserId { get; set; }
    public string Username { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
}