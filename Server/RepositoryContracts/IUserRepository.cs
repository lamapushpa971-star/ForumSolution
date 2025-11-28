using Entities;

namespace RepositoryContracts;

public interface IUserRepository
{
    Task<User> AddAsync(User user);  // Changed to async
    Task UpdateAsync(User user);     // Changed to async  
    Task DeleteAsync(int id);        // Changed to async
    Task<User?> GetByIdAsync(int id); // Changed to async
    IQueryable<User> GetMany();      // Changed from GetAll() to GetMany()
   
}
