using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace social_media_api.Migrations
{
    /// <inheritdoc />
    public partial class addMembersModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "email",
                table: "AspNetUsers",
                newName: "Email"
            );

            migrationBuilder.RenameColumn(
                name: "id",
                table: "AspNetUsers",
                newName: "Id"
            );

            migrationBuilder.RenameColumn(
                name: "user_name",
                table: "AspNetUsers",
                newName: "UserName"
            );

            migrationBuilder.RenameIndex(
                name: "IX_AspNetUsers_id",
                table: "AspNetUsers",
                newName: "IX_AspNetUsers_Id"
            );

            migrationBuilder.RenameIndex(
                name: "IX_AspNetUsers_email",
                table: "AspNetUsers",
                newName: "IX_AspNetUsers_Email"
            );

            migrationBuilder.RenameIndex(
                name: "IX_AspNetUsers_user_name",
                table: "AspNetUsers",
                newName: "IX_AspNetUsers_UserName"
            );
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Email",
                table: "AspNetUsers",
                newName: "email"
            );

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "AspNetUsers",
                newName: "id"
            );

            migrationBuilder.RenameColumn(
                name: "UserName",
                table: "AspNetUsers",
                newName: "user_name"
            );

            migrationBuilder.RenameIndex(
                name: "IX_AspNetUsers_Id",
                table: "AspNetUsers",
                newName: "IX_AspNetUsers_id"
            );

            migrationBuilder.RenameIndex(
                name: "IX_AspNetUsers_Email",
                table: "AspNetUsers",
                newName: "IX_AspNetUsers_email"
            );

            migrationBuilder.RenameIndex(
                name: "IX_AspNetUsers_UserName",
                table: "AspNetUsers",
                newName: "IX_AspNetUsers_user_name"
            );
        }
    }
}
