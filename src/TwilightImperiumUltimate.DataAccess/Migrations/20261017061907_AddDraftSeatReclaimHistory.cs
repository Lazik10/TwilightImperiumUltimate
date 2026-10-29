using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TwilightImperiumUltimate.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class AddDraftSeatReclaimHistory : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "HasBeenReleased",
                schema: "Draft",
                table: "DraftRoomSeats",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "CanReclaimDraftSeat",
                schema: "Draft",
                table: "DraftRoomParticipants",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<Guid>(
                name: "DraftSeatId",
                schema: "Draft",
                table: "DraftRoomParticipants",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.Sql("""
                UPDATE [participant]
                SET [DraftSeatId] = [seat].[Id]
                FROM [Draft].[DraftRoomParticipants] AS [participant]
                INNER JOIN [Draft].[DraftRoomSeats] AS [seat]
                    ON [seat].[ParticipantId] = [participant].[Id] AND [seat].[RoomId] = [participant].[RoomId]
                INNER JOIN [Draft].[DraftRooms] AS [room] ON [room].[Id] = [seat].[RoomId]
                WHERE [room].[Status] = 'InProgress';
                """);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "HasBeenReleased",
                schema: "Draft",
                table: "DraftRoomSeats");

            migrationBuilder.DropColumn(
                name: "CanReclaimDraftSeat",
                schema: "Draft",
                table: "DraftRoomParticipants");

            migrationBuilder.DropColumn(
                name: "DraftSeatId",
                schema: "Draft",
                table: "DraftRoomParticipants");
        }
    }
}
