using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TwilightImperiumUltimate.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class AddDraftRoomSeats : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "DraftRoomSeats",
                schema: "Draft",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    RoomId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Index = table.Column<int>(type: "int", nullable: false),
                    ParticipantId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ClaimedAtUtc = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DraftRoomSeats", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DraftRoomSeats_DraftRoomParticipants_ParticipantId",
                        column: x => x.ParticipantId,
                        principalSchema: "Draft",
                        principalTable: "DraftRoomParticipants",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_DraftRoomSeats_DraftRooms_RoomId",
                        column: x => x.RoomId,
                        principalSchema: "Draft",
                        principalTable: "DraftRooms",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DraftRoomSeats_ParticipantId",
                schema: "Draft",
                table: "DraftRoomSeats",
                column: "ParticipantId");

            migrationBuilder.CreateIndex(
                name: "IX_DraftRoomSeats_RoomId_Index",
                schema: "Draft",
                table: "DraftRoomSeats",
                columns: new[] { "RoomId", "Index" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_DraftRoomSeats_RoomId_ParticipantId",
                schema: "Draft",
                table: "DraftRoomSeats",
                columns: new[] { "RoomId", "ParticipantId" },
                unique: true,
                filter: "[ParticipantId] IS NOT NULL");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DraftRoomSeats",
                schema: "Draft");
        }
    }
}
