using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
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

    [Authorize]
    [HttpPost("{receiverId}/create")]
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

    [Authorize]
    [HttpPost("{friendRequestId}/accept")]
    public async Task<IActionResult> AcceptFriendRequest(Guid? friendRequestId)
    {
        if (friendRequestId == null)
        {
            return NotFound();
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
            return BadRequest("No friend request exists between you and the user");
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
            return CreatedAtAction(
                nameof(GetFriendRequest),
                new { friendRequestId = result.FriendshipAcceptId },
                result
            );
        }
        else
        {
            return BadRequest(result.Message);
        }
    }

    [Authorize]
    [HttpGet("{friendRequestId}")]
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
}
