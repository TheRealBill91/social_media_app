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

    public async Task<PostCreationResult> CreatePost(PostDTO post, Guid userId)
    {
        var createdAt = DateTime.UtcNow;
        var updatedAt = DateTime.UtcNow;
        var Id = Guid.NewGuid();
        Guid authorId = userId;

        var result = await _context.Database.ExecuteSqlAsync(
            $"INSERT INTO post (id, title, content, created_at, updated_at, author_id ) VALUES ( {Id},{post.Title}, {post.Content}, {createdAt}, {updatedAt}, {authorId})"
        );

        if (result > 0)
        {
            return new PostCreationResult
            {
                Success = true,
                Message = "Post created successfully",
                PostCreationId = Id
            };
        }
        else
        {
            return new PostCreationResult
            {
                Success = false,
                Message = "Post creation unsuccessful",
                PostCreationId = null
            };
        }
    }

    public async Task<Post?> GetPost(Guid id)
    {
        var post = await _context.Post
            .FromSql($"SELECT * FROM post WHERE id = {id} AND deleted_at IS NULL")
            .FirstOrDefaultAsync();

        return post;
    }

    public async Task<PostUpdateResult> UpdatePostAsync(Guid id, PostDTO postToUpdate)
    {
        var updatedAt = DateTime.UtcNow;
        var result = await _context.Database.ExecuteSqlAsync(
            $"UPDATE post SET title = {postToUpdate.Title}, content = {postToUpdate.Content}, updated_at = {updatedAt} WHERE id = {id} "
        );

        if (result > 0)
        {
            return new PostUpdateResult { Success = true, Message = "Post successfully updated" };
        }
        else
        {
            return new PostUpdateResult { Success = false, Message = "Failed to update post" };
        }
    }

    // Apply a soft delete to the target post document
    public async Task<PostDeletionResult> DeletePostAsync(Guid id)
    {
        var deletedAt = DateTime.UtcNow;
        var result = await _context.Database.ExecuteSqlAsync(
            $"UPDATE post SET deleted_at = {deletedAt}, updated_at = {deletedAt}  WHERE id = {id} "
        );

        if (result > 0)
        {
            return new PostDeletionResult { Success = true, Message = "Post successfully deleted" };
        }
        else
        {
            return new PostDeletionResult { Success = false, Message = "Post deletion failed" };
        }
    }
}
