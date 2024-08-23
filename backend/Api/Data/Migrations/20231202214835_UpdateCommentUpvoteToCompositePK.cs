using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace social_media_api.Migrations
{
    /// <inheritdoc />
    public partial class UpdateCommentUpvoteToCompositePK : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "pk_comment_upvote",
                table: "comment_upvote"
            );

            migrationBuilder.DropIndex(
                name: "ix_comment_upvote_author_id",
                table: "comment_upvote"
            );

            migrationBuilder.DropColumn(name: "id", table: "comment_upvote");

            migrationBuilder.AddPrimaryKey(
                name: "pk_comment_upvote",
                table: "comment_upvote",
                columns: new[] { "author_id", "comment_id" }
            );
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "pk_comment_upvote",
                table: "comment_upvote"
            );

            migrationBuilder.AddColumn<Guid>(
                name: "id",
                table: "comment_upvote",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000")
            );

            migrationBuilder.AddPrimaryKey(
                name: "pk_comment_upvote",
                table: "comment_upvote",
                column: "id"
            );

            migrationBuilder.CreateIndex(
                name: "ix_comment_upvote_author_id",
                table: "comment_upvote",
                column: "author_id"
            );
        }
    }
}
