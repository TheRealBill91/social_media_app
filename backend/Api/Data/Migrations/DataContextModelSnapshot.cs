﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using SocialMediaApp.Data;

#nullable disable

namespace social_media_api.Migrations
{
    [DbContext(typeof(DataContext))]
    partial class DataContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.0")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.HasPostgresEnum(modelBuilder, "friend_request_status", new[] { "pending", "accepted", "rejected" });
            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRole<System.Guid>", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("id");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("text")
                        .HasColumnName("concurrency_stamp");

                    b.Property<string>("Name")
                        .HasMaxLength(256)
                        .HasColumnType("character varying(256)")
                        .HasColumnName("name");

                    b.Property<string>("NormalizedName")
                        .HasMaxLength(256)
                        .HasColumnType("character varying(256)")
                        .HasColumnName("normalized_name");

                    b.HasKey("Id")
                        .HasName("pk_asp_net_roles");

                    b.HasIndex("NormalizedName")
                        .IsUnique()
                        .HasDatabaseName("RoleNameIndex");

                    b.ToTable("asp_net_roles", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<System.Guid>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("ClaimType")
                        .HasColumnType("text")
                        .HasColumnName("claim_type");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("text")
                        .HasColumnName("claim_value");

                    b.Property<Guid>("RoleId")
                        .HasColumnType("uuid")
                        .HasColumnName("role_id");

                    b.HasKey("Id")
                        .HasName("pk_asp_net_role_claims");

                    b.HasIndex("RoleId")
                        .HasDatabaseName("ix_asp_net_role_claims_role_id");

                    b.ToTable("asp_net_role_claims", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<System.Guid>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("ClaimType")
                        .HasColumnType("text")
                        .HasColumnName("claim_type");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("text")
                        .HasColumnName("claim_value");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uuid")
                        .HasColumnName("user_id");

                    b.HasKey("Id")
                        .HasName("pk_asp_net_user_claims");

                    b.HasIndex("UserId")
                        .HasDatabaseName("ix_asp_net_user_claims_user_id");

                    b.ToTable("asp_net_user_claims", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<System.Guid>", b =>
                {
                    b.Property<string>("LoginProvider")
                        .HasColumnType("text")
                        .HasColumnName("login_provider");

                    b.Property<string>("ProviderKey")
                        .HasColumnType("text")
                        .HasColumnName("provider_key");

                    b.Property<string>("ProviderDisplayName")
                        .HasColumnType("text")
                        .HasColumnName("provider_display_name");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uuid")
                        .HasColumnName("user_id");

                    b.HasKey("LoginProvider", "ProviderKey")
                        .HasName("pk_asp_net_user_logins");

                    b.HasIndex("UserId")
                        .HasDatabaseName("ix_asp_net_user_logins_user_id");

                    b.ToTable("asp_net_user_logins", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<System.Guid>", b =>
                {
                    b.Property<Guid>("UserId")
                        .HasColumnType("uuid")
                        .HasColumnName("user_id");

                    b.Property<Guid>("RoleId")
                        .HasColumnType("uuid")
                        .HasColumnName("role_id");

                    b.HasKey("UserId", "RoleId")
                        .HasName("pk_asp_net_user_roles");

                    b.HasIndex("RoleId")
                        .HasDatabaseName("ix_asp_net_user_roles_role_id");

                    b.ToTable("asp_net_user_roles", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<System.Guid>", b =>
                {
                    b.Property<Guid>("UserId")
                        .HasColumnType("uuid")
                        .HasColumnName("user_id");

                    b.Property<string>("LoginProvider")
                        .HasColumnType("text")
                        .HasColumnName("login_provider");

                    b.Property<string>("Name")
                        .HasColumnType("text")
                        .HasColumnName("name");

                    b.Property<string>("Value")
                        .HasColumnType("text")
                        .HasColumnName("value");

                    b.HasKey("UserId", "LoginProvider", "Name")
                        .HasName("pk_asp_net_user_tokens");

                    b.ToTable("asp_net_user_tokens", (string)null);
                });

            modelBuilder.Entity("SocialMediaApp.Models.Comment", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("id");

                    b.Property<Guid>("AuthorId")
                        .HasColumnType("uuid")
                        .HasColumnName("author_id");

                    b.Property<string>("Content")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("content");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("created_at");

                    b.Property<DateTime?>("DeletedAt")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("deleted_at");

                    b.Property<Guid>("PostId")
                        .HasColumnType("uuid")
                        .HasColumnName("post_id");

                    b.Property<DateTime>("UpdatedAt")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("updated_at");

                    b.HasKey("Id")
                        .HasName("pk_comment");

                    b.HasIndex("AuthorId")
                        .HasDatabaseName("ix_comment_author_id");

                    b.HasIndex("PostId")
                        .HasDatabaseName("ix_comment_post_id");

                    b.ToTable("comment", (string)null);
                });

            modelBuilder.Entity("SocialMediaApp.Models.CommentUpvote", b =>
                {
                    b.Property<Guid>("AuthorId")
                        .HasColumnType("uuid")
                        .HasColumnName("author_id");

                    b.Property<Guid>("CommentId")
                        .HasColumnType("uuid")
                        .HasColumnName("comment_id");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("created_at");

                    b.Property<DateTime>("UpdatedAt")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("updated_at");

                    b.HasKey("AuthorId", "CommentId")
                        .HasName("pk_comment_upvote");

                    b.HasIndex("CommentId")
                        .HasDatabaseName("ix_comment_upvote_comment_id");

                    b.ToTable("comment_upvote", (string)null);
                });

            modelBuilder.Entity("SocialMediaApp.Models.FriendRequest", b =>
                {
                    b.Property<Guid>("RequesterId")
                        .HasColumnType("uuid")
                        .HasColumnName("requester_id");

                    b.Property<Guid>("ReceiverId")
                        .HasColumnType("uuid")
                        .HasColumnName("receiver_id");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("created_at");

                    b.Property<FriendRequestStatus>("Status")
                        .HasColumnType("friend_request_status")
                        .HasColumnName("status");

                    b.Property<DateTime>("UpdatedAt")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("updated_at");

                    b.HasKey("RequesterId", "ReceiverId")
                        .HasName("pk_friend_request");

                    b.HasIndex("ReceiverId")
                        .HasDatabaseName("ix_friend_request_receiver_id");

                    b.ToTable("friend_request", (string)null);
                });

            modelBuilder.Entity("SocialMediaApp.Models.Friendship", b =>
                {
                    b.Property<Guid>("MemberId")
                        .HasColumnType("uuid")
                        .HasColumnName("member_id");

                    b.Property<Guid>("FriendId")
                        .HasColumnType("uuid")
                        .HasColumnName("friend_id");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("created_at");

                    b.Property<DateTime>("UpdatedAt")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("updated_at");

                    b.HasKey("MemberId", "FriendId")
                        .HasName("pk_friendship");

                    b.HasIndex("FriendId")
                        .HasDatabaseName("ix_friendship_friend_id");

                    b.ToTable("friendship", (string)null);
                });

            modelBuilder.Entity("SocialMediaApp.Models.Member", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("id");

                    b.Property<int>("AccessFailedCount")
                        .HasColumnType("integer")
                        .HasColumnName("access_failed_count");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("text")
                        .HasColumnName("concurrency_stamp");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("created_at");

                    b.Property<DateTime?>("DeletedAt")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("deleted_at");

                    b.Property<string>("Email")
                        .HasMaxLength(256)
                        .HasColumnType("character varying(256)")
                        .HasColumnName("email");

                    b.Property<int>("EmailConfirmationSentCount")
                        .HasColumnType("integer")
                        .HasColumnName("email_confirmation_sent_count");

                    b.Property<bool>("EmailConfirmed")
                        .HasColumnType("boolean")
                        .HasColumnName("email_confirmed");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("character varying(50)")
                        .HasColumnName("first_name");

                    b.Property<DateTime>("LastEmailConfirmationSentDate")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("last_email_confirmation_sent_date");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("character varying(50)")
                        .HasColumnName("last_name");

                    b.Property<DateTime>("LastPasswordResetEmailSentDate")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("last_password_reset_email_sent_date");

                    b.Property<DateTime>("LastUsernameRequestEmailSentDate")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("last_username_request_email_sent_date");

                    b.Property<DateTime>("LastUsernameUpdateDate")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("last_username_update_date");

                    b.Property<bool>("LockoutEnabled")
                        .HasColumnType("boolean")
                        .HasColumnName("lockout_enabled");

                    b.Property<DateTimeOffset?>("LockoutEnd")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("lockout_end");

                    b.Property<string>("NormalizedEmail")
                        .HasMaxLength(256)
                        .HasColumnType("character varying(256)")
                        .HasColumnName("normalized_email");

                    b.Property<string>("NormalizedUserName")
                        .HasMaxLength(256)
                        .HasColumnType("character varying(256)")
                        .HasColumnName("normalized_user_name");

                    b.Property<string>("PasswordHash")
                        .HasColumnType("text")
                        .HasColumnName("password_hash");

                    b.Property<int>("PasswordResetEmailSentCount")
                        .HasColumnType("integer")
                        .HasColumnName("password_reset_email_sent_count");

                    b.Property<string>("PhoneNumber")
                        .HasColumnType("text")
                        .HasColumnName("phone_number");

                    b.Property<bool>("PhoneNumberConfirmed")
                        .HasColumnType("boolean")
                        .HasColumnName("phone_number_confirmed");

                    b.Property<string>("SecurityStamp")
                        .HasColumnType("text")
                        .HasColumnName("security_stamp");

                    b.Property<bool>("TwoFactorEnabled")
                        .HasColumnType("boolean")
                        .HasColumnName("two_factor_enabled");

                    b.Property<DateTime>("UpdatedAt")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("updated_at");

                    b.Property<string>("UserName")
                        .HasMaxLength(256)
                        .HasColumnType("character varying(256)")
                        .HasColumnName("user_name");

                    b.Property<int>("UsernameRequestEmailSentCount")
                        .HasColumnType("integer")
                        .HasColumnName("username_request_email_sent_count");

                    b.HasKey("Id")
                        .HasName("pk_member");

                    b.HasIndex("Email")
                        .IsUnique()
                        .HasDatabaseName("ix_member_email");

                    b.HasIndex("Id")
                        .HasDatabaseName("ix_member_id");

                    b.HasIndex("NormalizedEmail")
                        .HasDatabaseName("EmailIndex");

                    b.HasIndex("NormalizedUserName")
                        .IsUnique()
                        .HasDatabaseName("UserNameIndex");

                    b.HasIndex("UserName")
                        .IsUnique()
                        .HasDatabaseName("ix_member_user_name");

                    b.ToTable("member", (string)null);
                });

            modelBuilder.Entity("SocialMediaApp.Models.MemberProfile", b =>
                {
                    b.Property<Guid>("MemberId")
                        .HasColumnType("uuid")
                        .HasColumnName("member_id");

                    b.Property<string>("Bio")
                        .HasColumnType("text")
                        .HasColumnName("bio");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("created_at");

                    b.Property<DateTime?>("DeletedAt")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("deleted_at");

                    b.Property<string>("Location")
                        .HasColumnType("text")
                        .HasColumnName("location");

                    b.Property<string>("PhotoURL")
                        .HasColumnType("text")
                        .HasColumnName("photo_url");

                    b.Property<string>("URL")
                        .HasColumnType("text")
                        .HasColumnName("url");

                    b.Property<DateTime>("UpdatedAt")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("updated_at");

                    b.HasKey("MemberId")
                        .HasName("pk_member_profile");

                    b.ToTable("member_profile", (string)null);
                });

            modelBuilder.Entity("SocialMediaApp.Models.PasswordHistory", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("id");

                    b.Property<string>("PasswordHash")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("password_hash");

                    b.HasKey("Id")
                        .HasName("pk_password_history");

                    b.ToTable("password_history", (string)null);
                });

            modelBuilder.Entity("SocialMediaApp.Models.Post", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("id");

                    b.Property<Guid>("AuthorId")
                        .HasColumnType("uuid")
                        .HasColumnName("author_id");

                    b.Property<string>("Content")
                        .IsRequired()
                        .HasMaxLength(5000)
                        .HasColumnType("character varying(5000)")
                        .HasColumnName("content");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("created_at");

                    b.Property<DateTime?>("DeletedAt")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("deleted_at");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("character varying(100)")
                        .HasColumnName("title");

                    b.Property<DateTime>("UpdatedAt")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("updated_at");

                    b.HasKey("Id")
                        .HasName("pk_post");

                    b.HasIndex("AuthorId")
                        .HasDatabaseName("ix_post_author_id");

                    b.ToTable("post", (string)null);
                });

            modelBuilder.Entity("SocialMediaApp.Models.PostUpvote", b =>
                {
                    b.Property<Guid>("AuthorId")
                        .HasColumnType("uuid")
                        .HasColumnName("author_id");

                    b.Property<Guid>("PostId")
                        .HasColumnType("uuid")
                        .HasColumnName("post_id");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("created_at");

                    b.Property<DateTime>("UpdatedAt")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("updated_at");

                    b.HasKey("AuthorId", "PostId")
                        .HasName("pk_post_upvote");

                    b.HasIndex("PostId")
                        .HasDatabaseName("ix_post_upvote_post_id");

                    b.ToTable("post_upvote", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<System.Guid>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole<System.Guid>", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_asp_net_role_claims_asp_net_roles_role_id");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<System.Guid>", b =>
                {
                    b.HasOne("SocialMediaApp.Models.Member", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_asp_net_user_claims_member_user_id");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<System.Guid>", b =>
                {
                    b.HasOne("SocialMediaApp.Models.Member", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_asp_net_user_logins_member_user_id");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<System.Guid>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole<System.Guid>", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_asp_net_user_roles_asp_net_roles_role_id");

                    b.HasOne("SocialMediaApp.Models.Member", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_asp_net_user_roles_member_user_id");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<System.Guid>", b =>
                {
                    b.HasOne("SocialMediaApp.Models.Member", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_asp_net_user_tokens_member_user_id");
                });

            modelBuilder.Entity("SocialMediaApp.Models.Comment", b =>
                {
                    b.HasOne("SocialMediaApp.Models.Member", null)
                        .WithMany()
                        .HasForeignKey("AuthorId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_comment_member_member_id");

                    b.HasOne("SocialMediaApp.Models.Post", null)
                        .WithMany()
                        .HasForeignKey("PostId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_comment_post_post_id");
                });

            modelBuilder.Entity("SocialMediaApp.Models.CommentUpvote", b =>
                {
                    b.HasOne("SocialMediaApp.Models.Member", null)
                        .WithMany()
                        .HasForeignKey("AuthorId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_comment_upvote_member_member_id");

                    b.HasOne("SocialMediaApp.Models.Comment", null)
                        .WithMany()
                        .HasForeignKey("CommentId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_comment_upvote_comment_comment_id");
                });

            modelBuilder.Entity("SocialMediaApp.Models.FriendRequest", b =>
                {
                    b.HasOne("SocialMediaApp.Models.Member", null)
                        .WithMany()
                        .HasForeignKey("ReceiverId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_friend_request_member_member_id");

                    b.HasOne("SocialMediaApp.Models.Member", null)
                        .WithMany()
                        .HasForeignKey("RequesterId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_friend_request_member_member_id1");
                });

            modelBuilder.Entity("SocialMediaApp.Models.Friendship", b =>
                {
                    b.HasOne("SocialMediaApp.Models.Member", null)
                        .WithMany()
                        .HasForeignKey("FriendId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_friendship_member_member_id1");

                    b.HasOne("SocialMediaApp.Models.Member", null)
                        .WithMany()
                        .HasForeignKey("MemberId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_friendship_member_member_id");
                });

            modelBuilder.Entity("SocialMediaApp.Models.MemberProfile", b =>
                {
                    b.HasOne("SocialMediaApp.Models.Member", null)
                        .WithOne()
                        .HasForeignKey("SocialMediaApp.Models.MemberProfile", "MemberId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_member_profile_member_member_id1");
                });

            modelBuilder.Entity("SocialMediaApp.Models.Post", b =>
                {
                    b.HasOne("SocialMediaApp.Models.Member", null)
                        .WithMany()
                        .HasForeignKey("AuthorId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_post_member_member_id");
                });

            modelBuilder.Entity("SocialMediaApp.Models.PostUpvote", b =>
                {
                    b.HasOne("SocialMediaApp.Models.Member", null)
                        .WithMany()
                        .HasForeignKey("AuthorId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_post_upvote_member_member_id");

                    b.HasOne("SocialMediaApp.Models.Post", null)
                        .WithMany()
                        .HasForeignKey("PostId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_post_upvote_post_post_id");
                });
#pragma warning restore 612, 618
        }
    }
}
