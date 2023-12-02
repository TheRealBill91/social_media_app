using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
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

    public FriendshipController(
        UserManager<Member> userManager,
        FriendshipService friendshipService
    )
    {
        _friendshipService = friendshipService;
        _userManager = userManager;
    }

    [Authorize]
    [HttpGet("{friendId}")]
    public async Task<IActionResult> GetFriendship(Guid? friendId)
    {
        if (friendId == null)
        {
            return NotFound("friendId does not exist");
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
}
