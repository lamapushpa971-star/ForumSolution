using Entities;
using RepositoryContracts;
using System.Text.Json;

namespace FileRepositories;

public class PostFileRepository : IPostRepository
{
    private const string FilePath = "posts.json";
    private readonly List<Post> _posts;
    private int _nextId = 1;

    public PostFileRepository()
    {
        _posts = LoadPostsFromFile();
        _nextId = _posts.Count > 0 ? _posts.Max(p => p.Id) + 1 : 1;
    }

    public async Task<Post> AddAsync(Post post)
    {
        post.Id = _nextId++;
        _posts.Add(post);
        await SavePostsToFile();
        return post;
    }

    public async Task UpdateAsync(Post post)
    {
        var existing = _posts.FirstOrDefault(p => p.Id == post.Id);
        if (existing != null)
        {
            _posts.Remove(existing);
            _posts.Add(post);
            await SavePostsToFile();
        }
    }

    public async Task DeleteAsync(int id)
    {
        var post = _posts.FirstOrDefault(p => p.Id == id);
        if (post != null)
        {
            _posts.Remove(post);
            await SavePostsToFile();
        }
    }

    public async Task<Post?> GetByIdAsync(int id)
    {
        return await Task.FromResult(_posts.FirstOrDefault(p => p.Id == id));
    }

    public IQueryable<Post> GetMany()
    {
        return _posts.AsQueryable();
    }

    private List<Post> LoadPostsFromFile()
    {
        if (!File.Exists(FilePath))
            return new List<Post>();

        var json = File.ReadAllText(FilePath);
        return JsonSerializer.Deserialize<List<Post>>(json) ?? new List<Post>();
    }

    private async Task SavePostsToFile()
    {
        var json = JsonSerializer.Serialize(_posts, new JsonSerializerOptions { WriteIndented = true });
        await File.WriteAllTextAsync(FilePath, json);
    }
}