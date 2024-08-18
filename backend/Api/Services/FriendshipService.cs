using Microsoft.EntityFrameworkCore;
using Npgsql;
using SocialMediaApp.Data;
using SocialMediaApp.Models;

namespace SocialMediaApp.Services;

public class FriendshipService
{
    private readonly DataContext _context;

    public FriendshipService(DataContext context)
    {
        _context = context;
    }

    public async Task<FriendshipCreationResponse> CreateFriendship(Guid memberId, Guid friendId)
    {
        var createdAt = DateTime.UtcNow;
        var updatedAt = createdAt;
        var result = await _context.Database.ExecuteSqlAsync(
            @$"INSERT INTO friendship (member_id, 
                                       friend_id, 
                                       created_at, 
                                       updated_at) 
               VALUES                  ({memberId}, 
                                        {friendId}, 
                                        {createdAt}, 
                                        {updatedAt})"
        );

        if (result > 0)
        {
            return new FriendshipCreationResponse
            {
                Success = true,
                Message = "Friendship successfully created",
                FriendshipCreationId = memberId
            };
        }
        else
        {
            return new FriendshipCreationResponse
            {
                Success = false,
                Message = "Failed to created friendship",
                FriendshipCreationId = null
            };
        }
    }

    public async Task<Friendship?> GetFriendship(Guid memberId, Guid friendId)
    {
        var friendship = await _context
            .Friendship.FromSql(
                @$"SELECT *
                   FROM friendship
                   WHERE (member_id = {memberId}
                          AND friend_id = {friendId})
                     OR (member_id = {friendId}
                         AND friend_id = {memberId})"
            )
            .FirstOrDefaultAsync();

        return friendship;
    }

    public async Task<FriendshipDeletionResponse> DeleteFriendship(Guid memberId, Guid friendId)
    {
        var friendshipDeletionResult = await _context.Database.ExecuteSqlAsync(
            @$"DELETE
               FROM friendship
               WHERE (member_id = {memberId}
                      AND friend_id = {friendId})"
        );

        if (friendshipDeletionResult > 0)
        {
            return new FriendshipDeletionResponse
            {
                Success = true,
                Message = "Successfully deleted the friendship"
            };
        }
        else
        {
            return new FriendshipDeletionResponse
            {
                Success = false,
                Message = "Failed to delete the friendship"
            };
        }
    }

    public async Task<IEnumerable<Guid>> GetFriendsIds(Guid currentUserId)
    {
        var friendIds = await _context.Friendship
            .Where(f => f.MemberId == currentUserId || f.FriendId == currentUserId)
            .Select(f => f.MemberId == currentUserId ? f.FriendId : f.MemberId)
            .Distinct()
            .ToListAsync();

        return friendIds;
    }
}
