using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using SocialMediaApp.Filters;
using SocialMediaApp.Models;
using SocialMediaApp.Services;

[ApiController]
[ValidateModel]
[Route("/api/friendships")]
public class FriendshipController : Controller
{
    private readonly UserManager<Member> _userManager;
    private readonly FriendshipService _friendshipService;

    private readonly FriendRequestService _friendRequestService;

    public FriendshipController(
        UserManager<Member> userManager,
        FriendshipService friendshipService,
        FriendRequestService friendRequestService
    )
    {
        _friendshipService = friendshipService;
        _userManager = userManager;
        _friendRequestService = friendRequestService;
    }

    [EnableRateLimiting("getResourceSlidingWindow")]
    [Authorize]
    [HttpGet("{friendId:guid}")]
    public async Task<IActionResult> GetFriendship(Guid? friendId)
    {
        if (friendId == null)
        {
            return NotFound("No friendship id available");
        }

        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (string.IsNullOrEmpty(userId))
        {
            return NotFound("No user id available");
        }

        var user = await _userManager.FindByIdAsync(userId);

        if (user == null)
        {
            return NotFound("Can't find the user");
        }

        var friendship = await _friendshipService.GetFriendship(Guid.Parse(userId), friendId.Value);

        if (friendship == null)
        {
            return NotFound("Friendship does not exist");
        }

        return Ok(friendship);
    }

    [EnableRateLimiting("deleteResourceSlidingWindow")]
    [Authorize]
    [HttpDelete("{friendId:guid}")]
    public async Task<IActionResult> DeleteFriendship(Guid? friendId)
    {
        if (friendId == null)
        {
            return NotFound("No friendship id available");
        }

        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (string.IsNullOrEmpty(userId))
        {
            return NotFound("No user id available");
        }

        var user = await _userManager.FindByIdAsync(userId);

        if (user == null)
        {
            return NotFound("Can't find the user");
        }

        var friendship = await _friendshipService.GetFriendship(Guid.Parse(userId), friendId.Value);

        if (friendship == null)
        {
            return NotFound("Friendship does not exist");
        }

        var friendshipDeletionResult = await _friendshipService.DeleteFriendship(
            Guid.Parse(userId),
            friendId.Value
        );

        if (friendshipDeletionResult.Success)
        {
            var friendRequestDeletionResult = await _friendRequestService.DeleteFriendRequest(
                friendId.Value,
                Guid.Parse(userId)
            );

            if (!friendRequestDeletionResult.Success)
            {
                // should not get here
                return BadRequest("Failed to delete friend request when deleting friendship");
            }

            return Ok(friendshipDeletionResult.Message);
        }
        else
        {
            return BadRequest(friendshipDeletionResult.Message);
        }
    }
}
