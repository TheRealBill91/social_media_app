using SocialMediaApp.Models;
using Microsoft.EntityFrameworkCore;
using SocialMediaApp.Data;
using Microsoft.AspNetCore.Authorization;

namespace SocialMediaApp.Services;

public class CommentService
{
    private readonly DataContext _context;

    public CommentService(DataContext context)
    {
        _context = context;
    }

    // Create a single comment
    public async Task<Guid> CreateComment(CommentDTO comment, Guid userId, Guid postId)
    {
        var CreatedAt = DateTime.UtcNow;
        var UpdatedAt = DateTime.UtcNow;
        var Id = Guid.NewGuid();
        Guid authorId = userId;

        await _context.Database.ExecuteSqlAsync(
            $"INSERT INTO comment (id, content, created_at, updated_at, author_id, post_id) VALUES ( {Id},  {comment.Content}, {CreatedAt}, {UpdatedAt}, {authorId}, {postId} )"
        );

        return Id;
    }

    public async Task<Comment?> GetComment(Guid id)
    {
        var comment = await _context.Comment
            .FromSql($"SELECT * FROM comment WHERE id = {id} AND deleted_at IS NULL")
            .FirstOrDefaultAsync();

        return comment;
    }

    public async Task UpdateCommentAsync(Guid id, CommentDTO commentToUpdate)
    {
        var UpdatedAt = DateTime.UtcNow;
        await _context.Database.ExecuteSqlAsync(
            $"UPDATE comment SET content = {commentToUpdate.Content}, updated_at = {UpdatedAt} WHERE id = {id} "
        );
    }

    // Apply a soft delete to the target comment document
    public async Task<int> DeleteCommentAsync(Guid id)
    {
        var deletedAt = DateTime.UtcNow;
        var result = await _context.Database.ExecuteSqlAsync(
            $"UPDATE comment SET deleted_at = {deletedAt}, updated_at = {deletedAt} WHERE id = {id}"
        );

        return result;
    }
}
