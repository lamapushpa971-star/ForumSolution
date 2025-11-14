using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UsersController : ControllerBase
{
    private static List<UserDto> _users = new();
    private static int _nextId = 1;

    [HttpGet]
    public IActionResult GetUsers()
    {
        return Ok(_users);
    }

    [HttpPost]
    public IActionResult CreateUser([FromBody] CreateUserDto createUserDto)
    {
        var user = new UserDto
        {
            Id = _nextId++,
            UserName = createUserDto.UserName ?? string.Empty, // Add null check
            Email = createUserDto.Email ?? string.Empty        // Add null check
        };
        _users.Add(user);
        return Ok(user);
    }
}

public class CreateUserDto
{
    public string UserName { get; set; } = string.Empty;  // Initialize with empty string
    public string Email { get; set; } = string.Empty;     // Initialize with empty string
}

public class UserDto
{
    public int Id { get; set; }
    public string UserName { get; set; } = string.Empty;  // Initialize with empty string
    public string Email { get; set; } = string.Empty;     // Initialize with empty string
}