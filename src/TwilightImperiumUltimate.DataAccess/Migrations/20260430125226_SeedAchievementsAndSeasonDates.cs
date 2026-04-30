using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace TwilightImperiumUltimate.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class SeedAchievementsAndSeasonDates : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                schema: "Tigl",
                table: "Achievements",
                columns: new[] { "Id", "Category", "Faction", "Name" },
                values: new object[,]
                {
                    { 42, "Wins", "None", "Mythical" },
                    { 43, "Wins", "None", "Icarus" }
                });

            migrationBuilder.UpdateData(
                schema: "Tigl",
                table: "Seasons",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "EndDate", "StartDate" },
                values: new object[] { new DateOnly(2025, 12, 1), new DateOnly(2025, 12, 1) });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                schema: "Tigl",
                table: "Achievements",
                keyColumn: "Id",
                keyValue: 42);

            migrationBuilder.DeleteData(
                schema: "Tigl",
                table: "Achievements",
                keyColumn: "Id",
                keyValue: 43);

            migrationBuilder.UpdateData(
                schema: "Tigl",
                table: "Seasons",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "EndDate", "StartDate" },
                values: new object[] { new DateOnly(2026, 2, 23), new DateOnly(2026, 2, 23) });
        }
    }
}
