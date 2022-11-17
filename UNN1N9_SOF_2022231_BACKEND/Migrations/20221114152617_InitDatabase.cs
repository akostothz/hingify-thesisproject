using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace UNN1N9SOF2022231BACKEND.Migrations
{
    /// <inheritdoc />
    public partial class InitDatabase : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Musics",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    Genre = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    ArtistName = table.Column<string>(type: "text", nullable: false),
                    TrackName = table.Column<string>(type: "text", nullable: false),
                    TrackId = table.Column<string>(type: "text", nullable: false),
                    Popularity = table.Column<int>(type: "integer", nullable: false),
                    Acousticness = table.Column<double>(type: "double precision", nullable: false),
                    Danceability = table.Column<double>(type: "double precision", nullable: false),
                    DurationMs = table.Column<int>(type: "integer", nullable: false),
                    Energy = table.Column<double>(type: "double precision", nullable: false),
                    Key = table.Column<string>(type: "character varying(2)", maxLength: 2, nullable: false),
                    Liveness = table.Column<double>(type: "double precision", nullable: false),
                    Loudness = table.Column<double>(type: "double precision", nullable: false),
                    Mode = table.Column<string>(type: "character varying(5)", maxLength: 5, nullable: false),
                    Speechiness = table.Column<double>(type: "double precision", nullable: false),
                    Tempo = table.Column<int>(type: "integer", nullable: false),
                    Valence = table.Column<double>(type: "double precision", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Musics", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    Email = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    UserName = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Password = table.Column<string>(type: "text", nullable: false),
                    FirstName = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    LastName = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Age = table.Column<int>(type: "integer", nullable: false),
                    Country = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "UserBehaviors",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    UserId = table.Column<int>(type: "integer", nullable: false),
                    MusicId = table.Column<int>(type: "integer", nullable: false),
                    ListeningCount = table.Column<int>(type: "integer", nullable: false),
                    NameOfDay = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false),
                    TimeOfDay = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserBehaviors", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserBehaviors_Musics_MusicId",
                        column: x => x.MusicId,
                        principalTable: "Musics",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_UserBehaviors_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateIndex(
                name: "IX_UserBehaviors_MusicId",
                table: "UserBehaviors",
                column: "MusicId");

            migrationBuilder.CreateIndex(
                name: "IX_UserBehaviors_UserId",
                table: "UserBehaviors",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UserBehaviors");

            migrationBuilder.DropTable(
                name: "Musics");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
