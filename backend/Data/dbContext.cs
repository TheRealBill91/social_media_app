using SocialMediaApp.Models;
using Microsoft.EntityFrameworkCore;

namespace SocialMediaApp.Data;

public class DataContext : DbContext
{
    public DataContext(DbContextOptions<DataContext> options)
        : base(options) { }

    public DbSet<Users> User { get; set; }
    public DbSet<Posts> Post { get; set; }
    public DbSet<Comments> Comment { get; set; }

    public DbSet<Comment_Upvotes> Comment_Upvote { get; set; }
    public DbSet<Friend_Requests> Friend_Request { get; set; }

    public DbSet<Friendships> Friendship { get; set; }

    public DbSet<User_Profiles> User_Profile { get; set; }

    public DbSet<Post_Upvotes> Post_Upvote { get; set; }
}
