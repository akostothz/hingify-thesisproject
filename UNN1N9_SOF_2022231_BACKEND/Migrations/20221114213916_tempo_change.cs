using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UNN1N9SOF2022231BACKEND.Migrations
{
    /// <inheritdoc />
    public partial class tempochange : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<double>(
                name: "Tempo",
                table: "Musics",
                type: "double precision",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "Tempo",
                table: "Musics",
                type: "integer",
                nullable: false,
                oldClrType: typeof(double),
                oldType: "double precision");
        }
    }
}
