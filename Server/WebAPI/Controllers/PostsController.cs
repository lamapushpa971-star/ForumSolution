using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RepositoryContracts;
using Entities;

namespace WebAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PostsController : ControllerBase
{
    private readonly IPostRepository _postRepository;
    private readonly IUserRepository _userRepository;

    public PostsController(IPostRepository postRepository, IUserRepository userRepository)
    {
        _postRepository = postRepository;
        _userRepository = userRepository;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<PostDto>>> GetPosts()
    {
        var posts = await _postRepository.GetMany()
            .Include(p => p.User)
            .ToListAsync();
            
        var postDtos = posts.Select(p => new PostDto
        {
            Id = p.Id,
            Title = p.Title,
            Body = p.Body,
            UserId = p.UserId,
            AuthorName = p.User.Username
        }).ToList();
        
        return Ok(postDtos);
    }

    [HttpPost]
    public async Task<ActionResult<PostDto>> CreatePost([FromBody] CreatePostDto createPostDto)
    {
        var user = await _userRepository.GetByIdAsync(createPostDto.UserId);
        if (user == null)
        {
            return BadRequest("User not found");
        }

        var post = new Post(createPostDto.Title, createPostDto.Body, createPostDto.UserId);
        var createdPost = await _postRepository.AddAsync(post);
        
        var postDto = new PostDto
        {
            Id = createdPost.Id,
            Title = createdPost.Title,
            Body = createdPost.Body,
            UserId = createdPost.UserId,
            AuthorName = user.Username
        };
        
        return CreatedAtAction(nameof(GetPosts), new { id = postDto.Id }, postDto);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<PostDto>> GetPost(int id)
    {
        var post = await _postRepository.GetMany()
            .Include(p => p.User)
            .FirstOrDefaultAsync(p => p.Id == id);
            
        if (post == null)
        {
            return NotFound();
        }
        
        var postDto = new PostDto
        {
            Id = post.Id,
            Title = post.Title,
            Body = post.Body,
            UserId = post.UserId,
            AuthorName = post.User.Username
        };
        
        return Ok(postDto);
    }

    // Remove the complex GetPostWithDetails method for now
    // You can add it back later once basic functionality works
}

public class CreatePostDto
{
    public string Title { get; set; } = string.Empty;
    public string Body { get; set; } = string.Empty;
    public int UserId { get; set; }
}

public class PostDto
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Body { get; set; } = string.Empty;
    public int UserId { get; set; }
    public string? AuthorName { get; set; }
}