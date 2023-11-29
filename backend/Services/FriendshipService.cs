using Microsoft.EntityFrameworkCore;
using SocialMediaApp.Data;

namespace SocialMediaApp.Services;

public class FriendshipService
{
    private readonly DataContext _context;

    public FriendshipService(DataContext context)
    {
        _context = context;
    }

    public async Task<FriendshipCreationResult> CreateFriendship(Guid memberId, Guid friendId)
    {
        var createdAt = DateTime.UtcNow;
        var updatedAt = createdAt;
        var result = await _context.Database.ExecuteSqlAsync(
            $"INSERT INTO friendship (member_id, friend_id, created_at, updated_at) VALUES ({memberId}, {friendId}, {createdAt}, {updatedAt})"
        );

        if (result > 0)
        {
            return new FriendshipCreationResult
            {
                Success = true,
                Message = "Friendship successfully created",
                FriendshipCreationId = memberId
            };
        }
        else
        {
            return new FriendshipCreationResult
            {
                Success = false,
                Message = "Failed to created friendship",
                FriendshipCreationId = null
            };
        }
    }
}
