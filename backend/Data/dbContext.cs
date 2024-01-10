using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using SocialMediaApp.Models;

namespace SocialMediaApp.Data;

public enum FriendRequestStatus
{
    Pending,
    Accepted,
    Rejected
}

public class DataContext : IdentityDbContext<Member, IdentityRole<Guid>, Guid>
{
    public DataContext(DbContextOptions<DataContext> options)
        : base(options) { }

    public DbSet<Member> Member { get; set; }
    public DbSet<Post> Post { get; set; }
    public DbSet<Comment> Comment { get; set; }

    public DbSet<CommentUpvote> CommentUpvote { get; set; }
    public DbSet<FriendRequest> FriendRequest { get; set; }

    public DbSet<Friendship> Friendship { get; set; }

    public DbSet<MemberProfile> MemberProfile { get; set; }

    public DbSet<PostUpvote> PostUpvote { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Member>().ToTable("member");

        // Rename ASP.NET Identity table names to snake case
        modelBuilder.Entity<IdentityRoleClaim<Guid>>(b =>
        {
            b.ToTable("asp_net_role_claims");
        });
        modelBuilder.Entity<IdentityRole<Guid>>(b =>
        {
            b.ToTable("asp_net_roles");
        });
        modelBuilder.Entity<IdentityUserClaim<Guid>>(b =>
        {
            b.ToTable("asp_net_user_claims");
        });
        modelBuilder.Entity<IdentityUserLogin<Guid>>(b =>
        {
            b.ToTable("asp_net_user_logins");
        });
        modelBuilder.Entity<IdentityUserRole<Guid>>(b =>
        {
            b.ToTable("asp_net_user_roles");
        });
        modelBuilder.Entity<IdentityUserToken<Guid>>(b =>
        {
            b.ToTable("asp_net_user_tokens");
        });

        modelBuilder.HasPostgresEnum<FriendRequestStatus>();
        modelBuilder.Entity<Post>().HasOne<Member>().WithMany().HasForeignKey(p => p.AuthorId);
        modelBuilder
            .Entity<PostUpvote>()
            .HasOne<Member>()
            .WithMany()
            .HasForeignKey(p => p.AuthorId);
        modelBuilder.Entity<PostUpvote>().HasOne<Post>().WithMany().HasForeignKey(p => p.PostId);
        modelBuilder
            .Entity<MemberProfile>()
            .HasOne<Member>()
            .WithOne()
            .HasForeignKey<MemberProfile>("MemberId");
        modelBuilder
            .Entity<Friendship>()
            .HasOne<Member>()
            .WithMany()
            .HasForeignKey(f => f.MemberId);
        modelBuilder
            .Entity<Friendship>()
            .HasOne<Member>()
            .WithMany()
            .HasForeignKey(f => f.FriendId);
        modelBuilder
            .Entity<FriendRequest>()
            .HasOne<Member>()
            .WithMany()
            .HasForeignKey(f => f.RequesterId);
        modelBuilder
            .Entity<FriendRequest>()
            .HasOne<Member>()
            .WithMany()
            .HasForeignKey(f => f.ReceiverId);
        modelBuilder.Entity<Comment>().HasOne<Member>().WithMany().HasForeignKey(c => c.AuthorId);
        modelBuilder.Entity<Comment>().HasOne<Post>().WithMany().HasForeignKey(c => c.PostId);
        modelBuilder
            .Entity<CommentUpvote>()
            .HasOne<Member>()
            .WithMany()
            .HasForeignKey(c => c.AuthorId);
        modelBuilder
            .Entity<CommentUpvote>()
            .HasOne<Comment>()
            .WithMany()
            .HasForeignKey(c => c.CommentId);
        modelBuilder.Entity<Member>().HasIndex(m => m.Id);
        modelBuilder.Entity<Member>().HasIndex(m => m.UserName).IsUnique();
        modelBuilder.Entity<Member>().HasIndex(m => m.Email).IsUnique();
    }
}
