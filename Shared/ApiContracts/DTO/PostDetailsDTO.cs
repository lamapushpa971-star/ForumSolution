namespace ApiContracts.DTO;
public class PostDetailsDto
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Body { get; set; } = string.Empty;
    public string AuthorName { get; set; } = string.Empty; // Add this line
    public DateTime CreatedAt { get; set; }
    public List<CommentDto> Comments { get; set; } = new();
}