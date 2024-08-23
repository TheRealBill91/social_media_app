using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using SocialMediaApp.Models;
using SocialMediaApp.Services;

namespace SocialMediaApp.Controllers;

public class MembersController : BaseApiController
{
    private readonly MemberService _memberService;

    private readonly UserManager<Member> _userManager;

    public MembersController(
        MemberService memberService,
        UserManager<Member> userManager
    )
    {
        _memberService = memberService;
        _userManager = userManager;
    }

    [EnableRateLimiting("getResourceSlidingWindow")]
    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetMember(Guid id)
    {
        var person = await _memberService.GetMemberAsync(id);
        if (person == null)
        {
            return NotFound();
        }
        return Ok(person);
    }

    [EnableRateLimiting("createResourceSlidingWindow")]
    [HttpPost]
    public async Task<IActionResult> CreateMember([FromBody] Member member)
    {
        await _memberService.AddAsync(member);
        return CreatedAtAction(
            nameof(GetMember),
            new { id = member.Id },
            member
        );
    }

    [EnableRateLimiting("updateResourceSlidingWindow")]
    [Authorize]
    [HttpPatch("{memberId:guid}")]
    public async Task<IActionResult> UpdateUsername(
        [FromBody] string newUsername,
        string memberId
    )
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (userId == null)
        // should not reach here
        {
            return NotFound();
        }

        var user = await _userManager.FindByIdAsync(userId);

        if (user == null)
        {
            // Should not reach here
            return NotFound();
        }

        if (userId != memberId)
        {
            return Forbid();
        }

        var lastUserNameUpdateDate = user.LastUsernameUpdateDate;

        // First can they update the username (one allowed per 30 day period)
        var usernameUpdateAllowed = _memberService.CanUpdateUsername(
            lastUserNameUpdateDate
        );

        if (!usernameUpdateAllowed)
        {
            return StatusCode(
                StatusCodes.Status403Forbidden,
                new { Message = "" }
            );
        }

        // checking to see if the user is enters the same username they already
        // currently have
        var usingSameUsername = newUsername == user.UserName;

        // Second, we need to check if the username is already taken
        var usernameTaken = await _memberService.UsernameExists(newUsername);

        if (!usingSameUsername && usernameTaken)
        {
            return Conflict(
                "Username already exists, please choose another one"
            );
        }

        var updateUsernameResponse = await _memberService.UpdateUsername(
            newUsername,
            Guid.Parse(memberId)
        );

        if (updateUsernameResponse.Success)
        {
            return Ok(updateUsernameResponse.Message);
        }
        else
        {
            return BadRequest(updateUsernameResponse.Message);
        }
    }
}
