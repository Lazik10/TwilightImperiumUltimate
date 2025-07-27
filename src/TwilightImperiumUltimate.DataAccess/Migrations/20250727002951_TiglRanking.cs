using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace TwilightImperiumUltimate.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class TiglRanking : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "Tigl");

            migrationBuilder.CreateTable(
                name: "GlickoRatings",
                schema: "Tigl",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    GlickoStatsId = table.Column<int>(type: "int", nullable: false),
                    Rating = table.Column<double>(type: "float", nullable: false),
                    Volatility = table.Column<double>(type: "float", nullable: false),
                    Rd = table.Column<double>(type: "float", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GlickoRatings", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "MatchReports",
                schema: "Tigl",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    GameId = table.Column<string>(type: "varchar(100)", nullable: false),
                    Source = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false),
                    Timestamp = table.Column<long>(type: "bigint", nullable: false),
                    State = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MatchReports", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Parameters",
                schema: "Tigl",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false),
                    Enabled = table.Column<string>(type: "varchar(10)", maxLength: 10, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Parameters", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Seasons",
                schema: "Tigl",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SeasonNumber = table.Column<int>(type: "integer", nullable: false),
                    Name = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false),
                    StartDate = table.Column<DateOnly>(type: "date", nullable: false),
                    EndDate = table.Column<DateOnly>(type: "date", nullable: false),
                    IsActive = table.Column<string>(type: "varchar(10)", maxLength: 10, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Seasons", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                schema: "Tigl",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DiscordId = table.Column<long>(type: "bigint", nullable: false),
                    TiglUserName = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false),
                    DiscordTag = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false),
                    TtsUserName = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false),
                    TtpgUserName = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false),
                    BgaUserName = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false),
                    TiglUserNameChanged = table.Column<bool>(type: "bit", nullable: false),
                    LastUserNameChange = table.Column<DateOnly>(type: "date", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PlayerResults",
                schema: "Tigl",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TiglUser = table.Column<int>(type: "integer", nullable: false),
                    Score = table.Column<int>(type: "integer", nullable: false),
                    Faction = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false),
                    MatchReportId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PlayerResults", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PlayerResults_MatchReports_MatchReportId",
                        column: x => x.MatchReportId,
                        principalSchema: "Tigl",
                        principalTable: "MatchReports",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AsyncStats",
                schema: "Tigl",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TiglUserId = table.Column<int>(type: "integer", nullable: false),
                    AsyncRatingId = table.Column<int>(type: "integer", nullable: false),
                    League = table.Column<string>(type: "varchar(30)", maxLength: 30, nullable: false),
                    Rank = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AsyncStats", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AsyncStats_Users_TiglUserId",
                        column: x => x.TiglUserId,
                        principalSchema: "Tigl",
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "GlickoStats",
                schema: "Tigl",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TiglUserId = table.Column<int>(type: "integer", nullable: false),
                    GlickoRatingId = table.Column<int>(type: "integer", nullable: false),
                    League = table.Column<string>(type: "varchar(30)", maxLength: 30, nullable: false),
                    Rank = table.Column<string>(type: "varchar(50)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GlickoStats", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GlickoStats_GlickoRatings_GlickoRatingId",
                        column: x => x.GlickoRatingId,
                        principalSchema: "Tigl",
                        principalTable: "GlickoRatings",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_GlickoStats_Users_TiglUserId",
                        column: x => x.TiglUserId,
                        principalSchema: "Tigl",
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TrueSkillStats",
                schema: "Tigl",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TiglUserId = table.Column<int>(type: "integer", nullable: false),
                    TrueSkillRatingId = table.Column<int>(type: "integer", nullable: false),
                    League = table.Column<string>(type: "varchar(30)", maxLength: 30, nullable: false),
                    Rank = table.Column<string>(type: "varchar(50)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TrueSkillStats", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TrueSkillStats_Users_TiglUserId",
                        column: x => x.TiglUserId,
                        principalSchema: "Tigl",
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AsyncPlayerMatchStats",
                schema: "Tigl",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AsyncStatsId = table.Column<int>(type: "int", nullable: false),
                    MatchId = table.Column<int>(type: "int", nullable: false),
                    DiscordUserId = table.Column<long>(type: "bigint", nullable: false),
                    Winner = table.Column<string>(type: "varchar(10)", nullable: false),
                    Score = table.Column<int>(type: "int", nullable: false),
                    Placement = table.Column<int>(type: "int", nullable: false),
                    ExpectedWinPercentage = table.Column<decimal>(type: "decimal(18,10)", nullable: false),
                    RatingOld = table.Column<decimal>(type: "decimal(18,10)", nullable: false),
                    RatingNew = table.Column<decimal>(type: "decimal(18,10)", nullable: false),
                    AussieScoreOld = table.Column<decimal>(type: "decimal(18,10)", nullable: false),
                    AussieScoreNew = table.Column<decimal>(type: "decimal(18,10)", nullable: false),
                    OpponentAvgRating = table.Column<decimal>(type: "decimal(18,10)", nullable: false),
                    Season = table.Column<int>(type: "int", nullable: false),
                    Faction = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false),
                    Timestamp = table.Column<long>(type: "bigint", nullable: false),
                    IsRankUpGame = table.Column<string>(type: "varchar(10)", maxLength: 10, nullable: false),
                    OldRank = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false),
                    NewRank = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AsyncPlayerMatchStats", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AsyncPlayerMatchStats_AsyncStats_AsyncStatsId",
                        column: x => x.AsyncStatsId,
                        principalSchema: "Tigl",
                        principalTable: "AsyncStats",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AsyncPlayerMatchStats_MatchReports_MatchId",
                        column: x => x.MatchId,
                        principalSchema: "Tigl",
                        principalTable: "MatchReports",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AsyncRatings",
                schema: "Tigl",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AsyncStatsId = table.Column<int>(type: "int", nullable: false),
                    Rating = table.Column<double>(type: "float", nullable: false),
                    AussieScore = table.Column<double>(type: "float", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AsyncRatings", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AsyncRatings_AsyncStats_AsyncStatsId",
                        column: x => x.AsyncStatsId,
                        principalSchema: "Tigl",
                        principalTable: "AsyncStats",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "GlickoPlayerMatchStats",
                schema: "Tigl",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    GlickoStatsId = table.Column<int>(type: "int", nullable: false),
                    MatchId = table.Column<int>(type: "int", nullable: false),
                    DiscordUserId = table.Column<long>(type: "bigint", nullable: false),
                    Winner = table.Column<string>(type: "varchar(10)", nullable: false),
                    Score = table.Column<int>(type: "int", nullable: false),
                    Placement = table.Column<int>(type: "int", nullable: false),
                    Performance = table.Column<decimal>(type: "decimal(18,10)", nullable: false),
                    RatingOld = table.Column<decimal>(type: "decimal(18,10)", nullable: false),
                    RatingNew = table.Column<decimal>(type: "decimal(18,10)", nullable: false),
                    RdOld = table.Column<decimal>(type: "decimal(18,10)", nullable: false),
                    RdNew = table.Column<decimal>(type: "decimal(18,10)", nullable: false),
                    VolatilityOld = table.Column<decimal>(type: "decimal(18,10)", nullable: false),
                    VolatilityNew = table.Column<decimal>(type: "decimal(18,10)", nullable: false),
                    OpponentAvgRating = table.Column<decimal>(type: "decimal(18,10)", nullable: false),
                    Season = table.Column<int>(type: "int", nullable: false),
                    Faction = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false),
                    Timestmap = table.Column<long>(type: "bigint", nullable: false),
                    IsRankUpGame = table.Column<string>(type: "varchar(10)", maxLength: 10, nullable: false),
                    OldRank = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false),
                    NewRank = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GlickoPlayerMatchStats", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GlickoPlayerMatchStats_GlickoStats_GlickoStatsId",
                        column: x => x.GlickoStatsId,
                        principalSchema: "Tigl",
                        principalTable: "GlickoStats",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_GlickoPlayerMatchStats_MatchReports_MatchId",
                        column: x => x.MatchId,
                        principalSchema: "Tigl",
                        principalTable: "MatchReports",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TrueSkillPlayerMatchStats",
                schema: "Tigl",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TrueSkillStatsId = table.Column<int>(type: "int", nullable: false),
                    MatchId = table.Column<int>(type: "int", nullable: false),
                    DiscordUserId = table.Column<long>(type: "bigint", nullable: false),
                    Winner = table.Column<string>(type: "varchar(10)", nullable: false),
                    Score = table.Column<int>(type: "int", nullable: false),
                    Placement = table.Column<int>(type: "int", nullable: false),
                    Performance = table.Column<decimal>(type: "decimal(18,10)", nullable: false),
                    MuOld = table.Column<decimal>(type: "decimal(18,10)", nullable: false),
                    MuNew = table.Column<decimal>(type: "decimal(18,10)", nullable: false),
                    SigmaOld = table.Column<decimal>(type: "decimal(18,10)", nullable: false),
                    SigmaNew = table.Column<decimal>(type: "decimal(18,10)", nullable: false),
                    OpponentAvgRating = table.Column<decimal>(type: "decimal(18,10)", nullable: false),
                    Season = table.Column<int>(type: "int", nullable: false),
                    Faction = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false),
                    Timestamp = table.Column<long>(type: "bigint", nullable: false),
                    IsRankUpGame = table.Column<string>(type: "varchar(10)", maxLength: 10, nullable: false),
                    OldRank = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false),
                    NewRank = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TrueSkillPlayerMatchStats", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TrueSkillPlayerMatchStats_MatchReports_MatchId",
                        column: x => x.MatchId,
                        principalSchema: "Tigl",
                        principalTable: "MatchReports",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TrueSkillPlayerMatchStats_TrueSkillStats_TrueSkillStatsId",
                        column: x => x.TrueSkillStatsId,
                        principalSchema: "Tigl",
                        principalTable: "TrueSkillStats",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TrueSkillRatings",
                schema: "Tigl",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TrueSkillStatsId = table.Column<int>(type: "int", nullable: false),
                    Mu = table.Column<double>(type: "float", nullable: false),
                    Sigma = table.Column<double>(type: "float", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TrueSkillRatings", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TrueSkillRatings_TrueSkillStats_TrueSkillStatsId",
                        column: x => x.TrueSkillStatsId,
                        principalSchema: "Tigl",
                        principalTable: "TrueSkillStats",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                schema: "Tigl",
                table: "Parameters",
                columns: new[] { "Id", "Enabled", "Name" },
                values: new object[,]
                {
                    { 1, "true", "RankingUp" },
                    { 2, "true", "EvaluateGames" }
                });

            migrationBuilder.InsertData(
                schema: "Tigl",
                table: "Seasons",
                columns: new[] { "Id", "EndDate", "IsActive", "Name", "SeasonNumber", "StartDate" },
                values: new object[] { 1, new DateOnly(2023, 12, 31), "true", "", 1, new DateOnly(2023, 1, 1) });

            migrationBuilder.CreateIndex(
                name: "IX_AsyncPlayerMatchStats_AsyncStatsId",
                schema: "Tigl",
                table: "AsyncPlayerMatchStats",
                column: "AsyncStatsId");

            migrationBuilder.CreateIndex(
                name: "IX_AsyncPlayerMatchStats_MatchId",
                schema: "Tigl",
                table: "AsyncPlayerMatchStats",
                column: "MatchId");

            migrationBuilder.CreateIndex(
                name: "IX_AsyncRatings_AsyncStatsId",
                schema: "Tigl",
                table: "AsyncRatings",
                column: "AsyncStatsId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AsyncStats_TiglUserId",
                schema: "Tigl",
                table: "AsyncStats",
                column: "TiglUserId");

            migrationBuilder.CreateIndex(
                name: "IX_GlickoPlayerMatchStats_GlickoStatsId",
                schema: "Tigl",
                table: "GlickoPlayerMatchStats",
                column: "GlickoStatsId");

            migrationBuilder.CreateIndex(
                name: "IX_GlickoPlayerMatchStats_MatchId",
                schema: "Tigl",
                table: "GlickoPlayerMatchStats",
                column: "MatchId");

            migrationBuilder.CreateIndex(
                name: "IX_GlickoStats_GlickoRatingId",
                schema: "Tigl",
                table: "GlickoStats",
                column: "GlickoRatingId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_GlickoStats_TiglUserId",
                schema: "Tigl",
                table: "GlickoStats",
                column: "TiglUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Parameters_Name",
                schema: "Tigl",
                table: "Parameters",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_PlayerResults_MatchReportId",
                schema: "Tigl",
                table: "PlayerResults",
                column: "MatchReportId");

            migrationBuilder.CreateIndex(
                name: "IX_TrueSkillPlayerMatchStats_MatchId",
                schema: "Tigl",
                table: "TrueSkillPlayerMatchStats",
                column: "MatchId");

            migrationBuilder.CreateIndex(
                name: "IX_TrueSkillPlayerMatchStats_TrueSkillStatsId",
                schema: "Tigl",
                table: "TrueSkillPlayerMatchStats",
                column: "TrueSkillStatsId");

            migrationBuilder.CreateIndex(
                name: "IX_TrueSkillRatings_TrueSkillStatsId",
                schema: "Tigl",
                table: "TrueSkillRatings",
                column: "TrueSkillStatsId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_TrueSkillStats_TiglUserId",
                schema: "Tigl",
                table: "TrueSkillStats",
                column: "TiglUserId");

            migrationBuilder.CreateIndex(
                name: "IX_TiglUser_DiscordId",
                schema: "Tigl",
                table: "Users",
                column: "DiscordId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_TiglUser_TiglUserName",
                schema: "Tigl",
                table: "Users",
                column: "TiglUserName",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AsyncPlayerMatchStats",
                schema: "Tigl");

            migrationBuilder.DropTable(
                name: "AsyncRatings",
                schema: "Tigl");

            migrationBuilder.DropTable(
                name: "GlickoPlayerMatchStats",
                schema: "Tigl");

            migrationBuilder.DropTable(
                name: "Parameters",
                schema: "Tigl");

            migrationBuilder.DropTable(
                name: "PlayerResults",
                schema: "Tigl");

            migrationBuilder.DropTable(
                name: "Seasons",
                schema: "Tigl");

            migrationBuilder.DropTable(
                name: "TrueSkillPlayerMatchStats",
                schema: "Tigl");

            migrationBuilder.DropTable(
                name: "TrueSkillRatings",
                schema: "Tigl");

            migrationBuilder.DropTable(
                name: "AsyncStats",
                schema: "Tigl");

            migrationBuilder.DropTable(
                name: "GlickoStats",
                schema: "Tigl");

            migrationBuilder.DropTable(
                name: "MatchReports",
                schema: "Tigl");

            migrationBuilder.DropTable(
                name: "TrueSkillStats",
                schema: "Tigl");

            migrationBuilder.DropTable(
                name: "GlickoRatings",
                schema: "Tigl");

            migrationBuilder.DropTable(
                name: "Users",
                schema: "Tigl");
        }
    }
}
