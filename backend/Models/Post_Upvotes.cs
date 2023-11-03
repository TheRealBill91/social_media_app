using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Components.Web;

namespace SocialMediaApp.Models;

[Table("post_upvote")]
public class Post_Upvotes
{
    [Key]
    [Column("id")]
    public Guid Id { get; set; }

    [Required]
    [Column("timestamp")]
    public required DateTime Timestamp { get; set; }

    [Column("author_id")]
    public Guid AuthorId { get; set; }

    [Column("post_id")]
    public Guid PostId { get; set; }
}
