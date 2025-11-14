using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PostsController : ControllerBase
{
    private static List<PostDto> _posts = new();
    private static int _nextId = 1;

    public PostsController()
    {
        // Add sample data for testing
        if (_posts.Count == 0)
        {
            _posts.Add(new PostDto
            {
                Id = _nextId++,
                Title = "Welcome to the Forum",
                Content = "This is the first post in our forum. Feel free to introduce yourself!",
                UserId = 1,
                Username = "admin",
                CreatedAt = DateTime.Now
            });
        }
    }

    [HttpGet]
    public IActionResult GetPosts()
    {
        return Ok(_posts);
    }

    [HttpGet("{id}")]
    public IActionResult GetPost(int id)
    {
        var post = _posts.FirstOrDefault(p => p.Id == id);
        if (post == null)
        {
            return NotFound();
        }
        return Ok(post);
    }

    [HttpPost]
    public IActionResult CreatePost([FromBody] CreatePostDto createPostDto)
    {
        var post = new PostDto
        {
            Id = _nextId++,
            Title = createPostDto.Title ?? string.Empty,
            Content = createPostDto.Content ?? string.Empty,
            UserId = createPostDto.UserId,
            Username = "user", // Hardcoded for now
            CreatedAt = DateTime.Now
        };
        _posts.Add(post);
        return Ok(post);
    }
}

public class CreatePostDto
{
    public string Title { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;
    public int UserId { get; set; }
}

public class PostDto
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;
    public int UserId { get; set; }
    public string Username { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
}