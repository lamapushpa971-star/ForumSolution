using Entities;

namespace RepositoryContracts;

public interface IUserRepository
{
    // Synchronous methods
    void Add(User user);
    void Update(User user);
    void Delete(int id);
    User? GetById(int id);
    List<User> GetAll();
    
    // Async method (for potential future async operations)
    Task<User?> GetUserByUsernameAsync(string username);
    
    // Optional useful methods you might want:
    User? GetByUsername(string username); // Synchronous version
    bool UsernameExists(string username);
}