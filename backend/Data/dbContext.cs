using SocialMediaApp.Models;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Migrations;
using System.Buffers;

namespace SocialMediaApp.Data;

public enum FriendRequestStatus
{
    Pending,
    Accepted,
    Rejected
}

public class DataContext : IdentityDbContext<Members, IdentityRole<Guid>, Guid>
{
    static DataContext()
    {
        NpgsqlConnection.GlobalTypeMapper.MapEnum<FriendRequestStatus>();
    }

    public DataContext(DbContextOptions<DataContext> options)
        : base(options) { }

    public DbSet<Members> Member { get; set; }
    public DbSet<Posts> Post { get; set; }
    public DbSet<Comments> Comment { get; set; }

    public DbSet<CommentUpvotes> CommentUpvote { get; set; }
    public DbSet<FriendRequests> FriendRequest { get; set; }

    public DbSet<Friendships> Friendship { get; set; }

    public DbSet<MemberProfiles> MemberProfile { get; set; }

    public DbSet<PostUpvotes> PostUpvote { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Members>().ToTable("member");

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
        modelBuilder.Entity<Posts>().HasOne<Members>().WithMany().HasForeignKey(p => p.AuthorId);
        modelBuilder
            .Entity<PostUpvotes>()
            .HasOne<Members>()
            .WithMany()
            .HasForeignKey(p => p.AuthorId);
        modelBuilder.Entity<PostUpvotes>().HasOne<Posts>().WithMany().HasForeignKey(p => p.PostId);
        modelBuilder
            .Entity<MemberProfiles>()
            .HasOne<Members>()
            .WithOne()
            .HasForeignKey<MemberProfiles>("MemberId");
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
            .Entity<FriendRequests>()
            .HasOne<Members>()
            .WithMany()
            .HasForeignKey(f => f.RequesterId);
        modelBuilder
            .Entity<FriendRequests>()
            .HasOne<Members>()
            .WithMany()
            .HasForeignKey(f => f.ReceiverId);
        modelBuilder.Entity<Comments>().HasOne<Members>().WithMany().HasForeignKey(c => c.AuthorId);
        modelBuilder.Entity<Comments>().HasOne<Posts>().WithMany().HasForeignKey(c => c.PostId);
        modelBuilder
            .Entity<CommentUpvotes>()
            .HasOne<Members>()
            .WithMany()
            .HasForeignKey(c => c.AuthorId);
        modelBuilder
            .Entity<CommentUpvotes>()
            .HasOne<Comments>()
            .WithMany()
            .HasForeignKey(c => c.CommentId);
        modelBuilder.Entity<Members>().HasIndex(m => m.Id);
        modelBuilder.Entity<Members>().HasIndex(m => m.UserName).IsUnique();
        modelBuilder.Entity<Members>().HasIndex(m => m.Email).IsUnique();
    }
}
