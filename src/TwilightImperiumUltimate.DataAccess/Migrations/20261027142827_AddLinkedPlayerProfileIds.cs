using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TwilightImperiumUltimate.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class AddLinkedPlayerProfileIds : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "AsyncPlayerProfileId",
                table: "AspNetUsers",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "TiglPlayerProfileId",
                table: "AspNetUsers",
                type: "int",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "1",
                columns: new[] { "AsyncPlayerProfileId", "TiglPlayerProfileId" },
                values: new object[] { null, null });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AsyncPlayerProfileId",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "TiglPlayerProfileId",
                table: "AspNetUsers");
        }
    }
}
