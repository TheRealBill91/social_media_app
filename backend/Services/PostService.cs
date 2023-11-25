using SocialMediaApp.Models;
using Microsoft.EntityFrameworkCore;
using SocialMediaApp.Data;
using Microsoft.AspNetCore.Authorization;

namespace SocialMediaApp.Services;

public class PostService
{
    private readonly DataContext _context;

    public PostService(DataContext context)
    {
        _context = context;
    }

    public async Task<Guid> CreatePost(PostDTO post, Guid userId)
    {
        var CreatedAt = DateTime.UtcNow;
        var UpdatedAt = DateTime.UtcNow;
        var Id = Guid.NewGuid();
        Guid authorId = userId;

        await _context.Database.ExecuteSqlAsync(
            $"INSERT INTO post (id, title, content, created_at, updated_at, author_id ) VALUES ( {Id},{post.Title}, {post.Content}, {CreatedAt}, {UpdatedAt}, {authorId})"
        );

        return Id;
    }

    public async Task<Post?> GetPost(Guid id)
    {
        var post = await _context.Post
            .FromSql($"SELECT * FROM post WHERE id = {id} AND deleted_at IS NULL")
            .FirstOrDefaultAsync();

        return post;
    }

    public async Task UpdatePostAsync(Guid id, PostDTO postToUpdate)
    {
        var UpdatedAt = DateTime.UtcNow;
        await _context.Database.ExecuteSqlAsync(
            $"UPDATE post SET title = {postToUpdate.Title}, content = {postToUpdate.Content}, updated_at = {UpdatedAt} WHERE id = {id} "
        );
    }

    // Apply a soft delete to the target post document
    public async Task<int> DeletePostAsync(Guid id)
    {
        var deletedAt = DateTime.UtcNow;
        var result = await _context.Database.ExecuteSqlAsync(
            $"UPDATE post SET deleted_at = {deletedAt}, updated_at = {deletedAt}  WHERE id = {id} "
        );

        return result;
    }
}
