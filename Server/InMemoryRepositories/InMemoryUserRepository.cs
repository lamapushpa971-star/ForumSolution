using Entities;
using RepositoryContracts;

namespace InMemoryRepositories;

public class InMemoryUserRepository : IUserRepository
{
    private readonly List<User> _users = [];
    private int _nextId = 1;

    public async Task<User> AddAsync(User user)
    {
        user.Id = _nextId++;
        _users.Add(user);
        return await Task.FromResult(user);
    }

    public async Task UpdateAsync(User user)
    {
        var existing = _users.FirstOrDefault(u => u.Id == user.Id);
        if (existing != null)
        {
            _users.Remove(existing);
            _users.Add(user);
        }
        await Task.CompletedTask;
    }

    public async Task DeleteAsync(int id)
    {
        var user = _users.FirstOrDefault(u => u.Id == id);
        if (user != null)
        {
            _users.Remove(user);
        }
        await Task.CompletedTask;
    }

    public async Task<User?> GetByIdAsync(int id)
    {
        var user = _users.FirstOrDefault(u => u.Id == id);
        return await Task.FromResult(user);
    }

    public IQueryable<User> GetMany()
    {
        return _users.AsQueryable();
    }
}