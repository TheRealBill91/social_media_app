using Microsoft.AspNetCore.Mvc;
using SocialMediaApp.Services;
using SocialMediaApp.Models;
using SocialMediaApp.Filters;
using Microsoft.EntityFrameworkCore;

[ApiController]
[ValidatePosts]
[Route("api/[controller]")]
public class PostsController : Controller
{
    private readonly PostService _postService;

    public PostsController(PostService postService)
    {
        _postService = postService;
    }

    // GET: api/Post/{id}
    [HttpGet("{id}")]
    public async Task<IActionResult> Get(int id)
    {
        var person = await _postService.GetByIdAsync(id);
        if (person == null)
        {
            return NotFound();
        }
        return Ok(person);
    }

    [HttpPost]
    public async Task<IActionResult> Post([FromBody] Posts Post)
    {
        try
        {
            await _postService.AddAsync(Post);
            return CreatedAtAction(nameof(Get), new { id = Post.Id }, Post);
        }
        catch (DbUpdateException)
        {
            return StatusCode(500, "An error occurred while creating the post. Please try again.");
        }
    }
}
