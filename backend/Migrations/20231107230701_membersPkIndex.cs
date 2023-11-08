using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace social_media_api.Migrations
{
    /// <inheritdoc />
    public partial class membersPkIndex : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_member_id",
                table: "member",
                column: "id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_member_id",
                table: "member");
        }
    }
}
