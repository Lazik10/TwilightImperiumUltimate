using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TwilightImperiumUltimate.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class AddDraftRoomChatAndLog : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "DraftRoomChatMessages",
                schema: "Draft",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoomId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ParticipantId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Kind = table.Column<string>(type: "varchar(16)", maxLength: 16, nullable: false),
                    Text = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: false),
                    CreatedAtUtc = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DraftRoomChatMessages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DraftRoomChatMessages_DraftRooms_RoomId",
                        column: x => x.RoomId,
                        principalSchema: "Draft",
                        principalTable: "DraftRooms",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DraftRoomLogEntries",
                schema: "Draft",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoomId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ActorParticipantId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Type = table.Column<string>(type: "varchar(48)", maxLength: 48, nullable: false),
                    DataJson = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedAtUtc = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DraftRoomLogEntries", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DraftRoomLogEntries_DraftRooms_RoomId",
                        column: x => x.RoomId,
                        principalSchema: "Draft",
                        principalTable: "DraftRooms",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DraftRoomChatMessages_RoomId_Id",
                schema: "Draft",
                table: "DraftRoomChatMessages",
                columns: new[] { "RoomId", "Id" });

            migrationBuilder.CreateIndex(
                name: "IX_DraftRoomLogEntries_RoomId_Id",
                schema: "Draft",
                table: "DraftRoomLogEntries",
                columns: new[] { "RoomId", "Id" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DraftRoomChatMessages",
                schema: "Draft");

            migrationBuilder.DropTable(
                name: "DraftRoomLogEntries",
                schema: "Draft");
        }
    }
}
