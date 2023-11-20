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

    // Get a single post
    public async Task<Post?> GetByIdAsync(Guid id)
    {
        var post = await _context.Post
            .FromSql($"SELECT * FROM post WHERE id = {id}")
            .FirstOrDefaultAsync();

        return post;
    }

    // Create a single post
    [Authorize]
    public async Task AddAsync(Post post)
    {
        post.CreatedAt = DateTime.UtcNow;
        post.UpdatedAt = DateTime.UtcNow;
        post.Id = Guid.NewGuid();

        await _context.Database.ExecuteSqlAsync(
            $"INSERT INTO post (id, title, content, created_at, updated_at, author_id ) VALUES ( {post.Id},{post.Title}, {post.Content}, {post.CreatedAt}, {post.UpdatedAt}, {post.AuthorId})"
        );
    }

    // Delete a single post
    public async Task<int> DeletePostAsync(Guid id)
    {
        var result = await _context.Database.ExecuteSqlAsync($"DELETE FROM post WHERE id = {id}");

        return result;
    }

    // Update a single post
    public async Task UpdatePostAsync(Guid id, Post postToUpdate)
    {
        await _context.Database.ExecuteSqlAsync(
            $"UPDATE post SET title = {postToUpdate.Title}, content = {postToUpdate.Content} WHERE id = {id} "
        );
    }
}
