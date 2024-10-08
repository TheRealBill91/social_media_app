using Microsoft.EntityFrameworkCore;

namespace SocialMediaApp.Models;

[PrimaryKey(nameof(MemberId), nameof(FriendId))]
public class Friendship
{
    public Guid MemberId { get; set; }

    public Guid FriendId { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }
}
