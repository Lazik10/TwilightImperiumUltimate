using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TwilightImperiumUltimate.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class FixRoleConcurrencyStamp : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "2147411d-19b7-4936-800a-b8d815271d00",
                column: "ConcurrencyStamp",
                value: "b1a1e6b0-1f2a-4b3c-9d4e-5f6a7b8c9d01");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "5b2bee5c-e5ce-4472-a141-bff7e040ac78",
                column: "ConcurrencyStamp",
                value: "b1a1e6b0-1f2a-4b3c-9d4e-5f6a7b8c9d03");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "cc4089b0-22e9-47df-b7c5-a4734b4423f4",
                column: "ConcurrencyStamp",
                value: "b1a1e6b0-1f2a-4b3c-9d4e-5f6a7b8c9d02");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "d3f1c4e2-3f4a-4e2b-8f4e-2c3b5e6d7f89",
                column: "ConcurrencyStamp",
                value: "b1a1e6b0-1f2a-4b3c-9d4e-5f6a7b8c9d04");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "2147411d-19b7-4936-800a-b8d815271d00",
                column: "ConcurrencyStamp",
                value: null);

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "5b2bee5c-e5ce-4472-a141-bff7e040ac78",
                column: "ConcurrencyStamp",
                value: null);

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "cc4089b0-22e9-47df-b7c5-a4734b4423f4",
                column: "ConcurrencyStamp",
                value: null);

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "d3f1c4e2-3f4a-4e2b-8f4e-2c3b5e6d7f89",
                column: "ConcurrencyStamp",
                value: null);
        }
    }
}
