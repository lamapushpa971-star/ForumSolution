namespace EfcRepositories;

using Microsoft.EntityFrameworkCore;
using RepositoryContracts;
using Entities;

public class EfcCommentRepository : ICommentRepository
{
    private readonly ForumAppContext _context;

    public EfcCommentRepository(ForumAppContext context)
    {
        _context = context;
    }

    public async Task<Comment> AddAsync(Comment comment)
    {
        _context.Comments.Add(comment);
        await _context.SaveChangesAsync();
        return comment;
    }

    public async Task UpdateAsync(Comment comment)
    {
        var existing = await _context.Comments.FindAsync(comment.Id);
        if (existing == null)
            throw new Exception($"Comment with id {comment.Id} not found");
        
        _context.Comments.Update(comment);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var comment = await _context.Comments.FindAsync(id);
        if (comment != null)
        {
            _context.Comments.Remove(comment);
            await _context.SaveChangesAsync();
        }
    }

    public async Task<Comment?> GetByIdAsync(int id)
    {
        return await _context.Comments
            .Include(c => c.User)
            .Include(c => c.Post)
            .FirstOrDefaultAsync(c => c.Id == id);
    }

    public IQueryable<Comment> GetMany()
    {
        return _context.Comments.AsQueryable();
    }
}
