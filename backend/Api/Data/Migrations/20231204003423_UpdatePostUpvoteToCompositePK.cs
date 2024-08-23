using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace social_media_api.Migrations
{
    /// <inheritdoc />
    public partial class UpdatePostUpvoteToCompositePK : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "pk_post_upvote",
                table: "post_upvote"
            );

            migrationBuilder.DropIndex(
                name: "ix_post_upvote_author_id",
                table: "post_upvote"
            );

            migrationBuilder.DropColumn(name: "id", table: "post_upvote");

            migrationBuilder.AddPrimaryKey(
                name: "pk_post_upvote",
                table: "post_upvote",
                columns: new[] { "author_id", "post_id" }
            );
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "pk_post_upvote",
                table: "post_upvote"
            );

            migrationBuilder.AddColumn<Guid>(
                name: "id",
                table: "post_upvote",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000")
            );

            migrationBuilder.AddPrimaryKey(
                name: "pk_post_upvote",
                table: "post_upvote",
                column: "id"
            );

            migrationBuilder.CreateIndex(
                name: "ix_post_upvote_author_id",
                table: "post_upvote",
                column: "author_id"
            );
        }
    }
}
