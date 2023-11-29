using SocialMediaApp.Models;
using Microsoft.EntityFrameworkCore;
using SocialMediaApp.Data;

namespace SocialMediaApp.Services;

public class CommentService
{
    private readonly DataContext _context;

    public CommentService(DataContext context)
    {
        _context = context;
    }

    // Create a single comment
    public async Task<CommentCreationResult> CreateComment(
        CommentDTO comment,
        Guid userId,
        Guid postId
    )
    {
        var createdAt = DateTime.UtcNow;
        var updatedAt = createdAt;
        var Id = Guid.NewGuid();
        Guid authorId = userId;

        var result = await _context.Database.ExecuteSqlAsync(
            $"INSERT INTO comment (id, content, created_at, updated_at, author_id, post_id) VALUES ( {Id},  {comment.Content}, {createdAt}, {updatedAt}, {authorId}, {postId} )"
        );

        if (result > 0)
        {
            return new CommentCreationResult
            {
                Success = true,
                Message = "Comment created successfully",
                CommentCreationId = Id
            };
        }
        else
        {
            return new CommentCreationResult
            {
                Success = false,
                Message = "Failed to create comment",
                CommentCreationId = null
            };
        }
    }

    public async Task<Comment?> GetComment(Guid id)
    {
        var comment = await _context.Comment
            .FromSql($"SELECT * FROM comment WHERE id = {id} AND deleted_at IS NULL")
            .FirstOrDefaultAsync();

        return comment;
    }

    public async Task<CommentUpdateResult> UpdateCommentAsync(Guid id, CommentDTO commentToUpdate)
    {
        var updatedAt = DateTime.UtcNow;
        var result = await _context.Database.ExecuteSqlAsync(
            $"UPDATE comment SET content = {commentToUpdate.Content}, updated_at = {updatedAt} WHERE id = {id} "
        );

        if (result > 0)
        {
            return new CommentUpdateResult
            {
                Success = true,
                Message = "Comment Updated Successfully"
            };
        }
        else
        {
            return new CommentUpdateResult
            {
                Success = false,
                Message = "Failed to update comment"
            };
        }
    }

    // Apply a soft delete to the target comment document
    public async Task<CommentDeletionResult> DeleteCommentAsync(Guid id)
    {
        var deletedAt = DateTime.UtcNow;
        var result = await _context.Database.ExecuteSqlAsync(
            $"UPDATE comment SET deleted_at = {deletedAt}, updated_at = {deletedAt} WHERE id = {id}"
        );

        if (result > 0)
        {
            return new CommentDeletionResult
            {
                Success = true,
                Message = "Comment deleted successfully"
            };
        }
        else
        {
            return new CommentDeletionResult
            {
                Success = false,
                Message = "Comment deletion failed"
            };
        }
    }
}
