using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UNN1N9SOF2022231BACKEND.Migrations
{
    /// <inheritdoc />
    public partial class tableadded : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserBehaviors_Musics_MusicId",
                table: "UserBehaviors");

            migrationBuilder.DropForeignKey(
                name: "FK_UserBehaviors_Users_UserId",
                table: "UserBehaviors");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UserBehaviors",
                table: "UserBehaviors");

            migrationBuilder.RenameTable(
                name: "UserBehaviors",
                newName: "UserBehavior");

            migrationBuilder.RenameIndex(
                name: "IX_UserBehaviors_UserId",
                table: "UserBehavior",
                newName: "IX_UserBehavior_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_UserBehaviors_MusicId",
                table: "UserBehavior",
                newName: "IX_UserBehavior_MusicId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserBehavior",
                table: "UserBehavior",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_UserBehavior_Musics_MusicId",
                table: "UserBehavior",
                column: "MusicId",
                principalTable: "Musics",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserBehavior_Users_UserId",
                table: "UserBehavior",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserBehavior_Musics_MusicId",
                table: "UserBehavior");

            migrationBuilder.DropForeignKey(
                name: "FK_UserBehavior_Users_UserId",
                table: "UserBehavior");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UserBehavior",
                table: "UserBehavior");

            migrationBuilder.RenameTable(
                name: "UserBehavior",
                newName: "UserBehaviors");

            migrationBuilder.RenameIndex(
                name: "IX_UserBehavior_UserId",
                table: "UserBehaviors",
                newName: "IX_UserBehaviors_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_UserBehavior_MusicId",
                table: "UserBehaviors",
                newName: "IX_UserBehaviors_MusicId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserBehaviors",
                table: "UserBehaviors",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_UserBehaviors_Musics_MusicId",
                table: "UserBehaviors",
                column: "MusicId",
                principalTable: "Musics",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserBehaviors_Users_UserId",
                table: "UserBehaviors",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
