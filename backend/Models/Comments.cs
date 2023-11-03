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
    [Column("created")]
    public required DateOnly Created { get; set; }

    [Column("author_id")]
    public Guid AuthorId { get; set; }

    [Column("post_id")]
    public Guid PostId { get; set; }
}
