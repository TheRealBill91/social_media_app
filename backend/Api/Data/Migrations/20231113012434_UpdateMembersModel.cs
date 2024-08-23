using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace social_media_api.Migrations
{
    /// <inheritdoc />
    public partial class UpdateMembersModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_asp_net_user_claims_members_user_id",
                table: "asp_net_user_claims"
            );

            migrationBuilder.DropForeignKey(
                name: "fk_asp_net_user_logins_members_user_id",
                table: "asp_net_user_logins"
            );

            migrationBuilder.DropForeignKey(
                name: "fk_asp_net_user_roles_members_user_id",
                table: "asp_net_user_roles"
            );

            migrationBuilder.DropForeignKey(
                name: "fk_asp_net_user_tokens_members_user_id",
                table: "asp_net_user_tokens"
            );

            migrationBuilder.DropForeignKey(
                name: "fk_comment_members_members_id",
                table: "comment"
            );

            migrationBuilder.DropForeignKey(
                name: "fk_comment_upvote_members_members_id",
                table: "comment_upvote"
            );

            migrationBuilder.DropForeignKey(
                name: "fk_friend_request_members_members_id",
                table: "friend_request"
            );

            migrationBuilder.DropForeignKey(
                name: "fk_friend_request_members_members_id1",
                table: "friend_request"
            );

            migrationBuilder.DropForeignKey(
                name: "fk_friendship_members_members_id",
                table: "friendship"
            );

            migrationBuilder.DropForeignKey(
                name: "fk_friendship_members_members_id1",
                table: "friendship"
            );

            migrationBuilder.DropForeignKey(
                name: "fk_member_profile_members_members_id",
                table: "member_profile"
            );

            migrationBuilder.DropForeignKey(
                name: "fk_post_members_members_id",
                table: "post"
            );

            migrationBuilder.DropForeignKey(
                name: "fk_post_upvote_members_members_id",
                table: "post_upvote"
            );

            migrationBuilder.DropPrimaryKey(
                name: "pk_members",
                table: "members"
            );

            migrationBuilder.RenameTable(name: "members", newName: "member");

            migrationBuilder.RenameIndex(
                name: "ix_members_user_name",
                table: "member",
                newName: "ix_member_user_name"
            );

            migrationBuilder.RenameIndex(
                name: "ix_members_id",
                table: "member",
                newName: "ix_member_id"
            );

            migrationBuilder.RenameIndex(
                name: "ix_members_email",
                table: "member",
                newName: "ix_member_email"
            );

            migrationBuilder.AddColumn<int>(
                name: "email_confirmation_sent_count",
                table: "member",
                type: "integer",
                nullable: false,
                defaultValue: 0
            );

            migrationBuilder.AddColumn<DateTime>(
                name: "last_email_confirmation_sent_date",
                table: "member",
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

            migrationBuilder.AddPrimaryKey(
                name: "pk_member",
                table: "member",
                column: "id"
            );

            migrationBuilder.AddForeignKey(
                name: "fk_asp_net_user_claims_member_user_id",
                table: "asp_net_user_claims",
                column: "user_id",
                principalTable: "member",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade
            );

            migrationBuilder.AddForeignKey(
                name: "fk_asp_net_user_logins_member_user_id",
                table: "asp_net_user_logins",
                column: "user_id",
                principalTable: "member",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade
            );

            migrationBuilder.AddForeignKey(
                name: "fk_asp_net_user_roles_member_user_id",
                table: "asp_net_user_roles",
                column: "user_id",
                principalTable: "member",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade
            );

            migrationBuilder.AddForeignKey(
                name: "fk_asp_net_user_tokens_member_user_id",
                table: "asp_net_user_tokens",
                column: "user_id",
                principalTable: "member",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade
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
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_asp_net_user_claims_member_user_id",
                table: "asp_net_user_claims"
            );

            migrationBuilder.DropForeignKey(
                name: "fk_asp_net_user_logins_member_user_id",
                table: "asp_net_user_logins"
            );

            migrationBuilder.DropForeignKey(
                name: "fk_asp_net_user_roles_member_user_id",
                table: "asp_net_user_roles"
            );

            migrationBuilder.DropForeignKey(
                name: "fk_asp_net_user_tokens_member_user_id",
                table: "asp_net_user_tokens"
            );

            migrationBuilder.DropForeignKey(
                name: "fk_comment_member_members_id",
                table: "comment"
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

            migrationBuilder.DropPrimaryKey(name: "pk_member", table: "member");

            migrationBuilder.DropColumn(
                name: "email_confirmation_sent_count",
                table: "member"
            );

            migrationBuilder.DropColumn(
                name: "last_email_confirmation_sent_date",
                table: "member"
            );

            migrationBuilder.RenameTable(name: "member", newName: "members");

            migrationBuilder.RenameIndex(
                name: "ix_member_user_name",
                table: "members",
                newName: "ix_members_user_name"
            );

            migrationBuilder.RenameIndex(
                name: "ix_member_id",
                table: "members",
                newName: "ix_members_id"
            );

            migrationBuilder.RenameIndex(
                name: "ix_member_email",
                table: "members",
                newName: "ix_members_email"
            );

            migrationBuilder.AddPrimaryKey(
                name: "pk_members",
                table: "members",
                column: "id"
            );

            migrationBuilder.AddForeignKey(
                name: "fk_asp_net_user_claims_members_user_id",
                table: "asp_net_user_claims",
                column: "user_id",
                principalTable: "members",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade
            );

            migrationBuilder.AddForeignKey(
                name: "fk_asp_net_user_logins_members_user_id",
                table: "asp_net_user_logins",
                column: "user_id",
                principalTable: "members",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade
            );

            migrationBuilder.AddForeignKey(
                name: "fk_asp_net_user_roles_members_user_id",
                table: "asp_net_user_roles",
                column: "user_id",
                principalTable: "members",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade
            );

            migrationBuilder.AddForeignKey(
                name: "fk_asp_net_user_tokens_members_user_id",
                table: "asp_net_user_tokens",
                column: "user_id",
                principalTable: "members",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade
            );

            migrationBuilder.AddForeignKey(
                name: "fk_comment_members_members_id",
                table: "comment",
                column: "author_id",
                principalTable: "members",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade
            );

            migrationBuilder.AddForeignKey(
                name: "fk_comment_upvote_members_members_id",
                table: "comment_upvote",
                column: "author_id",
                principalTable: "members",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade
            );

            migrationBuilder.AddForeignKey(
                name: "fk_friend_request_members_members_id",
                table: "friend_request",
                column: "receiver_id",
                principalTable: "members",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade
            );

            migrationBuilder.AddForeignKey(
                name: "fk_friend_request_members_members_id1",
                table: "friend_request",
                column: "requester_id",
                principalTable: "members",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade
            );

            migrationBuilder.AddForeignKey(
                name: "fk_friendship_members_members_id",
                table: "friendship",
                column: "friend_id",
                principalTable: "members",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade
            );

            migrationBuilder.AddForeignKey(
                name: "fk_friendship_members_members_id1",
                table: "friendship",
                column: "member_id",
                principalTable: "members",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade
            );

            migrationBuilder.AddForeignKey(
                name: "fk_member_profile_members_members_id",
                table: "member_profile",
                column: "member_id",
                principalTable: "members",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade
            );

            migrationBuilder.AddForeignKey(
                name: "fk_post_members_members_id",
                table: "post",
                column: "author_id",
                principalTable: "members",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade
            );

            migrationBuilder.AddForeignKey(
                name: "fk_post_upvote_members_members_id",
                table: "post_upvote",
                column: "author_id",
                principalTable: "members",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade
            );
        }
    }
}
