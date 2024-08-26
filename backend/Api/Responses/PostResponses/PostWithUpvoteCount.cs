using Microsoft.EntityFrameworkCore;

[Keyless]
public class PostWithUpvoteCount
{
    public string Title { get; set; } = null!;
    public string Content { get; set; } = null!;

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    public Guid AuthorId { get; set; }

    public int PostUpvoteCount { get; set; }
}
