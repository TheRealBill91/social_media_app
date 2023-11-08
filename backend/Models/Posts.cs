using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Components.Web;

namespace SocialMediaApp.Models;

[Table("post")]
public class Posts
{
    [Key]
    [Column("id")]
    public Guid Id { get; set; }

    [Required(ErrorMessage = "Title is required.")]
    [MaxLength(100, ErrorMessage = "Maximum length of 100 characters exceeded.")]
    [Column("title")]
    public required string Title { get; set; }

    [Required(ErrorMessage = "Content is required.")]
    [MaxLength(5000, ErrorMessage = "Maximum length of 5000 characters exceeded.")]
    [Column("content")]
    public required string Content { get; set; }

    [Required]
    [Column("created_at")]
    public required DateTime CreatedAt { get; set; }

    [Required]
    [Column("updated_at")]
    public required DateTime UpdatedAt { get; set; }

    [Required]
    [Column("deleted_at")]
    public required DateTime DeletedAt { get; set; }

    [Column("author_id")]
    public Guid AuthorId { get; set; }
}
