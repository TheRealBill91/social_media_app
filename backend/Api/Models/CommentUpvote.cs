using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.EntityFrameworkCore;

namespace SocialMediaApp.Models;

[PrimaryKey(nameof(AuthorId), nameof(CommentId))]
public class CommentUpvote
{
    [Required]
    public DateTime CreatedAt { get; set; }

    [Required]
    public DateTime UpdatedAt { get; set; }

    public Guid AuthorId { get; set; }

    public Guid CommentId { get; set; }
}
