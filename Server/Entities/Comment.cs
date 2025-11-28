namespace Entities;

public class Comment
{
    public int Id { get; set; }
    public string Body { get; set; } = null!;
    public int UserId { get; set; }   // foreign key
    public int PostId { get; set; }   // foreign key
    
    // Navigation properties
    public User User { get; set; }
    public Post Post { get; set; }
    
    // Public constructor for CLI
    public Comment(string body, int userId, int postId)
    {
        Body = body;
        UserId = userId;
        PostId = postId;
    }
    
    private Comment() { } // For EFC
}
