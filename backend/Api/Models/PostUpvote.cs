using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace SocialMediaApp.Models;

[PrimaryKey(nameof(AuthorId), nameof(PostId))]
public class PostUpvote
{
    [Required]
    public DateTime CreatedAt { get; set; }

    [Required]
    public DateTime UpdatedAt { get; set; }

    public Guid AuthorId { get; set; }

    public Guid PostId { get; set; }
}
