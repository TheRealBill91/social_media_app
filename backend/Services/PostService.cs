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
    public async Task<Posts?> GetByIdAsync(Guid id)
    {
        var post = await _context.Post
            .FromSql($"SELECT * FROM post WHERE id = {id}")
            .FirstOrDefaultAsync();

        return post;
    }

    // Create a single post
    [Authorize]
    public async Task AddAsync(Posts Post)
    {
        Post.CreatedAt = DateTime.UtcNow;
        Post.UpdatedAt = DateTime.UtcNow;
        Post.Id = Guid.NewGuid();

        await _context.Database.ExecuteSqlAsync(
            $"INSERT INTO post (id, title, content, created_at, updated_at, author_id ) VALUES ( {Post.Id},{Post.Title}, {Post.Content}, {Post.CreatedAt}, {Post.UpdatedAt}, {Post.AuthorId})"
        );
    }

    // Delete a single post
    public async Task<int> DeletePostAsync(Guid id)
    {
        var result = await _context.Database.ExecuteSqlAsync($"DELETE FROM post WHERE id = {id}");

        return result;
    }

    // Update a single post
    public async Task UpdatePostAsync(Guid id, Posts postToUpdate)
    {
        await _context.Database.ExecuteSqlAsync(
            $"UPDATE post SET title = {postToUpdate.Title}, content = {postToUpdate.Content} WHERE id = {id} "
        );
    }
}
