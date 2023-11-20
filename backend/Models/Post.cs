using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Components.Web;

namespace SocialMediaApp.Models;

public class Post
{
    [Key]
    public Guid Id { get; set; }

    [Required(ErrorMessage = "Title is required.")]
    [MaxLength(100, ErrorMessage = "Maximum length of 100 characters exceeded.")]
    public string Title { get; set; } = null!;

    [Required(ErrorMessage = "Content is required.")]
    [MaxLength(5000, ErrorMessage = "Maximum length of 5000 characters exceeded.")]
    public string Content { get; set; } = null!;

    [Required]
    public DateTime CreatedAt { get; set; }

    [Required]
    public DateTime UpdatedAt { get; set; }

    public DateTime? DeletedAt { get; set; }

    public Guid AuthorId { get; set; }
}
