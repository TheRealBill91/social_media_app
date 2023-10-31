using SocialMediaApp.Models;
using Microsoft.EntityFrameworkCore;
using SocialMediaApp.Data;

namespace SocialMediaApp.Services;

public class PostService
{
    private readonly DataContext _context;

    public PostService(DataContext context)
    {
        _context = context;
    }

    // Get a single post
    public async Task<Posts> GetByIdAsync(int id) => await _context.Post.FindAsync(id);

    // Create a single post
    public async Task AddAsync(Posts Post)
    {
        Post.Created = DateOnly.FromDateTime(DateTime.Now);
        Post.Modified = DateOnly.FromDateTime(DateTime.Now);

        await _context.Database.ExecuteSqlAsync(
            $"INSERT INTO Posts (Title, Content, Created, Modified) VALUES ({Post.Title}, {Post.Content}, {Post.Created}, {Post.Modified})"
        );
    }
}
