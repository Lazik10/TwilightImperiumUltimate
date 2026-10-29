using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TwilightImperiumUltimate.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class AddFactionPickSelections : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "LockedBanSeatIds",
                schema: "Draft",
                table: "FactionPickStates",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "[]");

            migrationBuilder.CreateTable(
                name: "FactionPickSelections",
                schema: "Draft",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    RoomId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SeatId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Kind = table.Column<string>(type: "varchar(16)", unicode: false, maxLength: 16, nullable: false),
                    Faction = table.Column<string>(type: "varchar(128)", unicode: false, maxLength: 128, nullable: false),
                    State = table.Column<string>(type: "varchar(16)", unicode: false, maxLength: 16, nullable: false),
                    Round = table.Column<int>(type: "int", nullable: false),
                    DecidedByParticipantId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CreatedAtUtc = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LockedAtUtc = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FactionPickSelections", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FactionPickSelections_DraftRoomSeats_RoomId_SeatId",
                        columns: x => new { x.RoomId, x.SeatId },
                        principalSchema: "Draft",
                        principalTable: "DraftRoomSeats",
                        principalColumns: new[] { "RoomId", "Id" });
                    table.ForeignKey(
                        name: "FK_FactionPickSelections_DraftRooms_RoomId",
                        column: x => x.RoomId,
                        principalSchema: "Draft",
                        principalTable: "DraftRooms",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_FactionPickSelections_RoomId_SeatId_Kind",
                schema: "Draft",
                table: "FactionPickSelections",
                columns: new[] { "RoomId", "SeatId", "Kind" });

            migrationBuilder.CreateIndex(
                name: "IX_FactionPickSelections_RoomId_SeatId_Kind_Faction",
                schema: "Draft",
                table: "FactionPickSelections",
                columns: new[] { "RoomId", "SeatId", "Kind", "Faction" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FactionPickSelections",
                schema: "Draft");

            migrationBuilder.DropColumn(
                name: "LockedBanSeatIds",
                schema: "Draft",
                table: "FactionPickStates");
        }
    }
}
