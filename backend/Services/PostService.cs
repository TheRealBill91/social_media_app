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

    public async Task<PostCreationResponse> CreatePost(PostDTO post, Guid userId)
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
            return new PostCreationResponse
            {
                Success = true,
                Message = "Post created successfully",
                PostCreationId = Id
            };
        }
        else
        {
            return new PostCreationResponse
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

    public async Task<List<Post>> GetPosts(int page)
    {
        int PageSize = 10;
        int totalPosts = await _context.Database
            .SqlQuery<int>($"SELECT COUNT(id) AS \"Value\" FROM post")
            .SingleAsync();

        int totalPages = (int)Math.Ceiling(totalPosts / (double)PageSize);

        if (page > totalPages)
        {
            page = totalPages;
        }
        else if (page < 1)
        {
            page = 1;
        }

        int pagesToSkip = PageSize * (page - 1);

        var posts = await _context.Post
            .FromSql(
                $"SELECT * FROM post ORDER BY created_at DESC LIMIT {PageSize} OFFSET {pagesToSkip}"
            )
            .ToListAsync();

        return posts;
    }

    public async Task<PostUpdateResponse> UpdatePostAsync(Guid id, PostDTO postToUpdate)
    {
        var updatedAt = DateTime.UtcNow;
        var result = await _context.Database.ExecuteSqlAsync(
            $"UPDATE post SET title = {postToUpdate.Title}, content = {postToUpdate.Content}, updated_at = {updatedAt} WHERE id = {id} "
        );

        if (result > 0)
        {
            return new PostUpdateResponse { Success = true, Message = "Post successfully updated" };
        }
        else
        {
            return new PostUpdateResponse { Success = false, Message = "Failed to update post" };
        }
    }

    // Apply a soft delete to the target post document
    public async Task<PostDeletionResponse> DeletePostAsync(Guid id)
    {
        var deletedAt = DateTime.UtcNow;
        var result = await _context.Database.ExecuteSqlAsync(
            $"UPDATE post SET deleted_at = {deletedAt}, updated_at = {deletedAt}  WHERE id = {id} "
        );

        if (result > 0)
        {
            return new PostDeletionResponse
            {
                Success = true,
                Message = "Post successfully deleted"
            };
        }
        else
        {
            return new PostDeletionResponse { Success = false, Message = "Post deletion failed" };
        }
    }
}
