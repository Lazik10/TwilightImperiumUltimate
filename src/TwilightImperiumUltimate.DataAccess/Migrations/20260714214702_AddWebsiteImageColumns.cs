using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TwilightImperiumUltimate.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class AddWebsiteImageColumns : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ImageContentType",
                schema: "Website",
                table: "Websites",
                type: "varchar(100)",
                maxLength: 100,
                nullable: true)
                .Annotation("Relational:ColumnOrder", 5);

            migrationBuilder.AddColumn<byte[]>(
                name: "ImageData",
                schema: "Website",
                table: "Websites",
                type: "varbinary(max)",
                nullable: true)
                .Annotation("Relational:ColumnOrder", 4);

            migrationBuilder.UpdateData(
                schema: "Website",
                table: "Websites",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "ImageContentType", "ImageData" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                schema: "Website",
                table: "Websites",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "ImageContentType", "ImageData" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                schema: "Website",
                table: "Websites",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "ImageContentType", "ImageData" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                schema: "Website",
                table: "Websites",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "ImageContentType", "ImageData" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                schema: "Website",
                table: "Websites",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "ImageContentType", "ImageData" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                schema: "Website",
                table: "Websites",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "ImageContentType", "ImageData" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                schema: "Website",
                table: "Websites",
                keyColumn: "Id",
                keyValue: 7,
                columns: new[] { "ImageContentType", "ImageData" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                schema: "Website",
                table: "Websites",
                keyColumn: "Id",
                keyValue: 8,
                columns: new[] { "ImageContentType", "ImageData" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                schema: "Website",
                table: "Websites",
                keyColumn: "Id",
                keyValue: 9,
                columns: new[] { "ImageContentType", "ImageData" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                schema: "Website",
                table: "Websites",
                keyColumn: "Id",
                keyValue: 10,
                columns: new[] { "ImageContentType", "ImageData" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                schema: "Website",
                table: "Websites",
                keyColumn: "Id",
                keyValue: 11,
                columns: new[] { "ImageContentType", "ImageData" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                schema: "Website",
                table: "Websites",
                keyColumn: "Id",
                keyValue: 12,
                columns: new[] { "ImageContentType", "ImageData" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                schema: "Website",
                table: "Websites",
                keyColumn: "Id",
                keyValue: 13,
                columns: new[] { "ImageContentType", "ImageData" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                schema: "Website",
                table: "Websites",
                keyColumn: "Id",
                keyValue: 14,
                columns: new[] { "ImageContentType", "ImageData" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                schema: "Website",
                table: "Websites",
                keyColumn: "Id",
                keyValue: 15,
                columns: new[] { "ImageContentType", "ImageData" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                schema: "Website",
                table: "Websites",
                keyColumn: "Id",
                keyValue: 16,
                columns: new[] { "ImageContentType", "ImageData" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                schema: "Website",
                table: "Websites",
                keyColumn: "Id",
                keyValue: 17,
                columns: new[] { "ImageContentType", "ImageData" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                schema: "Website",
                table: "Websites",
                keyColumn: "Id",
                keyValue: 18,
                columns: new[] { "ImageContentType", "ImageData" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                schema: "Website",
                table: "Websites",
                keyColumn: "Id",
                keyValue: 19,
                columns: new[] { "ImageContentType", "ImageData" },
                values: new object[] { null, null });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ImageContentType",
                schema: "Website",
                table: "Websites");

            migrationBuilder.DropColumn(
                name: "ImageData",
                schema: "Website",
                table: "Websites");
        }
    }
}
