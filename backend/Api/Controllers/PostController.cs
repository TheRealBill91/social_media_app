using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using SocialMediaApp.Models;
using SocialMediaApp.Services;

namespace SocialMediaApp.Controllers;

public class PostController : BaseApiController
{
    private readonly PostService _postService;

    private readonly UserManager<Member> _userManager;

    private readonly FriendshipService _friendshipService;

    public PostController(
        PostService postService,
        UserManager<Member> userManager,
        FriendshipService friendshipService
    )
    {
        _postService = postService;
        _userManager = userManager;
        _friendshipService = friendshipService;
    }

    [EnableRateLimiting("createResourceSlidingWindow")]
    [Authorize]
    [HttpPost]
    public async Task<IActionResult> CreatePost([FromBody] PostDTO post)
    {
        if (post == null)
        {
            return NotFound();
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

        var result = await _postService.CreatePost(post, user.Id);
        if (result.Success)
        {
            return CreatedAtAction(nameof(GetPost), new { id = result.PostCreationId }, result);
        }
        else
        {
            return BadRequest(result.Message);
        }
    }

    [EnableRateLimiting("getResourceSlidingWindow")]
    [Authorize]
    [HttpGet]
    public async Task<IActionResult> GetAllPosts(int page)
    {
        //TODO: check if user has permission to get all the posts
        var posts = await _postService.GetPosts(page);

        return Ok(posts);
    }

    [EnableRateLimiting("getResourceSlidingWindow")]
    [Authorize]
    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetPost(Guid? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var postId = id.Value;

        var post = await _postService.GetPost(postId);
        if (post == null)
        {
            return NotFound("Post does not exist");
        }

        var postAuthorId = post.AuthorId;

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

        var friendship = await _friendshipService.GetFriendship(Guid.Parse(userId), postAuthorId);

        // return 403 if users aren't friends or the logged in user is not the author
        // of the post
        if (friendship == null && post.AuthorId.ToString() != userId)
        {
            return Forbid();
        }

        return Ok(post);
    }

    [EnableRateLimiting("updateResourceSlidingWindow")]
    [Authorize]
    [HttpPatch("{id:guid}")]
    public async Task<IActionResult> UpdatePost(Guid? id, [FromBody] PostDTO postToUpdate)
    {
        if (id == null)
        {
            return NotFound();
        }

        var postId = id.Value;

        var post = await _postService.GetPost(postId);

        if (post == null)
        {
            return NotFound("Post does not exists");
        }

        string? userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var postAuthorId = post.AuthorId;

        if (userId != postAuthorId.ToString())
        {
            return Forbid();
        }

        var result = await _postService.UpdatePostAsync(postId, postToUpdate);

        if (result.Success)
        {
            return Ok(result.Message);
        }
        else
        {
            return BadRequest(result.Message);
        }
    }

    [EnableRateLimiting("deleteResourceSlidingWindow")]
    [Authorize]
    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> DeletePost(Guid? id)
    {
        if (id == null)
        {
            return NotFound("Post id does not exist");
        }

        var postId = id.Value;

        var post = await _postService.GetPost(postId);

        if (post == null)
        {
            return NotFound();
        }

        string? userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var postAuthorId = post.AuthorId;

        if (userId != postAuthorId.ToString())
        {
            return Forbid();
        }

        var result = await _postService.DeletePostAsync(postId);
        if (result.Success)
        {
            return Ok(result.Message);
        }
        else
        {
            return BadRequest(result.Message);
        }
    }
}
