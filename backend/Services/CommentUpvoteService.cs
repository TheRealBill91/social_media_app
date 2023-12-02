using Microsoft.EntityFrameworkCore;
using SocialMediaApp.Data;
using SocialMediaApp.Models;

namespace SocialMediaApp.Services;

public class CommentUpvoteService
{
    private readonly DataContext _context;

    public CommentUpvoteService(DataContext context)
    {
        _context = context;
    }

    public async Task<CommentUpvote?> GetCommentUpvote(Guid? commentId, Guid? userId)
    {
        var commentUpvote = await _context.CommentUpvote
            .FromSql(
                $"SELECT * FROM comment_upvote WHERE comment_id = {commentId} AND author_id = {userId}  "
            )
            .FirstOrDefaultAsync();

        return commentUpvote;
    }

    public async Task<UpvoteDeletionResponse> DeleteCommentUpvote(Guid? commentId, Guid? userId)
    {
        var result = await _context.Database.ExecuteSqlAsync(
            $"DELETE from comment_upvote WHERE (id = {commentId} AND author_id = {userId})"
        );

        if (result > 0)
        {
            return new UpvoteDeletionResponse
            {
                Success = true,
                Message = "Comment Upvote successfully deleted"
            };
        }
        else
        {
            return new UpvoteDeletionResponse
            {
                Success = false,
                Message = "Failed to delete the comment upvote"
            };
        }
    }

    public async Task<UpvoteCreationResponse> CreateCommentUpvote(Guid? commentId, Guid userId)
    {
        var createdAt = DateTime.UtcNow;
        var updatedAt = DateTime.UtcNow;
        var Id = Guid.NewGuid();
        Guid authorId = userId;

        var result = await _context.Database.ExecuteSqlAsync(
            $"INSERT INTO comment_upvote (id, created_at, updated_at, author_id, comment_id) VALUES ( {Id},{createdAt},{updatedAt}, {userId},{commentId} )"
        );

        if (result > 0)
        {
            return new UpvoteCreationResponse
            {
                Success = true,
                Message = "Successfully created comment upvote"
            };
        }
        else
        {
            return new UpvoteCreationResponse
            {
                Success = false,
                Message = "Failed to create the comment upvote"
            };
        }
    }
}
