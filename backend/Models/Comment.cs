using System.ComponentModel.DataAnnotations;

namespace SocialMediaApp.Models;

public class Comment
{
    [Key]
    public Guid Id { get; set; }

    [Required]
    public string Content { get; set; } = null!;

    [Required]
    public DateTime CreatedAt { get; set; }

    [Required]
    public DateTime UpdatedAt { get; set; }

    public DateTime? DeletedAt { get; set; }

    public Guid AuthorId { get; set; }

    public Guid PostId { get; set; }
}
