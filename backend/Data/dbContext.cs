using SocialMediaApp.Models;
using Microsoft.EntityFrameworkCore;

namespace SocialMediaApp.Data;

public class DataContext : DbContext
{
    public DataContext(DbContextOptions<DataContext> options)
        : base(options) { }

    public DbSet<Members> Member { get; set; }
    public DbSet<Posts> Post { get; set; }
    public DbSet<Comments> Comment { get; set; }

    public DbSet<Comment_Upvotes> Comment_Upvote { get; set; }
    public DbSet<Friend_Requests> Friend_Request { get; set; }

    public DbSet<Friendships> Friendship { get; set; }

    public DbSet<Member_Profiles> Member_Profile { get; set; }

    public DbSet<Post_Upvotes> Post_Upvote { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Posts>().HasOne<Members>().WithMany().HasForeignKey(p => p.AuthorId);
        modelBuilder
            .Entity<Post_Upvotes>()
            .HasOne<Members>()
            .WithMany()
            .HasForeignKey(p => p.AuthorId);
        modelBuilder.Entity<Post_Upvotes>().HasOne<Posts>().WithMany().HasForeignKey(p => p.PostId);
        modelBuilder
            .Entity<Member_Profiles>()
            .HasOne<Members>()
            .WithOne()
            .HasForeignKey<Member_Profiles>("MemberId");
        modelBuilder
            .Entity<Friendships>()
            .HasOne<Members>()
            .WithMany()
            .HasForeignKey(f => f.MemberId);
        modelBuilder
            .Entity<Friendships>()
            .HasOne<Members>()
            .WithMany()
            .HasForeignKey(f => f.FriendId);
        modelBuilder
            .Entity<Friend_Requests>()
            .HasOne<Members>()
            .WithMany()
            .HasForeignKey(f => f.RequesterId);
        modelBuilder
            .Entity<Friend_Requests>()
            .HasOne<Members>()
            .WithMany()
            .HasForeignKey(f => f.ReceiverId);
        modelBuilder.Entity<Comments>().HasOne<Members>().WithMany().HasForeignKey(c => c.AuthorId);
        modelBuilder.Entity<Comments>().HasOne<Posts>().WithMany().HasForeignKey(c => c.PostId);
        modelBuilder
            .Entity<Comment_Upvotes>()
            .HasOne<Members>()
            .WithMany()
            .HasForeignKey(c => c.AuthorId);
        modelBuilder
            .Entity<Comment_Upvotes>()
            .HasOne<Comments>()
            .WithMany()
            .HasForeignKey(c => c.CommentId);
    }
}
