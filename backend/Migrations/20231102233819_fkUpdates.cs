using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace social_media_api.Migrations
{
    /// <inheritdoc />
    public partial class fkUpdates : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_post_upvote_author_id",
                table: "post_upvote",
                column: "author_id");

            migrationBuilder.CreateIndex(
                name: "IX_post_upvote_post_id",
                table: "post_upvote",
                column: "post_id");

            migrationBuilder.CreateIndex(
                name: "IX_post_author_id",
                table: "post",
                column: "author_id");

            migrationBuilder.CreateIndex(
                name: "IX_friendship_friend_id",
                table: "friendship",
                column: "friend_id");

            migrationBuilder.CreateIndex(
                name: "IX_friend_request_receiver_id",
                table: "friend_request",
                column: "receiver_id");

            migrationBuilder.CreateIndex(
                name: "IX_comment_upvote_author_id",
                table: "comment_upvote",
                column: "author_id");

            migrationBuilder.CreateIndex(
                name: "IX_comment_upvote_comment_id",
                table: "comment_upvote",
                column: "comment_id");

            migrationBuilder.CreateIndex(
                name: "IX_comment_author_id",
                table: "comment",
                column: "author_id");

            migrationBuilder.CreateIndex(
                name: "IX_comment_post_id",
                table: "comment",
                column: "post_id");

            migrationBuilder.AddForeignKey(
                name: "FK_comment_member_author_id",
                table: "comment",
                column: "author_id",
                principalTable: "member",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_comment_post_post_id",
                table: "comment",
                column: "post_id",
                principalTable: "post",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_comment_upvote_comment_comment_id",
                table: "comment_upvote",
                column: "comment_id",
                principalTable: "comment",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_comment_upvote_member_author_id",
                table: "comment_upvote",
                column: "author_id",
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
                name: "FK_member_profile_member_member_id",
                table: "member_profile",
                column: "member_id",
                principalTable: "member",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_post_member_author_id",
                table: "post",
                column: "author_id",
                principalTable: "member",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_post_upvote_member_author_id",
                table: "post_upvote",
                column: "author_id",
                principalTable: "member",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_post_upvote_post_post_id",
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
                name: "FK_comment_member_author_id",
                table: "comment");

            migrationBuilder.DropForeignKey(
                name: "FK_comment_post_post_id",
                table: "comment");

            migrationBuilder.DropForeignKey(
                name: "FK_comment_upvote_comment_comment_id",
                table: "comment_upvote");

            migrationBuilder.DropForeignKey(
                name: "FK_comment_upvote_member_author_id",
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
                name: "FK_member_profile_member_member_id",
                table: "member_profile");

            migrationBuilder.DropForeignKey(
                name: "FK_post_member_author_id",
                table: "post");

            migrationBuilder.DropForeignKey(
                name: "FK_post_upvote_member_author_id",
                table: "post_upvote");

            migrationBuilder.DropForeignKey(
                name: "FK_post_upvote_post_post_id",
                table: "post_upvote");

            migrationBuilder.DropIndex(
                name: "IX_post_upvote_author_id",
                table: "post_upvote");

            migrationBuilder.DropIndex(
                name: "IX_post_upvote_post_id",
                table: "post_upvote");

            migrationBuilder.DropIndex(
                name: "IX_post_author_id",
                table: "post");

            migrationBuilder.DropIndex(
                name: "IX_friendship_friend_id",
                table: "friendship");

            migrationBuilder.DropIndex(
                name: "IX_friend_request_receiver_id",
                table: "friend_request");

            migrationBuilder.DropIndex(
                name: "IX_comment_upvote_author_id",
                table: "comment_upvote");

            migrationBuilder.DropIndex(
                name: "IX_comment_upvote_comment_id",
                table: "comment_upvote");

            migrationBuilder.DropIndex(
                name: "IX_comment_author_id",
                table: "comment");

            migrationBuilder.DropIndex(
                name: "IX_comment_post_id",
                table: "comment");
        }
    }
}
