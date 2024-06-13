using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace social_media_api.Migrations
{
    /// <inheritdoc />
    public partial class UpdateTablesToSnakeCase : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetRoleClaims_AspNetRoles_RoleId",
                table: "AspNetRoleClaims");

            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUserClaims_Members_UserId",
                table: "AspNetUserClaims");

            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUserLogins_Members_UserId",
                table: "AspNetUserLogins");

            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUserRoles_AspNetRoles_RoleId",
                table: "AspNetUserRoles");

            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUserRoles_Members_UserId",
                table: "AspNetUserRoles");

            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUserTokens_Members_UserId",
                table: "AspNetUserTokens");

            migrationBuilder.DropForeignKey(
                name: "FK_Comment_Members_AuthorId",
                table: "Comment");

            migrationBuilder.DropForeignKey(
                name: "FK_Comment_Post_PostId",
                table: "Comment");

            migrationBuilder.DropForeignKey(
                name: "FK_CommentUpvote_Comment_CommentId",
                table: "CommentUpvote");

            migrationBuilder.DropForeignKey(
                name: "FK_CommentUpvote_Members_AuthorId",
                table: "CommentUpvote");

            migrationBuilder.DropForeignKey(
                name: "FK_FriendRequest_Members_ReceiverId",
                table: "FriendRequest");

            migrationBuilder.DropForeignKey(
                name: "FK_FriendRequest_Members_RequesterId",
                table: "FriendRequest");

            migrationBuilder.DropForeignKey(
                name: "FK_Friendship_Members_FriendId",
                table: "Friendship");

            migrationBuilder.DropForeignKey(
                name: "FK_Friendship_Members_MemberId",
                table: "Friendship");

            migrationBuilder.DropForeignKey(
                name: "FK_MemberProfile_Members_MemberId",
                table: "MemberProfile");

            migrationBuilder.DropForeignKey(
                name: "FK_Post_Members_AuthorId",
                table: "Post");

            migrationBuilder.DropForeignKey(
                name: "FK_PostUpvote_Members_AuthorId",
                table: "PostUpvote");

            migrationBuilder.DropForeignKey(
                name: "FK_PostUpvote_Post_PostId",
                table: "PostUpvote");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Post",
                table: "Post");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Members",
                table: "Members");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Friendship",
                table: "Friendship");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Comment",
                table: "Comment");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PostUpvote",
                table: "PostUpvote");

            migrationBuilder.DropPrimaryKey(
                name: "PK_MemberProfile",
                table: "MemberProfile");

            migrationBuilder.DropPrimaryKey(
                name: "PK_FriendRequest",
                table: "FriendRequest");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CommentUpvote",
                table: "CommentUpvote");

            migrationBuilder.DropPrimaryKey(
                name: "PK_AspNetUserTokens",
                table: "AspNetUserTokens");

            migrationBuilder.DropPrimaryKey(
                name: "PK_AspNetUserRoles",
                table: "AspNetUserRoles");

            migrationBuilder.DropPrimaryKey(
                name: "PK_AspNetUserLogins",
                table: "AspNetUserLogins");

            migrationBuilder.DropPrimaryKey(
                name: "PK_AspNetUserClaims",
                table: "AspNetUserClaims");

            migrationBuilder.DropPrimaryKey(
                name: "PK_AspNetRoles",
                table: "AspNetRoles");

            migrationBuilder.DropPrimaryKey(
                name: "PK_AspNetRoleClaims",
                table: "AspNetRoleClaims");

            migrationBuilder.RenameTable(
                name: "Post",
                newName: "post");

            migrationBuilder.RenameTable(
                name: "Members",
                newName: "members");

            migrationBuilder.RenameTable(
                name: "Friendship",
                newName: "friendship");

            migrationBuilder.RenameTable(
                name: "Comment",
                newName: "comment");

            migrationBuilder.RenameTable(
                name: "PostUpvote",
                newName: "post_upvote");

            migrationBuilder.RenameTable(
                name: "MemberProfile",
                newName: "member_profile");

            migrationBuilder.RenameTable(
                name: "FriendRequest",
                newName: "friend_request");

            migrationBuilder.RenameTable(
                name: "CommentUpvote",
                newName: "comment_upvote");

            migrationBuilder.RenameTable(
                name: "AspNetUserTokens",
                newName: "asp_net_user_tokens");

            migrationBuilder.RenameTable(
                name: "AspNetUserRoles",
                newName: "asp_net_user_roles");

            migrationBuilder.RenameTable(
                name: "AspNetUserLogins",
                newName: "asp_net_user_logins");

            migrationBuilder.RenameTable(
                name: "AspNetUserClaims",
                newName: "asp_net_user_claims");

            migrationBuilder.RenameTable(
                name: "AspNetRoles",
                newName: "asp_net_roles");

            migrationBuilder.RenameTable(
                name: "AspNetRoleClaims",
                newName: "asp_net_role_claims");

            migrationBuilder.RenameColumn(
                name: "Title",
                table: "post",
                newName: "title");

            migrationBuilder.RenameColumn(
                name: "Content",
                table: "post",
                newName: "content");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "post",
                newName: "id");

            migrationBuilder.RenameColumn(
                name: "UpdatedAt",
                table: "post",
                newName: "updated_at");

            migrationBuilder.RenameColumn(
                name: "DeletedAt",
                table: "post",
                newName: "deleted_at");

            migrationBuilder.RenameColumn(
                name: "CreatedAt",
                table: "post",
                newName: "created_at");

            migrationBuilder.RenameColumn(
                name: "AuthorId",
                table: "post",
                newName: "author_id");

            migrationBuilder.RenameIndex(
                name: "IX_Post_AuthorId",
                table: "post",
                newName: "ix_post_author_id");

            migrationBuilder.RenameColumn(
                name: "Email",
                table: "members",
                newName: "email");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "members",
                newName: "id");

            migrationBuilder.RenameColumn(
                name: "UserName",
                table: "members",
                newName: "user_name");

            migrationBuilder.RenameColumn(
                name: "UpdatedAt",
                table: "members",
                newName: "updated_at");

            migrationBuilder.RenameColumn(
                name: "TwoFactorEnabled",
                table: "members",
                newName: "two_factor_enabled");

            migrationBuilder.RenameColumn(
                name: "SecurityStamp",
                table: "members",
                newName: "security_stamp");

            migrationBuilder.RenameColumn(
                name: "PhoneNumberConfirmed",
                table: "members",
                newName: "phone_number_confirmed");

            migrationBuilder.RenameColumn(
                name: "PhoneNumber",
                table: "members",
                newName: "phone_number");

            migrationBuilder.RenameColumn(
                name: "PasswordHash",
                table: "members",
                newName: "password_hash");

            migrationBuilder.RenameColumn(
                name: "NormalizedUserName",
                table: "members",
                newName: "normalized_user_name");

            migrationBuilder.RenameColumn(
                name: "NormalizedEmail",
                table: "members",
                newName: "normalized_email");

            migrationBuilder.RenameColumn(
                name: "LockoutEnd",
                table: "members",
                newName: "lockout_end");

            migrationBuilder.RenameColumn(
                name: "LockoutEnabled",
                table: "members",
                newName: "lockout_enabled");

            migrationBuilder.RenameColumn(
                name: "LastName",
                table: "members",
                newName: "last_name");

            migrationBuilder.RenameColumn(
                name: "FirstName",
                table: "members",
                newName: "first_name");

            migrationBuilder.RenameColumn(
                name: "EmailConfirmed",
                table: "members",
                newName: "email_confirmed");

            migrationBuilder.RenameColumn(
                name: "DeletedAt",
                table: "members",
                newName: "deleted_at");

            migrationBuilder.RenameColumn(
                name: "CreatedAt",
                table: "members",
                newName: "created_at");

            migrationBuilder.RenameColumn(
                name: "ConcurrencyStamp",
                table: "members",
                newName: "concurrency_stamp");

            migrationBuilder.RenameColumn(
                name: "AccessFailedCount",
                table: "members",
                newName: "access_failed_count");

            migrationBuilder.RenameIndex(
                name: "IX_Members_Id",
                table: "members",
                newName: "ix_members_id");

            migrationBuilder.RenameIndex(
                name: "IX_Members_Email",
                table: "members",
                newName: "ix_members_email");

            migrationBuilder.RenameIndex(
                name: "IX_Members_UserName",
                table: "members",
                newName: "ix_members_user_name");

            migrationBuilder.RenameColumn(
                name: "FriendId",
                table: "friendship",
                newName: "friend_id");

            migrationBuilder.RenameColumn(
                name: "MemberId",
                table: "friendship",
                newName: "member_id");

            migrationBuilder.RenameIndex(
                name: "IX_Friendship_FriendId",
                table: "friendship",
                newName: "ix_friendship_friend_id");

            migrationBuilder.RenameColumn(
                name: "Content",
                table: "comment",
                newName: "content");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "comment",
                newName: "id");

            migrationBuilder.RenameColumn(
                name: "UpdatedAt",
                table: "comment",
                newName: "updated_at");

            migrationBuilder.RenameColumn(
                name: "PostId",
                table: "comment",
                newName: "post_id");

            migrationBuilder.RenameColumn(
                name: "DeletedAt",
                table: "comment",
                newName: "deleted_at");

            migrationBuilder.RenameColumn(
                name: "CreatedAt",
                table: "comment",
                newName: "created_at");

            migrationBuilder.RenameColumn(
                name: "AuthorId",
                table: "comment",
                newName: "author_id");

            migrationBuilder.RenameIndex(
                name: "IX_Comment_PostId",
                table: "comment",
                newName: "ix_comment_post_id");

            migrationBuilder.RenameIndex(
                name: "IX_Comment_AuthorId",
                table: "comment",
                newName: "ix_comment_author_id");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "post_upvote",
                newName: "id");

            migrationBuilder.RenameColumn(
                name: "UpdatedAt",
                table: "post_upvote",
                newName: "updated_at");

            migrationBuilder.RenameColumn(
                name: "PostId",
                table: "post_upvote",
                newName: "post_id");

            migrationBuilder.RenameColumn(
                name: "CreatedAt",
                table: "post_upvote",
                newName: "created_at");

            migrationBuilder.RenameColumn(
                name: "AuthorId",
                table: "post_upvote",
                newName: "author_id");

            migrationBuilder.RenameIndex(
                name: "IX_PostUpvote_PostId",
                table: "post_upvote",
                newName: "ix_post_upvote_post_id");

            migrationBuilder.RenameIndex(
                name: "IX_PostUpvote_AuthorId",
                table: "post_upvote",
                newName: "ix_post_upvote_author_id");

            migrationBuilder.RenameColumn(
                name: "URL",
                table: "member_profile",
                newName: "url");

            migrationBuilder.RenameColumn(
                name: "Location",
                table: "member_profile",
                newName: "location");

            migrationBuilder.RenameColumn(
                name: "Bio",
                table: "member_profile",
                newName: "bio");

            migrationBuilder.RenameColumn(
                name: "UpdatedAt",
                table: "member_profile",
                newName: "updated_at");

            migrationBuilder.RenameColumn(
                name: "PhotoURL",
                table: "member_profile",
                newName: "photo_url");

            migrationBuilder.RenameColumn(
                name: "DeletedAt",
                table: "member_profile",
                newName: "deleted_at");

            migrationBuilder.RenameColumn(
                name: "CreatedAt",
                table: "member_profile",
                newName: "created_at");

            migrationBuilder.RenameColumn(
                name: "MemberId",
                table: "member_profile",
                newName: "member_id");

            migrationBuilder.RenameColumn(
                name: "Status",
                table: "friend_request",
                newName: "status");

            migrationBuilder.RenameColumn(
                name: "ReceiverId",
                table: "friend_request",
                newName: "receiver_id");

            migrationBuilder.RenameColumn(
                name: "RequesterId",
                table: "friend_request",
                newName: "requester_id");

            migrationBuilder.RenameIndex(
                name: "IX_FriendRequest_ReceiverId",
                table: "friend_request",
                newName: "ix_friend_request_receiver_id");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "comment_upvote",
                newName: "id");

            migrationBuilder.RenameColumn(
                name: "UpdatedAt",
                table: "comment_upvote",
                newName: "updated_at");

            migrationBuilder.RenameColumn(
                name: "CreatedAt",
                table: "comment_upvote",
                newName: "created_at");

            migrationBuilder.RenameColumn(
                name: "CommentId",
                table: "comment_upvote",
                newName: "comment_id");

            migrationBuilder.RenameColumn(
                name: "AuthorId",
                table: "comment_upvote",
                newName: "author_id");

            migrationBuilder.RenameIndex(
                name: "IX_CommentUpvote_CommentId",
                table: "comment_upvote",
                newName: "ix_comment_upvote_comment_id");

            migrationBuilder.RenameIndex(
                name: "IX_CommentUpvote_AuthorId",
                table: "comment_upvote",
                newName: "ix_comment_upvote_author_id");

            migrationBuilder.RenameColumn(
                name: "Value",
                table: "asp_net_user_tokens",
                newName: "value");

            migrationBuilder.RenameColumn(
                name: "Name",
                table: "asp_net_user_tokens",
                newName: "name");

            migrationBuilder.RenameColumn(
                name: "LoginProvider",
                table: "asp_net_user_tokens",
                newName: "login_provider");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "asp_net_user_tokens",
                newName: "user_id");

            migrationBuilder.RenameColumn(
                name: "RoleId",
                table: "asp_net_user_roles",
                newName: "role_id");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "asp_net_user_roles",
                newName: "user_id");

            migrationBuilder.RenameIndex(
                name: "IX_AspNetUserRoles_RoleId",
                table: "asp_net_user_roles",
                newName: "ix_asp_net_user_roles_role_id");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "asp_net_user_logins",
                newName: "user_id");

            migrationBuilder.RenameColumn(
                name: "ProviderDisplayName",
                table: "asp_net_user_logins",
                newName: "provider_display_name");

            migrationBuilder.RenameColumn(
                name: "ProviderKey",
                table: "asp_net_user_logins",
                newName: "provider_key");

            migrationBuilder.RenameColumn(
                name: "LoginProvider",
                table: "asp_net_user_logins",
                newName: "login_provider");

            migrationBuilder.RenameIndex(
                name: "IX_AspNetUserLogins_UserId",
                table: "asp_net_user_logins",
                newName: "ix_asp_net_user_logins_user_id");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "asp_net_user_claims",
                newName: "id");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "asp_net_user_claims",
                newName: "user_id");

            migrationBuilder.RenameColumn(
                name: "ClaimValue",
                table: "asp_net_user_claims",
                newName: "claim_value");

            migrationBuilder.RenameColumn(
                name: "ClaimType",
                table: "asp_net_user_claims",
                newName: "claim_type");

            migrationBuilder.RenameIndex(
                name: "IX_AspNetUserClaims_UserId",
                table: "asp_net_user_claims",
                newName: "ix_asp_net_user_claims_user_id");

            migrationBuilder.RenameColumn(
                name: "Name",
                table: "asp_net_roles",
                newName: "name");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "asp_net_roles",
                newName: "id");

            migrationBuilder.RenameColumn(
                name: "NormalizedName",
                table: "asp_net_roles",
                newName: "normalized_name");

            migrationBuilder.RenameColumn(
                name: "ConcurrencyStamp",
                table: "asp_net_roles",
                newName: "concurrency_stamp");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "asp_net_role_claims",
                newName: "id");

            migrationBuilder.RenameColumn(
                name: "RoleId",
                table: "asp_net_role_claims",
                newName: "role_id");

            migrationBuilder.RenameColumn(
                name: "ClaimValue",
                table: "asp_net_role_claims",
                newName: "claim_value");

            migrationBuilder.RenameColumn(
                name: "ClaimType",
                table: "asp_net_role_claims",
                newName: "claim_type");

            migrationBuilder.RenameIndex(
                name: "IX_AspNetRoleClaims_RoleId",
                table: "asp_net_role_claims",
                newName: "ix_asp_net_role_claims_role_id");

            migrationBuilder.AddPrimaryKey(
                name: "pk_post",
                table: "post",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "pk_members",
                table: "members",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "pk_friendship",
                table: "friendship",
                columns: new[] { "member_id", "friend_id" });

            migrationBuilder.AddPrimaryKey(
                name: "pk_comment",
                table: "comment",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "pk_post_upvote",
                table: "post_upvote",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "pk_member_profile",
                table: "member_profile",
                column: "member_id");

            migrationBuilder.AddPrimaryKey(
                name: "pk_friend_request",
                table: "friend_request",
                columns: new[] { "requester_id", "receiver_id" });

            migrationBuilder.AddPrimaryKey(
                name: "pk_comment_upvote",
                table: "comment_upvote",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "pk_asp_net_user_tokens",
                table: "asp_net_user_tokens",
                columns: new[] { "user_id", "login_provider", "name" });

            migrationBuilder.AddPrimaryKey(
                name: "pk_asp_net_user_roles",
                table: "asp_net_user_roles",
                columns: new[] { "user_id", "role_id" });

            migrationBuilder.AddPrimaryKey(
                name: "pk_asp_net_user_logins",
                table: "asp_net_user_logins",
                columns: new[] { "login_provider", "provider_key" });

            migrationBuilder.AddPrimaryKey(
                name: "pk_asp_net_user_claims",
                table: "asp_net_user_claims",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "pk_asp_net_roles",
                table: "asp_net_roles",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "pk_asp_net_role_claims",
                table: "asp_net_role_claims",
                column: "id");

            migrationBuilder.AddForeignKey(
                name: "fk_asp_net_role_claims_asp_net_roles_role_id",
                table: "asp_net_role_claims",
                column: "role_id",
                principalTable: "asp_net_roles",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_asp_net_user_claims_members_user_id",
                table: "asp_net_user_claims",
                column: "user_id",
                principalTable: "members",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_asp_net_user_logins_members_user_id",
                table: "asp_net_user_logins",
                column: "user_id",
                principalTable: "members",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_asp_net_user_roles_asp_net_roles_role_id",
                table: "asp_net_user_roles",
                column: "role_id",
                principalTable: "asp_net_roles",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_asp_net_user_roles_members_user_id",
                table: "asp_net_user_roles",
                column: "user_id",
                principalTable: "members",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_asp_net_user_tokens_members_user_id",
                table: "asp_net_user_tokens",
                column: "user_id",
                principalTable: "members",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_comment_members_members_id",
                table: "comment",
                column: "author_id",
                principalTable: "members",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_comment_post_posts_id",
                table: "comment",
                column: "post_id",
                principalTable: "post",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_comment_upvote_comment_comments_id",
                table: "comment_upvote",
                column: "comment_id",
                principalTable: "comment",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_comment_upvote_members_members_id",
                table: "comment_upvote",
                column: "author_id",
                principalTable: "members",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_friend_request_members_members_id",
                table: "friend_request",
                column: "receiver_id",
                principalTable: "members",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_friend_request_members_members_id1",
                table: "friend_request",
                column: "requester_id",
                principalTable: "members",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_friendship_members_members_id",
                table: "friendship",
                column: "friend_id",
                principalTable: "members",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_friendship_members_members_id1",
                table: "friendship",
                column: "member_id",
                principalTable: "members",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_member_profile_members_members_id",
                table: "member_profile",
                column: "member_id",
                principalTable: "members",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_post_members_members_id",
                table: "post",
                column: "author_id",
                principalTable: "members",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_post_upvote_members_members_id",
                table: "post_upvote",
                column: "author_id",
                principalTable: "members",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_post_upvote_post_posts_id",
                table: "post_upvote",
                column: "post_id",
                principalTable: "post",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_asp_net_role_claims_asp_net_roles_role_id",
                table: "asp_net_role_claims");

            migrationBuilder.DropForeignKey(
                name: "fk_asp_net_user_claims_members_user_id",
                table: "asp_net_user_claims");

            migrationBuilder.DropForeignKey(
                name: "fk_asp_net_user_logins_members_user_id",
                table: "asp_net_user_logins");

            migrationBuilder.DropForeignKey(
                name: "fk_asp_net_user_roles_asp_net_roles_role_id",
                table: "asp_net_user_roles");

            migrationBuilder.DropForeignKey(
                name: "fk_asp_net_user_roles_members_user_id",
                table: "asp_net_user_roles");

            migrationBuilder.DropForeignKey(
                name: "fk_asp_net_user_tokens_members_user_id",
                table: "asp_net_user_tokens");

            migrationBuilder.DropForeignKey(
                name: "fk_comment_members_members_id",
                table: "comment");

            migrationBuilder.DropForeignKey(
                name: "fk_comment_post_posts_id",
                table: "comment");

            migrationBuilder.DropForeignKey(
                name: "fk_comment_upvote_comment_comments_id",
                table: "comment_upvote");

            migrationBuilder.DropForeignKey(
                name: "fk_comment_upvote_members_members_id",
                table: "comment_upvote");

            migrationBuilder.DropForeignKey(
                name: "fk_friend_request_members_members_id",
                table: "friend_request");

            migrationBuilder.DropForeignKey(
                name: "fk_friend_request_members_members_id1",
                table: "friend_request");

            migrationBuilder.DropForeignKey(
                name: "fk_friendship_members_members_id",
                table: "friendship");

            migrationBuilder.DropForeignKey(
                name: "fk_friendship_members_members_id1",
                table: "friendship");

            migrationBuilder.DropForeignKey(
                name: "fk_member_profile_members_members_id",
                table: "member_profile");

            migrationBuilder.DropForeignKey(
                name: "fk_post_members_members_id",
                table: "post");

            migrationBuilder.DropForeignKey(
                name: "fk_post_upvote_members_members_id",
                table: "post_upvote");

            migrationBuilder.DropForeignKey(
                name: "fk_post_upvote_post_posts_id",
                table: "post_upvote");

            migrationBuilder.DropPrimaryKey(
                name: "pk_post",
                table: "post");

            migrationBuilder.DropPrimaryKey(
                name: "pk_members",
                table: "members");

            migrationBuilder.DropPrimaryKey(
                name: "pk_friendship",
                table: "friendship");

            migrationBuilder.DropPrimaryKey(
                name: "pk_comment",
                table: "comment");

            migrationBuilder.DropPrimaryKey(
                name: "pk_post_upvote",
                table: "post_upvote");

            migrationBuilder.DropPrimaryKey(
                name: "pk_member_profile",
                table: "member_profile");

            migrationBuilder.DropPrimaryKey(
                name: "pk_friend_request",
                table: "friend_request");

            migrationBuilder.DropPrimaryKey(
                name: "pk_comment_upvote",
                table: "comment_upvote");

            migrationBuilder.DropPrimaryKey(
                name: "pk_asp_net_user_tokens",
                table: "asp_net_user_tokens");

            migrationBuilder.DropPrimaryKey(
                name: "pk_asp_net_user_roles",
                table: "asp_net_user_roles");

            migrationBuilder.DropPrimaryKey(
                name: "pk_asp_net_user_logins",
                table: "asp_net_user_logins");

            migrationBuilder.DropPrimaryKey(
                name: "pk_asp_net_user_claims",
                table: "asp_net_user_claims");

            migrationBuilder.DropPrimaryKey(
                name: "pk_asp_net_roles",
                table: "asp_net_roles");

            migrationBuilder.DropPrimaryKey(
                name: "pk_asp_net_role_claims",
                table: "asp_net_role_claims");

            migrationBuilder.RenameTable(
                name: "post",
                newName: "Post");

            migrationBuilder.RenameTable(
                name: "members",
                newName: "Members");

            migrationBuilder.RenameTable(
                name: "friendship",
                newName: "Friendship");

            migrationBuilder.RenameTable(
                name: "comment",
                newName: "Comment");

            migrationBuilder.RenameTable(
                name: "post_upvote",
                newName: "PostUpvote");

            migrationBuilder.RenameTable(
                name: "member_profile",
                newName: "MemberProfile");

            migrationBuilder.RenameTable(
                name: "friend_request",
                newName: "FriendRequest");

            migrationBuilder.RenameTable(
                name: "comment_upvote",
                newName: "CommentUpvote");

            migrationBuilder.RenameTable(
                name: "asp_net_user_tokens",
                newName: "AspNetUserTokens");

            migrationBuilder.RenameTable(
                name: "asp_net_user_roles",
                newName: "AspNetUserRoles");

            migrationBuilder.RenameTable(
                name: "asp_net_user_logins",
                newName: "AspNetUserLogins");

            migrationBuilder.RenameTable(
                name: "asp_net_user_claims",
                newName: "AspNetUserClaims");

            migrationBuilder.RenameTable(
                name: "asp_net_roles",
                newName: "AspNetRoles");

            migrationBuilder.RenameTable(
                name: "asp_net_role_claims",
                newName: "AspNetRoleClaims");

            migrationBuilder.RenameColumn(
                name: "title",
                table: "Post",
                newName: "Title");

            migrationBuilder.RenameColumn(
                name: "content",
                table: "Post",
                newName: "Content");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "Post",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "updated_at",
                table: "Post",
                newName: "UpdatedAt");

            migrationBuilder.RenameColumn(
                name: "deleted_at",
                table: "Post",
                newName: "DeletedAt");

            migrationBuilder.RenameColumn(
                name: "created_at",
                table: "Post",
                newName: "CreatedAt");

            migrationBuilder.RenameColumn(
                name: "author_id",
                table: "Post",
                newName: "AuthorId");

            migrationBuilder.RenameIndex(
                name: "ix_post_author_id",
                table: "Post",
                newName: "IX_Post_AuthorId");

            migrationBuilder.RenameColumn(
                name: "email",
                table: "Members",
                newName: "Email");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "Members",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "user_name",
                table: "Members",
                newName: "UserName");

            migrationBuilder.RenameColumn(
                name: "updated_at",
                table: "Members",
                newName: "UpdatedAt");

            migrationBuilder.RenameColumn(
                name: "two_factor_enabled",
                table: "Members",
                newName: "TwoFactorEnabled");

            migrationBuilder.RenameColumn(
                name: "security_stamp",
                table: "Members",
                newName: "SecurityStamp");

            migrationBuilder.RenameColumn(
                name: "phone_number_confirmed",
                table: "Members",
                newName: "PhoneNumberConfirmed");

            migrationBuilder.RenameColumn(
                name: "phone_number",
                table: "Members",
                newName: "PhoneNumber");

            migrationBuilder.RenameColumn(
                name: "password_hash",
                table: "Members",
                newName: "PasswordHash");

            migrationBuilder.RenameColumn(
                name: "normalized_user_name",
                table: "Members",
                newName: "NormalizedUserName");

            migrationBuilder.RenameColumn(
                name: "normalized_email",
                table: "Members",
                newName: "NormalizedEmail");

            migrationBuilder.RenameColumn(
                name: "lockout_end",
                table: "Members",
                newName: "LockoutEnd");

            migrationBuilder.RenameColumn(
                name: "lockout_enabled",
                table: "Members",
                newName: "LockoutEnabled");

            migrationBuilder.RenameColumn(
                name: "last_name",
                table: "Members",
                newName: "LastName");

            migrationBuilder.RenameColumn(
                name: "first_name",
                table: "Members",
                newName: "FirstName");

            migrationBuilder.RenameColumn(
                name: "email_confirmed",
                table: "Members",
                newName: "EmailConfirmed");

            migrationBuilder.RenameColumn(
                name: "deleted_at",
                table: "Members",
                newName: "DeletedAt");

            migrationBuilder.RenameColumn(
                name: "created_at",
                table: "Members",
                newName: "CreatedAt");

            migrationBuilder.RenameColumn(
                name: "concurrency_stamp",
                table: "Members",
                newName: "ConcurrencyStamp");

            migrationBuilder.RenameColumn(
                name: "access_failed_count",
                table: "Members",
                newName: "AccessFailedCount");

            migrationBuilder.RenameIndex(
                name: "ix_members_id",
                table: "Members",
                newName: "IX_Members_Id");

            migrationBuilder.RenameIndex(
                name: "ix_members_email",
                table: "Members",
                newName: "IX_Members_Email");

            migrationBuilder.RenameIndex(
                name: "ix_members_user_name",
                table: "Members",
                newName: "IX_Members_UserName");

            migrationBuilder.RenameColumn(
                name: "friend_id",
                table: "Friendship",
                newName: "FriendId");

            migrationBuilder.RenameColumn(
                name: "member_id",
                table: "Friendship",
                newName: "MemberId");

            migrationBuilder.RenameIndex(
                name: "ix_friendship_friend_id",
                table: "Friendship",
                newName: "IX_Friendship_FriendId");

            migrationBuilder.RenameColumn(
                name: "content",
                table: "Comment",
                newName: "Content");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "Comment",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "updated_at",
                table: "Comment",
                newName: "UpdatedAt");

            migrationBuilder.RenameColumn(
                name: "post_id",
                table: "Comment",
                newName: "PostId");

            migrationBuilder.RenameColumn(
                name: "deleted_at",
                table: "Comment",
                newName: "DeletedAt");

            migrationBuilder.RenameColumn(
                name: "created_at",
                table: "Comment",
                newName: "CreatedAt");

            migrationBuilder.RenameColumn(
                name: "author_id",
                table: "Comment",
                newName: "AuthorId");

            migrationBuilder.RenameIndex(
                name: "ix_comment_post_id",
                table: "Comment",
                newName: "IX_Comment_PostId");

            migrationBuilder.RenameIndex(
                name: "ix_comment_author_id",
                table: "Comment",
                newName: "IX_Comment_AuthorId");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "PostUpvote",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "updated_at",
                table: "PostUpvote",
                newName: "UpdatedAt");

            migrationBuilder.RenameColumn(
                name: "post_id",
                table: "PostUpvote",
                newName: "PostId");

            migrationBuilder.RenameColumn(
                name: "created_at",
                table: "PostUpvote",
                newName: "CreatedAt");

            migrationBuilder.RenameColumn(
                name: "author_id",
                table: "PostUpvote",
                newName: "AuthorId");

            migrationBuilder.RenameIndex(
                name: "ix_post_upvote_post_id",
                table: "PostUpvote",
                newName: "IX_PostUpvote_PostId");

            migrationBuilder.RenameIndex(
                name: "ix_post_upvote_author_id",
                table: "PostUpvote",
                newName: "IX_PostUpvote_AuthorId");

            migrationBuilder.RenameColumn(
                name: "url",
                table: "MemberProfile",
                newName: "URL");

            migrationBuilder.RenameColumn(
                name: "location",
                table: "MemberProfile",
                newName: "Location");

            migrationBuilder.RenameColumn(
                name: "bio",
                table: "MemberProfile",
                newName: "Bio");

            migrationBuilder.RenameColumn(
                name: "updated_at",
                table: "MemberProfile",
                newName: "UpdatedAt");

            migrationBuilder.RenameColumn(
                name: "photo_url",
                table: "MemberProfile",
                newName: "PhotoURL");

            migrationBuilder.RenameColumn(
                name: "deleted_at",
                table: "MemberProfile",
                newName: "DeletedAt");

            migrationBuilder.RenameColumn(
                name: "created_at",
                table: "MemberProfile",
                newName: "CreatedAt");

            migrationBuilder.RenameColumn(
                name: "member_id",
                table: "MemberProfile",
                newName: "MemberId");

            migrationBuilder.RenameColumn(
                name: "status",
                table: "FriendRequest",
                newName: "Status");

            migrationBuilder.RenameColumn(
                name: "receiver_id",
                table: "FriendRequest",
                newName: "ReceiverId");

            migrationBuilder.RenameColumn(
                name: "requester_id",
                table: "FriendRequest",
                newName: "RequesterId");

            migrationBuilder.RenameIndex(
                name: "ix_friend_request_receiver_id",
                table: "FriendRequest",
                newName: "IX_FriendRequest_ReceiverId");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "CommentUpvote",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "updated_at",
                table: "CommentUpvote",
                newName: "UpdatedAt");

            migrationBuilder.RenameColumn(
                name: "created_at",
                table: "CommentUpvote",
                newName: "CreatedAt");

            migrationBuilder.RenameColumn(
                name: "comment_id",
                table: "CommentUpvote",
                newName: "CommentId");

            migrationBuilder.RenameColumn(
                name: "author_id",
                table: "CommentUpvote",
                newName: "AuthorId");

            migrationBuilder.RenameIndex(
                name: "ix_comment_upvote_comment_id",
                table: "CommentUpvote",
                newName: "IX_CommentUpvote_CommentId");

            migrationBuilder.RenameIndex(
                name: "ix_comment_upvote_author_id",
                table: "CommentUpvote",
                newName: "IX_CommentUpvote_AuthorId");

            migrationBuilder.RenameColumn(
                name: "value",
                table: "AspNetUserTokens",
                newName: "Value");

            migrationBuilder.RenameColumn(
                name: "name",
                table: "AspNetUserTokens",
                newName: "Name");

            migrationBuilder.RenameColumn(
                name: "login_provider",
                table: "AspNetUserTokens",
                newName: "LoginProvider");

            migrationBuilder.RenameColumn(
                name: "user_id",
                table: "AspNetUserTokens",
                newName: "UserId");

            migrationBuilder.RenameColumn(
                name: "role_id",
                table: "AspNetUserRoles",
                newName: "RoleId");

            migrationBuilder.RenameColumn(
                name: "user_id",
                table: "AspNetUserRoles",
                newName: "UserId");

            migrationBuilder.RenameIndex(
                name: "ix_asp_net_user_roles_role_id",
                table: "AspNetUserRoles",
                newName: "IX_AspNetUserRoles_RoleId");

            migrationBuilder.RenameColumn(
                name: "user_id",
                table: "AspNetUserLogins",
                newName: "UserId");

            migrationBuilder.RenameColumn(
                name: "provider_display_name",
                table: "AspNetUserLogins",
                newName: "ProviderDisplayName");

            migrationBuilder.RenameColumn(
                name: "provider_key",
                table: "AspNetUserLogins",
                newName: "ProviderKey");

            migrationBuilder.RenameColumn(
                name: "login_provider",
                table: "AspNetUserLogins",
                newName: "LoginProvider");

            migrationBuilder.RenameIndex(
                name: "ix_asp_net_user_logins_user_id",
                table: "AspNetUserLogins",
                newName: "IX_AspNetUserLogins_UserId");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "AspNetUserClaims",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "user_id",
                table: "AspNetUserClaims",
                newName: "UserId");

            migrationBuilder.RenameColumn(
                name: "claim_value",
                table: "AspNetUserClaims",
                newName: "ClaimValue");

            migrationBuilder.RenameColumn(
                name: "claim_type",
                table: "AspNetUserClaims",
                newName: "ClaimType");

            migrationBuilder.RenameIndex(
                name: "ix_asp_net_user_claims_user_id",
                table: "AspNetUserClaims",
                newName: "IX_AspNetUserClaims_UserId");

            migrationBuilder.RenameColumn(
                name: "name",
                table: "AspNetRoles",
                newName: "Name");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "AspNetRoles",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "normalized_name",
                table: "AspNetRoles",
                newName: "NormalizedName");

            migrationBuilder.RenameColumn(
                name: "concurrency_stamp",
                table: "AspNetRoles",
                newName: "ConcurrencyStamp");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "AspNetRoleClaims",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "role_id",
                table: "AspNetRoleClaims",
                newName: "RoleId");

            migrationBuilder.RenameColumn(
                name: "claim_value",
                table: "AspNetRoleClaims",
                newName: "ClaimValue");

            migrationBuilder.RenameColumn(
                name: "claim_type",
                table: "AspNetRoleClaims",
                newName: "ClaimType");

            migrationBuilder.RenameIndex(
                name: "ix_asp_net_role_claims_role_id",
                table: "AspNetRoleClaims",
                newName: "IX_AspNetRoleClaims_RoleId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Post",
                table: "Post",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Members",
                table: "Members",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Friendship",
                table: "Friendship",
                columns: new[] { "MemberId", "FriendId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_Comment",
                table: "Comment",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_PostUpvote",
                table: "PostUpvote",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_MemberProfile",
                table: "MemberProfile",
                column: "MemberId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_FriendRequest",
                table: "FriendRequest",
                columns: new[] { "RequesterId", "ReceiverId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_CommentUpvote",
                table: "CommentUpvote",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_AspNetUserTokens",
                table: "AspNetUserTokens",
                columns: new[] { "UserId", "LoginProvider", "Name" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_AspNetUserRoles",
                table: "AspNetUserRoles",
                columns: new[] { "UserId", "RoleId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_AspNetUserLogins",
                table: "AspNetUserLogins",
                columns: new[] { "LoginProvider", "ProviderKey" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_AspNetUserClaims",
                table: "AspNetUserClaims",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_AspNetRoles",
                table: "AspNetRoles",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_AspNetRoleClaims",
                table: "AspNetRoleClaims",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetRoleClaims_AspNetRoles_RoleId",
                table: "AspNetRoleClaims",
                column: "RoleId",
                principalTable: "AspNetRoles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUserClaims_Members_UserId",
                table: "AspNetUserClaims",
                column: "UserId",
                principalTable: "Members",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUserLogins_Members_UserId",
                table: "AspNetUserLogins",
                column: "UserId",
                principalTable: "Members",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUserRoles_AspNetRoles_RoleId",
                table: "AspNetUserRoles",
                column: "RoleId",
                principalTable: "AspNetRoles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUserRoles_Members_UserId",
                table: "AspNetUserRoles",
                column: "UserId",
                principalTable: "Members",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUserTokens_Members_UserId",
                table: "AspNetUserTokens",
                column: "UserId",
                principalTable: "Members",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Comment_Members_AuthorId",
                table: "Comment",
                column: "AuthorId",
                principalTable: "Members",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Comment_Post_PostId",
                table: "Comment",
                column: "PostId",
                principalTable: "Post",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_CommentUpvote_Comment_CommentId",
                table: "CommentUpvote",
                column: "CommentId",
                principalTable: "Comment",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_CommentUpvote_Members_AuthorId",
                table: "CommentUpvote",
                column: "AuthorId",
                principalTable: "Members",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_FriendRequest_Members_ReceiverId",
                table: "FriendRequest",
                column: "ReceiverId",
                principalTable: "Members",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_FriendRequest_Members_RequesterId",
                table: "FriendRequest",
                column: "RequesterId",
                principalTable: "Members",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Friendship_Members_FriendId",
                table: "Friendship",
                column: "FriendId",
                principalTable: "Members",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Friendship_Members_MemberId",
                table: "Friendship",
                column: "MemberId",
                principalTable: "Members",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_MemberProfile_Members_MemberId",
                table: "MemberProfile",
                column: "MemberId",
                principalTable: "Members",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Post_Members_AuthorId",
                table: "Post",
                column: "AuthorId",
                principalTable: "Members",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PostUpvote_Members_AuthorId",
                table: "PostUpvote",
                column: "AuthorId",
                principalTable: "Members",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PostUpvote_Post_PostId",
                table: "PostUpvote",
                column: "PostId",
                principalTable: "Post",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
