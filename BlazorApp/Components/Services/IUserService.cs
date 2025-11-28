using ApiContracts.DTO;

namespace BlazorApp.Components.Services;

public interface IUserService
{
    Task<UserDto> AddUserAsync(CreateUserDto request);
    Task<UserDto> GetUserAsync(int id);
    Task<List<UserDto>> GetUsersAsync();
}