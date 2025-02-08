using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TwilightImperiumUltimate.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class AsyncStats : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AsyncGameStats",
                schema: "Statistics",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AsyncGameID = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false),
                    AsyncFunGameName = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: false),
                    Platform = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false),
                    Timestamp = table.Column<long>(type: "bigint", nullable: false),
                    SetupTimestamp = table.Column<long>(type: "bigint", nullable: false),
                    EndedTimestamp = table.Column<long>(type: "bigint", nullable: true),
                    HasWinner = table.Column<string>(type: "nvarchar(5)", nullable: false),
                    NumberOfPlayers = table.Column<int>(type: "integer", nullable: false),
                    Round = table.Column<int>(type: "integer", nullable: false),
                    Turn = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: true),
                    Scoreboard = table.Column<int>(type: "integer", nullable: false),
                    MapString = table.Column<string>(type: "varchar(MAX)", nullable: false),
                    AbsolMode = table.Column<string>(type: "varchar(5)", nullable: false),
                    DiscordantStarsMode = table.Column<string>(type: "varchar(5)", nullable: false),
                    FrankenGame = table.Column<string>(type: "varchar(5)", nullable: false),
                    Homebrew = table.Column<string>(type: "varchar(5)", nullable: false),
                    IsPoK = table.Column<string>(type: "varchar(5)", nullable: false),
                    IsTigl = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AsyncGameStats", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AsyncPlayerProfile",
                schema: "Statistics",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DiscordUserId = table.Column<long>(type: "bigint", nullable: false),
                    DiscordUserName = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AsyncPlayerProfile", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AsyncPlayerStats",
                schema: "Statistics",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DiscordUserID = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false),
                    FactionName = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false),
                    Score = table.Column<int>(type: "integer", nullable: false),
                    Color = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false),
                    TotalNumberOfTurns = table.Column<int>(type: "integer", nullable: false),
                    TotalTurnTime = table.Column<long>(type: "bigint", nullable: false),
                    ExpectedHits = table.Column<float>(type: "real", nullable: false),
                    ActualHits = table.Column<float>(type: "real", nullable: false),
                    Winner = table.Column<string>(type: "varchar(5)", nullable: false),
                    GameStatsId = table.Column<int>(type: "integer", nullable: false),
                    DiscordUserName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Eliminated = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AsyncPlayerStats", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AsyncPlayerStats_AsyncGameStats_GameStatsId",
                        column: x => x.GameStatsId,
                        principalSchema: "Statistics",
                        principalTable: "AsyncGameStats",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AsyncPlayerProfileGameStats",
                schema: "Relationships",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    AsyncPlayerProfileId = table.Column<int>(type: "integer", nullable: false),
                    GameStatsId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AsyncPlayerProfileGameStats", x => new { x.AsyncPlayerProfileId, x.GameStatsId });
                    table.ForeignKey(
                        name: "FK_AsyncPlayerProfileGameStats_AsyncGameStats_GameStatsId",
                        column: x => x.GameStatsId,
                        principalSchema: "Statistics",
                        principalTable: "AsyncGameStats",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AsyncPlayerProfileGameStats_AsyncPlayerProfile_AsyncPlayerProfileId",
                        column: x => x.AsyncPlayerProfileId,
                        principalSchema: "Statistics",
                        principalTable: "AsyncPlayerProfile",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AsyncPlayerProfileSettings",
                schema: "Statistics",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ExcludeFromAsyncStats = table.Column<string>(type: "varchar(5)", nullable: false),
                    ShowWinRate = table.Column<string>(type: "varchar(5)", nullable: false),
                    ShowTurnStats = table.Column<string>(type: "varchar(5)", nullable: false),
                    ShowCombatStats = table.Column<string>(type: "varchar(5)", nullable: false),
                    ShowVpStats = table.Column<string>(type: "varchar(5)", nullable: false),
                    ShowFactionStats = table.Column<string>(type: "varchar(5)", nullable: false),
                    ShowOpponents = table.Column<string>(type: "varchar(5)", nullable: false),
                    ShowGames = table.Column<string>(type: "varchar(5)", nullable: false),
                    AsyncPlayerProfileId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AsyncPlayerProfileSettings", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AsyncPlayerProfileSettings_AsyncPlayerProfile_AsyncPlayerProfileId",
                        column: x => x.AsyncPlayerProfileId,
                        principalSchema: "Statistics",
                        principalTable: "AsyncPlayerProfile",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AsyncGameStats_AsyncFunGameName",
                schema: "Statistics",
                table: "AsyncGameStats",
                column: "AsyncFunGameName");

            migrationBuilder.CreateIndex(
                name: "IX_AsyncGameStats_AsyncGameID",
                schema: "Statistics",
                table: "AsyncGameStats",
                column: "AsyncGameID")
                .Annotation("SqlServer:Clustered", false);

            migrationBuilder.CreateIndex(
                name: "IX_AsyncPlayerProfile_DiscordUserId",
                schema: "Statistics",
                table: "AsyncPlayerProfile",
                column: "DiscordUserId",
                unique: true)
                .Annotation("SqlServer:Clustered", false);

            migrationBuilder.CreateIndex(
                name: "IX_AsyncPlayerProfileGameStats_GameStatsId",
                schema: "Relationships",
                table: "AsyncPlayerProfileGameStats",
                column: "GameStatsId");

            migrationBuilder.CreateIndex(
                name: "IX_AsyncPlayerProfileSettings_AsyncPlayerProfileId",
                schema: "Statistics",
                table: "AsyncPlayerProfileSettings",
                column: "AsyncPlayerProfileId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AsyncPlayerStats_DiscordUserID",
                schema: "Statistics",
                table: "AsyncPlayerStats",
                column: "DiscordUserID")
                .Annotation("SqlServer:Clustered", false);

            migrationBuilder.CreateIndex(
                name: "IX_AsyncPlayerStats_GameStatsId",
                schema: "Statistics",
                table: "AsyncPlayerStats",
                column: "GameStatsId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AsyncPlayerProfileGameStats",
                schema: "Relationships");

            migrationBuilder.DropTable(
                name: "AsyncPlayerProfileSettings",
                schema: "Statistics");

            migrationBuilder.DropTable(
                name: "AsyncPlayerStats",
                schema: "Statistics");

            migrationBuilder.DropTable(
                name: "AsyncPlayerProfile",
                schema: "Statistics");

            migrationBuilder.DropTable(
                name: "AsyncGameStats",
                schema: "Statistics");
        }
    }
}
