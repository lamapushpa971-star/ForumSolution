using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RepositoryContracts;
using Entities;

namespace WebAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CommentsController : ControllerBase
{
    private readonly ICommentRepository _commentRepository;
    private readonly IUserRepository _userRepository;

    public CommentsController(ICommentRepository commentRepository, IUserRepository userRepository)
    {
        _commentRepository = commentRepository;
        _userRepository = userRepository;
    }

    [HttpGet("post/{postId}")]
    public async Task<ActionResult<IEnumerable<CommentDto>>> GetCommentsByPost(int postId)
    {
        var comments = await _commentRepository.GetMany()
            .Where(c => c.PostId == postId)
            .Include(c => c.User)
            .ToListAsync();
        
        var commentDtos = comments.Select(c => new CommentDto
        {
            Id = c.Id,
            Content = c.Body,
            PostId = c.PostId,
            UserId = c.UserId,
            Username = c.User.Username,
            CreatedAt = DateTime.Now
        }).ToList();
        
        return Ok(commentDtos);
    }

    [HttpPost]
    public async Task<ActionResult<CommentDto>> CreateComment([FromBody] CreateCommentDto createCommentDto)
    {
        var user = await _userRepository.GetByIdAsync(createCommentDto.UserId);
        if (user == null)
        {
            return BadRequest("User not found");
        }

        var comment = new Comment(createCommentDto.Content, createCommentDto.UserId, createCommentDto.PostId);
        var createdComment = await _commentRepository.AddAsync(comment);
        
        var commentDto = new CommentDto
        {
            Id = createdComment.Id,
            Content = createdComment.Body,
            PostId = createdComment.PostId,
            UserId = createdComment.UserId,
            Username = user.Username,
            CreatedAt = DateTime.Now
        };
        
        return Ok(commentDto);
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<CommentDto>>> GetAllComments()
    {
        var comments = await _commentRepository.GetMany()
            .Include(c => c.User)
            .ToListAsync();
        
        var commentDtos = comments.Select(c => new CommentDto
        {
            Id = c.Id,
            Content = c.Body,
            PostId = c.PostId,
            UserId = c.UserId,
            Username = c.User.Username,
            CreatedAt = DateTime.Now
        }).ToList();
        
        return Ok(commentDtos);
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