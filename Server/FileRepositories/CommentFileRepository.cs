using Entities;
using RepositoryContracts;
using System.Text.Json;

namespace FileRepositories;

public class CommentFileRepository : ICommentRepository
{
    private const string FilePath = "comments.json";
    private readonly List<Comment> _comments;
    private int _nextId = 1;

    public CommentFileRepository()
    {
        _comments = LoadCommentsFromFile();
        _nextId = _comments.Count > 0 ? _comments.Max(c => c.Id) + 1 : 1;
    }

    public async Task<Comment> AddAsync(Comment comment)
    {
        comment.Id = _nextId++;
        _comments.Add(comment);
        await SaveCommentsToFile();
        return comment;
    }

    public async Task UpdateAsync(Comment comment)
    {
        var existing = _comments.FirstOrDefault(c => c.Id == comment.Id);
        if (existing != null)
        {
            _comments.Remove(existing);
            _comments.Add(comment);
            await SaveCommentsToFile();
        }
    }

    public async Task DeleteAsync(int id)
    {
        var comment = _comments.FirstOrDefault(c => c.Id == id);
        if (comment != null)
        {
            _comments.Remove(comment);
            await SaveCommentsToFile();
        }
    }

    public async Task<Comment?> GetByIdAsync(int id)
    {
        return await Task.FromResult(_comments.FirstOrDefault(c => c.Id == id));
    }

    public IQueryable<Comment> GetMany()
    {
        return _comments.AsQueryable();
    }

    private List<Comment> LoadCommentsFromFile()
    {
        if (!File.Exists(FilePath))
            return new List<Comment>();

        var json = File.ReadAllText(FilePath);
        return JsonSerializer.Deserialize<List<Comment>>(json) ?? new List<Comment>();
    }

    private async Task SaveCommentsToFile()
    {
        var json = JsonSerializer.Serialize(_comments, new JsonSerializerOptions { WriteIndented = true });
        await File.WriteAllTextAsync(FilePath, json);
    }
}