using Microsoft.EntityFrameworkCore;
using SocialMediaApp.Data;
using SocialMediaApp.Models;

namespace SocialMediaApp.Services;

public class PostService
{
    private readonly DataContext _context;

    private readonly FriendshipService _friendshipService;

    public PostService(DataContext context, FriendshipService friendshipService)
    {
        _context = context;
        _friendshipService = friendshipService;
    }

    public async Task<PostCreationResponse> CreatePost(
        PostDTO post,
        Guid authorId
    )
    {
        var createdAt = DateTime.UtcNow;
        var updatedAt = DateTime.UtcNow;
        var Id = Guid.NewGuid();

        var result = await _context.Database.ExecuteSqlAsync(
            @$"INSERT INTO post 
                            (id, 
                            title, 
                            content, 
                            created_at, 
                            updated_at, 
                            author_id ) 
               VALUES      ( {Id},
                            {post.Title}, 
                            {post.Content}, 
                            {createdAt}, 
                            {updatedAt}, 
                            {authorId})"
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

    public async Task<PostWithUpvoteCount?> GetPost(Guid id)
    {
        var postWithUpvotes = await _context
            .Database.SqlQuery<PostWithUpvoteCount>(
                @$"SELECT post.title,
                   post.content,
                   post.created_at,
                   post.updated_at,
                   post.author_id,
                   COUNT(post_upvote.post_id) AS post_upvote_count
                FROM post
                LEFT JOIN post_upvote ON post.id = post_upvote.post_id
                WHERE post.id = {id} AND post.deleted_at IS NULL
                GROUP BY post.title,
                   post.content,
                   post.created_at,
                   post.updated_at,
                   post.author_id"
            )
            .SingleOrDefaultAsync();

        return postWithUpvotes;
    }

    public async Task<IReadOnlyList<PostWithUpvoteCount>> GetPosts(int page)
    {
        int PageSize = 10;
        int totalPosts = await _context
            .Database.SqlQuery<int>($"SELECT COUNT(id) AS \"Value\" FROM post")
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

        var postWithUpvotes = await _context
            .Database.SqlQuery<PostWithUpvoteCount>(
                @$"SELECT post.title,
                      post.content,
                      post.created_at,
                      post.updated_at,
                      post.author_id,
                      COUNT(post_upvote.post_id) AS post_upvote_count
                   FROM post
                   LEFT JOIN post_upvote ON post.id = post_upvote.post_id
                   WHERE post.deleted_at IS NULL
                   GROUP BY post.title,
                       post.content,
                       post.created_at,
                       post.updated_at,
                       post.author_id
                   ORDER BY post.created_at DESC
                   LIMIT {PageSize} OFFSET {pagesToSkip}"
            )
            .ToListAsync();

        return postWithUpvotes;
    }

    public async Task<PostUpdateResponse> UpdatePostAsync(
        Guid id,
        PostDTO postToUpdate
    )
    {
        var updatedAt = DateTime.UtcNow;
        var result = await _context.Database.ExecuteSqlAsync(
            @$"UPDATE post 
               SET title = {postToUpdate.Title}, 
                      content = {postToUpdate.Content}, 
                      updated_at = {updatedAt} 
               WHERE id = {id}"
        );

        if (result > 0)
        {
            return new PostUpdateResponse
            {
                Success = true,
                Message = "Post successfully updated"
            };
        }
        else
        {
            return new PostUpdateResponse
            {
                Success = false,
                Message = "Failed to update post"
            };
        }
    }

    // Apply a soft delete to the target post document
    public async Task<PostDeletionResponse> DeletePostAsync(Guid id)
    {
        var deletedAt = DateTime.UtcNow;
        var result = await _context.Database.ExecuteSqlAsync(
            @$"UPDATE post
               SET deleted_at = {deletedAt},
                   updated_at = {deletedAt}
               WHERE id = {id} "
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
            return new PostDeletionResponse
            {
                Success = false,
                Message = "Post deletion failed"
            };
        }
    }

    // Get posts for logged in users home feed
    public async Task<IReadOnlyList<PostWithUpvoteCount>> GetHomeFeedPosts(
        Guid currentUserId
    )
    {
        var friendIds = await _friendshipService.GetFriendsIds(currentUserId);

        // ! Using LINQ instead of vanilla SQL as we cant use the IN
        // ! operator with the List of friendIds
        var homeFeedPosts = await _context
            .Post.Where(p =>
                friendIds.Contains(p.AuthorId) && p.DeletedAt == null
            )
            .GroupJoin(
                _context.PostUpvote,
                post => post.Id,
                upvote => upvote.PostId,
                (post, upvotes) => new { Post = post, Upvotes = upvotes }
            )
            .Select(pu => new PostWithUpvoteCount
            {
                Title = pu.Post.Title,
                Content = pu.Post.Content,
                CreatedAt = pu.Post.CreatedAt,
                UpdatedAt = pu.Post.UpdatedAt,
                AuthorId = pu.Post.AuthorId,
                PostUpvoteCount = pu.Upvotes.Count()
            })
            .OrderByDescending(pu => pu.CreatedAt)
            .Take(10)
            .ToListAsync();

        return homeFeedPosts;
    }

    public async Task<IReadOnlyList<PostWithUpvoteCount>> GetProfilePosts(
        Guid memberId
    )
    {
        var profilePosts = await _context
            .Database.SqlQuery<PostWithUpvoteCount>(
                @$"SELECT post.title,
	                   post.content,
	                   post.created_at,
	                   post.updated_at,
	                   post.author_id,
	                   COUNT(post_upvote.post_id) AS post_upvote_count
                   FROM post
                   LEFT JOIN post_upvote ON post.id = post_upvote.post_id
                   WHERE post.author_id = {memberId}
	                   AND post.deleted_AT IS NULL
                   GROUP BY post.title,
	                   post.content,
	                   post.created_at,
	                   post.updated_at,
	                   post.author_id
                   ORDER BY post.created_at DESC
                   LIMIT 10"
            )
            .ToListAsync();

        return profilePosts;
    }
}
