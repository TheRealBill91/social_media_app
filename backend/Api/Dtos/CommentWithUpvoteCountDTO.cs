using Microsoft.EntityFrameworkCore;

namespace SocialMediaApp.DTOs;

[Keyless]
public class CommentWithUpvoteCountDTO
{
    public string Content { get; set; } = null!;

    public DateTime CreatedAt { get; set; }

    public Guid AuthorId { get; set; }

    public DateTime UpdatedAt { get; set; }

    public int CommentUpvoteCount { get; set; }

    public string Author { get; set; } = null!;
}
