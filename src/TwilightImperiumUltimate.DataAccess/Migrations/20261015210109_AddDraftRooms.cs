using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TwilightImperiumUltimate.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class AddDraftRooms : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "Draft");

            migrationBuilder.CreateTable(
                name: "DraftRooms",
                schema: "Draft",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Code = table.Column<string>(type: "varchar(8)", maxLength: 8, nullable: false),
                    Name = table.Column<string>(type: "nvarchar(60)", maxLength: 60, nullable: false),
                    PasswordHash = table.Column<string>(type: "varchar(256)", maxLength: 256, nullable: false),
                    Status = table.Column<string>(type: "varchar(16)", maxLength: 16, nullable: false),
                    DraftType = table.Column<string>(type: "varchar(64)", maxLength: 64, nullable: false),
                    IsLocked = table.Column<bool>(type: "bit", nullable: false),
                    Version = table.Column<int>(type: "int", nullable: false),
                    CreatedAtUtc = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastActivityUtc = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ResultId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DraftRooms", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "DraftRoomParticipants",
                schema: "Draft",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    RoomId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(450)", maxLength: 450, nullable: true),
                    SecretHash = table.Column<byte[]>(type: "varbinary(32)", maxLength: 32, nullable: true),
                    DisplayName = table.Column<string>(type: "nvarchar(24)", maxLength: 24, nullable: false),
                    DisplayNameKey = table.Column<string>(type: "nvarchar(24)", maxLength: 24, nullable: false),
                    Role = table.Column<string>(type: "varchar(16)", maxLength: 16, nullable: false),
                    Status = table.Column<string>(type: "varchar(16)", maxLength: 16, nullable: false),
                    JoinedAtUtc = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastSeenUtc = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DraftRoomParticipants", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DraftRoomParticipants_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_DraftRoomParticipants_DraftRooms_RoomId",
                        column: x => x.RoomId,
                        principalSchema: "Draft",
                        principalTable: "DraftRooms",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DraftRoomParticipants_RoomId_DisplayNameKey",
                schema: "Draft",
                table: "DraftRoomParticipants",
                columns: new[] { "RoomId", "DisplayNameKey" },
                unique: true,
                filter: "[Status] = 'Active'");

            migrationBuilder.CreateIndex(
                name: "IX_DraftRoomParticipants_RoomId_UserId",
                schema: "Draft",
                table: "DraftRoomParticipants",
                columns: new[] { "RoomId", "UserId" },
                unique: true,
                filter: "[UserId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_DraftRoomParticipants_UserId",
                schema: "Draft",
                table: "DraftRoomParticipants",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_DraftRooms_Code",
                schema: "Draft",
                table: "DraftRooms",
                column: "Code",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DraftRoomParticipants",
                schema: "Draft");

            migrationBuilder.DropTable(
                name: "DraftRooms",
                schema: "Draft");
        }
    }
}
