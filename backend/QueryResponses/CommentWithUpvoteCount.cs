using System.Numerics;
using Microsoft.EntityFrameworkCore;

[Keyless]
public class CommentWithUpvoteCount
{
    public string Content { get; set; } = null!;

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    public Guid AuthorId { get; set; }

    public Guid PostId { get; set; }

    public int CommentUpvoteCount { get; set; }
}
