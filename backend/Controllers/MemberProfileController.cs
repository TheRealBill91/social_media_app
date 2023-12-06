using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SocialMediaApp.Filters;
using SocialMediaApp.Models;
using SocialMediaApp.Services;

[ApiController]
[ValidateModel]
[Route("/api/user-profile")]
public class MemberProfileController : Controller
{
    private readonly UserManager<Member> _userManager;

    private readonly MemberProfileService _memberProfileService;

    public MemberProfileController(
        UserManager<Member> userManager,
        MemberProfileService memberProfileService
    )
    {
        _userManager = userManager;
        _memberProfileService = memberProfileService;
    }

    [Authorize]
    [HttpGet("{memberId?}")]
    public async Task<IActionResult> GetMemberProfile(Guid? memberId)
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
}
