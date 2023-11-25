using System.ComponentModel.DataAnnotations;

namespace SocialMediaApp.Models;

public class PostDTO
{
    [Required(ErrorMessage = "Title is required.")]
    [MaxLength(100, ErrorMessage = "Maximum length of 100 characters exceeded.")]
    public string Title { get; set; } = null!;

    [Required(ErrorMessage = "Content is required.")]
    [MaxLength(5000, ErrorMessage = "Maximum length of 5000 characters exceeded.")]
    public string Content { get; set; } = null!;
}
