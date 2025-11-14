namespace ApiContracts.DTO;

using System.ComponentModel.DataAnnotations;

public class CreatePostDto
{
    [Required(ErrorMessage = "Title is required")]
    [StringLength(100, ErrorMessage = "Title cannot exceed 100 characters")]
    public string Title { get; set; } = string.Empty;

    [Required(ErrorMessage = "Content is required")]
    public string Content { get; set; } = string.Empty;

    // Hardcode to 1 for now (as per assignment instructions)
    public int UserId { get; set; } = 1;  
}