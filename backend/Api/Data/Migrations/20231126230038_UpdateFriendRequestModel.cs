using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace social_media_api.Migrations
{
    /// <inheritdoc />
    public partial class UpdateFriendRequestModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_comment_member_members_id",
                table: "comment"
            );

            migrationBuilder.DropForeignKey(
                name: "fk_comment_post_posts_id",
                table: "comment"
            );

            migrationBuilder.DropForeignKey(
                name: "fk_comment_upvote_comment_comments_id",
                table: "comment_upvote"
            );

            migrationBuilder.DropForeignKey(
                name: "fk_comment_upvote_member_members_id",
                table: "comment_upvote"
            );

            migrationBuilder.DropForeignKey(
                name: "fk_friend_request_member_members_id",
                table: "friend_request"
            );

            migrationBuilder.DropForeignKey(
                name: "fk_friend_request_member_members_id1",
                table: "friend_request"
            );

            migrationBuilder.DropForeignKey(
                name: "fk_friendship_member_members_id",
                table: "friendship"
            );

            migrationBuilder.DropForeignKey(
                name: "fk_friendship_member_members_id1",
                table: "friendship"
            );

            migrationBuilder.DropForeignKey(
                name: "fk_member_profile_member_members_id",
                table: "member_profile"
            );

            migrationBuilder.DropForeignKey(
                name: "fk_post_member_members_id",
                table: "post"
            );

            migrationBuilder.DropForeignKey(
                name: "fk_post_upvote_member_members_id",
                table: "post_upvote"
            );

            migrationBuilder.DropForeignKey(
                name: "fk_post_upvote_post_posts_id",
                table: "post_upvote"
            );

            migrationBuilder.AddColumn<DateTime>(
                name: "created_at",
                table: "friend_request",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(
                    1,
                    1,
                    1,
                    0,
                    0,
                    0,
                    0,
                    DateTimeKind.Unspecified
                )
            );

            migrationBuilder.AddColumn<DateTime>(
                name: "updated_at",
                table: "friend_request",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(
                    1,
                    1,
                    1,
                    0,
                    0,
                    0,
                    0,
                    DateTimeKind.Unspecified
                )
            );

            migrationBuilder.AddForeignKey(
                name: "fk_comment_member_member_id",
                table: "comment",
                column: "author_id",
                principalTable: "member",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade
            );

            migrationBuilder.AddForeignKey(
                name: "fk_comment_post_post_id",
                table: "comment",
                column: "post_id",
                principalTable: "post",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade
            );

            migrationBuilder.AddForeignKey(
                name: "fk_comment_upvote_comment_comment_id",
                table: "comment_upvote",
                column: "comment_id",
                principalTable: "comment",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade
            );

            migrationBuilder.AddForeignKey(
                name: "fk_comment_upvote_member_member_id",
                table: "comment_upvote",
                column: "author_id",
                principalTable: "member",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade
            );

            migrationBuilder.AddForeignKey(
                name: "fk_friend_request_member_member_id",
                table: "friend_request",
                column: "receiver_id",
                principalTable: "member",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade
            );

            migrationBuilder.AddForeignKey(
                name: "fk_friend_request_member_member_id1",
                table: "friend_request",
                column: "requester_id",
                principalTable: "member",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade
            );

            migrationBuilder.AddForeignKey(
                name: "fk_friendship_member_member_id",
                table: "friendship",
                column: "member_id",
                principalTable: "member",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade
            );

            migrationBuilder.AddForeignKey(
                name: "fk_friendship_member_member_id1",
                table: "friendship",
                column: "friend_id",
                principalTable: "member",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade
            );

            migrationBuilder.AddForeignKey(
                name: "fk_member_profile_member_member_id1",
                table: "member_profile",
                column: "member_id",
                principalTable: "member",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade
            );

            migrationBuilder.AddForeignKey(
                name: "fk_post_member_member_id",
                table: "post",
                column: "author_id",
                principalTable: "member",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade
            );

            migrationBuilder.AddForeignKey(
                name: "fk_post_upvote_member_member_id",
                table: "post_upvote",
                column: "author_id",
                principalTable: "member",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade
            );

            migrationBuilder.AddForeignKey(
                name: "fk_post_upvote_post_post_id",
                table: "post_upvote",
                column: "post_id",
                principalTable: "post",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade
            );
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_comment_member_member_id",
                table: "comment"
            );

            migrationBuilder.DropForeignKey(
                name: "fk_comment_post_post_id",
                table: "comment"
            );

            migrationBuilder.DropForeignKey(
                name: "fk_comment_upvote_comment_comment_id",
                table: "comment_upvote"
            );

            migrationBuilder.DropForeignKey(
                name: "fk_comment_upvote_member_member_id",
                table: "comment_upvote"
            );

            migrationBuilder.DropForeignKey(
                name: "fk_friend_request_member_member_id",
                table: "friend_request"
            );

            migrationBuilder.DropForeignKey(
                name: "fk_friend_request_member_member_id1",
                table: "friend_request"
            );

            migrationBuilder.DropForeignKey(
                name: "fk_friendship_member_member_id",
                table: "friendship"
            );

            migrationBuilder.DropForeignKey(
                name: "fk_friendship_member_member_id1",
                table: "friendship"
            );

            migrationBuilder.DropForeignKey(
                name: "fk_member_profile_member_member_id1",
                table: "member_profile"
            );

            migrationBuilder.DropForeignKey(
                name: "fk_post_member_member_id",
                table: "post"
            );

            migrationBuilder.DropForeignKey(
                name: "fk_post_upvote_member_member_id",
                table: "post_upvote"
            );

            migrationBuilder.DropForeignKey(
                name: "fk_post_upvote_post_post_id",
                table: "post_upvote"
            );

            migrationBuilder.DropColumn(
                name: "created_at",
                table: "friend_request"
            );

            migrationBuilder.DropColumn(
                name: "updated_at",
                table: "friend_request"
            );

            migrationBuilder.AddForeignKey(
                name: "fk_comment_member_members_id",
                table: "comment",
                column: "author_id",
                principalTable: "member",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade
            );

            migrationBuilder.AddForeignKey(
                name: "fk_comment_post_posts_id",
                table: "comment",
                column: "post_id",
                principalTable: "post",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade
            );

            migrationBuilder.AddForeignKey(
                name: "fk_comment_upvote_comment_comments_id",
                table: "comment_upvote",
                column: "comment_id",
                principalTable: "comment",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade
            );

            migrationBuilder.AddForeignKey(
                name: "fk_comment_upvote_member_members_id",
                table: "comment_upvote",
                column: "author_id",
                principalTable: "member",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade
            );

            migrationBuilder.AddForeignKey(
                name: "fk_friend_request_member_members_id",
                table: "friend_request",
                column: "receiver_id",
                principalTable: "member",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade
            );

            migrationBuilder.AddForeignKey(
                name: "fk_friend_request_member_members_id1",
                table: "friend_request",
                column: "requester_id",
                principalTable: "member",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade
            );

            migrationBuilder.AddForeignKey(
                name: "fk_friendship_member_members_id",
                table: "friendship",
                column: "friend_id",
                principalTable: "member",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade
            );

            migrationBuilder.AddForeignKey(
                name: "fk_friendship_member_members_id1",
                table: "friendship",
                column: "member_id",
                principalTable: "member",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade
            );

            migrationBuilder.AddForeignKey(
                name: "fk_member_profile_member_members_id",
                table: "member_profile",
                column: "member_id",
                principalTable: "member",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade
            );

            migrationBuilder.AddForeignKey(
                name: "fk_post_member_members_id",
                table: "post",
                column: "author_id",
                principalTable: "member",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade
            );

            migrationBuilder.AddForeignKey(
                name: "fk_post_upvote_member_members_id",
                table: "post_upvote",
                column: "author_id",
                principalTable: "member",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade
            );

            migrationBuilder.AddForeignKey(
                name: "fk_post_upvote_post_posts_id",
                table: "post_upvote",
                column: "post_id",
                principalTable: "post",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade
            );
        }
    }
}
