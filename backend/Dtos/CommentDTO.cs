using System.ComponentModel.DataAnnotations;

namespace SocialMediaApp.Models;

public class CommentDTO
{
    [Required(ErrorMessage = "Content is required.")]
    [MaxLength(200, ErrorMessage = "Maximum length of 200 characters exceeded.")]
    [DataType(DataType.Text)]
    public string Content { get; set; } = null!;
}
