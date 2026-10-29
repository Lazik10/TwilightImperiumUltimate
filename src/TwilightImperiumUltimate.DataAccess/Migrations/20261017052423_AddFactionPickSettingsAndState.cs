using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TwilightImperiumUltimate.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class AddFactionPickSettingsAndState : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddUniqueConstraint(
                name: "AK_DraftRoomSeats_RoomId_Id",
                schema: "Draft",
                table: "DraftRoomSeats",
                columns: new[] { "RoomId", "Id" });

            migrationBuilder.CreateTable(
                name: "FactionPickSettings",
                schema: "Draft",
                columns: table => new
                {
                    RoomId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    GameVersions = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    BansMode = table.Column<string>(type: "varchar(32)", unicode: false, maxLength: 32, nullable: false),
                    BansPerPlayer = table.Column<int>(type: "int", nullable: false),
                    HostBanCount = table.Column<int>(type: "int", nullable: false),
                    PickOrderMode = table.Column<string>(type: "varchar(16)", unicode: false, maxLength: 16, nullable: false),
                    PicksPerPlayer = table.Column<int>(type: "int", nullable: false),
                    Snake = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FactionPickSettings", x => x.RoomId);
                    table.ForeignKey(
                        name: "FK_FactionPickSettings_DraftRooms_RoomId",
                        column: x => x.RoomId,
                        principalSchema: "Draft",
                        principalTable: "DraftRooms",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "FactionPickStates",
                schema: "Draft",
                columns: table => new
                {
                    RoomId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Phase = table.Column<string>(type: "varchar(16)", unicode: false, maxLength: 16, nullable: false),
                    TurnIndex = table.Column<int>(type: "int", nullable: false),
                    BansRevealed = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FactionPickStates", x => x.RoomId);
                    table.ForeignKey(
                        name: "FK_FactionPickStates_DraftRooms_RoomId",
                        column: x => x.RoomId,
                        principalSchema: "Draft",
                        principalTable: "DraftRooms",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "FactionPickTurnOrder",
                schema: "Draft",
                columns: table => new
                {
                    RoomId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Position = table.Column<int>(type: "int", nullable: false),
                    SeatId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FactionPickTurnOrder", x => new { x.RoomId, x.Position });
                    table.ForeignKey(
                        name: "FK_FactionPickTurnOrder_DraftRoomSeats_RoomId_SeatId",
                        columns: x => new { x.RoomId, x.SeatId },
                        principalSchema: "Draft",
                        principalTable: "DraftRoomSeats",
                        principalColumns: new[] { "RoomId", "Id" });
                    table.ForeignKey(
                        name: "FK_FactionPickTurnOrder_DraftRooms_RoomId",
                        column: x => x.RoomId,
                        principalSchema: "Draft",
                        principalTable: "DraftRooms",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_FactionPickTurnOrder_RoomId_SeatId",
                schema: "Draft",
                table: "FactionPickTurnOrder",
                columns: new[] { "RoomId", "SeatId" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FactionPickSettings",
                schema: "Draft");

            migrationBuilder.DropTable(
                name: "FactionPickStates",
                schema: "Draft");

            migrationBuilder.DropTable(
                name: "FactionPickTurnOrder",
                schema: "Draft");

            migrationBuilder.DropUniqueConstraint(
                name: "AK_DraftRoomSeats_RoomId_Id",
                schema: "Draft",
                table: "DraftRoomSeats");
        }
    }
}
