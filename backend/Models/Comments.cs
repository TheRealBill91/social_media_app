using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Components.Web;

namespace SocialMediaApp.Models;

[Table("comment")]
public class Comments
{
    [Key]
    [Column("id")]
    public Guid Id { get; set; }

    [Required]
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

    [Column("post_id")]
    public Guid PostId { get; set; }
}
