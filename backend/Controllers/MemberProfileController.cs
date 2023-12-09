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
[Route("/api/user-profile")]
public class MemberProfileController : Controller
{
    private readonly UserManager<Member> _userManager;

    private readonly MemberProfileService _memberProfileService;

    private readonly FriendshipService _friendshipService;

    private readonly PostService _postService;

    private readonly AuthService _authService;

    public MemberProfileController(
        UserManager<Member> userManager,
        MemberProfileService memberProfileService,
        FriendshipService friendshipService,
        PostService postService,
        AuthService authService
    )
    {
        _userManager = userManager;
        _memberProfileService = memberProfileService;
        _friendshipService = friendshipService;
        _postService = postService;
        _authService = authService;
    }

    // retrieves the profile information for the user
    [Authorize]
    [HttpGet("{memberId?}")]
    public async Task<IActionResult> GetProfileInformation(Guid? memberId)
    {
        // if currently logged in user clicks on profile, they will
        // use their logged in user id instead of the memberId
        if (memberId == null)
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

            memberId = Guid.Parse(userId);
        }

        var memberProfile = await _memberProfileService.GetMemberProfile(memberId.Value);

        return Ok(memberProfile);
    }

    // When clicking on your own user profile, exclude memberID
    // If clicking on any other user profile, include memberID
    [Authorize]
    [HttpGet("posts/{memberId?}")]
    public async Task<IActionResult> GetProfilePosts(Guid? memberId)
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

        // memberId passed to endpoint, implies we want to get our own (logged in user)
        // profile posts
        if (memberId != null)
        {
            //check for friendship between memberId and currently logged in user
            var friendship = await _friendshipService.GetFriendship(
                Guid.Parse(userId),
                memberId.Value
            );
            //if friendship does not exist, return forbid
            if (friendship == null)
            {
                return Forbid();
            }

            // get profile posts for logged in users friend
            var otherUserProfilePosts = await _postService.GetProfilePosts(memberId.Value);
            return Ok(otherUserProfilePosts);
        }

        // assign member id to logged in user (we want to get our profile posts)
        memberId = Guid.Parse(userId);

        // retrieve profile posts of the logged in user
        var currentUserProfilePosts = await _postService.GetProfilePosts(memberId.Value);

        return Ok(currentUserProfilePosts);
    }

    [Authorize]
    [HttpPatch("{memberId:guid}")]
    public async Task<IActionResult> UpdateMemberProfile(
        [FromBody] MemberProfileUpdateDTO updatedInfo,
        Guid memberId
    )
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

        // check if logged in user id matches the member profile id
        // return 403 if not
        if (userId != memberId.ToString())
        {
            return Forbid();
        }

        // if logged in user does not change their username (or enter's the same one)
        // then the username does not exist globally
        bool usingSameUsername = updatedInfo.UserName == user.UserName;

        if (!usingSameUsername && await _authService.UsernameExists(updatedInfo.UserName))
        {
            return Conflict("Username already exists, please choose another");
        }

        var profileUpdated = await _memberProfileService.UpdateMemberProfile(memberId, updatedInfo);

        if (profileUpdated)
        {
            return Ok("Profile information successfully updated");
        }
        else
        {
            return BadRequest("Failed to update the profile information");
        }
    }
}
