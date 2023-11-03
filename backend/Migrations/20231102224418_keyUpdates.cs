using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace social_media_api.Migrations
{
    /// <inheritdoc />
    public partial class keyUpdates : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
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

            migrationBuilder.DropIndex(
                name: "IX_post_upvote_MembersId",
                table: "post_upvote");

            migrationBuilder.DropIndex(
                name: "IX_post_upvote_PostsId",
                table: "post_upvote");

            migrationBuilder.DropIndex(
                name: "IX_post_MembersId",
                table: "post");

            migrationBuilder.DropIndex(
                name: "IX_member_profile_MembersId",
                table: "member_profile");

            migrationBuilder.DropIndex(
                name: "IX_friendship_friend_id",
                table: "friendship");

            migrationBuilder.DropIndex(
                name: "IX_friend_request_receiver_id",
                table: "friend_request");

            migrationBuilder.DropIndex(
                name: "IX_comment_upvote_CommentsId",
                table: "comment_upvote");

            migrationBuilder.DropIndex(
                name: "IX_comment_upvote_MembersId",
                table: "comment_upvote");

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
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
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
                name: "IX_friendship_friend_id",
                table: "friendship",
                column: "friend_id");

            migrationBuilder.CreateIndex(
                name: "IX_friend_request_receiver_id",
                table: "friend_request",
                column: "receiver_id");

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
    }
}
