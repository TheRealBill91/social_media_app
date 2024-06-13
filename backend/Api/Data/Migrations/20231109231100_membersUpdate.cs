using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace social_media_api.Migrations
{
    /// <inheritdoc />
    public partial class membersUpdate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUserClaims_AspNetUsers_UserId",
                table: "AspNetUserClaims");

            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUserLogins_AspNetUsers_UserId",
                table: "AspNetUserLogins");

            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUserRoles_AspNetUsers_UserId",
                table: "AspNetUserRoles");

            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUserTokens_AspNetUsers_UserId",
                table: "AspNetUserTokens");

            migrationBuilder.DropForeignKey(
                name: "FK_Comment_AspNetUsers_AuthorId",
                table: "Comment");

            migrationBuilder.DropForeignKey(
                name: "FK_Comment_Upvote_AspNetUsers_AuthorId",
                table: "Comment_Upvote");

            migrationBuilder.DropForeignKey(
                name: "FK_Friend_Request_AspNetUsers_ReceiverId",
                table: "Friend_Request");

            migrationBuilder.DropForeignKey(
                name: "FK_Friend_Request_AspNetUsers_RequesterId",
                table: "Friend_Request");

            migrationBuilder.DropForeignKey(
                name: "FK_Friendship_AspNetUsers_FriendId",
                table: "Friendship");

            migrationBuilder.DropForeignKey(
                name: "FK_Friendship_AspNetUsers_MemberId",
                table: "Friendship");

            migrationBuilder.DropForeignKey(
                name: "FK_Member_Profile_AspNetUsers_MemberId",
                table: "Member_Profile");

            migrationBuilder.DropForeignKey(
                name: "FK_Post_AspNetUsers_AuthorId",
                table: "Post");

            migrationBuilder.DropForeignKey(
                name: "FK_Post_Upvote_AspNetUsers_AuthorId",
                table: "Post_Upvote");

            migrationBuilder.DropPrimaryKey(
                name: "PK_AspNetUsers",
                table: "AspNetUsers");

            migrationBuilder.RenameTable(
                name: "AspNetUsers",
                newName: "Members");

            migrationBuilder.RenameIndex(
                name: "IX_AspNetUsers_UserName",
                table: "Members",
                newName: "IX_Members_UserName");

            migrationBuilder.RenameIndex(
                name: "IX_AspNetUsers_Id",
                table: "Members",
                newName: "IX_Members_Id");

            migrationBuilder.RenameIndex(
                name: "IX_AspNetUsers_Email",
                table: "Members",
                newName: "IX_Members_Email");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Members",
                table: "Members",
                column: "Id");

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
                name: "FK_Comment_Upvote_Members_AuthorId",
                table: "Comment_Upvote",
                column: "AuthorId",
                principalTable: "Members",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Friend_Request_Members_ReceiverId",
                table: "Friend_Request",
                column: "ReceiverId",
                principalTable: "Members",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Friend_Request_Members_RequesterId",
                table: "Friend_Request",
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
                name: "FK_Member_Profile_Members_MemberId",
                table: "Member_Profile",
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
                name: "FK_Post_Upvote_Members_AuthorId",
                table: "Post_Upvote",
                column: "AuthorId",
                principalTable: "Members",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUserClaims_Members_UserId",
                table: "AspNetUserClaims");

            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUserLogins_Members_UserId",
                table: "AspNetUserLogins");

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
                name: "FK_Comment_Upvote_Members_AuthorId",
                table: "Comment_Upvote");

            migrationBuilder.DropForeignKey(
                name: "FK_Friend_Request_Members_ReceiverId",
                table: "Friend_Request");

            migrationBuilder.DropForeignKey(
                name: "FK_Friend_Request_Members_RequesterId",
                table: "Friend_Request");

            migrationBuilder.DropForeignKey(
                name: "FK_Friendship_Members_FriendId",
                table: "Friendship");

            migrationBuilder.DropForeignKey(
                name: "FK_Friendship_Members_MemberId",
                table: "Friendship");

            migrationBuilder.DropForeignKey(
                name: "FK_Member_Profile_Members_MemberId",
                table: "Member_Profile");

            migrationBuilder.DropForeignKey(
                name: "FK_Post_Members_AuthorId",
                table: "Post");

            migrationBuilder.DropForeignKey(
                name: "FK_Post_Upvote_Members_AuthorId",
                table: "Post_Upvote");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Members",
                table: "Members");

            migrationBuilder.RenameTable(
                name: "Members",
                newName: "AspNetUsers");

            migrationBuilder.RenameIndex(
                name: "IX_Members_UserName",
                table: "AspNetUsers",
                newName: "IX_AspNetUsers_UserName");

            migrationBuilder.RenameIndex(
                name: "IX_Members_Id",
                table: "AspNetUsers",
                newName: "IX_AspNetUsers_Id");

            migrationBuilder.RenameIndex(
                name: "IX_Members_Email",
                table: "AspNetUsers",
                newName: "IX_AspNetUsers_Email");

            migrationBuilder.AddPrimaryKey(
                name: "PK_AspNetUsers",
                table: "AspNetUsers",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUserClaims_AspNetUsers_UserId",
                table: "AspNetUserClaims",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUserLogins_AspNetUsers_UserId",
                table: "AspNetUserLogins",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUserRoles_AspNetUsers_UserId",
                table: "AspNetUserRoles",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUserTokens_AspNetUsers_UserId",
                table: "AspNetUserTokens",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Comment_AspNetUsers_AuthorId",
                table: "Comment",
                column: "AuthorId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Comment_Upvote_AspNetUsers_AuthorId",
                table: "Comment_Upvote",
                column: "AuthorId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Friend_Request_AspNetUsers_ReceiverId",
                table: "Friend_Request",
                column: "ReceiverId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Friend_Request_AspNetUsers_RequesterId",
                table: "Friend_Request",
                column: "RequesterId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Friendship_AspNetUsers_FriendId",
                table: "Friendship",
                column: "FriendId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Friendship_AspNetUsers_MemberId",
                table: "Friendship",
                column: "MemberId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Member_Profile_AspNetUsers_MemberId",
                table: "Member_Profile",
                column: "MemberId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Post_AspNetUsers_AuthorId",
                table: "Post",
                column: "AuthorId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Post_Upvote_AspNetUsers_AuthorId",
                table: "Post_Upvote",
                column: "AuthorId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
