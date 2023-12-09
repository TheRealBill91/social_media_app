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
[EnableRateLimiting("GeneralFixed")]
[Route("/api/homefeed")]
public class HomeFeedController : Controller
{
    private readonly UserManager<Member> _userManager;

    private readonly FriendshipService _friendshipService;

    private readonly PostService _postService;

    public HomeFeedController(
        UserManager<Member> userManager,
        FriendshipService friendshipService,
        PostService postService
    )
    {
        _userManager = userManager;
        _friendshipService = friendshipService;
        _postService = postService;
    }

    [Authorize]
    [HttpGet]
    public async Task<IActionResult> GetHomeFeedPosts()
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

        var homeFeedPosts = await _postService.GetHomeFeedPosts(Guid.Parse(userId));

        return Ok(homeFeedPosts);
    }
}
