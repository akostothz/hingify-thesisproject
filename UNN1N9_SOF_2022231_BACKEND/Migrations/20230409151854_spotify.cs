using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UNN1N9SOF2022231BACKEND.Migrations
{
    /// <inheritdoc />
    public partial class spotify : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "SpotifyAccessToken",
                table: "Users",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "SpotifyId",
                table: "Users",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SpotifyAccessToken",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "SpotifyId",
                table: "Users");
        }
    }
}
