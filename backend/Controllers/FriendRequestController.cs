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
[Route("/api/friend-requests")]
public class FriendRequestController : Controller
{
    private readonly UserManager<Member> _userManager;

    private readonly FriendRequestService _friendRequestService;

    public FriendRequestController(
        UserManager<Member> userManager,
        FriendRequestService friendRequestService
    )
    {
        _userManager = userManager;
        _friendRequestService = friendRequestService;
    }

    [EnableRateLimiting("createResourceSlidingWindow")]
    [Authorize]
    [HttpPost("{receiverId:guid}/create")]
    public async Task<IActionResult> CreateFriendRequest(Guid receiverId)
    {
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

        var friendRequestExists = await _friendRequestService.GetFriendRequest(
            receiverId,
            Guid.Parse(userId)
        );

        if (friendRequestExists != null)
        {
            return BadRequest("Friend request already exists between you and the receipent");
        }

        var requesterId = Guid.Parse(userId);

        var result = await _friendRequestService.CreateFriendRequest(requesterId, receiverId);

        if (result.Success)
        {
            return CreatedAtAction(
                nameof(GetFriendRequest),
                new { friendRequestId = result.FriendRequestCreationId },
                result
            );
        }
        else
        {
            return BadRequest(result.Message);
        }
    }

    [EnableRateLimiting("createResourceSlidingWindow")]
    [Authorize]
    [HttpPost("{friendRequestId:guid}/accept")]
    public async Task<IActionResult> AcceptFriendRequest(Guid? friendRequestId)
    {
        if (friendRequestId == null)
        {
            return BadRequest("No friend request id is available");
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

        var friendRequest = await _friendRequestService.GetFriendRequest(
            friendRequestId.Value,
            Guid.Parse(userId)
        );

        if (friendRequest == null)
        {
            return NotFound("No friend request exists between you and the user");
        }

        // reject if signed in user id does not equal the friend request receiver id
        if (userId != friendRequest.ReceiverId.ToString())
        {
            return Forbid();
        }

        var result = await _friendRequestService.AcceptFriendRequest(
            friendRequestId.Value,
            Guid.Parse(userId)
        );

        if (result.Success)
        {
            string? friendshipUrl = Url.Action(
                nameof(FriendshipController.GetFriendship),
                "Friendship",
                new { friendId = result.FriendshipAcceptId },
                Request.Scheme
            );

            if (friendshipUrl == null)
            {
                // Handle the case where the URL could not be generated
                return StatusCode(
                    StatusCodes.Status500InternalServerError,
                    "Could not generate URL for the new friendship"
                );
            }

            return Created(friendshipUrl, result);
        }
        else
        {
            return BadRequest(result.Message);
        }
    }

    [EnableRateLimiting("getResourceSlidingWindow")]
    [Authorize]
    [HttpGet("{friendRequestId:guid}")]
    public async Task<IActionResult> GetFriendRequest(Guid? friendRequestId)
    {
        if (friendRequestId == null)
        {
            return NotFound();
        }

        var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (string.IsNullOrEmpty(currentUserId))
        {
            return NotFound("No user id available");
        }

        var user = await _userManager.FindByIdAsync(currentUserId);

        if (user == null)
        {
            return NotFound("Can't find the user");
        }

        var friendRequest = await _friendRequestService.GetFriendRequest(
            friendRequestId.Value,
            Guid.Parse(currentUserId)
        );
        if (friendRequest == null)
        {
            return NotFound("Friend request does not exist");
        }

        return Ok(friendRequest);
    }

    // Gets all incoming friend requests for the logged in user
    [EnableRateLimiting("getResourceSlidingWindow")]
    [Authorize]
    [HttpGet("incoming")]
    public async Task<IActionResult> IncomingFriendRequests()
    {
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

        var incomingFriendRequests = await _friendRequestService.IncomingFriendRequests(
            Guid.Parse(userId)
        );

        return Ok(incomingFriendRequests);
    }

    // Gets all outgoing friend requests for the logged in user
    [EnableRateLimiting("getResourceSlidingWindow")]
    [Authorize]
    [HttpGet("outgoing")]
    public async Task<IActionResult> OutgoingFriendRequests()
    {
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

        var outgoingFriendRequests = await _friendRequestService.OutgoingFriendRequests(
            Guid.Parse(userId)
        );

        return Ok(outgoingFriendRequests);
    }

    // Removes friend request from perspective of user who sent it
    [EnableRateLimiting("deleteResourceSlidingWindow")]
    [Authorize]
    [HttpDelete("{friendRequestId:guid}/cancel")]
    public async Task<IActionResult> CancelFriendRequest(Guid? friendRequestId)
    {
        if (friendRequestId == null)
        {
            return BadRequest("No friend request id is available");
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

        var friendRequest = await _friendRequestService.GetFriendRequest(
            friendRequestId.Value,
            Guid.Parse(userId)
        );

        if (friendRequest == null)
        {
            return NotFound("No friend request exists between you and the user");
        }

        // reject if signed in user id does not equal the friend requester id
        if (userId != friendRequest.RequesterId.ToString())
        {
            return Forbid();
        }

        var result = await _friendRequestService.CancelFriendRequest(
            friendRequestId.Value,
            Guid.Parse(userId)
        );

        if (result.Success)
        {
            return Ok(result.Message);
        }
        else
        {
            return BadRequest(result.Message);
        }
    }

    // Recipient rejects the friend request
    [EnableRateLimiting("updateResourceSlidingWindow")]
    [Authorize]
    [HttpPatch("{friendRequestId:guid}/reject")]
    public async Task<IActionResult> RejectFriendRequest(Guid? friendRequestId)
    {
        if (friendRequestId == null)
        {
            return BadRequest("No friend request id is available");
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

        var friendRequest = await _friendRequestService.GetFriendRequest(
            friendRequestId.Value,
            Guid.Parse(userId)
        );

        if (friendRequest == null)
        {
            return NotFound("No friend request exists between you and the user");
        }

        // reject if signed in user id does not equal the friend request receiver id
        if (userId != friendRequest.ReceiverId.ToString())
        {
            return Forbid();
        }

        var result = await _friendRequestService.RejectFriendRequest(
            friendRequestId.Value,
            Guid.Parse(userId)
        );

        if (result.Success)
        {
            return Ok(result.Message);
        }
        else
        {
            return BadRequest(result.Message);
        }
    }
}
