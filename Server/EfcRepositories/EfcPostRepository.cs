namespace EfcRepositories;

using Microsoft.EntityFrameworkCore;
using RepositoryContracts;
using Entities;

public class EfcPostRepository : IPostRepository
{
    private readonly ForumAppContext _context;

    public EfcPostRepository(ForumAppContext context)
    {
        _context = context;
    }

    public async Task<Post> AddAsync(Post post)
    {
        _context.Posts.Add(post);
        await _context.SaveChangesAsync();
        return post;
    }

    public async Task UpdateAsync(Post post)
    {
        var existing = await _context.Posts.FindAsync(post.Id);
        if (existing == null)
            throw new Exception($"Post with id {post.Id} not found");
        
        _context.Posts.Update(post);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var post = await _context.Posts.FindAsync(id);
        if (post != null)
        {
            _context.Posts.Remove(post);
            await _context.SaveChangesAsync();
        }
    }

    public async Task<Post?> GetByIdAsync(int id)
    {
        return await _context.Posts
            .Include(p => p.User)
            .Include(p => p.Comments)
            .FirstOrDefaultAsync(p => p.Id == id);
    }

    public IQueryable<Post> GetMany()
    {
        return _context.Posts.AsQueryable();
    }
}