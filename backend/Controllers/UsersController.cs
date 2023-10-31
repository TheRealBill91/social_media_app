using Microsoft.AspNetCore.Mvc;
using SocialMediaApp.Services;
using SocialMediaApp.Models;
using SocialMediaApp.Filters;
using Microsoft.EntityFrameworkCore;

[ApiController]
[ValidateUsers]
[Route("api/[controller]")]
public class UsersController : Controller
{
    private readonly UserService _userService;

    public UsersController(UserService userService)
    {
        _userService = userService;
    }

    // GET: api/User/{id}
    [HttpGet("{id}")]
    public async Task<IActionResult> Get(int id)
    {
        var person = await _userService.GetByIdAsync(id);
        if (person == null)
        {
            return NotFound();
        }
        return Ok(person);
    }

    [HttpPost]
    public async Task<IActionResult> User([FromBody] Users User)
    {
        try
        {
            await _userService.AddAsync(User);
            return CreatedAtAction(nameof(Get), new { id = User.Id }, User);
        }
        catch (DbUpdateException)
        {
            return StatusCode(500, "An error occurred while creating the post. Please try again.");
        }
    }
}
