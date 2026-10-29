using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TwilightImperiumUltimate.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class ExpandDraftRoomDisplayNameColumnsForSuffixes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "DisplayNameKey",
                schema: "Draft",
                table: "DraftRoomParticipants",
                type: "nvarchar(258)",
                maxLength: 258,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(256)",
                oldMaxLength: 256);

            migrationBuilder.AlterColumn<string>(
                name: "DisplayName",
                schema: "Draft",
                table: "DraftRoomParticipants",
                type: "nvarchar(258)",
                maxLength: 258,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(256)",
                oldMaxLength: 256);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "DisplayNameKey",
                schema: "Draft",
                table: "DraftRoomParticipants",
                type: "nvarchar(256)",
                maxLength: 256,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(258)",
                oldMaxLength: 258);

            migrationBuilder.AlterColumn<string>(
                name: "DisplayName",
                schema: "Draft",
                table: "DraftRoomParticipants",
                type: "nvarchar(256)",
                maxLength: 256,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(258)",
                oldMaxLength: 258);
        }
    }
}
