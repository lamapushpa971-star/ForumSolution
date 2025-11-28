using Entities;
using RepositoryContracts;

namespace InMemoryRepositories;

public class InMemoryCommentRepository : ICommentRepository
{
    private readonly List<Comment> _comments = []; // Use collection expression
    private int _nextId = 1;

    public async Task<Comment> AddAsync(Comment comment)
    {
        comment.Id = _nextId++;
        _comments.Add(comment);
        return await Task.FromResult(comment); // Make it async
    }

    public async Task UpdateAsync(Comment comment)
    {
        var existing = _comments.FirstOrDefault(c => c.Id == comment.Id);
        if (existing != null)
        {
            _comments.Remove(existing);
            _comments.Add(comment);
        }
        await Task.CompletedTask; // Make it async
    }

    public async Task DeleteAsync(int id)
    {
        var comment = _comments.FirstOrDefault(c => c.Id == id);
        if (comment != null)
        {
            _comments.Remove(comment);
        }
        await Task.CompletedTask; // Make it async
    }

    public async Task<Comment?> GetByIdAsync(int id)
    {
        var comment = _comments.FirstOrDefault(c => c.Id == id);
        return await Task.FromResult(comment); // Make it async
    }

    public IQueryable<Comment> GetMany()
    {
        return _comments.AsQueryable();
    }

    // If you had the old synchronous methods, remove them:
    // public void Add(Comment comment) - REMOVE THIS
    // public Comment? GetById(int id) - REMOVE THIS  
    // public List<Comment> GetAll() - REMOVE THIS
}