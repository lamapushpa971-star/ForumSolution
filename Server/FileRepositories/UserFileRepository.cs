using Entities;
using RepositoryContracts;
using System.Text.Json;

namespace FileRepositories;

public class UserFileRepository : IUserRepository
{
    private const string FilePath = "users.json";
    private readonly List<User> _users;
    private int _nextId = 1;

    public UserFileRepository()
    {
        _users = LoadUsersFromFile();
        _nextId = _users.Count > 0 ? _users.Max(u => u.Id) + 1 : 1;
    }

    public async Task<User> AddAsync(User user)
    {
        user.Id = _nextId++;
        _users.Add(user);
        await SaveUsersToFile();
        return user;
    }

    public async Task UpdateAsync(User user)
    {
        var existing = _users.FirstOrDefault(u => u.Id == user.Id);
        if (existing != null)
        {
            _users.Remove(existing);
            _users.Add(user);
            await SaveUsersToFile();
        }
    }

    public async Task DeleteAsync(int id)
    {
        var user = _users.FirstOrDefault(u => u.Id == id);
        if (user != null)
        {
            _users.Remove(user);
            await SaveUsersToFile();
        }
    }

    public async Task<User?> GetByIdAsync(int id)
    {
        return await Task.FromResult(_users.FirstOrDefault(u => u.Id == id));
    }

    public IQueryable<User> GetMany()
    {
        return _users.AsQueryable();
    }

    private List<User> LoadUsersFromFile()
    {
        if (!File.Exists(FilePath))
            return new List<User>();

        var json = File.ReadAllText(FilePath);
        return JsonSerializer.Deserialize<List<User>>(json) ?? new List<User>();
    }

    private async Task SaveUsersToFile()
    {
        var json = JsonSerializer.Serialize(_users, new JsonSerializerOptions { WriteIndented = true });
        await File.WriteAllTextAsync(FilePath, json);
    }

    // Remove these old synchronous methods if they exist:
    // public void Add(User user) - REMOVE THIS
    // public User? GetById(int id) - REMOVE THIS
    // public List<User> GetAll() - REMOVE THIS
    // public void Update(User user) - REMOVE THIS
    // public void Delete(int id) - REMOVE THIS
}