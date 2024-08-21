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
    public async Task<FriendRequestCreationResponse> CreateFriendRequest(
        Guid requesterId,
        Guid receiverId
    )
    {
        var createdAt = DateTime.UtcNow;
        var updatedAt = createdAt;

        var result = await _context.Database.ExecuteSqlAsync(
            @$"INSERT INTO friend_request
                               (requester_id,
                               receiver_id,
                               status,
                               created_at,
                               updated_at)
               VALUES          ({requesterId},
                               {receiverId},
                               'pending',
                               {createdAt},
                               {updatedAt})"
        );

        if (result > 0)
        {
            return new FriendRequestCreationResponse
            {
                Success = true,
                Message = "Sucessfully created friend request",
                FriendRequestCreationId = requesterId
            };
        }
        else
        {
            return new FriendRequestCreationResponse
            {
                Success = false,
                Message = "Failed to create friend request",
                FriendRequestCreationId = null
            };
        }
    }

    public async Task<FriendRequestAcceptResponse> AcceptFriendRequest(
        Guid friendRequestId,
        Guid currentUserId
    )
    {
        var friendship = await _friendshipService.GetFriendship(currentUserId, friendRequestId);

        if (friendship != null)
        {
            return new FriendRequestAcceptResponse
            {
                Success = false,
                Message = "Friendship already exists",
                FriendshipAcceptId = null
            };
        }

        var result = await _friendshipService.CreateFriendship(currentUserId, friendRequestId);

        if (result.Success)
        {
            var updatedAt = DateTime.UtcNow;
            await _context.Database.ExecuteSqlAsync(
                @$"UPDATE friend_request
                   SET status = 'accepted',
                       updated_at = {updatedAt}
                   WHERE requester_id = {friendRequestId}
                     AND receiver_id = {currentUserId}"
            );
            return new FriendRequestAcceptResponse
            {
                Success = true,
                Message = result.Message,
                FriendshipAcceptId = result.FriendshipCreationId
            };
        }
        else
        {
            return new FriendRequestAcceptResponse
            {
                Success = false,
                Message = "Failed to accept and/or create the friendship",
                FriendshipAcceptId = null
            };
        }
    }

    public async Task<FriendRequest?> GetFriendRequest(Guid friendRequestId, Guid currentUserId)
    {
        var friendRequest = await _context
            .FriendRequest.FromSql(
                @$"SELECT *
                   FROM friend_request
                   WHERE (requester_id = {friendRequestId}
                          AND receiver_id = {currentUserId})
                     OR (requester_id = {currentUserId}
                         AND receiver_id = {friendRequestId})
                     AND status != 'rejected'"
            )
            .FirstOrDefaultAsync();

        if (friendRequest == null)
        {
            return null;
        }

        // Check if the current user is involved in the friend request
        if (friendRequest.RequesterId == currentUserId || friendRequest.ReceiverId == currentUserId)
        {
            return friendRequest;
        }

        return null;
    }

    public async Task<IReadOnlyList<FriendRequest>> IncomingFriendRequests(
        Guid currentUserId
    )
    {
        var incomingFriendRequests = await _context
            .FriendRequest.FromSql(
                @$"SELECT *
                   FROM friend_request
                   WHERE receiver_id = {currentUserId}
                     AND status = 'pending'
                   ORDER BY created_at DESC"
            )
            .ToListAsync();

        return incomingFriendRequests;
    }

    public async Task<IReadOnlyList<FriendRequest>> OutgoingFriendRequests(
        Guid currentUserId
    )
    {
        var outgoingFriendRequests = await _context
            .FriendRequest.FromSql(
                @$"SELECT *
                   FROM friend_request
                   WHERE requester_id = {currentUserId}
                     AND status = 'pending'
                   ORDER BY created_at DESC"
            )
            .ToListAsync();

        return outgoingFriendRequests;
    }

    public async Task<FriendRequestCancelResponse> CancelFriendRequest(
        Guid friendRequestId,
        Guid currentUserId
    )
    {
        var result = await _context.Database.ExecuteSqlAsync(
            @$"DELETE
               FROM friend_request
               WHERE (requester_id = {currentUserId}
                      AND receiver_id = {friendRequestId})"
        );

        if (result > 0)
        {
            return new FriendRequestCancelResponse
            {
                Success = true,
                Message = "Successfully cancelled the friend request"
            };
        }
        else
        {
            return new FriendRequestCancelResponse
            {
                Success = false,
                Message = "Failed to cancel the friend request"
            };
        }
    }

    public async Task<FriendRequestRejectResponse> RejectFriendRequest(
        Guid friendRequestId,
        Guid currentUserId
    )
    {
        var updatedAt = DateTime.UtcNow;
        var result = await _context.Database.ExecuteSqlAsync(
            @$"UPDATE friend_request
               SET status = 'rejected',
                   updated_at = {updatedAt}
               WHERE receiver_id = {currentUserId}
                 AND requester_id = {friendRequestId}"
        );

        if (result > 0)
        {
            return new FriendRequestRejectResponse
            {
                Success = true,
                Message = "Successfully rejected the friend request"
            };
        }
        else
        {
            return new FriendRequestRejectResponse
            {
                Success = false,
                Message = "Failed to reject the friend request"
            };
        }
    }

    // Completely removes friend request from database (e.g., after removing friendship)
    public async Task<FriendRequestCancelResponse> DeleteFriendRequest(
        Guid friendRequestId,
        Guid currentUserId
    )
    {
        var friendRequestDeletionResult =
            await _context.Database.ExecuteSqlAsync(
                @$"DELETE
                   FROM friend_request
                   WHERE (receiver_id = {currentUserId}
                          AND requester_id = {friendRequestId})
                     OR (receiver_id = {friendRequestId}
                         AND requester_id = {currentUserId})"
            );

        if (friendRequestDeletionResult > 0)
        {
            return new FriendRequestCancelResponse
            {
                Success = true,
                Message = "Successfully deleted the friend request"
            };
        }
        else
        {
            return new FriendRequestCancelResponse
            {
                Success = false,
                Message = "Failed to delete the friend request"
            };
        }
    }
}
