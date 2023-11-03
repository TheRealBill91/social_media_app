using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace social_media_api.Migrations
{
    /// <inheritdoc />
    public partial class updateTitleAndColumns : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Comment_Member_AuthorId",
                table: "Comment");

            migrationBuilder.DropForeignKey(
                name: "FK_Comment_Post_PostId",
                table: "Comment");

            migrationBuilder.DropForeignKey(
                name: "FK_Comment_Upvote_Comment_CommentId",
                table: "Comment_Upvote");

            migrationBuilder.DropForeignKey(
                name: "FK_Comment_Upvote_Member_AuthorId",
                table: "Comment_Upvote");

            migrationBuilder.DropForeignKey(
                name: "FK_Friend_Request_Member_ReceiverId",
                table: "Friend_Request");

            migrationBuilder.DropForeignKey(
                name: "FK_Friend_Request_Member_RequesterId",
                table: "Friend_Request");

            migrationBuilder.DropForeignKey(
                name: "FK_Friendship_Member_FriendId",
                table: "Friendship");

            migrationBuilder.DropForeignKey(
                name: "FK_Friendship_Member_MemberId",
                table: "Friendship");

            migrationBuilder.DropForeignKey(
                name: "FK_Member_Profile_Member_MemberId",
                table: "Member_Profile");

            migrationBuilder.DropForeignKey(
                name: "FK_Post_Member_AuthorId",
                table: "Post");

            migrationBuilder.DropForeignKey(
                name: "FK_Post_Upvote_Member_AuthorId",
                table: "Post_Upvote");

            migrationBuilder.DropForeignKey(
                name: "FK_Post_Upvote_Post_PostId",
                table: "Post_Upvote");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Post_Upvote",
                table: "Post_Upvote");

            migrationBuilder.DropIndex(
                name: "IX_Post_Upvote_AuthorId",
                table: "Post_Upvote");

            migrationBuilder.DropIndex(
                name: "IX_Post_Upvote_PostId",
                table: "Post_Upvote");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Post",
                table: "Post");

            migrationBuilder.DropIndex(
                name: "IX_Post_AuthorId",
                table: "Post");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Member_Profile",
                table: "Member_Profile");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Member",
                table: "Member");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Friendship",
                table: "Friendship");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Friend_Request",
                table: "Friend_Request");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Comment_Upvote",
                table: "Comment_Upvote");

            migrationBuilder.DropIndex(
                name: "IX_Comment_Upvote_AuthorId",
                table: "Comment_Upvote");

            migrationBuilder.DropIndex(
                name: "IX_Comment_Upvote_CommentId",
                table: "Comment_Upvote");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Comment",
                table: "Comment");

            migrationBuilder.DropIndex(
                name: "IX_Comment_AuthorId",
                table: "Comment");

            migrationBuilder.DropIndex(
                name: "IX_Comment_PostId",
                table: "Comment");

            migrationBuilder.RenameTable(
                name: "Post_Upvote",
                newName: "post_upvote");

            migrationBuilder.RenameTable(
                name: "Post",
                newName: "post");

            migrationBuilder.RenameTable(
                name: "Member_Profile",
                newName: "member_profile");

            migrationBuilder.RenameTable(
                name: "Member",
                newName: "member");

            migrationBuilder.RenameTable(
                name: "Friendship",
                newName: "friendship");

            migrationBuilder.RenameTable(
                name: "Friend_Request",
                newName: "friend_request");

            migrationBuilder.RenameTable(
                name: "Comment_Upvote",
                newName: "comment_upvote");

            migrationBuilder.RenameTable(
                name: "Comment",
                newName: "comment");

            migrationBuilder.RenameColumn(
                name: "Timestamp",
                table: "post_upvote",
                newName: "timestamp");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "post_upvote",
                newName: "id");

            migrationBuilder.RenameColumn(
                name: "PostId",
                table: "post_upvote",
                newName: "post_id");

            migrationBuilder.RenameColumn(
                name: "AuthorId",
                table: "post_upvote",
                newName: "author_id");

            migrationBuilder.RenameColumn(
                name: "Title",
                table: "post",
                newName: "title");

            migrationBuilder.RenameColumn(
                name: "Modified",
                table: "post",
                newName: "modified");

            migrationBuilder.RenameColumn(
                name: "Created",
                table: "post",
                newName: "created");

            migrationBuilder.RenameColumn(
                name: "Content",
                table: "post",
                newName: "content");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "post",
                newName: "id");

            migrationBuilder.RenameColumn(
                name: "AuthorId",
                table: "post",
                newName: "author_id");

            migrationBuilder.RenameColumn(
                name: "PhotoURL",
                table: "member_profile",
                newName: "photo_url");

            migrationBuilder.RenameColumn(
                name: "MemberId",
                table: "member_profile",
                newName: "member_id");

            migrationBuilder.RenameColumn(
                name: "UserName",
                table: "member",
                newName: "username");

            migrationBuilder.RenameColumn(
                name: "Email",
                table: "member",
                newName: "email");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "member",
                newName: "id");

            migrationBuilder.RenameColumn(
                name: "LastName",
                table: "member",
                newName: "last_name");

            migrationBuilder.RenameColumn(
                name: "LastActive",
                table: "member",
                newName: "last_active");

            migrationBuilder.RenameColumn(
                name: "FirstName",
                table: "member",
                newName: "first_name");

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
                newName: "IX_friendship_friend_id");

            migrationBuilder.RenameColumn(
                name: "ReceiverId",
                table: "friend_request",
                newName: "receiver_id");

            migrationBuilder.RenameColumn(
                name: "RequesterId",
                table: "friend_request",
                newName: "requester_id");

            migrationBuilder.RenameIndex(
                name: "IX_Friend_Request_ReceiverId",
                table: "friend_request",
                newName: "IX_friend_request_receiver_id");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "comment_upvote",
                newName: "id");

            migrationBuilder.RenameColumn(
                name: "Timestamp",
                table: "comment_upvote",
                newName: "time_stamp");

            migrationBuilder.RenameColumn(
                name: "CommentId",
                table: "comment_upvote",
                newName: "comment_id");

            migrationBuilder.RenameColumn(
                name: "AuthorId",
                table: "comment_upvote",
                newName: "author_id");

            migrationBuilder.RenameColumn(
                name: "Created",
                table: "comment",
                newName: "created");

            migrationBuilder.RenameColumn(
                name: "Content",
                table: "comment",
                newName: "content");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "comment",
                newName: "id");

            migrationBuilder.RenameColumn(
                name: "PostId",
                table: "comment",
                newName: "post_id");

            migrationBuilder.RenameColumn(
                name: "AuthorId",
                table: "comment",
                newName: "author_id");

            migrationBuilder.AddColumn<Guid>(
                name: "MembersId",
                table: "post_upvote",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "PostsId",
                table: "post_upvote",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "MembersId",
                table: "post",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "MembersId",
                table: "member_profile",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "CommentsId",
                table: "comment_upvote",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "MembersId",
                table: "comment_upvote",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "MembersId",
                table: "comment",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "PostsId",
                table: "comment",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddPrimaryKey(
                name: "PK_post_upvote",
                table: "post_upvote",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_post",
                table: "post",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_member_profile",
                table: "member_profile",
                column: "member_id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_member",
                table: "member",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_friendship",
                table: "friendship",
                columns: new[] { "member_id", "friend_id" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_friend_request",
                table: "friend_request",
                columns: new[] { "requester_id", "receiver_id" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_comment_upvote",
                table: "comment_upvote",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_comment",
                table: "comment",
                column: "id");

            migrationBuilder.CreateIndex(
                name: "IX_post_upvote_MembersId",
                table: "post_upvote",
                column: "MembersId");

            migrationBuilder.CreateIndex(
                name: "IX_post_upvote_PostsId",
                table: "post_upvote",
                column: "PostsId");

            migrationBuilder.CreateIndex(
                name: "IX_post_MembersId",
                table: "post",
                column: "MembersId");

            migrationBuilder.CreateIndex(
                name: "IX_member_profile_MembersId",
                table: "member_profile",
                column: "MembersId");

            migrationBuilder.CreateIndex(
                name: "IX_comment_upvote_CommentsId",
                table: "comment_upvote",
                column: "CommentsId");

            migrationBuilder.CreateIndex(
                name: "IX_comment_upvote_MembersId",
                table: "comment_upvote",
                column: "MembersId");

            migrationBuilder.CreateIndex(
                name: "IX_comment_MembersId",
                table: "comment",
                column: "MembersId");

            migrationBuilder.CreateIndex(
                name: "IX_comment_PostsId",
                table: "comment",
                column: "PostsId");

            migrationBuilder.AddForeignKey(
                name: "FK_comment_member_MembersId",
                table: "comment",
                column: "MembersId",
                principalTable: "member",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_comment_post_PostsId",
                table: "comment",
                column: "PostsId",
                principalTable: "post",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_comment_upvote_comment_CommentsId",
                table: "comment_upvote",
                column: "CommentsId",
                principalTable: "comment",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_comment_upvote_member_MembersId",
                table: "comment_upvote",
                column: "MembersId",
                principalTable: "member",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_friend_request_member_receiver_id",
                table: "friend_request",
                column: "receiver_id",
                principalTable: "member",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_friend_request_member_requester_id",
                table: "friend_request",
                column: "requester_id",
                principalTable: "member",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_friendship_member_friend_id",
                table: "friendship",
                column: "friend_id",
                principalTable: "member",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_friendship_member_member_id",
                table: "friendship",
                column: "member_id",
                principalTable: "member",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_member_profile_member_MembersId",
                table: "member_profile",
                column: "MembersId",
                principalTable: "member",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_post_member_MembersId",
                table: "post",
                column: "MembersId",
                principalTable: "member",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_post_upvote_member_MembersId",
                table: "post_upvote",
                column: "MembersId",
                principalTable: "member",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_post_upvote_post_PostsId",
                table: "post_upvote",
                column: "PostsId",
                principalTable: "post",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_comment_member_MembersId",
                table: "comment");

            migrationBuilder.DropForeignKey(
                name: "FK_comment_post_PostsId",
                table: "comment");

            migrationBuilder.DropForeignKey(
                name: "FK_comment_upvote_comment_CommentsId",
                table: "comment_upvote");

            migrationBuilder.DropForeignKey(
                name: "FK_comment_upvote_member_MembersId",
                table: "comment_upvote");

            migrationBuilder.DropForeignKey(
                name: "FK_friend_request_member_receiver_id",
                table: "friend_request");

            migrationBuilder.DropForeignKey(
                name: "FK_friend_request_member_requester_id",
                table: "friend_request");

            migrationBuilder.DropForeignKey(
                name: "FK_friendship_member_friend_id",
                table: "friendship");

            migrationBuilder.DropForeignKey(
                name: "FK_friendship_member_member_id",
                table: "friendship");

            migrationBuilder.DropForeignKey(
                name: "FK_member_profile_member_MembersId",
                table: "member_profile");

            migrationBuilder.DropForeignKey(
                name: "FK_post_member_MembersId",
                table: "post");

            migrationBuilder.DropForeignKey(
                name: "FK_post_upvote_member_MembersId",
                table: "post_upvote");

            migrationBuilder.DropForeignKey(
                name: "FK_post_upvote_post_PostsId",
                table: "post_upvote");

            migrationBuilder.DropPrimaryKey(
                name: "PK_post_upvote",
                table: "post_upvote");

            migrationBuilder.DropIndex(
                name: "IX_post_upvote_MembersId",
                table: "post_upvote");

            migrationBuilder.DropIndex(
                name: "IX_post_upvote_PostsId",
                table: "post_upvote");

            migrationBuilder.DropPrimaryKey(
                name: "PK_post",
                table: "post");

            migrationBuilder.DropIndex(
                name: "IX_post_MembersId",
                table: "post");

            migrationBuilder.DropPrimaryKey(
                name: "PK_member_profile",
                table: "member_profile");

            migrationBuilder.DropIndex(
                name: "IX_member_profile_MembersId",
                table: "member_profile");

            migrationBuilder.DropPrimaryKey(
                name: "PK_member",
                table: "member");

            migrationBuilder.DropPrimaryKey(
                name: "PK_friendship",
                table: "friendship");

            migrationBuilder.DropPrimaryKey(
                name: "PK_friend_request",
                table: "friend_request");

            migrationBuilder.DropPrimaryKey(
                name: "PK_comment_upvote",
                table: "comment_upvote");

            migrationBuilder.DropIndex(
                name: "IX_comment_upvote_CommentsId",
                table: "comment_upvote");

            migrationBuilder.DropIndex(
                name: "IX_comment_upvote_MembersId",
                table: "comment_upvote");

            migrationBuilder.DropPrimaryKey(
                name: "PK_comment",
                table: "comment");

            migrationBuilder.DropIndex(
                name: "IX_comment_MembersId",
                table: "comment");

            migrationBuilder.DropIndex(
                name: "IX_comment_PostsId",
                table: "comment");

            migrationBuilder.DropColumn(
                name: "MembersId",
                table: "post_upvote");

            migrationBuilder.DropColumn(
                name: "PostsId",
                table: "post_upvote");

            migrationBuilder.DropColumn(
                name: "MembersId",
                table: "post");

            migrationBuilder.DropColumn(
                name: "MembersId",
                table: "member_profile");

            migrationBuilder.DropColumn(
                name: "CommentsId",
                table: "comment_upvote");

            migrationBuilder.DropColumn(
                name: "MembersId",
                table: "comment_upvote");

            migrationBuilder.DropColumn(
                name: "MembersId",
                table: "comment");

            migrationBuilder.DropColumn(
                name: "PostsId",
                table: "comment");

            migrationBuilder.RenameTable(
                name: "post_upvote",
                newName: "Post_Upvote");

            migrationBuilder.RenameTable(
                name: "post",
                newName: "Post");

            migrationBuilder.RenameTable(
                name: "member_profile",
                newName: "Member_Profile");

            migrationBuilder.RenameTable(
                name: "member",
                newName: "Member");

            migrationBuilder.RenameTable(
                name: "friendship",
                newName: "Friendship");

            migrationBuilder.RenameTable(
                name: "friend_request",
                newName: "Friend_Request");

            migrationBuilder.RenameTable(
                name: "comment_upvote",
                newName: "Comment_Upvote");

            migrationBuilder.RenameTable(
                name: "comment",
                newName: "Comment");

            migrationBuilder.RenameColumn(
                name: "timestamp",
                table: "Post_Upvote",
                newName: "Timestamp");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "Post_Upvote",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "post_id",
                table: "Post_Upvote",
                newName: "PostId");

            migrationBuilder.RenameColumn(
                name: "author_id",
                table: "Post_Upvote",
                newName: "AuthorId");

            migrationBuilder.RenameColumn(
                name: "title",
                table: "Post",
                newName: "Title");

            migrationBuilder.RenameColumn(
                name: "modified",
                table: "Post",
                newName: "Modified");

            migrationBuilder.RenameColumn(
                name: "created",
                table: "Post",
                newName: "Created");

            migrationBuilder.RenameColumn(
                name: "content",
                table: "Post",
                newName: "Content");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "Post",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "author_id",
                table: "Post",
                newName: "AuthorId");

            migrationBuilder.RenameColumn(
                name: "photo_url",
                table: "Member_Profile",
                newName: "PhotoURL");

            migrationBuilder.RenameColumn(
                name: "member_id",
                table: "Member_Profile",
                newName: "MemberId");

            migrationBuilder.RenameColumn(
                name: "username",
                table: "Member",
                newName: "UserName");

            migrationBuilder.RenameColumn(
                name: "email",
                table: "Member",
                newName: "Email");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "Member",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "last_name",
                table: "Member",
                newName: "LastName");

            migrationBuilder.RenameColumn(
                name: "last_active",
                table: "Member",
                newName: "LastActive");

            migrationBuilder.RenameColumn(
                name: "first_name",
                table: "Member",
                newName: "FirstName");

            migrationBuilder.RenameColumn(
                name: "friend_id",
                table: "Friendship",
                newName: "FriendId");

            migrationBuilder.RenameColumn(
                name: "member_id",
                table: "Friendship",
                newName: "MemberId");

            migrationBuilder.RenameIndex(
                name: "IX_friendship_friend_id",
                table: "Friendship",
                newName: "IX_Friendship_FriendId");

            migrationBuilder.RenameColumn(
                name: "receiver_id",
                table: "Friend_Request",
                newName: "ReceiverId");

            migrationBuilder.RenameColumn(
                name: "requester_id",
                table: "Friend_Request",
                newName: "RequesterId");

            migrationBuilder.RenameIndex(
                name: "IX_friend_request_receiver_id",
                table: "Friend_Request",
                newName: "IX_Friend_Request_ReceiverId");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "Comment_Upvote",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "time_stamp",
                table: "Comment_Upvote",
                newName: "Timestamp");

            migrationBuilder.RenameColumn(
                name: "comment_id",
                table: "Comment_Upvote",
                newName: "CommentId");

            migrationBuilder.RenameColumn(
                name: "author_id",
                table: "Comment_Upvote",
                newName: "AuthorId");

            migrationBuilder.RenameColumn(
                name: "created",
                table: "Comment",
                newName: "Created");

            migrationBuilder.RenameColumn(
                name: "content",
                table: "Comment",
                newName: "Content");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "Comment",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "post_id",
                table: "Comment",
                newName: "PostId");

            migrationBuilder.RenameColumn(
                name: "author_id",
                table: "Comment",
                newName: "AuthorId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Post_Upvote",
                table: "Post_Upvote",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Post",
                table: "Post",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Member_Profile",
                table: "Member_Profile",
                column: "MemberId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Member",
                table: "Member",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Friendship",
                table: "Friendship",
                columns: new[] { "MemberId", "FriendId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_Friend_Request",
                table: "Friend_Request",
                columns: new[] { "RequesterId", "ReceiverId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_Comment_Upvote",
                table: "Comment_Upvote",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Comment",
                table: "Comment",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_Post_Upvote_AuthorId",
                table: "Post_Upvote",
                column: "AuthorId");

            migrationBuilder.CreateIndex(
                name: "IX_Post_Upvote_PostId",
                table: "Post_Upvote",
                column: "PostId");

            migrationBuilder.CreateIndex(
                name: "IX_Post_AuthorId",
                table: "Post",
                column: "AuthorId");

            migrationBuilder.CreateIndex(
                name: "IX_Comment_Upvote_AuthorId",
                table: "Comment_Upvote",
                column: "AuthorId");

            migrationBuilder.CreateIndex(
                name: "IX_Comment_Upvote_CommentId",
                table: "Comment_Upvote",
                column: "CommentId");

            migrationBuilder.CreateIndex(
                name: "IX_Comment_AuthorId",
                table: "Comment",
                column: "AuthorId");

            migrationBuilder.CreateIndex(
                name: "IX_Comment_PostId",
                table: "Comment",
                column: "PostId");

            migrationBuilder.AddForeignKey(
                name: "FK_Comment_Member_AuthorId",
                table: "Comment",
                column: "AuthorId",
                principalTable: "Member",
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
                name: "FK_Comment_Upvote_Comment_CommentId",
                table: "Comment_Upvote",
                column: "CommentId",
                principalTable: "Comment",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Comment_Upvote_Member_AuthorId",
                table: "Comment_Upvote",
                column: "AuthorId",
                principalTable: "Member",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Friend_Request_Member_ReceiverId",
                table: "Friend_Request",
                column: "ReceiverId",
                principalTable: "Member",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Friend_Request_Member_RequesterId",
                table: "Friend_Request",
                column: "RequesterId",
                principalTable: "Member",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Friendship_Member_FriendId",
                table: "Friendship",
                column: "FriendId",
                principalTable: "Member",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Friendship_Member_MemberId",
                table: "Friendship",
                column: "MemberId",
                principalTable: "Member",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Member_Profile_Member_MemberId",
                table: "Member_Profile",
                column: "MemberId",
                principalTable: "Member",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Post_Member_AuthorId",
                table: "Post",
                column: "AuthorId",
                principalTable: "Member",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Post_Upvote_Member_AuthorId",
                table: "Post_Upvote",
                column: "AuthorId",
                principalTable: "Member",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Post_Upvote_Post_PostId",
                table: "Post_Upvote",
                column: "PostId",
                principalTable: "Post",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
