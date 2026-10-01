using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TwilightImperiumUltimate.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class AsyncStatisticsSnapshot : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AsyncStatisticsSnapshots",
                schema: "Statistics",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    GeneratedAtUtc = table.Column<DateTime>(type: "datetime2", nullable: false),
                    SnapshotVersion = table.Column<long>(type: "bigint", nullable: false),
                    SourceDataVersion = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: true),
                    IsPublished = table.Column<bool>(type: "bit", nullable: false),
                    Payload = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AsyncStatisticsSnapshots", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AsyncStatisticsSnapshots_IsPublished",
                schema: "Statistics",
                table: "AsyncStatisticsSnapshots",
                column: "IsPublished",
                unique: true,
                filter: "[IsPublished] = 1");

            migrationBuilder.CreateIndex(
                name: "IX_AsyncStatisticsSnapshots_IsPublished_SnapshotVersion",
                schema: "Statistics",
                table: "AsyncStatisticsSnapshots",
                columns: new[] { "IsPublished", "SnapshotVersion" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AsyncStatisticsSnapshots",
                schema: "Statistics");
        }
    }
}
