using Microsoft.AspNetCore.Mvc;
using RepositoryContracts;
using Entities;
using System.Threading.Tasks;

namespace WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IUserRepository _userRepository;

        public AuthController(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest loginRequest)
        {
            if (string.IsNullOrEmpty(loginRequest.Username) || string.IsNullOrEmpty(loginRequest.Password))
            {
                return BadRequest("Username and password are required");
            }

            // Get user by username
            var user = await _userRepository.GetUserByUsernameAsync(loginRequest.Username);
            
            if (user == null)
            {
                return Unauthorized("Invalid username or password");
            }

            // Check password - using Password property (not PasswordHash)
            if (user.Password != loginRequest.Password)
            {
                return Unauthorized("Invalid username or password");
            }

            // Login successful - return user info (without password)
            var response = new
            {
                user.Id,
                user.Username,
                user.Email,
                user.FirstName,
                user.LastName,
                user.Role
            };

            return Ok(response);
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequest registerRequest)
        {
            if (string.IsNullOrEmpty(registerRequest.Username) || string.IsNullOrEmpty(registerRequest.Password))
            {
                return BadRequest("Username and password are required");
            }

            // Check if username already exists
            var existingUser = await _userRepository.GetUserByUsernameAsync(registerRequest.Username);
            if (existingUser != null)
            {
                return BadRequest("Username already exists");
            }

            // Create new user
            var newUser = new User
            {
                Username = registerRequest.Username,
                Password = registerRequest.Password, // Using Password property
                Email = registerRequest.Email,
                FirstName = registerRequest.FirstName,
                LastName = registerRequest.LastName,
                Role = "User", // Default role
                CreatedAt = DateTime.UtcNow
            };

            // Add user to repository
            _userRepository.Add(newUser);

            // Return created user (without password)
            var response = new
            {
                newUser.Id,
                newUser.Username,
                newUser.Email,
                newUser.FirstName,
                newUser.LastName,
                newUser.Role
            };

            return CreatedAtAction(nameof(Login), response);
        }
    }

    // DTO classes for requests
    public class LoginRequest
    {
        public string Username { get; set; } = string.Empty; // Using Username (not UserName)
        public string Password { get; set; } = string.Empty; // Using Password (not PasswordHash)
    }

    public class RegisterRequest
    {
        public string Username { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
    }
}