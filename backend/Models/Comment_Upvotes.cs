using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Components.Web;

namespace SocialMediaApp.Models;

[Table("comment_upvote")]
public class Comment_Upvotes
{
    [Key]
    [Column("id")]
    public Guid Id { get; set; }

    [Required]
    [Column("time_stamp")]
    public required DateTime Timestamp { get; set; }

    [Column("author_id")]
    public Guid AuthorId { get; set; }

    [Column("comment_id")]
    public Guid CommentId { get; set; }
}
