using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RepositoryContracts;
using Entities;

namespace WebAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UsersController : ControllerBase
{
    private readonly IUserRepository _userRepository;

    public UsersController(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<UserDto>>> GetUsers([FromQuery] string? userNameContains = null)
    {
        IList<User> users = await _userRepository.GetMany()
            .Where(u => userNameContains == null || 
                       u.Username.ToLower().Contains(userNameContains.ToLower()))
            .ToListAsync();
        
        var userDtos = users.Select(u => new UserDto
        {
            Id = u.Id,
            Username = u.Username
        }).ToList();
        
        return Ok(userDtos);
    }

    [HttpPost]
    public async Task<ActionResult<UserDto>> CreateUser([FromBody] CreateUserDto createUserDto)
    {
        bool userExists = await _userRepository.GetMany()
            .AnyAsync(u => u.Username == createUserDto.UserName);
        
        if (userExists)
        {
            return BadRequest("Username already exists");
        }

        var user = new User(createUserDto.UserName, createUserDto.Password);
        var createdUser = await _userRepository.AddAsync(user);
        
        var userDto = new UserDto
        {
            Id = createdUser.Id,
            Username = createdUser.Username
        };
        
        return CreatedAtAction(nameof(GetUsers), new { id = userDto.Id }, userDto);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<UserDto>> GetUser(int id)
    {
        var user = await _userRepository.GetByIdAsync(id);
        if (user == null)
        {
            return NotFound();
        }
        
        var userDto = new UserDto
        {
            Id = user.Id,
            Username = user.Username
        };
        
        return Ok(userDto);
    }
}

public class CreateUserDto
{
    public string UserName { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
}

public class UserDto
{
    public int Id { get; set; }
    public string Username { get; set; } = string.Empty;
}