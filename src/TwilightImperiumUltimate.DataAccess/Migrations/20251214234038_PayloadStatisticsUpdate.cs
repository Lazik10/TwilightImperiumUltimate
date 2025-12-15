using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TwilightImperiumUltimate.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class PayloadStatisticsUpdate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "IsPoK",
                schema: "Statistics",
                table: "AsyncGameStats",
                type: "varchar(5)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(5)")
                .Annotation("Relational:ColumnOrder", 18)
                .OldAnnotation("Relational:ColumnOrder", 17);

            migrationBuilder.AlterColumn<string>(
                name: "Homebrew",
                schema: "Statistics",
                table: "AsyncGameStats",
                type: "varchar(5)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(5)")
                .Annotation("Relational:ColumnOrder", 17)
                .OldAnnotation("Relational:ColumnOrder", 16);

            migrationBuilder.AddColumn<string>(
                name: "AllianceGame",
                schema: "Statistics",
                table: "AsyncGameStats",
                type: "varchar(5)",
                nullable: false,
                defaultValue: "")
                .Annotation("Relational:ColumnOrder", 16);

            migrationBuilder.AddColumn<string>(
                name: "Completed",
                schema: "Statistics",
                table: "AsyncGameStats",
                type: "varchar(10)",
                nullable: false,
                defaultValue: "")
                .Annotation("Relational:ColumnOrder", 21);

            migrationBuilder.AddColumn<long>(
                name: "CreationEpochTimestamp",
                schema: "Statistics",
                table: "AsyncGameStats",
                type: "bigint",
                nullable: false,
                defaultValue: 0L)
                .Annotation("Relational:ColumnOrder", 19);

            migrationBuilder.AddColumn<long>(
                name: "EndedEpochTimestamp",
                schema: "Statistics",
                table: "AsyncGameStats",
                type: "bigint",
                nullable: true)
                .Annotation("Relational:ColumnOrder", 20);

            migrationBuilder.AddColumn<long>(
                name: "Events",
                schema: "Statistics",
                table: "AsyncGameStats",
                type: "bigint",
                nullable: false,
                defaultValue: 0L)
                .Annotation("Relational:ColumnOrder", 22);

            migrationBuilder.UpdateData(
                schema: "Tigl",
                table: "Seasons",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "EndDate", "StartDate" },
                values: new object[] { new DateOnly(2025, 12, 14), new DateOnly(2025, 12, 14) });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AllianceGame",
                schema: "Statistics",
                table: "AsyncGameStats");

            migrationBuilder.DropColumn(
                name: "Completed",
                schema: "Statistics",
                table: "AsyncGameStats");

            migrationBuilder.DropColumn(
                name: "CreationEpochTimestamp",
                schema: "Statistics",
                table: "AsyncGameStats");

            migrationBuilder.DropColumn(
                name: "EndedEpochTimestamp",
                schema: "Statistics",
                table: "AsyncGameStats");

            migrationBuilder.DropColumn(
                name: "Events",
                schema: "Statistics",
                table: "AsyncGameStats");

            migrationBuilder.AlterColumn<string>(
                name: "IsPoK",
                schema: "Statistics",
                table: "AsyncGameStats",
                type: "varchar(5)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(5)")
                .Annotation("Relational:ColumnOrder", 17)
                .OldAnnotation("Relational:ColumnOrder", 18);

            migrationBuilder.AlterColumn<string>(
                name: "Homebrew",
                schema: "Statistics",
                table: "AsyncGameStats",
                type: "varchar(5)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(5)")
                .Annotation("Relational:ColumnOrder", 16)
                .OldAnnotation("Relational:ColumnOrder", 17);

            migrationBuilder.UpdateData(
                schema: "Tigl",
                table: "Seasons",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "EndDate", "StartDate" },
                values: new object[] { new DateOnly(2025, 11, 30), new DateOnly(2025, 11, 30) });
        }
    }
}
