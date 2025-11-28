using Entities;
using RepositoryContracts;

namespace InMemoryRepositories;

public class InMemoryPostRepository : IPostRepository
{
    private readonly List<Post> _posts = [];
    private int _nextId = 1;

    public async Task<Post> AddAsync(Post post)
    {
        post.Id = _nextId++;
        _posts.Add(post);
        return await Task.FromResult(post);
    }

    public async Task UpdateAsync(Post post)
    {
        var existing = _posts.FirstOrDefault(p => p.Id == post.Id);
        if (existing != null)
        {
            _posts.Remove(existing);
            _posts.Add(post);
        }
        await Task.CompletedTask;
    }

    public async Task DeleteAsync(int id)
    {
        var post = _posts.FirstOrDefault(p => p.Id == id);
        if (post != null)
        {
            _posts.Remove(post);
        }
        await Task.CompletedTask;
    }

    public async Task<Post?> GetByIdAsync(int id)
    {
        var post = _posts.FirstOrDefault(p => p.Id == id);
        return await Task.FromResult(post);
    }

    public IQueryable<Post> GetMany()
    {
        return _posts.AsQueryable();
    }
}