using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using SocialMediaApp.Data;
using SocialMediaApp.Models;

namespace SocialMediaApp.Services;

public class FriendRequestService
{
    private readonly DataContext _context;

    private readonly FriendshipService _friendshipService;

    public FriendRequestService(DataContext context, FriendshipService friendshipService)
    {
        _context = context;
        _friendshipService = friendshipService;
    }

    // Send a friend request
    public async Task<FriendRequestCreationResult> CreateFriendRequest(
        Guid requesterId,
        Guid receiverId
    )
    {
        var createdAt = DateTime.UtcNow;
        var updatedAt = createdAt;

        var result = await _context.Database.ExecuteSqlAsync(
            $"INSERT INTO friend_request (requester_id, receiver_id, status, created_at, updated_at) VALUES ({requesterId}, {receiverId}, 'pending', {createdAt}, {updatedAt})"
        );

        if (result > 0)
        {
            return new FriendRequestCreationResult
            {
                Success = true,
                Message = "Sucessfully created friend request",
                FriendRequestCreationId = requesterId
            };
        }
        else
        {
            return new FriendRequestCreationResult
            {
                Success = false,
                Message = "Failed to create friend request",
                FriendRequestCreationId = null
            };
        }
    }

    public async Task<FriendRequestAcceptResult> AcceptFriendRequest(
        Guid friendRequestId,
        Guid currentUserId
    )
    {

        // TODO: check if friendship already exists

        var result = await _friendshipService.CreateFriendship(currentUserId, friendRequestId);

        if (result.Success)
        {
            var updatedAt = DateTime.UtcNow;
            var acceptFriendRequest = await _context.Database.ExecuteSqlAsync(
                $"UPDATE friend_request SET status = 'accepted', updated_at = {updatedAt}"
            );
            return new FriendRequestAcceptResult
            {
                Success = true,
                Message = "Successfully created the friend request",
                FriendshipAcceptId = result.FriendshipCreationId
            };
        }
        else
        {
            return new FriendRequestAcceptResult
            {
                Success = false,
                Message = "Failed to accept and/or create the friendship",
                FriendshipAcceptId = null
            };
        }
    }

    public async Task<FriendRequest?> GetFriendRequest(Guid friendRequestId, Guid currentUserId)
    {
        var friendRequest = await _context.FriendRequest
            .FromSql(
                $"SELECT * FROM friend_request WHERE (requester_id = {friendRequestId} OR receiver_id = {friendRequestId}) AND status != 'rejected'"
            )
            .FirstOrDefaultAsync();

        if (friendRequest == null)
        {
            return null;
        }

        if (friendRequest.RequesterId == currentUserId || friendRequest.ReceiverId == currentUserId)
        {
            return friendRequest;
        }

        return null;
    }
}
