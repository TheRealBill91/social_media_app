using System;
using Microsoft.EntityFrameworkCore.Migrations;
using SocialMediaApp.Data;

#nullable disable

namespace social_media_api.Migrations
{
    /// <inheritdoc />
    public partial class SchemaUpdates : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "created",
                table: "post");

            migrationBuilder.DropColumn(
                name: "modified",
                table: "post");

            migrationBuilder.DropColumn(
                name: "created",
                table: "comment");

            migrationBuilder.RenameColumn(
                name: "timestamp",
                table: "post_upvote",
                newName: "updated_at");

            migrationBuilder.RenameColumn(
                name: "last_active",
                table: "member",
                newName: "updated_at");

            migrationBuilder.RenameColumn(
                name: "time_stamp",
                table: "comment_upvote",
                newName: "updated_at");

            migrationBuilder.AlterDatabase()
                .Annotation("Npgsql:Enum:friend_request_status", "pending,accepted,rejected");

            migrationBuilder.AddColumn<DateTime>(
                name: "created_at",
                table: "post_upvote",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "created_at",
                table: "post",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "deleted_at",
                table: "post",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "updated_at",
                table: "post",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AlterColumn<string>(
                name: "photo_url",
                table: "member_profile",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AddColumn<string>(
                name: "bio",
                table: "member_profile",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "created_at",
                table: "member_profile",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "deleted_at",
                table: "member_profile",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "location",
                table: "member_profile",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "updated_at",
                table: "member_profile",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "url",
                table: "member_profile",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "created_at",
                table: "member",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "deleted_at",
                table: "member",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<FriendRequestStatus>(
                name: "status",
                table: "friend_request",
                type: "friend_request_status",
                nullable: false,
                defaultValue: FriendRequestStatus.Pending);

            migrationBuilder.AddColumn<DateTime>(
                name: "created_at",
                table: "comment_upvote",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "created_at",
                table: "comment",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "deleted_at",
                table: "comment",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "updated_at",
                table: "comment",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.CreateIndex(
                name: "IX_member_email",
                table: "member",
                column: "email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_member_username",
                table: "member",
                column: "username",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_member_email",
                table: "member");

            migrationBuilder.DropIndex(
                name: "IX_member_username",
                table: "member");

            migrationBuilder.DropColumn(
                name: "created_at",
                table: "post_upvote");

            migrationBuilder.DropColumn(
                name: "created_at",
                table: "post");

            migrationBuilder.DropColumn(
                name: "deleted_at",
                table: "post");

            migrationBuilder.DropColumn(
                name: "updated_at",
                table: "post");

            migrationBuilder.DropColumn(
                name: "bio",
                table: "member_profile");

            migrationBuilder.DropColumn(
                name: "created_at",
                table: "member_profile");

            migrationBuilder.DropColumn(
                name: "deleted_at",
                table: "member_profile");

            migrationBuilder.DropColumn(
                name: "location",
                table: "member_profile");

            migrationBuilder.DropColumn(
                name: "updated_at",
                table: "member_profile");

            migrationBuilder.DropColumn(
                name: "url",
                table: "member_profile");

            migrationBuilder.DropColumn(
                name: "created_at",
                table: "member");

            migrationBuilder.DropColumn(
                name: "deleted_at",
                table: "member");

            migrationBuilder.DropColumn(
                name: "status",
                table: "friend_request");

            migrationBuilder.DropColumn(
                name: "created_at",
                table: "comment_upvote");

            migrationBuilder.DropColumn(
                name: "created_at",
                table: "comment");

            migrationBuilder.DropColumn(
                name: "deleted_at",
                table: "comment");

            migrationBuilder.DropColumn(
                name: "updated_at",
                table: "comment");

            migrationBuilder.RenameColumn(
                name: "updated_at",
                table: "post_upvote",
                newName: "timestamp");

            migrationBuilder.RenameColumn(
                name: "updated_at",
                table: "member",
                newName: "last_active");

            migrationBuilder.RenameColumn(
                name: "updated_at",
                table: "comment_upvote",
                newName: "time_stamp");

            migrationBuilder.AlterDatabase()
                .OldAnnotation("Npgsql:Enum:friend_request_status", "pending,accepted,rejected");

            migrationBuilder.AddColumn<DateOnly>(
                name: "created",
                table: "post",
                type: "date",
                nullable: false,
                defaultValue: new DateOnly(1, 1, 1));

            migrationBuilder.AddColumn<DateOnly>(
                name: "modified",
                table: "post",
                type: "date",
                nullable: false,
                defaultValue: new DateOnly(1, 1, 1));

            migrationBuilder.AlterColumn<string>(
                name: "photo_url",
                table: "member_profile",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AddColumn<DateOnly>(
                name: "created",
                table: "comment",
                type: "date",
                nullable: false,
                defaultValue: new DateOnly(1, 1, 1));
        }
    }
}
