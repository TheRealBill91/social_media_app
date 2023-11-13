using Microsoft.AspNetCore.Mvc;
using SocialMediaApp.Services;
using SocialMediaApp.Models;
using SocialMediaApp.Filters;

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

    // GET: api/Member/{id}
    [HttpGet("{id}")]
    public async Task<IActionResult> GetMember(Guid id)
    {
        var person = await _memberService.GetMemberAsync(id);
        if (person == null)
        {
            return NotFound();
        }
        return Ok(person);
    }

    [HttpPost]
    public async Task<IActionResult> CreatMember([FromBody] Members Member)
    {
        await _memberService.AddAsync(Member);
        return CreatedAtAction(nameof(GetMember), new { id = Member.Id }, Member);
    }
}
