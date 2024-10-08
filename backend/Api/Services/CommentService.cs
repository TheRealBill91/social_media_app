using Microsoft.EntityFrameworkCore;
using SocialMediaApp.Data;
using SocialMediaApp.Models;

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
        Guid authorId,
        Guid postId
    )
    {
        var createdAt = DateTime.UtcNow;
        var updatedAt = createdAt;
        var Id = Guid.NewGuid();

        var result = await _context
            .Database
            .ExecuteSqlAsync(
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

    public async Task<CommentWithUpvoteCount?> GetComment(Guid id)
    {
        var commentWithUpvotes = await _context
            .Comment
            // First, filter the comments
            .Where(c => c.Id == id && c.DeletedAt == null)
            // Then, perform the left join with CommentUpvotes
            .GroupJoin(
                _context.CommentUpvote,
                comment => comment.Id,
                upvote => upvote.CommentId,
                (comment, upvotes) => new { Comment = comment, Upvotes = upvotes }
            )
            // Now, select the data into a new shape
            .Select(
                cu =>
                    new CommentWithUpvoteCount
                    {
                        Content = cu.Comment.Content,
                        CreatedAt = cu.Comment.CreatedAt,
                        UpdatedAt = cu.Comment.UpdatedAt,
                        AuthorId = cu.Comment.AuthorId,
                        PostId = cu.Comment.PostId,
                        CommentUpvoteCount = cu.Upvotes.Count() // This is how you'd count the upvotes
                    }
            )
            // Finally, get the first or default result asynchronously
            .FirstOrDefaultAsync();

        return commentWithUpvotes;
    }

    public async Task<List<CommentWithUpvoteCount>?> GetComments(int page, Guid postId)
    {
        int PageSize = 10;
        int totalComments = await _context
            .Database
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

        var commentsWithUpvotes = await _context
            .Comment
            .Where(c => c.PostId == postId)
            .GroupJoin(
                _context.CommentUpvote,
                comment => comment.Id,
                upvote => upvote.CommentId,
                (comment, upvotes) => new { Comment = comment, Upvotes = upvotes }
            )
            .Select(
                cu =>
                    new CommentWithUpvoteCount
                    {
                        Content = cu.Comment.Content,
                        CreatedAt = cu.Comment.CreatedAt,
                        UpdatedAt = cu.Comment.UpdatedAt,
                        AuthorId = cu.Comment.AuthorId,
                        PostId = cu.Comment.PostId,
                        CommentUpvoteCount = cu.Upvotes.Count()
                    }
            )
            .OrderByDescending(cu => cu.CreatedAt)
            .Skip(pagesToSkip)
            .Take(PageSize)
            .ToListAsync();

        return commentsWithUpvotes;
    }

    public async Task<CommentUpdateResponse> UpdateCommentAsync(Guid id, CommentDTO commentToUpdate)
    {
        var updatedAt = DateTime.UtcNow;
        var result = await _context
            .Database
            .ExecuteSqlAsync(
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
        var result = await _context
            .Database
            .ExecuteSqlAsync(
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
