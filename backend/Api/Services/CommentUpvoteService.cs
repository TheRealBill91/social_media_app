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

    public async Task<CommentUpvote?> GetCommentUpvote(
        Guid? commentId,
        Guid? authorId
    )
    {
        var commentUpvote = await _context
            .CommentUpvote.FromSql(
                @$"SELECT *
                   FROM  comment_upvote
                   WHERE comment_id = {commentId}
                         AND author_id = {authorId} "
            )
            .FirstOrDefaultAsync();

        return commentUpvote;
    }

    public async Task<UpvoteDeletionResponse> DeleteCommentUpvote(
        Guid? commentId,
        Guid? authorId
    )
    {
        var result = await _context.Database.ExecuteSqlAsync(
            @$"DELETE FROM comment_upvote
               WHERE  (comment_id = {commentId}
                      AND author_id = {authorId})"
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

    public async Task<UpvoteCreationResponse> CreateCommentUpvote(
        Guid? commentId,
        Guid authorId
    )
    {
        var createdAt = DateTime.UtcNow;
        var updatedAt = createdAt;

        var result = await _context.Database.ExecuteSqlAsync(
            @$"INSERT INTO comment_upvote
                           (created_at,
                           updated_at,
                           author_id,
                           comment_id)
               VALUES      ({createdAt},
                           {updatedAt},
                           {authorId},
                           {commentId})"
        );

        if (result > 0)
        {
            return new UpvoteCreationResponse
            {
                Success = true,
                Message = "Comment upvote successfully created"
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
