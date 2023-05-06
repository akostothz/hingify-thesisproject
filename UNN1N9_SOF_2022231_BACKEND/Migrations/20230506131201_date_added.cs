using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UNN1N9SOF2022231BACKEND.Migrations
{
    /// <inheritdoc />
    public partial class dateadded : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateOnly>(
                name: "Date",
                table: "UserBehaviors",
                type: "date",
                nullable: false,
                defaultValue: new DateOnly(1, 1, 1));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Date",
                table: "UserBehaviors");
        }
    }
}
