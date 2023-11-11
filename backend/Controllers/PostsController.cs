using Microsoft.AspNetCore.Mvc;
using SocialMediaApp.Services;
using SocialMediaApp.Models;
using SocialMediaApp.Filters;
using Microsoft.EntityFrameworkCore;

[ApiController]
[ValidateModel]
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
    public async Task<IActionResult> GetPost(Guid? id)
    {
        if (id == null)
        {
            return NotFound();
        }
        var person = await _postService.GetByIdAsync((Guid)id);
        if (person == null)
        {
            return NotFound();
        }
        return Ok(person);
    }

    [HttpPost("Create")]
    public async Task<IActionResult> CreatePost([FromBody] Posts Post)
    {
        if (Post == null)
        {
            return NotFound();
        }

        await _postService.AddAsync(Post);
        return CreatedAtAction(nameof(GetPost), new { id = Post.Id }, Post);
    }

    [HttpPost("Delete/{id}")]
    public async Task<IActionResult> DeletePost(Guid? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var result = await _postService.DeletePostAsync((Guid)id);
        if (result > 0)
        {
            return NoContent();
        }
        else
        {
            // If no rows were affected, the post with the given ID was not found.
            return NotFound();
        }
    }

    [HttpPost("Update/{id}")]
    public async Task<IActionResult> UpdatePost(Guid? id, [FromBody] Posts postToUpdate)
    {
        if (id == null)
        {
            return NotFound();
        }

        await _postService.UpdatePostAsync((Guid)id, postToUpdate);
        return Ok(postToUpdate);
    }
}
