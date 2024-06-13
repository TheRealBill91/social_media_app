using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using SocialMediaApp.Models;
using SocialMediaApp.Services;

namespace SocialMediaApp.Controllers;

[Route("/api/posts/{postId:guid}/comments/{commentId:guid}")]
public class CommentUpvoteController : BaseApiController
{
    private readonly UserManager<Member> _userManager;

    private readonly CommentUpvoteService _commentUpvoteService;

    private readonly PostService _postService;

    private readonly FriendshipService _friendshipService;

    public CommentUpvoteController(
        UserManager<Member> userManager,
        CommentUpvoteService commentUpvoteService,
        PostService postService,
        FriendshipService friendshipService
    )
    {
        _userManager = userManager;
        _commentUpvoteService = commentUpvoteService;
        _postService = postService;
        _friendshipService = friendshipService;
    }

    [EnableRateLimiting("resourceUpvoteTokenBucket")]
    [Authorize]
    [HttpPost]
    public async Task<IActionResult> ToggleCommentUpvote(Guid? postId, Guid? commentId)
    {
        if (postId == null || commentId == null)
        {
            return BadRequest();
        }

        var post = await _postService.GetPost(postId.Value);

        if (post == null)
        {
            return BadRequest("Post does not exist");
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

        // check for existing comment upvote for the current user
        var commentUpvote = await _commentUpvoteService.GetCommentUpvote(
            commentId,
            Guid.Parse(userId)
        );

        if (commentUpvote != null)
        {
            // delete comment upvote and return 200
            var upvoteDeletionResponse = await _commentUpvoteService.DeleteCommentUpvote(
                commentId,
                Guid.Parse(userId)
            );

            if (upvoteDeletionResponse.Success)
            {
                return Ok(upvoteDeletionResponse.Message);
            }
            else
            {
                return BadRequest(upvoteDeletionResponse.Message);
            }
        }

        // create the comment upvote since it is not null
        var upvoteCreationReponse = await _commentUpvoteService.CreateCommentUpvote(
            commentId,
            Guid.Parse(userId)
        );

        if (upvoteCreationReponse.Success)
        {
            return Ok(upvoteCreationReponse.Message);
        }
        else
        {
            return BadRequest(upvoteCreationReponse.Message);
        }
    }
}
