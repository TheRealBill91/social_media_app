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
    public async Task<CommentCreationResponse> CreateComment(
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
            return new CommentCreationResponse
            {
                Success = true,
                Message = "Comment created successfully",
                CommentCreationId = Id
            };
        }
        else
        {
            return new CommentCreationResponse
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

    public async Task<List<Comment>> GetComments(int page, Guid postId)
    {
        int PageSize = 10;
        int totalComments = await _context.Database
            .SqlQuery<int>($"SELECT COUNT(id) AS \"Value\" FROM comment WHERE post_id = {postId}")
            .SingleAsync();

        int totalPages = (int)Math.Ceiling(totalComments / (double)PageSize);

        if (page > totalPages)
        {
            page = totalPages;
        }
        else if (page < 1)
        {
            page = 1;
        }

        int pagesToSkip = PageSize * (page - 1);

        var comments = await _context.Comment
            .FromSql(
                $"SELECT * FROM comment WHERE post_id = {postId} ORDER BY created_at DESC LIMIT {PageSize} OFFSET {pagesToSkip} "
            )
            .ToListAsync();

        return comments;
    }

    public async Task<CommentUpdateResponse> UpdateCommentAsync(Guid id, CommentDTO commentToUpdate)
    {
        var updatedAt = DateTime.UtcNow;
        var result = await _context.Database.ExecuteSqlAsync(
            $"UPDATE comment SET content = {commentToUpdate.Content}, updated_at = {updatedAt} WHERE id = {id} "
        );

        if (result > 0)
        {
            return new CommentUpdateResponse
            {
                Success = true,
                Message = "Comment Updated Successfully"
            };
        }
        else
        {
            return new CommentUpdateResponse
            {
                Success = false,
                Message = "Failed to update comment"
            };
        }
    }

    // Apply a soft delete to the target comment document
    public async Task<CommentDeletionResponse> DeleteCommentAsync(Guid id)
    {
        var deletedAt = DateTime.UtcNow;
        var result = await _context.Database.ExecuteSqlAsync(
            $"UPDATE comment SET deleted_at = {deletedAt}, updated_at = {deletedAt} WHERE id = {id}"
        );

        if (result > 0)
        {
            return new CommentDeletionResponse
            {
                Success = true,
                Message = "Comment deleted successfully"
            };
        }
        else
        {
            return new CommentDeletionResponse
            {
                Success = false,
                Message = "Comment deletion failed"
            };
        }
    }
}
