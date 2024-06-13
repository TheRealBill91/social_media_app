using System;
using Microsoft.EntityFrameworkCore.Migrations;
using SocialMediaApp.Data;

#nullable disable

namespace social_media_api.Migrations
{
    /// <inheritdoc />
    public partial class UpdateAllTablesToCamelCase : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Comment_Upvote");

            migrationBuilder.DropTable(
                name: "Friend_Request");

            migrationBuilder.DropTable(
                name: "Member_Profile");

            migrationBuilder.DropTable(
                name: "Post_Upvote");

            migrationBuilder.CreateTable(
                name: "CommentUpvote",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    AuthorId = table.Column<Guid>(type: "uuid", nullable: false),
                    CommentId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CommentUpvote", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CommentUpvote_Comment_CommentId",
                        column: x => x.CommentId,
                        principalTable: "Comment",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CommentUpvote_Members_AuthorId",
                        column: x => x.AuthorId,
                        principalTable: "Members",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "FriendRequest",
                columns: table => new
                {
                    RequesterId = table.Column<Guid>(type: "uuid", nullable: false),
                    ReceiverId = table.Column<Guid>(type: "uuid", nullable: false),
                    Status = table.Column<FriendRequestStatus>(type: "friend_request_status", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FriendRequest", x => new { x.RequesterId, x.ReceiverId });
                    table.ForeignKey(
                        name: "FK_FriendRequest_Members_ReceiverId",
                        column: x => x.ReceiverId,
                        principalTable: "Members",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FriendRequest_Members_RequesterId",
                        column: x => x.RequesterId,
                        principalTable: "Members",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MemberProfile",
                columns: table => new
                {
                    MemberId = table.Column<Guid>(type: "uuid", nullable: false),
                    PhotoURL = table.Column<string>(type: "text", nullable: true),
                    Bio = table.Column<string>(type: "text", nullable: true),
                    Location = table.Column<string>(type: "text", nullable: true),
                    URL = table.Column<string>(type: "text", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MemberProfile", x => x.MemberId);
                    table.ForeignKey(
                        name: "FK_MemberProfile_Members_MemberId",
                        column: x => x.MemberId,
                        principalTable: "Members",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PostUpvote",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    AuthorId = table.Column<Guid>(type: "uuid", nullable: false),
                    PostId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PostUpvote", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PostUpvote_Members_AuthorId",
                        column: x => x.AuthorId,
                        principalTable: "Members",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PostUpvote_Post_PostId",
                        column: x => x.PostId,
                        principalTable: "Post",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CommentUpvote_AuthorId",
                table: "CommentUpvote",
                column: "AuthorId");

            migrationBuilder.CreateIndex(
                name: "IX_CommentUpvote_CommentId",
                table: "CommentUpvote",
                column: "CommentId");

            migrationBuilder.CreateIndex(
                name: "IX_FriendRequest_ReceiverId",
                table: "FriendRequest",
                column: "ReceiverId");

            migrationBuilder.CreateIndex(
                name: "IX_PostUpvote_AuthorId",
                table: "PostUpvote",
                column: "AuthorId");

            migrationBuilder.CreateIndex(
                name: "IX_PostUpvote_PostId",
                table: "PostUpvote",
                column: "PostId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CommentUpvote");

            migrationBuilder.DropTable(
                name: "FriendRequest");

            migrationBuilder.DropTable(
                name: "MemberProfile");

            migrationBuilder.DropTable(
                name: "PostUpvote");

            migrationBuilder.CreateTable(
                name: "Comment_Upvote",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    AuthorId = table.Column<Guid>(type: "uuid", nullable: false),
                    CommentId = table.Column<Guid>(type: "uuid", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Comment_Upvote", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Comment_Upvote_Comment_CommentId",
                        column: x => x.CommentId,
                        principalTable: "Comment",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Comment_Upvote_Members_AuthorId",
                        column: x => x.AuthorId,
                        principalTable: "Members",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Friend_Request",
                columns: table => new
                {
                    RequesterId = table.Column<Guid>(type: "uuid", nullable: false),
                    ReceiverId = table.Column<Guid>(type: "uuid", nullable: false),
                    Status = table.Column<FriendRequestStatus>(type: "friend_request_status", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Friend_Request", x => new { x.RequesterId, x.ReceiverId });
                    table.ForeignKey(
                        name: "FK_Friend_Request_Members_ReceiverId",
                        column: x => x.ReceiverId,
                        principalTable: "Members",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Friend_Request_Members_RequesterId",
                        column: x => x.RequesterId,
                        principalTable: "Members",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Member_Profile",
                columns: table => new
                {
                    MemberId = table.Column<Guid>(type: "uuid", nullable: false),
                    Bio = table.Column<string>(type: "text", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    Location = table.Column<string>(type: "text", nullable: true),
                    PhotoURL = table.Column<string>(type: "text", nullable: true),
                    URL = table.Column<string>(type: "text", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Member_Profile", x => x.MemberId);
                    table.ForeignKey(
                        name: "FK_Member_Profile_Members_MemberId",
                        column: x => x.MemberId,
                        principalTable: "Members",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Post_Upvote",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    AuthorId = table.Column<Guid>(type: "uuid", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    PostId = table.Column<Guid>(type: "uuid", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Post_Upvote", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Post_Upvote_Members_AuthorId",
                        column: x => x.AuthorId,
                        principalTable: "Members",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Post_Upvote_Post_PostId",
                        column: x => x.PostId,
                        principalTable: "Post",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Comment_Upvote_AuthorId",
                table: "Comment_Upvote",
                column: "AuthorId");

            migrationBuilder.CreateIndex(
                name: "IX_Comment_Upvote_CommentId",
                table: "Comment_Upvote",
                column: "CommentId");

            migrationBuilder.CreateIndex(
                name: "IX_Friend_Request_ReceiverId",
                table: "Friend_Request",
                column: "ReceiverId");

            migrationBuilder.CreateIndex(
                name: "IX_Post_Upvote_AuthorId",
                table: "Post_Upvote",
                column: "AuthorId");

            migrationBuilder.CreateIndex(
                name: "IX_Post_Upvote_PostId",
                table: "Post_Upvote",
                column: "PostId");
        }
    }
}
