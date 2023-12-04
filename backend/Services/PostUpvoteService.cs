using Microsoft.EntityFrameworkCore;
using SocialMediaApp.Data;
using SocialMediaApp.Models;

namespace SocialMediaApp.Services;

public class PostUpvoteService
{
    private readonly DataContext _context;

    public PostUpvoteService(DataContext context)
    {
        _context = context;
    }

    public async Task<PostUpvote?> GetPostUpvote(Guid? postId, Guid? authorId)
    {
        var postUpvote = await _context.PostUpvote
            .FromSql(
                $"SELECT * FROM post_upvote WHERE post_id = {postId} AND author_id = {authorId}"
            )
            .FirstOrDefaultAsync();

        return postUpvote;
    }

    public async Task<UpvoteDeletionResponse> DeletePostUpvote(Guid? postId, Guid? authorId)
    {
        var result = await _context.Database.ExecuteSqlAsync(
            $"DELETE from post_upvote WHERE (post_id = {postId} AND author_id = {authorId})"
        );

        if (result > 0)
        {
            return new UpvoteDeletionResponse
            {
                Success = true,
                Message = "Post upvote successfully deleted"
            };
        }
        else
        {
            return new UpvoteDeletionResponse
            {
                Success = false,
                Message = "Failed to delete the post upvote"
            };
        }
    }

    public async Task<UpvoteCreationResponse> CreatePostUpvote(Guid? postId, Guid? authorId)
    {
        var createdAt = DateTime.UtcNow;
        var updatedAt = createdAt;

        var result = await _context.Database.ExecuteSqlAsync(
            $"INSERT INTO post_upvote (created_at, updated_at, author_id, post_id) VALUES ( {createdAt},{updatedAt},{authorId},{postId})"
        );

        if (result > 0)
        {
            return new UpvoteCreationResponse
            {
                Success = true,
                Message = "Post upvote created successfully"
            };
        }
        else
        {
            return new UpvoteCreationResponse
            {
                Success = false,
                Message = "Failed to create the post upvote"
            };
        }
    }
}
