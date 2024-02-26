using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using SocialMediaApp.Filters;
using SocialMediaApp.Models;
using SocialMediaApp.Services;

namespace SocialMediaApp.Controllers;

[ApiController]
[ValidateModel]
[Route("api/[controller]")]
public class MembersController : ControllerBase
{
    private readonly MemberService _memberService;

    public MembersController(MemberService memberService)
    {
        _memberService = memberService;
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
        return CreatedAtAction(nameof(GetMember), new { id = member.Id }, member);
    }
}
