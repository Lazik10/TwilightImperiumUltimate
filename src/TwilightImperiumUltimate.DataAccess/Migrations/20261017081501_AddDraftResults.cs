using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TwilightImperiumUltimate.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class AddDraftResults : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "DraftResults",
                schema: "Draft",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ShareToken = table.Column<string>(type: "varchar(22)", unicode: false, maxLength: 22, nullable: false, collation: "Latin1_General_100_BIN2"),
                    DraftType = table.Column<string>(type: "varchar(32)", unicode: false, maxLength: 32, nullable: false),
                    RoomName = table.Column<string>(type: "nvarchar(60)", maxLength: 60, nullable: false),
                    SourceRoomId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    OwnerUserId = table.Column<string>(type: "nvarchar(450)", maxLength: 450, nullable: true),
                    FinishedAtUtc = table.Column<DateTime>(type: "datetime2", nullable: false),
                    PayloadVersion = table.Column<int>(type: "int", nullable: false),
                    PayloadJson = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedAtUtc = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DraftResults", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DraftResults_AspNetUsers_OwnerUserId",
                        column: x => x.OwnerUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "DraftResultPlayers",
                schema: "Draft",
                columns: table => new
                {
                    ResultId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SeatIndex = table.Column<int>(type: "int", nullable: false),
                    DisplayName = table.Column<string>(type: "nvarchar(258)", maxLength: 258, nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(450)", maxLength: 450, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DraftResultPlayers", x => new { x.ResultId, x.SeatIndex });
                    table.ForeignKey(
                        name: "FK_DraftResultPlayers_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_DraftResultPlayers_DraftResults_ResultId",
                        column: x => x.ResultId,
                        principalSchema: "Draft",
                        principalTable: "DraftResults",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DraftResultPlayers_UserId",
                schema: "Draft",
                table: "DraftResultPlayers",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_DraftResults_OwnerUserId_FinishedAtUtc",
                schema: "Draft",
                table: "DraftResults",
                columns: new[] { "OwnerUserId", "FinishedAtUtc" });

            migrationBuilder.CreateIndex(
                name: "IX_DraftResults_ShareToken",
                schema: "Draft",
                table: "DraftResults",
                column: "ShareToken",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_DraftResults_SourceRoomId",
                schema: "Draft",
                table: "DraftResults",
                column: "SourceRoomId",
                unique: true,
                filter: "[SourceRoomId] IS NOT NULL");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DraftResultPlayers",
                schema: "Draft");

            migrationBuilder.DropTable(
                name: "DraftResults",
                schema: "Draft");
        }
    }
}
