using System.ComponentModel.DataAnnotations;

namespace ApiContracts.DTO;
public class CreateCommentDto
{
    [Required(ErrorMessage = "Comment content is required")]
    public string Content { get; set; } = string.Empty; // Changed to Content
    
    public int PostId { get; set; }
    public int UserId { get; set; } = 1;
}