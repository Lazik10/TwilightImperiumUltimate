using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace TwilightImperiumUltimate.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class TiglRankingNewSchema : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GlickoStats_GlickoRatings_GlickoRatingId",
                schema: "Tigl",
                table: "GlickoStats");

            migrationBuilder.DropIndex(
                name: "IX_TrueSkillStats_TiglUserId",
                schema: "Tigl",
                table: "TrueSkillStats");

            migrationBuilder.DropIndex(
                name: "IX_GlickoStats_GlickoRatingId",
                schema: "Tigl",
                table: "GlickoStats");

            migrationBuilder.DropIndex(
                name: "IX_GlickoStats_TiglUserId",
                schema: "Tigl",
                table: "GlickoStats");

            migrationBuilder.DropIndex(
                name: "IX_AsyncStats_TiglUserId",
                schema: "Tigl",
                table: "AsyncStats");

            migrationBuilder.DropColumn(
                name: "Rank",
                schema: "Tigl",
                table: "TrueSkillStats");

            migrationBuilder.DropColumn(
                name: "Performance",
                schema: "Tigl",
                table: "TrueSkillPlayerMatchStats");

            migrationBuilder.DropColumn(
                name: "Rank",
                schema: "Tigl",
                table: "GlickoStats");

            migrationBuilder.DropColumn(
                name: "Performance",
                schema: "Tigl",
                table: "GlickoPlayerMatchStats");

            migrationBuilder.DropColumn(
                name: "Rank",
                schema: "Tigl",
                table: "AsyncStats");

            migrationBuilder.EnsureSchema(
                name: "Log");

            migrationBuilder.RenameColumn(
                name: "Timestamp",
                schema: "Tigl",
                table: "TrueSkillPlayerMatchStats",
                newName: "StartTimestamp");

            migrationBuilder.RenameColumn(
                name: "TiglUser",
                schema: "Tigl",
                table: "PlayerResults",
                newName: "TiglUserId");

            migrationBuilder.RenameColumn(
                name: "Timestamp",
                schema: "Tigl",
                table: "MatchReports",
                newName: "StartTimestamp");

            migrationBuilder.RenameColumn(
                name: "Timestmap",
                schema: "Tigl",
                table: "GlickoPlayerMatchStats",
                newName: "StartTimestamp");

            migrationBuilder.RenameColumn(
                name: "Timestamp",
                schema: "Tigl",
                table: "AsyncPlayerMatchStats",
                newName: "StartTimestamp");

            migrationBuilder.AlterColumn<string>(
                name: "TiglUserNameChanged",
                schema: "Tigl",
                table: "Users",
                type: "varchar(10)",
                maxLength: 10,
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "bit")
                .Annotation("Relational:ColumnOrder", 7);

            migrationBuilder.AlterColumn<DateOnly>(
                name: "LastUserNameChange",
                schema: "Tigl",
                table: "Users",
                type: "date",
                nullable: false,
                oldClrType: typeof(DateOnly),
                oldType: "date")
                .Annotation("Relational:ColumnOrder", 8);

            migrationBuilder.AddColumn<string>(
                name: "ProphecyOfKingsRank",
                schema: "Tigl",
                table: "Users",
                type: "varchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "")
                .Annotation("Relational:ColumnOrder", 9);

            migrationBuilder.AddColumn<string>(
                name: "ShatteredRank",
                schema: "Tigl",
                table: "Users",
                type: "varchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "")
                .Annotation("Relational:ColumnOrder", 11);

            migrationBuilder.AddColumn<string>(
                name: "ThundersEdgeRank",
                schema: "Tigl",
                table: "Users",
                type: "varchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "")
                .Annotation("Relational:ColumnOrder", 10);

            migrationBuilder.AlterColumn<decimal>(
                name: "SigmaOld",
                schema: "Tigl",
                table: "TrueSkillPlayerMatchStats",
                type: "decimal(18,10)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,10)")
                .Annotation("Relational:ColumnOrder", 9)
                .OldAnnotation("Relational:ColumnOrder", 10);

            migrationBuilder.AlterColumn<decimal>(
                name: "SigmaNew",
                schema: "Tigl",
                table: "TrueSkillPlayerMatchStats",
                type: "decimal(18,10)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,10)")
                .Annotation("Relational:ColumnOrder", 10)
                .OldAnnotation("Relational:ColumnOrder", 11);

            migrationBuilder.AlterColumn<int>(
                name: "Season",
                schema: "Tigl",
                table: "TrueSkillPlayerMatchStats",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int")
                .Annotation("Relational:ColumnOrder", 12)
                .OldAnnotation("Relational:ColumnOrder", 13);

            migrationBuilder.AlterColumn<decimal>(
                name: "OpponentAvgRating",
                schema: "Tigl",
                table: "TrueSkillPlayerMatchStats",
                type: "decimal(18,10)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,10)")
                .Annotation("Relational:ColumnOrder", 11)
                .OldAnnotation("Relational:ColumnOrder", 12);

            migrationBuilder.AlterColumn<decimal>(
                name: "MuOld",
                schema: "Tigl",
                table: "TrueSkillPlayerMatchStats",
                type: "decimal(18,10)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,10)")
                .Annotation("Relational:ColumnOrder", 7)
                .OldAnnotation("Relational:ColumnOrder", 8);

            migrationBuilder.AlterColumn<decimal>(
                name: "MuNew",
                schema: "Tigl",
                table: "TrueSkillPlayerMatchStats",
                type: "decimal(18,10)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,10)")
                .Annotation("Relational:ColumnOrder", 8)
                .OldAnnotation("Relational:ColumnOrder", 9);

            migrationBuilder.AlterColumn<string>(
                name: "Faction",
                schema: "Tigl",
                table: "TrueSkillPlayerMatchStats",
                type: "varchar(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(100)",
                oldMaxLength: 100)
                .Annotation("Relational:ColumnOrder", 13)
                .OldAnnotation("Relational:ColumnOrder", 14);

            migrationBuilder.AlterColumn<long>(
                name: "StartTimestamp",
                schema: "Tigl",
                table: "TrueSkillPlayerMatchStats",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint")
                .Annotation("Relational:ColumnOrder", 14)
                .OldAnnotation("Relational:ColumnOrder", 15);

            migrationBuilder.AddColumn<long>(
                name: "EndTimestamp",
                schema: "Tigl",
                table: "TrueSkillPlayerMatchStats",
                type: "bigint",
                nullable: false,
                defaultValue: 0L)
                .Annotation("Relational:ColumnOrder", 15);

            migrationBuilder.AddColumn<string>(
                name: "ForcedReset",
                schema: "Tigl",
                table: "TrueSkillPlayerMatchStats",
                type: "varchar(10)",
                maxLength: 10,
                nullable: false,
                defaultValue: "")
                .Annotation("Relational:ColumnOrder", 19);

            migrationBuilder.AddColumn<int>(
                name: "EmperorId",
                schema: "Tigl",
                table: "Seasons",
                type: "int",
                nullable: true)
                .Annotation("Relational:ColumnOrder", 6);

            migrationBuilder.AlterColumn<string>(
                name: "Faction",
                schema: "Tigl",
                table: "PlayerResults",
                type: "varchar(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(100)",
                oldMaxLength: 100)
                .Annotation("Relational:ColumnOrder", 4)
                .OldAnnotation("Relational:ColumnOrder", 3);

            migrationBuilder.AddColumn<string>(
                name: "IsWinner",
                schema: "Tigl",
                table: "PlayerResults",
                type: "varchar(10)",
                maxLength: 10,
                nullable: false,
                defaultValue: "")
                .Annotation("Relational:ColumnOrder", 3);

            migrationBuilder.AlterColumn<string>(
                name: "State",
                schema: "Tigl",
                table: "MatchReports",
                type: "varchar(50)",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(50)",
                oldMaxLength: 50)
                .Annotation("Relational:ColumnOrder", 11)
                .OldAnnotation("Relational:ColumnOrder", 4);

            migrationBuilder.AddColumn<long>(
                name: "EndTimestamp",
                schema: "Tigl",
                table: "MatchReports",
                type: "bigint",
                nullable: false,
                defaultValue: 0L)
                .Annotation("Relational:ColumnOrder", 4);

            migrationBuilder.AddColumn<long>(
                name: "Events",
                schema: "Tigl",
                table: "MatchReports",
                type: "bigint",
                nullable: false,
                defaultValue: 0L)
                .Annotation("Relational:ColumnOrder", 10);

            migrationBuilder.AddColumn<string>(
                name: "League",
                schema: "Tigl",
                table: "MatchReports",
                type: "varchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "")
                .Annotation("Relational:ColumnOrder", 9);

            migrationBuilder.AddColumn<int>(
                name: "PlayerCount",
                schema: "Tigl",
                table: "MatchReports",
                type: "int",
                nullable: false,
                defaultValue: 0)
                .Annotation("Relational:ColumnOrder", 7);

            migrationBuilder.AddColumn<int>(
                name: "Round",
                schema: "Tigl",
                table: "MatchReports",
                type: "int",
                nullable: false,
                defaultValue: 0)
                .Annotation("Relational:ColumnOrder", 6);

            migrationBuilder.AddColumn<int>(
                name: "Score",
                schema: "Tigl",
                table: "MatchReports",
                type: "int",
                nullable: false,
                defaultValue: 0)
                .Annotation("Relational:ColumnOrder", 5);

            migrationBuilder.AddColumn<int>(
                name: "Season",
                schema: "Tigl",
                table: "MatchReports",
                type: "int",
                nullable: false,
                defaultValue: 0)
                .Annotation("Relational:ColumnOrder", 8);

            migrationBuilder.AlterColumn<decimal>(
                name: "VolatilityOld",
                schema: "Tigl",
                table: "GlickoPlayerMatchStats",
                type: "decimal(18,10)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,10)")
                .Annotation("Relational:ColumnOrder", 11)
                .OldAnnotation("Relational:ColumnOrder", 12);

            migrationBuilder.AlterColumn<decimal>(
                name: "VolatilityNew",
                schema: "Tigl",
                table: "GlickoPlayerMatchStats",
                type: "decimal(18,10)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,10)")
                .Annotation("Relational:ColumnOrder", 12)
                .OldAnnotation("Relational:ColumnOrder", 13);

            migrationBuilder.AlterColumn<int>(
                name: "Season",
                schema: "Tigl",
                table: "GlickoPlayerMatchStats",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int")
                .Annotation("Relational:ColumnOrder", 14)
                .OldAnnotation("Relational:ColumnOrder", 15);

            migrationBuilder.AlterColumn<decimal>(
                name: "RdOld",
                schema: "Tigl",
                table: "GlickoPlayerMatchStats",
                type: "decimal(18,10)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,10)")
                .Annotation("Relational:ColumnOrder", 9)
                .OldAnnotation("Relational:ColumnOrder", 10);

            migrationBuilder.AlterColumn<decimal>(
                name: "RdNew",
                schema: "Tigl",
                table: "GlickoPlayerMatchStats",
                type: "decimal(18,10)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,10)")
                .Annotation("Relational:ColumnOrder", 10)
                .OldAnnotation("Relational:ColumnOrder", 11);

            migrationBuilder.AlterColumn<decimal>(
                name: "RatingOld",
                schema: "Tigl",
                table: "GlickoPlayerMatchStats",
                type: "decimal(18,10)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,10)")
                .Annotation("Relational:ColumnOrder", 7)
                .OldAnnotation("Relational:ColumnOrder", 8);

            migrationBuilder.AlterColumn<decimal>(
                name: "RatingNew",
                schema: "Tigl",
                table: "GlickoPlayerMatchStats",
                type: "decimal(18,10)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,10)")
                .Annotation("Relational:ColumnOrder", 8)
                .OldAnnotation("Relational:ColumnOrder", 9);

            migrationBuilder.AlterColumn<decimal>(
                name: "OpponentAvgRating",
                schema: "Tigl",
                table: "GlickoPlayerMatchStats",
                type: "decimal(18,10)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,10)")
                .Annotation("Relational:ColumnOrder", 13)
                .OldAnnotation("Relational:ColumnOrder", 14);

            migrationBuilder.AlterColumn<string>(
                name: "Faction",
                schema: "Tigl",
                table: "GlickoPlayerMatchStats",
                type: "varchar(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(100)",
                oldMaxLength: 100)
                .Annotation("Relational:ColumnOrder", 15)
                .OldAnnotation("Relational:ColumnOrder", 16);

            migrationBuilder.AlterColumn<long>(
                name: "StartTimestamp",
                schema: "Tigl",
                table: "GlickoPlayerMatchStats",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint")
                .Annotation("Relational:ColumnOrder", 16)
                .OldAnnotation("Relational:ColumnOrder", 17);

            migrationBuilder.AddColumn<long>(
                name: "EndTimestamp",
                schema: "Tigl",
                table: "GlickoPlayerMatchStats",
                type: "bigint",
                nullable: false,
                defaultValue: 0L)
                .Annotation("Relational:ColumnOrder", 17);

            migrationBuilder.AddColumn<string>(
                name: "ForcedReset",
                schema: "Tigl",
                table: "GlickoPlayerMatchStats",
                type: "varchar(10)",
                maxLength: 10,
                nullable: false,
                defaultValue: "")
                .Annotation("Relational:ColumnOrder", 21);

            migrationBuilder.AlterColumn<string>(
                name: "OldRank",
                schema: "Tigl",
                table: "AsyncPlayerMatchStats",
                type: "varchar(50)",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(50)",
                oldMaxLength: 50)
                .Annotation("Relational:ColumnOrder", 18)
                .OldAnnotation("Relational:ColumnOrder", 17);

            migrationBuilder.AlterColumn<string>(
                name: "NewRank",
                schema: "Tigl",
                table: "AsyncPlayerMatchStats",
                type: "varchar(50)",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(50)",
                oldMaxLength: 50)
                .Annotation("Relational:ColumnOrder", 19)
                .OldAnnotation("Relational:ColumnOrder", 18);

            migrationBuilder.AlterColumn<string>(
                name: "IsRankUpGame",
                schema: "Tigl",
                table: "AsyncPlayerMatchStats",
                type: "varchar(10)",
                maxLength: 10,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(10)",
                oldMaxLength: 10)
                .Annotation("Relational:ColumnOrder", 17)
                .OldAnnotation("Relational:ColumnOrder", 16);

            migrationBuilder.AddColumn<long>(
                name: "EndTimestamp",
                schema: "Tigl",
                table: "AsyncPlayerMatchStats",
                type: "bigint",
                nullable: false,
                defaultValue: 0L)
                .Annotation("Relational:ColumnOrder", 16);

            migrationBuilder.AddColumn<string>(
                name: "ForcedReset",
                schema: "Tigl",
                table: "AsyncPlayerMatchStats",
                type: "varchar(10)",
                maxLength: 10,
                nullable: false,
                defaultValue: "")
                .Annotation("Relational:ColumnOrder", 20);

            migrationBuilder.CreateTable(
                name: "AchievementLogs",
                schema: "Log",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Timestamp = table.Column<long>(type: "bigint", nullable: false),
                    AchievementName = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false),
                    TiglUserId = table.Column<int>(type: "int", nullable: false),
                    TiglUserName = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false),
                    TiglUserDiscordId = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false),
                    MatchId = table.Column<int>(type: "int", nullable: false),
                    Published = table.Column<string>(type: "varchar(10)", nullable: false),
                    Faction = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AchievementLogs", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Achievements",
                schema: "Tigl",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false),
                    Faction = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false),
                    Category = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Achievements", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "DiscordRoleChangeLogs",
                schema: "Log",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoleId = table.Column<long>(type: "bigint", nullable: false),
                    RoleName = table.Column<string>(type: "varchar(100)", nullable: false),
                    Operation = table.Column<string>(type: "varchar(50)", nullable: false),
                    Timestamp = table.Column<long>(type: "bigint", nullable: false),
                    UserId = table.Column<long>(type: "bigint", nullable: false),
                    GameId = table.Column<int>(type: "int", nullable: false),
                    Result = table.Column<string>(type: "varchar(50)", nullable: false),
                    UserTag = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DiscordRoleChangeLogs", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "GamePublishLogs",
                schema: "Log",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MatchId = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<long>(type: "bigint", nullable: false),
                    RankUpPublish = table.Column<string>(type: "varchar(10)", nullable: false),
                    PrestigePublish = table.Column<string>(type: "varchar(10)", nullable: false),
                    LeaderPublish = table.Column<string>(type: "varchar(10)", nullable: false),
                    AchievementPublish = table.Column<string>(type: "varchar(10)", nullable: false),
                    AchievementsEvaluated = table.Column<string>(type: "varchar(10)", nullable: false),
                    PublishingInProgress = table.Column<string>(type: "varchar(10)", nullable: false),
                    Published = table.Column<string>(type: "varchar(10)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GamePublishLogs", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "LeaderLogs",
                schema: "Log",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Timestamp = table.Column<long>(type: "bigint", nullable: false),
                    Name = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false),
                    Faction = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false),
                    League = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false),
                    PreviousOwnerId = table.Column<int>(type: "integer", nullable: true),
                    PreviousOwnerDiscordId = table.Column<long>(type: "bigint", nullable: false),
                    PreviousOwner = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false),
                    NewOwnerDiscordId = table.Column<long>(type: "bigint", nullable: false),
                    NewOwnerId = table.Column<int>(type: "integer", nullable: true),
                    NewOwner = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false),
                    Duration = table.Column<long>(type: "bigint", nullable: false),
                    MatchId = table.Column<int>(type: "int", nullable: false),
                    Published = table.Column<string>(type: "varchar(10)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LeaderLogs", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Leaders",
                schema: "Tigl",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false),
                    Faction = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false),
                    FirstUpdate = table.Column<long>(type: "bigint", nullable: true),
                    LastUpdate = table.Column<long>(type: "bigint", nullable: true),
                    PreviousUpdate = table.Column<long>(type: "bigint", nullable: true),
                    ShortestDuration = table.Column<long>(type: "bigint", nullable: true),
                    LongestDuration = table.Column<long>(type: "bigint", nullable: true),
                    ChangeCount = table.Column<int>(type: "int", nullable: false),
                    League = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false),
                    PreviousOwnerId = table.Column<int>(type: "int", nullable: true),
                    CurrentOwnerId = table.Column<int>(type: "int", nullable: true),
                    ShortestHolderId = table.Column<int>(type: "int", nullable: true),
                    LongestHolderId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Leaders", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Leaders_Users_CurrentOwnerId",
                        column: x => x.CurrentOwnerId,
                        principalSchema: "Tigl",
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_Leaders_Users_LongestHolderId",
                        column: x => x.LongestHolderId,
                        principalSchema: "Tigl",
                        principalTable: "Users",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Leaders_Users_ShortestHolderId",
                        column: x => x.ShortestHolderId,
                        principalSchema: "Tigl",
                        principalTable: "Users",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "PlayerSeasonResults",
                schema: "Tigl",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Season = table.Column<int>(type: "int", nullable: false),
                    TiglUserId = table.Column<int>(type: "int", nullable: false),
                    TiglUserName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    TiglDiscordTag = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    League = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    GamesPlayed = table.Column<int>(type: "int", nullable: false),
                    WinPercentage = table.Column<double>(type: "float", nullable: false),
                    AsyncRating = table.Column<double>(type: "float", nullable: false),
                    AussieScore = table.Column<double>(type: "float", nullable: false),
                    GlickoRating = table.Column<double>(type: "float", nullable: false),
                    GlickoRd = table.Column<double>(type: "float", nullable: false),
                    GlickoVolatility = table.Column<double>(type: "float", nullable: false),
                    TrueSkillMu = table.Column<double>(type: "float", nullable: false),
                    TrueSkillSigma = table.Column<double>(type: "float", nullable: false),
                    TrueSkillConservativeRating = table.Column<double>(type: "float", nullable: false),
                    IsActive = table.Column<string>(type: "nvarchar(1)", nullable: false),
                    CreatedAt = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PlayerSeasonResults", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PrestigeLogs",
                schema: "Log",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Timestamp = table.Column<long>(type: "bigint", nullable: false),
                    Name = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false),
                    FactionName = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false),
                    League = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false),
                    Rank = table.Column<int>(type: "integer", nullable: false),
                    TiglUserId = table.Column<int>(type: "integer", nullable: true),
                    TiglUserDiscordId = table.Column<long>(type: "bigint", nullable: false),
                    TiglUserName = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false),
                    MatchId = table.Column<int>(type: "int", nullable: false),
                    Published = table.Column<string>(type: "varchar(10)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PrestigeLogs", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PrestigeRanks",
                schema: "Tigl",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false),
                    FactionName = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false),
                    League = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PrestigeRanks", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Ranks",
                schema: "Tigl",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false),
                    AchievedAt = table.Column<long>(type: "bigint", nullable: false),
                    League = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false),
                    TiglUserId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Ranks", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Ranks_Users_TiglUserId",
                        column: x => x.TiglUserId,
                        principalSchema: "Tigl",
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RankUpLogs",
                schema: "Log",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Timestamp = table.Column<long>(type: "bigint", nullable: false),
                    League = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false),
                    OldRank = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false),
                    NewRank = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false),
                    TiglUserId = table.Column<int>(type: "integer", nullable: true),
                    TiglUserDiscordId = table.Column<long>(type: "bigint", nullable: false),
                    TiglUserName = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false),
                    Duration = table.Column<long>(type: "bigint", nullable: false),
                    MatchId = table.Column<int>(type: "int", nullable: false),
                    Published = table.Column<string>(type: "varchar(10)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RankUpLogs", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "RatingDecays",
                schema: "Tigl",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Amount = table.Column<double>(type: "float", nullable: false),
                    TiglUserId = table.Column<int>(type: "int", nullable: false),
                    Timestamp = table.Column<long>(type: "bigint", nullable: false),
                    League = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false),
                    Season = table.Column<int>(type: "int", nullable: false),
                    RatingSystem = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RatingDecays", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RatingDecays_Users_TiglUserId",
                        column: x => x.TiglUserId,
                        principalSchema: "Tigl",
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TiglUserAchievement",
                schema: "Relationships",
                columns: table => new
                {
                    TiglUserId = table.Column<int>(type: "integer", nullable: false),
                    AchievementId = table.Column<int>(type: "integer", nullable: false),
                    AchievementName = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false),
                    Rank = table.Column<int>(type: "integer", nullable: false),
                    Faction = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false),
                    AchievedAt = table.Column<long>(type: "bigint", nullable: false),
                    MatchId = table.Column<int>(type: "integer", nullable: false),
                    MatchName = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TiglUserAchievement", x => new { x.TiglUserId, x.AchievementId, x.Faction });
                    table.ForeignKey(
                        name: "FK_TiglUserAchievement_Achievements_AchievementId",
                        column: x => x.AchievementId,
                        principalSchema: "Tigl",
                        principalTable: "Achievements",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TiglUserAchievement_Users_TiglUserId",
                        column: x => x.TiglUserId,
                        principalSchema: "Tigl",
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TiglUserPrestigeRank",
                schema: "Relationships",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TiglUserId = table.Column<int>(type: "integer", nullable: false),
                    PrestigeRankId = table.Column<int>(type: "integer", nullable: false),
                    Rank = table.Column<int>(type: "integer", nullable: false),
                    AchievedAt = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TiglUserPrestigeRank", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TiglUserPrestigeRank_PrestigeRanks_PrestigeRankId",
                        column: x => x.PrestigeRankId,
                        principalSchema: "Tigl",
                        principalTable: "PrestigeRanks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TiglUserPrestigeRank_Users_TiglUserId",
                        column: x => x.TiglUserId,
                        principalSchema: "Tigl",
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                schema: "Tigl",
                table: "Achievements",
                columns: new[] { "Id", "Category", "Faction", "Name" },
                values: new object[,]
                {
                    { 1, "Faction", "None", "FactionAmateur" },
                    { 2, "Faction", "None", "FactionEnthusiast" },
                    { 3, "Faction", "None", "FactionExpert" },
                    { 4, "Faction", "None", "FlauntingIt" },
                    { 5, "Wins", "None", "IAmAnOG" },
                    { 6, "Wins", "None", "BaseGameBoss" },
                    { 7, "Wins", "None", "TheProphesiedKing" },
                    { 8, "Wins", "None", "TheTribunii" },
                    { 9, "Wins", "None", "BringTheThunder" },
                    { 10, "Wins", "None", "ThirtyIsNotEnough" },
                    { 11, "Wins", "None", "BlueDaBaDee" },
                    { 12, "Wins", "None", "TyrantsGauntlet" },
                    { 13, "Wins", "None", "DrFrankenstein" },
                    { 14, "Wins", "None", "ICookedThisMyself" },
                    { 15, "Wins", "None", "AllThatsSilverIsGold" },
                    { 16, "Wins", "None", "HomeSystemWhoNeedsThat" },
                    { 17, "Wins", "None", "GottaCatchEmAllPartOne" },
                    { 18, "Wins", "None", "GottaCatchEmAllPartTwo" },
                    { 19, "Wins", "None", "CrusherOfEnemies" },
                    { 20, "Wins", "None", "MarrowEnjoyer" },
                    { 21, "Wins", "None", "BlinkAndYouWillMissIt" },
                    { 22, "Wins", "None", "LongHauler" },
                    { 23, "Wins", "None", "IGotBetter" },
                    { 24, "Wins", "None", "BackToBack" },
                    { 25, "Wins", "None", "Inconceivable" },
                    { 26, "Wins", "None", "UnderdogStory" },
                    { 27, "GameMode", "None", "LiveInInterestingTimes" },
                    { 28, "Games", "None", "FirstSteps" },
                    { 29, "Games", "None", "GettingWarmedUp" },
                    { 30, "Games", "None", "InTheGroove" },
                    { 31, "Games", "None", "SeasonedVeteran" },
                    { 32, "Games", "None", "Centurion" },
                    { 33, "Season", "None", "SpeedDemon" },
                    { 34, "Season", "None", "Marathoner" },
                    { 35, "Wins", "None", "Decathlon" },
                    { 36, "Wins", "None", "DozenDone" },
                    { 37, "Wins", "None", "TheLongWar" },
                    { 38, "Wins", "None", "Versatile" },
                    { 39, "Summary", "None", "Collector" },
                    { 40, "Summary", "None", "Overachiever" },
                    { 41, "Summary", "None", "Completionist" }
                });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "d3f1c4e2-3f4a-4e2b-8f4e-2c3b5e6d7f89", null, "TiglAdmin", "TIGLADMIN" });

            migrationBuilder.InsertData(
                schema: "Tigl",
                table: "Leaders",
                columns: new[] { "Id", "ChangeCount", "CurrentOwnerId", "Faction", "FirstUpdate", "LastUpdate", "League", "LongestDuration", "LongestHolderId", "Name", "PreviousOwnerId", "PreviousUpdate", "ShortestDuration", "ShortestHolderId" },
                values: new object[,]
                {
                    { 1, 0, null, "TheArborec", null, null, "ThundersEdge", null, null, "LetaniMiasmiala", null, null, null, null },
                    { 2, 0, null, "TheBaronyOfLetnev", null, null, "ThundersEdge", null, null, "DarktalonTreilla", null, null, null, null },
                    { 3, 0, null, "TheClanOfSaar", null, null, "ThundersEdge", null, null, "GurnoAggero", null, null, null, null },
                    { 4, 0, null, "TheEmbersOfMuaat", null, null, "ThundersEdge", null, null, "AdjudicatorBaal", null, null, null, null },
                    { 5, 0, null, "TheEmiratesOfHacan", null, null, "ThundersEdge", null, null, "HarrughGefhara", null, null, null, null },
                    { 6, 0, null, "TheFederationOfSol", null, null, "ThundersEdge", null, null, "JaceX4thAirLegion", null, null, null, null },
                    { 7, 0, null, "TheGhostsOfCreuss", null, null, "ThundersEdge", null, null, "RiftwalkerMeian", null, null, null, null },
                    { 8, 0, null, "TheL1z1xMindnet", null, null, "ThundersEdge", null, null, "TheHelmsman", null, null, null, null },
                    { 9, 0, null, "TheMentakCoalition", null, null, "ThundersEdge", null, null, "IpswitchLooseCannon", null, null, null, null },
                    { 10, 0, null, "TheNaaluCollective", null, null, "ThundersEdge", null, null, "TheOracle", null, null, null, null },
                    { 11, 0, null, "TheNekroVirus", null, null, "ThundersEdge", null, null, "UNITDSGNFLAYESH", null, null, null, null },
                    { 12, 0, null, "SardakkNorr", null, null, "ThundersEdge", null, null, "ShvalHarbinger", null, null, null, null },
                    { 13, 0, null, "TheUniversitiesOfJolNar", null, null, "ThundersEdge", null, null, "RinTheMastersLegacy", null, null, null, null },
                    { 14, 0, null, "TheWinnu", null, null, "ThundersEdge", null, null, "MathisMathinus", null, null, null, null },
                    { 15, 0, null, "TheXxchaKingdom", null, null, "ThundersEdge", null, null, "XxekirGrom", null, null, null, null },
                    { 16, 0, null, "TheYinBrotherhood", null, null, "ThundersEdge", null, null, "DannelOfTheTenth", null, null, null, null },
                    { 17, 0, null, "TheYssarilTribes", null, null, "ThundersEdge", null, null, "KyverBladeAndKey", null, null, null, null },
                    { 18, 0, null, "TheArgentFlight", null, null, "ThundersEdge", null, null, "MirikAunSissiri", null, null, null, null },
                    { 19, 0, null, "TheEmpyrean", null, null, "ThundersEdge", null, null, "ConservatorProcyon", null, null, null, null },
                    { 20, 0, null, "TheMahactGeneSorcerers", null, null, "ThundersEdge", null, null, "AiroShirAur", null, null, null, null },
                    { 21, 0, null, "TheNaazRokhaAlliance", null, null, "ThundersEdge", null, null, "HeshAndPrit", null, null, null, null },
                    { 22, 0, null, "TheNomad", null, null, "ThundersEdge", null, null, "AhkSylSiven", null, null, null, null },
                    { 23, 0, null, "TheTitansOfUl", null, null, "ThundersEdge", null, null, "UlTheProgenitor", null, null, null, null },
                    { 24, 0, null, "TheVuilRaithCabal", null, null, "ThundersEdge", null, null, "ItFeedsOnCarrion", null, null, null, null },
                    { 25, 0, null, "TheCouncilKeleresArgent", null, null, "ThundersEdge", null, null, "KuuasiAunJalatai", null, null, null, null },
                    { 26, 0, null, "TheCouncilKeleresMentak", null, null, "ThundersEdge", null, null, "HarkaLeeds", null, null, null, null },
                    { 27, 0, null, "TheCouncilKeleresXxcha", null, null, "ThundersEdge", null, null, "OdlynnMyrr", null, null, null, null },
                    { 28, 0, null, "LastBastion", null, null, "ThundersEdge", null, null, "Entity4X41AApollo", null, null, null, null },
                    { 29, 0, null, "TheRalNelConsortium", null, null, "ThundersEdge", null, null, "DirectorNel", null, null, null, null },
                    { 30, 0, null, "TheDeepwroughtScholarate", null, null, "ThundersEdge", null, null, "TaZern", null, null, null, null },
                    { 31, 0, null, "TheCrimsonRebellion", null, null, "ThundersEdge", null, null, "HomesickPhantom", null, null, null, null },
                    { 32, 0, null, "TheFirmamentTheObsidian", null, null, "ThundersEdge", null, null, "Sharsiss", null, null, null, null },
                    { 33, 0, null, "TheArborec", null, null, "Fractured", null, null, "LetaniMiasmiala", null, null, null, null },
                    { 34, 0, null, "TheBaronyOfLetnev", null, null, "Fractured", null, null, "DarktalonTreilla", null, null, null, null },
                    { 35, 0, null, "TheClanOfSaar", null, null, "Fractured", null, null, "GurnoAggero", null, null, null, null },
                    { 36, 0, null, "TheEmbersOfMuaat", null, null, "Fractured", null, null, "AdjudicatorBaal", null, null, null, null },
                    { 37, 0, null, "TheEmiratesOfHacan", null, null, "Fractured", null, null, "HarrughGefhara", null, null, null, null },
                    { 38, 0, null, "TheFederationOfSol", null, null, "Fractured", null, null, "JaceX4thAirLegion", null, null, null, null },
                    { 39, 0, null, "TheGhostsOfCreuss", null, null, "Fractured", null, null, "RiftwalkerMeian", null, null, null, null },
                    { 40, 0, null, "TheL1z1xMindnet", null, null, "Fractured", null, null, "TheHelmsman", null, null, null, null },
                    { 41, 0, null, "TheMentakCoalition", null, null, "Fractured", null, null, "IpswitchLooseCannon", null, null, null, null },
                    { 42, 0, null, "TheNaaluCollective", null, null, "Fractured", null, null, "TheOracle", null, null, null, null },
                    { 43, 0, null, "TheNekroVirus", null, null, "Fractured", null, null, "UNITDSGNFLAYESH", null, null, null, null },
                    { 44, 0, null, "SardakkNorr", null, null, "Fractured", null, null, "ShvalHarbinger", null, null, null, null },
                    { 45, 0, null, "TheUniversitiesOfJolNar", null, null, "Fractured", null, null, "RinTheMastersLegacy", null, null, null, null },
                    { 46, 0, null, "TheWinnu", null, null, "Fractured", null, null, "MathisMathinus", null, null, null, null },
                    { 47, 0, null, "TheXxchaKingdom", null, null, "Fractured", null, null, "XxekirGrom", null, null, null, null },
                    { 48, 0, null, "TheYinBrotherhood", null, null, "Fractured", null, null, "DannelOfTheTenth", null, null, null, null },
                    { 49, 0, null, "TheYssarilTribes", null, null, "Fractured", null, null, "KyverBladeAndKey", null, null, null, null },
                    { 50, 0, null, "TheArgentFlight", null, null, "Fractured", null, null, "MirikAunSissiri", null, null, null, null },
                    { 51, 0, null, "TheEmpyrean", null, null, "Fractured", null, null, "ConservatorProcyon", null, null, null, null },
                    { 52, 0, null, "TheMahactGeneSorcerers", null, null, "Fractured", null, null, "AiroShirAur", null, null, null, null },
                    { 53, 0, null, "TheNaazRokhaAlliance", null, null, "Fractured", null, null, "HeshAndPrit", null, null, null, null },
                    { 54, 0, null, "TheNomad", null, null, "Fractured", null, null, "AhkSylSiven", null, null, null, null },
                    { 55, 0, null, "TheTitansOfUl", null, null, "Fractured", null, null, "UlTheProgenitor", null, null, null, null },
                    { 56, 0, null, "TheVuilRaithCabal", null, null, "Fractured", null, null, "ItFeedsOnCarrion", null, null, null, null },
                    { 57, 0, null, "TheCouncilKeleresArgent", null, null, "Fractured", null, null, "KuuasiAunJalatai", null, null, null, null },
                    { 58, 0, null, "TheCouncilKeleresMentak", null, null, "Fractured", null, null, "HarkaLeeds", null, null, null, null },
                    { 59, 0, null, "TheCouncilKeleresXxcha", null, null, "Fractured", null, null, "OdlynnMyrr", null, null, null, null },
                    { 60, 0, null, "LastBastion", null, null, "Fractured", null, null, "Entity4X41AApollo", null, null, null, null },
                    { 61, 0, null, "TheRalNelConsortium", null, null, "Fractured", null, null, "DirectorNel", null, null, null, null },
                    { 62, 0, null, "TheDeepwroughtScholarate", null, null, "Fractured", null, null, "TaZern", null, null, null, null },
                    { 63, 0, null, "TheCrimsonRebellion", null, null, "Fractured", null, null, "HomesickPhantom", null, null, null, null },
                    { 64, 0, null, "TheFirmamentTheObsidian", null, null, "Fractured", null, null, "Sharsiss", null, null, null, null },
                    { 65, 0, null, "TheRubyMonarch", null, null, "Fractured", null, null, "TheRubyMonarch", null, null, null, null },
                    { 66, 0, null, "RadiantAur", null, null, "Fractured", null, null, "RadiantAur", null, null, null, null },
                    { 67, 0, null, "AvariceRex", null, null, "Fractured", null, null, "AvariceRex", null, null, null, null },
                    { 68, 0, null, "IlSaiLakoeHeraldOfThorns", null, null, "Fractured", null, null, "IlSaiLakoeHeraldOfThorns", null, null, null, null },
                    { 69, 0, null, "TheSaintOfSwords", null, null, "Fractured", null, null, "TheSaintOfSwords", null, null, null, null },
                    { 70, 0, null, "IlNaViroset", null, null, "Fractured", null, null, "IlNaViroset", null, null, null, null },
                    { 71, 0, null, "ElNenJanovet", null, null, "Fractured", null, null, "ElNenJanovet", null, null, null, null },
                    { 72, 0, null, "ASickeningLurch", null, null, "Fractured", null, null, "ASickeningLurch", null, null, null, null },
                    { 73, 0, null, "TheAugursOfIlyxum", null, null, "Fractured", null, null, "AtrophaWeaver", null, null, null, null },
                    { 74, 0, null, "TheBentorConglomerate", null, null, "Fractured", null, null, "CEOKenTuccVisionaryExplorer", null, null, null, null },
                    { 75, 0, null, "TheBerserkersOfKjalengard", null, null, "Fractured", null, null, "YgegnadTheThunderHonorarySkald", null, null, null, null },
                    { 76, 0, null, "TheCeldauriTradeConfederation", null, null, "Fractured", null, null, "TitusFlaviusCouncilman", null, null, null, null },
                    { 77, 0, null, "TheCheiranHordes", null, null, "Fractured", null, null, "ThaktClquaPolemarch", null, null, null, null },
                    { 78, 0, null, "TheDihMohnFlotilla", null, null, "Fractured", null, null, "VerrisusYpruFormerAdmiralOfTheUnrelentingBattlegroup", null, null, null, null },
                    { 79, 0, null, "TheEdynMandate", null, null, "Fractured", null, null, "MidirLivingWill", null, null, null, null },
                    { 80, 0, null, "TheFlorzenProfiteers", null, null, "Fractured", null, null, "BanuaGowenAdministratorOfMinds", null, null, null, null },
                    { 81, 0, null, "TheFreeSystemsCompact", null, null, "Fractured", null, null, "CountOttoPmayInspiringRhetorician", null, null, null, null },
                    { 82, 0, null, "TheGheminaRaiders", null, null, "Fractured", null, null, "KorelaTheLadyAndKantrusTheLord", null, null, null, null },
                    { 83, 0, null, "TheGhotiWayfarers", null, null, "Fractured", null, null, "NmenmedeGhotiAllMother", null, null, null, null },
                    { 84, 0, null, "TheGledgeUnion", null, null, "Fractured", null, null, "GorthrimChiefOfExpeditions", null, null, null, null },
                    { 85, 0, null, "TheGlimmerOfMortheus", null, null, "Fractured", null, null, "BayanDeepMagenta", null, null, null, null },
                    { 86, 0, null, "TheKolleccSociety", null, null, "Fractured", null, null, "DorrahnGriphynTheCollector", null, null, null, null },
                    { 87, 0, null, "TheKortaliTribunal", null, null, "Fractured", null, null, "QueenNadaliaLifeAndDeath", null, null, null, null },
                    { 88, 0, null, "TheKyroSodality", null, null, "Fractured", null, null, "SpeyghBlightmaster", null, null, null, null },
                    { 89, 0, null, "TheLanefirRemnants", null, null, "Fractured", null, null, "TheVenerableKeeperOfMyths", null, null, null, null },
                    { 90, 0, null, "TheLiZhoDynasty", null, null, "Fractured", null, null, "KhazRinLiZhoEmpress", null, null, null, null },
                    { 91, 0, null, "TheLTokkKhrask", null, null, "Fractured", null, null, "VehlTikarArchDruid", null, null, null, null },
                    { 92, 0, null, "TheMirvedaProtectorate", null, null, "Fractured", null, null, "WrathMachinaAIMainframe", null, null, null, null },
                    { 93, 0, null, "TheMonksOfKolume", null, null, "Fractured", null, null, "WonellTheSilentGrandmasterOfTheOrder", null, null, null, null },
                    { 94, 0, null, "TheMykoMentori", null, null, "Fractured", null, null, "CoprinusComatusNecromancer", null, null, null, null },
                    { 95, 0, null, "TheNivynStarKings", null, null, "Fractured", null, null, "KrillDrakkonStarCrownedKing", null, null, null, null },
                    { 96, 0, null, "TheNokarSellships", null, null, "Fractured", null, null, "StarsailsMercenaryKing", null, null, null, null },
                    { 97, 0, null, "TheOlradinLeague", null, null, "Fractured", null, null, "PahnSilverfurCouncilSpeaker", null, null, null, null },
                    { 98, 0, null, "RohDhnaMechatronics", null, null, "Fractured", null, null, "RohVhinDhnaMK4RuthlessExecutive", null, null, null, null },
                    { 99, 0, null, "TheSavagesOfCymiae", null, null, "Fractured", null, null, "TheVoiceUnitedPsionicMaelstrom", null, null, null, null },
                    { 100, 0, null, "TheShipwrightsofAxis", null, null, "Fractured", null, null, "DemiQueenMdckssskCommissionerOfProfits", null, null, null, null },
                    { 101, 0, null, "TheTnelisSyndicate", null, null, "Fractured", null, null, "TurraSveyarShadowCouncilor", null, null, null, null },
                    { 102, 0, null, "TheVadenBankingClans", null, null, "Fractured", null, null, "PutrivSirvonskClanmasterPrime", null, null, null, null },
                    { 103, 0, null, "TheVaylerianScourge", null, null, "Fractured", null, null, "DylnHarthuulViceAdmiralOfFleetGroup15", null, null, null, null },
                    { 104, 0, null, "TheVeldyrSovereignty", null, null, "Fractured", null, null, "AuberonElyrinChairman", null, null, null, null },
                    { 105, 0, null, "TheZealotsOfRhodun", null, null, "Fractured", null, null, "SaintBinalTheProphet", null, null, null, null },
                    { 106, 0, null, "TheZelianPurifier", null, null, "Fractured", null, null, "ZelianRTheDestroyer", null, null, null, null },
                    { 107, 0, null, "AtokeraLegacy", null, null, "Fractured", null, null, "KapokoVuiGreatSpirit", null, null, null, null },
                    { 108, 0, null, "BelkoseaAlliedStates", null, null, "Fractured", null, null, "MobiusSpikeTheReaper", null, null, null, null },
                    { 109, 0, null, "PharadnOrder", null, null, "Fractured", null, null, "PharadnTheImmortalImperishableUnifier", null, null, null, null },
                    { 110, 0, null, "QhetRepublic", null, null, "Fractured", null, null, "TvorKhageShieldOfQhet", null, null, null, null },
                    { 111, 0, null, "ToldarConcordat", null, null, "Fractured", null, null, "CadhAcamontConcordatMarshall", null, null, null, null },
                    { 112, 0, null, "UydaiConclave", null, null, "Fractured", null, null, "LondorIIGodEmperor", null, null, null, null },
                    { 113, 0, null, "Franken", null, null, "Fractured", null, null, "Franken", null, null, null, null },
                    { 114, 0, null, "Homebrew", null, null, "Fractured", null, null, "Homebrew", null, null, null, null },
                    { 115, 0, null, "TwilightsFall", null, null, "Fractured", null, null, "TwilightsFall", null, null, null, null }
                });

            migrationBuilder.InsertData(
                schema: "Tigl",
                table: "Parameters",
                columns: new[] { "Id", "Enabled", "Name" },
                values: new object[,]
                {
                    { 3, "true", "ManualGameReview" },
                    { 4, "false", "RankingUpProphecyOfKings" },
                    { 5, "false", "TiglTestUserRegistrations" },
                    { 6, "false", "OnlyImportReports" },
                    { 7, "false", "AchievementsPublish" },
                    { 8, "false", "HeroPublish" },
                    { 9, "false", "RankUpPublish" },
                    { 10, "true", "OnlyThundersEdgeEra" },
                    { 11, "false", "DiscordInteraction" }
                });

            migrationBuilder.InsertData(
                schema: "Tigl",
                table: "PrestigeRanks",
                columns: new[] { "Id", "FactionName", "League", "Name" },
                values: new object[,]
                {
                    { 1, "TheArborec", "ProphecyOfKings", "LetaniMiasmiala" },
                    { 2, "TheBaronyOfLetnev", "ProphecyOfKings", "DarktalonTreilla" },
                    { 3, "TheClanOfSaar", "ProphecyOfKings", "GurnoAggero" },
                    { 4, "TheEmbersOfMuaat", "ProphecyOfKings", "AdjudicatorBaal" },
                    { 5, "TheEmiratesOfHacan", "ProphecyOfKings", "HarrughGefhara" },
                    { 6, "TheFederationOfSol", "ProphecyOfKings", "JaceX4thAirLegion" },
                    { 7, "TheGhostsOfCreuss", "ProphecyOfKings", "RiftwalkerMeian" },
                    { 8, "TheL1z1xMindnet", "ProphecyOfKings", "TheHelmsman" },
                    { 9, "TheMentakCoalition", "ProphecyOfKings", "IpswitchLooseCannon" },
                    { 10, "TheNaaluCollective", "ProphecyOfKings", "TheOracle" },
                    { 11, "TheNekroVirus", "ProphecyOfKings", "UNITDSGNFLAYESH" },
                    { 12, "SardakkNorr", "ProphecyOfKings", "ShvalHarbinger" },
                    { 13, "TheUniversitiesOfJolNar", "ProphecyOfKings", "RinTheMastersLegacy" },
                    { 14, "TheWinnu", "ProphecyOfKings", "MathisMathinus" },
                    { 15, "TheXxchaKingdom", "ProphecyOfKings", "XxekirGrom" },
                    { 16, "TheYinBrotherhood", "ProphecyOfKings", "DannelOfTheTenth" },
                    { 17, "TheYssarilTribes", "ProphecyOfKings", "KyverBladeAndKey" },
                    { 18, "TheArgentFlight", "ProphecyOfKings", "MirikAunSissiri" },
                    { 19, "TheEmpyrean", "ProphecyOfKings", "ConservatorProcyon" },
                    { 20, "TheMahactGeneSorcerers", "ProphecyOfKings", "AiroShirAur" },
                    { 21, "TheNaazRokhaAlliance", "ProphecyOfKings", "HeshAndPrit" },
                    { 22, "TheNomad", "ProphecyOfKings", "AhkSylSiven" },
                    { 23, "TheTitansOfUl", "ProphecyOfKings", "UlTheProgenitor" },
                    { 24, "TheVuilRaithCabal", "ProphecyOfKings", "ItFeedsOnCarrion" },
                    { 25, "TheCouncilKeleresArgent", "ProphecyOfKings", "KuuasiAunJalatai" },
                    { 26, "TheCouncilKeleresMentak", "ProphecyOfKings", "HarkaLeeds" },
                    { 27, "TheCouncilKeleresXxcha", "ProphecyOfKings", "OdlynnMyrr" },
                    { 28, "TheArborec", "ThundersEdge", "LetaniMiasmiala" },
                    { 29, "TheBaronyOfLetnev", "ThundersEdge", "DarktalonTreilla" },
                    { 30, "TheClanOfSaar", "ThundersEdge", "GurnoAggero" },
                    { 31, "TheEmbersOfMuaat", "ThundersEdge", "AdjudicatorBaal" },
                    { 32, "TheEmiratesOfHacan", "ThundersEdge", "HarrughGefhara" },
                    { 33, "TheFederationOfSol", "ThundersEdge", "JaceX4thAirLegion" },
                    { 34, "TheGhostsOfCreuss", "ThundersEdge", "RiftwalkerMeian" },
                    { 35, "TheL1z1xMindnet", "ThundersEdge", "TheHelmsman" },
                    { 36, "TheMentakCoalition", "ThundersEdge", "IpswitchLooseCannon" },
                    { 37, "TheNaaluCollective", "ThundersEdge", "TheOracle" },
                    { 38, "TheNekroVirus", "ThundersEdge", "UNITDSGNFLAYESH" },
                    { 39, "SardakkNorr", "ThundersEdge", "ShvalHarbinger" },
                    { 40, "TheUniversitiesOfJolNar", "ThundersEdge", "RinTheMastersLegacy" },
                    { 41, "TheWinnu", "ThundersEdge", "MathisMathinus" },
                    { 42, "TheXxchaKingdom", "ThundersEdge", "XxekirGrom" },
                    { 43, "TheYinBrotherhood", "ThundersEdge", "DannelOfTheTenth" },
                    { 44, "TheYssarilTribes", "ThundersEdge", "KyverBladeAndKey" },
                    { 45, "TheArgentFlight", "ThundersEdge", "MirikAunSissiri" },
                    { 46, "TheEmpyrean", "ThundersEdge", "ConservatorProcyon" },
                    { 47, "TheMahactGeneSorcerers", "ThundersEdge", "AiroShirAur" },
                    { 48, "TheNaazRokhaAlliance", "ThundersEdge", "HeshAndPrit" },
                    { 49, "TheNomad", "ThundersEdge", "AhkSylSiven" },
                    { 50, "TheTitansOfUl", "ThundersEdge", "UlTheProgenitor" },
                    { 51, "TheVuilRaithCabal", "ThundersEdge", "ItFeedsOnCarrion" },
                    { 52, "TheCouncilKeleresArgent", "ThundersEdge", "KuuasiAunJalatai" },
                    { 53, "TheCouncilKeleresMentak", "ThundersEdge", "HarkaLeeds" },
                    { 54, "TheCouncilKeleresXxcha", "ThundersEdge", "OdlynnMyrr" },
                    { 55, "LastBastion", "ThundersEdge", "Entity4X41AApollo" },
                    { 56, "TheRalNelConsortium", "ThundersEdge", "DirectorNel" },
                    { 57, "TheDeepwroughtScholarate", "ThundersEdge", "TaZern" },
                    { 58, "TheCrimsonRebellion", "ThundersEdge", "HomesickPhantom" },
                    { 59, "TheFirmamentTheObsidian", "ThundersEdge", "Sharsiss" },
                    { 60, "TheArborec", "Fractured", "LetaniMiasmiala" },
                    { 61, "TheBaronyOfLetnev", "Fractured", "DarktalonTreilla" },
                    { 62, "TheClanOfSaar", "Fractured", "GurnoAggero" },
                    { 63, "TheEmbersOfMuaat", "Fractured", "AdjudicatorBaal" },
                    { 64, "TheEmiratesOfHacan", "Fractured", "HarrughGefhara" },
                    { 65, "TheFederationOfSol", "Fractured", "JaceX4thAirLegion" },
                    { 66, "TheGhostsOfCreuss", "Fractured", "RiftwalkerMeian" },
                    { 67, "TheL1z1xMindnet", "Fractured", "TheHelmsman" },
                    { 68, "TheMentakCoalition", "Fractured", "IpswitchLooseCannon" },
                    { 69, "TheNaaluCollective", "Fractured", "TheOracle" },
                    { 70, "TheNekroVirus", "Fractured", "UNITDSGNFLAYESH" },
                    { 71, "SardakkNorr", "Fractured", "ShvalHarbinger" },
                    { 72, "TheUniversitiesOfJolNar", "Fractured", "RinTheMastersLegacy" },
                    { 73, "TheWinnu", "Fractured", "MathisMathinus" },
                    { 74, "TheXxchaKingdom", "Fractured", "XxekirGrom" },
                    { 75, "TheYinBrotherhood", "Fractured", "DannelOfTheTenth" },
                    { 76, "TheYssarilTribes", "Fractured", "KyverBladeAndKey" },
                    { 77, "TheArgentFlight", "Fractured", "MirikAunSissiri" },
                    { 78, "TheEmpyrean", "Fractured", "ConservatorProcyon" },
                    { 79, "TheMahactGeneSorcerers", "Fractured", "AiroShirAur" },
                    { 80, "TheNaazRokhaAlliance", "Fractured", "HeshAndPrit" },
                    { 81, "TheNomad", "Fractured", "AhkSylSiven" },
                    { 82, "TheTitansOfUl", "Fractured", "UlTheProgenitor" },
                    { 83, "TheVuilRaithCabal", "Fractured", "ItFeedsOnCarrion" },
                    { 84, "TheCouncilKeleresArgent", "Fractured", "KuuasiAunJalatai" },
                    { 85, "TheCouncilKeleresMentak", "Fractured", "HarkaLeeds" },
                    { 86, "TheCouncilKeleresXxcha", "Fractured", "OdlynnMyrr" },
                    { 87, "LastBastion", "Fractured", "Entity4X41AApollo" },
                    { 88, "TheRalNelConsortium", "Fractured", "DirectorNel" },
                    { 89, "TheDeepwroughtScholarate", "Fractured", "TaZern" },
                    { 90, "TheCrimsonRebellion", "Fractured", "HomesickPhantom" },
                    { 91, "TheFirmamentTheObsidian", "Fractured", "Sharsiss" },
                    { 92, "TheRubyMonarch", "Fractured", "TheRubyMonarch" },
                    { 93, "RadiantAur", "Fractured", "RadiantAur" },
                    { 94, "AvariceRex", "Fractured", "AvariceRex" },
                    { 95, "IlSaiLakoeHeraldOfThorns", "Fractured", "IlSaiLakoeHeraldOfThorns" },
                    { 96, "TheSaintOfSwords", "Fractured", "TheSaintOfSwords" },
                    { 97, "IlNaViroset", "Fractured", "IlNaViroset" },
                    { 98, "ElNenJanovet", "Fractured", "ElNenJanovet" },
                    { 99, "ASickeningLurch", "Fractured", "ASickeningLurch" },
                    { 100, "TheAugursOfIlyxum", "Fractured", "AtrophaWeaver" },
                    { 101, "TheBentorConglomerate", "Fractured", "CEOKenTuccVisionaryExplorer" },
                    { 102, "TheBerserkersOfKjalengard", "Fractured", "YgegnadTheThunderHonorarySkald" },
                    { 103, "TheCeldauriTradeConfederation", "Fractured", "TitusFlaviusCouncilman" },
                    { 104, "TheCheiranHordes", "Fractured", "ThaktClquaPolemarch" },
                    { 105, "TheDihMohnFlotilla", "Fractured", "VerrisusYpruFormerAdmiralOfTheUnrelentingBattlegroup" },
                    { 106, "TheEdynMandate", "Fractured", "MidirLivingWill" },
                    { 107, "TheFlorzenProfiteers", "Fractured", "BanuaGowenAdministratorOfMinds" },
                    { 108, "TheFreeSystemsCompact", "Fractured", "CountOttoPmayInspiringRhetorician" },
                    { 109, "TheGheminaRaiders", "Fractured", "KorelaTheLadyAndKantrusTheLord" },
                    { 110, "TheGhotiWayfarers", "Fractured", "NmenmedeGhotiAllMother" },
                    { 111, "TheGledgeUnion", "Fractured", "GorthrimChiefOfExpeditions" },
                    { 112, "TheGlimmerOfMortheus", "Fractured", "BayanDeepMagenta" },
                    { 113, "TheKolleccSociety", "Fractured", "DorrahnGriphynTheCollector" },
                    { 114, "TheKortaliTribunal", "Fractured", "QueenNadaliaLifeAndDeath" },
                    { 115, "TheKyroSodality", "Fractured", "SpeyghBlightmaster" },
                    { 116, "TheLanefirRemnants", "Fractured", "TheVenerableKeeperOfMyths" },
                    { 117, "TheLiZhoDynasty", "Fractured", "KhazRinLiZhoEmpress" },
                    { 118, "TheLTokkKhrask", "Fractured", "VehlTikarArchDruid" },
                    { 119, "TheMirvedaProtectorate", "Fractured", "WrathMachinaAIMainframe" },
                    { 120, "TheMonksOfKolume", "Fractured", "WonellTheSilentGrandmasterOfTheOrder" },
                    { 121, "TheMykoMentori", "Fractured", "CoprinusComatusNecromancer" },
                    { 122, "TheNivynStarKings", "Fractured", "KrillDrakkonStarCrownedKing" },
                    { 123, "TheNokarSellships", "Fractured", "StarsailsMercenaryKing" },
                    { 124, "TheOlradinLeague", "Fractured", "PahnSilverfurCouncilSpeaker" },
                    { 125, "RohDhnaMechatronics", "Fractured", "RohVhinDhnaMK4RuthlessExecutive" },
                    { 126, "TheSavagesOfCymiae", "Fractured", "TheVoiceUnitedPsionicMaelstrom" },
                    { 127, "TheShipwrightsofAxis", "Fractured", "DemiQueenMdckssskCommissionerOfProfits" },
                    { 128, "TheTnelisSyndicate", "Fractured", "TurraSveyarShadowCouncilor" },
                    { 129, "TheVadenBankingClans", "Fractured", "PutrivSirvonskClanmasterPrime" },
                    { 130, "TheVaylerianScourge", "Fractured", "DylnHarthuulViceAdmiralOfFleetGroup15" },
                    { 131, "TheVeldyrSovereignty", "Fractured", "AuberonElyrinChairman" },
                    { 132, "TheZealotsOfRhodun", "Fractured", "SaintBinalTheProphet" },
                    { 133, "TheZelianPurifier", "Fractured", "ZelianRTheDestroyer" },
                    { 134, "AtokeraLegacy", "Fractured", "KapokoVuiGreatSpirit" },
                    { 135, "BelkoseaAlliedStates", "Fractured", "MobiusSpikeTheReaper" },
                    { 136, "PharadnOrder", "Fractured", "PharadnTheImmortalImperishableUnifier" },
                    { 137, "QhetRepublic", "Fractured", "TvorKhageShieldOfQhet" },
                    { 138, "ToldarConcordat", "Fractured", "CadhAcamontConcordatMarshall" },
                    { 139, "UydaiConclave", "Fractured", "LondorIIGodEmperor" },
                    { 140, "Franken", "Fractured", "Franken" },
                    { 141, "Homebrew", "Fractured", "Homebrew" },
                    { 142, "TwilightsFall", "Fractured", "TwilightsFall" },
                    { 143, "None", "ProphecyOfKings", "GalacticThreat" },
                    { 144, "None", "ProphecyOfKings", "PaxMagnificaBellumGloriosum" },
                    { 145, "None", "ThundersEdge", "GalacticThreat" },
                    { 146, "None", "ThundersEdge", "PaxMagnificaBellumGloriosum" },
                    { 147, "None", "Fractured", "Tyrant" },
                    { 148, "None", "ProphecyOfKings", "DeposedEmperor" },
                    { 149, "None", "ThundersEdge", "DeposedEmperor" },
                    { 150, "None", "Fractured", "DeposedEmperor" }
                });

            migrationBuilder.UpdateData(
                schema: "Tigl",
                table: "Seasons",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "EmperorId", "EndDate", "Name", "StartDate" },
                values: new object[] { null, new DateOnly(2025, 11, 30), "Season 1", new DateOnly(2025, 11, 30) });

            migrationBuilder.CreateIndex(
                name: "IX_TrueSkillStats_TiglUserId_League",
                schema: "Tigl",
                table: "TrueSkillStats",
                columns: new[] { "TiglUserId", "League" });

            migrationBuilder.CreateIndex(
                name: "IX_TrueSkillPlayerMatchStats_DiscordUserId",
                schema: "Tigl",
                table: "TrueSkillPlayerMatchStats",
                column: "DiscordUserId");

            migrationBuilder.CreateIndex(
                name: "IX_TrueSkillPlayerMatchStats_Faction",
                schema: "Tigl",
                table: "TrueSkillPlayerMatchStats",
                column: "Faction");

            migrationBuilder.CreateIndex(
                name: "IX_TrueSkillPlayerMatchStats_IsRankUpGame",
                schema: "Tigl",
                table: "TrueSkillPlayerMatchStats",
                column: "IsRankUpGame");

            migrationBuilder.CreateIndex(
                name: "IX_TrueSkillPlayerMatchStats_Season",
                schema: "Tigl",
                table: "TrueSkillPlayerMatchStats",
                column: "Season");

            migrationBuilder.CreateIndex(
                name: "IX_TrueSkillPlayerMatchStats_StartTimestamp",
                schema: "Tigl",
                table: "TrueSkillPlayerMatchStats",
                column: "StartTimestamp");

            migrationBuilder.CreateIndex(
                name: "IX_TrueSkillPlayerMatchStats_Winner",
                schema: "Tigl",
                table: "TrueSkillPlayerMatchStats",
                column: "Winner");

            migrationBuilder.CreateIndex(
                name: "IX_Seasons_EmperorId",
                schema: "Tigl",
                table: "Seasons",
                column: "EmperorId");

            migrationBuilder.CreateIndex(
                name: "IX_Seasons_SeasonNumber",
                schema: "Tigl",
                table: "Seasons",
                column: "SeasonNumber",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_PlayerResults_Faction_Score",
                schema: "Tigl",
                table: "PlayerResults",
                columns: new[] { "Faction", "Score" });

            migrationBuilder.CreateIndex(
                name: "IX_PlayerResults_TiglUser",
                schema: "Tigl",
                table: "PlayerResults",
                column: "TiglUserId");

            migrationBuilder.CreateIndex(
                name: "IX_MatchReports_GameId_Source",
                schema: "Tigl",
                table: "MatchReports",
                columns: new[] { "GameId", "Source" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_MatchReports_League_Season",
                schema: "Tigl",
                table: "MatchReports",
                columns: new[] { "League", "Season" });

            migrationBuilder.CreateIndex(
                name: "IX_GlickoStats_TiglUserId_League",
                schema: "Tigl",
                table: "GlickoStats",
                columns: new[] { "TiglUserId", "League" });

            migrationBuilder.CreateIndex(
                name: "IX_GlickoRatings_GlickoStatsId",
                schema: "Tigl",
                table: "GlickoRatings",
                column: "GlickoStatsId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_GlickoPlayerMatchStats_DiscordUserId",
                schema: "Tigl",
                table: "GlickoPlayerMatchStats",
                column: "DiscordUserId");

            migrationBuilder.CreateIndex(
                name: "IX_GlickoPlayerMatchStats_Faction",
                schema: "Tigl",
                table: "GlickoPlayerMatchStats",
                column: "Faction");

            migrationBuilder.CreateIndex(
                name: "IX_GlickoPlayerMatchStats_IsRankUpGame",
                schema: "Tigl",
                table: "GlickoPlayerMatchStats",
                column: "IsRankUpGame");

            migrationBuilder.CreateIndex(
                name: "IX_GlickoPlayerMatchStats_Season",
                schema: "Tigl",
                table: "GlickoPlayerMatchStats",
                column: "Season");

            migrationBuilder.CreateIndex(
                name: "IX_GlickoPlayerMatchStats_StartTimestamp",
                schema: "Tigl",
                table: "GlickoPlayerMatchStats",
                column: "StartTimestamp");

            migrationBuilder.CreateIndex(
                name: "IX_GlickoPlayerMatchStats_Winner",
                schema: "Tigl",
                table: "GlickoPlayerMatchStats",
                column: "Winner");

            migrationBuilder.CreateIndex(
                name: "IX_AsyncStats_TiglUserId_League",
                schema: "Tigl",
                table: "AsyncStats",
                columns: new[] { "TiglUserId", "League" });

            migrationBuilder.CreateIndex(
                name: "IX_AsyncPlayerStats_FactionName",
                schema: "Statistics",
                table: "AsyncPlayerStats",
                column: "FactionName")
                .Annotation("SqlServer:Clustered", false);

            migrationBuilder.CreateIndex(
                name: "IX_AsyncPlayerMatchStats_DiscordUserId",
                schema: "Tigl",
                table: "AsyncPlayerMatchStats",
                column: "DiscordUserId");

            migrationBuilder.CreateIndex(
                name: "IX_AsyncPlayerMatchStats_Faction",
                schema: "Tigl",
                table: "AsyncPlayerMatchStats",
                column: "Faction");

            migrationBuilder.CreateIndex(
                name: "IX_AsyncPlayerMatchStats_IsRankUpGame",
                schema: "Tigl",
                table: "AsyncPlayerMatchStats",
                column: "IsRankUpGame");

            migrationBuilder.CreateIndex(
                name: "IX_AsyncPlayerMatchStats_Season",
                schema: "Tigl",
                table: "AsyncPlayerMatchStats",
                column: "Season");

            migrationBuilder.CreateIndex(
                name: "IX_AsyncPlayerMatchStats_StartTimestamp",
                schema: "Tigl",
                table: "AsyncPlayerMatchStats",
                column: "StartTimestamp");

            migrationBuilder.CreateIndex(
                name: "IX_AsyncPlayerMatchStats_Winner",
                schema: "Tigl",
                table: "AsyncPlayerMatchStats",
                column: "Winner");

            migrationBuilder.CreateIndex(
                name: "IX_AsyncGameStats_Setup_Ended",
                schema: "Statistics",
                table: "AsyncGameStats",
                columns: new[] { "SetupTimestamp", "EndedTimestamp" });

            migrationBuilder.CreateIndex(
                name: "IX_Achievements_Faction_Category",
                schema: "Tigl",
                table: "Achievements",
                columns: new[] { "Faction", "Category" });

            migrationBuilder.CreateIndex(
                name: "IX_GamePublish_Published_CreatedAt",
                schema: "Log",
                table: "GamePublishLogs",
                columns: new[] { "Published", "CreatedAt" });

            migrationBuilder.CreateIndex(
                name: "IX_LeaderLogs_League_Faction_Name_Timestamp_AfterDuration",
                schema: "Log",
                table: "LeaderLogs",
                columns: new[] { "League", "Faction", "Name", "Timestamp", "Duration" });

            migrationBuilder.CreateIndex(
                name: "IX_LeaderLogs_NewOwnerId_NewOwnerDiscordId_PreviousOwnerId_PreviousOwnerDiscordId",
                schema: "Log",
                table: "LeaderLogs",
                columns: new[] { "NewOwnerId", "NewOwnerDiscordId", "PreviousOwnerId", "PreviousOwnerDiscordId" });

            migrationBuilder.CreateIndex(
                name: "IX_Leaders_CurrentOwnerId",
                schema: "Tigl",
                table: "Leaders",
                column: "CurrentOwnerId");

            migrationBuilder.CreateIndex(
                name: "IX_Leaders_Faction_League",
                schema: "Tigl",
                table: "Leaders",
                columns: new[] { "Faction", "League" });

            migrationBuilder.CreateIndex(
                name: "IX_Leaders_LongestHolderId",
                schema: "Tigl",
                table: "Leaders",
                column: "LongestHolderId");

            migrationBuilder.CreateIndex(
                name: "IX_Leaders_ShortestHolderId",
                schema: "Tigl",
                table: "Leaders",
                column: "ShortestHolderId");

            migrationBuilder.CreateIndex(
                name: "IX_PlayerSeasonResults_League",
                schema: "Tigl",
                table: "PlayerSeasonResults",
                column: "League");

            migrationBuilder.CreateIndex(
                name: "IX_PlayerSeasonResults_Season",
                schema: "Tigl",
                table: "PlayerSeasonResults",
                column: "Season");

            migrationBuilder.CreateIndex(
                name: "IX_PlayerSeasonResults_TiglUserId",
                schema: "Tigl",
                table: "PlayerSeasonResults",
                column: "TiglUserId");

            migrationBuilder.CreateIndex(
                name: "IX_PrestigeLogs_League_TiglUserDiscordId_Faction_Name_Timestamp_TiglUserId",
                schema: "Log",
                table: "PrestigeLogs",
                columns: new[] { "League", "TiglUserDiscordId", "FactionName", "Name", "Timestamp", "TiglUserId" });

            migrationBuilder.CreateIndex(
                name: "IX_PrestigeRanks_Name_Faction_League",
                schema: "Tigl",
                table: "PrestigeRanks",
                columns: new[] { "Name", "FactionName", "League" });

            migrationBuilder.CreateIndex(
                name: "IX_Ranks_AchievedAt",
                schema: "Tigl",
                table: "Ranks",
                column: "AchievedAt");

            migrationBuilder.CreateIndex(
                name: "IX_Ranks_Name_League",
                schema: "Tigl",
                table: "Ranks",
                columns: new[] { "Name", "League" });

            migrationBuilder.CreateIndex(
                name: "IX_Ranks_TiglUserId",
                schema: "Tigl",
                table: "Ranks",
                column: "TiglUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Ranks_User_League_AchievedAt",
                schema: "Tigl",
                table: "Ranks",
                columns: new[] { "TiglUserId", "League", "AchievedAt" });

            migrationBuilder.CreateIndex(
                name: "IX_RankUpLogs_League_TiglUserDiscordId_OldRank_NewRank_Timestamp_Duration",
                schema: "Log",
                table: "RankUpLogs",
                columns: new[] { "League", "TiglUserDiscordId", "OldRank", "NewRank", "Timestamp", "Duration" });

            migrationBuilder.CreateIndex(
                name: "IX_RatingDecays_TiglUserId",
                schema: "Tigl",
                table: "RatingDecays",
                column: "TiglUserId");

            migrationBuilder.CreateIndex(
                name: "IX_TiglUserAchievement_AchievementId",
                schema: "Relationships",
                table: "TiglUserAchievement",
                column: "AchievementId");

            migrationBuilder.CreateIndex(
                name: "IX_TiglUserPrestigeRank_PrestigeRankId",
                schema: "Relationships",
                table: "TiglUserPrestigeRank",
                column: "PrestigeRankId");

            migrationBuilder.CreateIndex(
                name: "IX_TiglUserPrestigeRank_TiglUserId_PrestigeRankId",
                schema: "Relationships",
                table: "TiglUserPrestigeRank",
                columns: new[] { "TiglUserId", "PrestigeRankId" });

            migrationBuilder.AddForeignKey(
                name: "FK_GlickoRatings_GlickoStats_GlickoStatsId",
                schema: "Tigl",
                table: "GlickoRatings",
                column: "GlickoStatsId",
                principalSchema: "Tigl",
                principalTable: "GlickoStats",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Seasons_Users_EmperorId",
                schema: "Tigl",
                table: "Seasons",
                column: "EmperorId",
                principalSchema: "Tigl",
                principalTable: "Users",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GlickoRatings_GlickoStats_GlickoStatsId",
                schema: "Tigl",
                table: "GlickoRatings");

            migrationBuilder.DropForeignKey(
                name: "FK_Seasons_Users_EmperorId",
                schema: "Tigl",
                table: "Seasons");

            migrationBuilder.DropTable(
                name: "AchievementLogs",
                schema: "Log");

            migrationBuilder.DropTable(
                name: "DiscordRoleChangeLogs",
                schema: "Log");

            migrationBuilder.DropTable(
                name: "GamePublishLogs",
                schema: "Log");

            migrationBuilder.DropTable(
                name: "LeaderLogs",
                schema: "Log");

            migrationBuilder.DropTable(
                name: "Leaders",
                schema: "Tigl");

            migrationBuilder.DropTable(
                name: "PlayerSeasonResults",
                schema: "Tigl");

            migrationBuilder.DropTable(
                name: "PrestigeLogs",
                schema: "Log");

            migrationBuilder.DropTable(
                name: "Ranks",
                schema: "Tigl");

            migrationBuilder.DropTable(
                name: "RankUpLogs",
                schema: "Log");

            migrationBuilder.DropTable(
                name: "RatingDecays",
                schema: "Tigl");

            migrationBuilder.DropTable(
                name: "TiglUserAchievement",
                schema: "Relationships");

            migrationBuilder.DropTable(
                name: "TiglUserPrestigeRank",
                schema: "Relationships");

            migrationBuilder.DropTable(
                name: "Achievements",
                schema: "Tigl");

            migrationBuilder.DropTable(
                name: "PrestigeRanks",
                schema: "Tigl");

            migrationBuilder.DropIndex(
                name: "IX_TrueSkillStats_TiglUserId_League",
                schema: "Tigl",
                table: "TrueSkillStats");

            migrationBuilder.DropIndex(
                name: "IX_TrueSkillPlayerMatchStats_DiscordUserId",
                schema: "Tigl",
                table: "TrueSkillPlayerMatchStats");

            migrationBuilder.DropIndex(
                name: "IX_TrueSkillPlayerMatchStats_Faction",
                schema: "Tigl",
                table: "TrueSkillPlayerMatchStats");

            migrationBuilder.DropIndex(
                name: "IX_TrueSkillPlayerMatchStats_IsRankUpGame",
                schema: "Tigl",
                table: "TrueSkillPlayerMatchStats");

            migrationBuilder.DropIndex(
                name: "IX_TrueSkillPlayerMatchStats_Season",
                schema: "Tigl",
                table: "TrueSkillPlayerMatchStats");

            migrationBuilder.DropIndex(
                name: "IX_TrueSkillPlayerMatchStats_StartTimestamp",
                schema: "Tigl",
                table: "TrueSkillPlayerMatchStats");

            migrationBuilder.DropIndex(
                name: "IX_TrueSkillPlayerMatchStats_Winner",
                schema: "Tigl",
                table: "TrueSkillPlayerMatchStats");

            migrationBuilder.DropIndex(
                name: "IX_Seasons_EmperorId",
                schema: "Tigl",
                table: "Seasons");

            migrationBuilder.DropIndex(
                name: "IX_Seasons_SeasonNumber",
                schema: "Tigl",
                table: "Seasons");

            migrationBuilder.DropIndex(
                name: "IX_PlayerResults_Faction_Score",
                schema: "Tigl",
                table: "PlayerResults");

            migrationBuilder.DropIndex(
                name: "IX_PlayerResults_TiglUser",
                schema: "Tigl",
                table: "PlayerResults");

            migrationBuilder.DropIndex(
                name: "IX_MatchReports_GameId_Source",
                schema: "Tigl",
                table: "MatchReports");

            migrationBuilder.DropIndex(
                name: "IX_MatchReports_League_Season",
                schema: "Tigl",
                table: "MatchReports");

            migrationBuilder.DropIndex(
                name: "IX_GlickoStats_TiglUserId_League",
                schema: "Tigl",
                table: "GlickoStats");

            migrationBuilder.DropIndex(
                name: "IX_GlickoRatings_GlickoStatsId",
                schema: "Tigl",
                table: "GlickoRatings");

            migrationBuilder.DropIndex(
                name: "IX_GlickoPlayerMatchStats_DiscordUserId",
                schema: "Tigl",
                table: "GlickoPlayerMatchStats");

            migrationBuilder.DropIndex(
                name: "IX_GlickoPlayerMatchStats_Faction",
                schema: "Tigl",
                table: "GlickoPlayerMatchStats");

            migrationBuilder.DropIndex(
                name: "IX_GlickoPlayerMatchStats_IsRankUpGame",
                schema: "Tigl",
                table: "GlickoPlayerMatchStats");

            migrationBuilder.DropIndex(
                name: "IX_GlickoPlayerMatchStats_Season",
                schema: "Tigl",
                table: "GlickoPlayerMatchStats");

            migrationBuilder.DropIndex(
                name: "IX_GlickoPlayerMatchStats_StartTimestamp",
                schema: "Tigl",
                table: "GlickoPlayerMatchStats");

            migrationBuilder.DropIndex(
                name: "IX_GlickoPlayerMatchStats_Winner",
                schema: "Tigl",
                table: "GlickoPlayerMatchStats");

            migrationBuilder.DropIndex(
                name: "IX_AsyncStats_TiglUserId_League",
                schema: "Tigl",
                table: "AsyncStats");

            migrationBuilder.DropIndex(
                name: "IX_AsyncPlayerStats_FactionName",
                schema: "Statistics",
                table: "AsyncPlayerStats");

            migrationBuilder.DropIndex(
                name: "IX_AsyncPlayerMatchStats_DiscordUserId",
                schema: "Tigl",
                table: "AsyncPlayerMatchStats");

            migrationBuilder.DropIndex(
                name: "IX_AsyncPlayerMatchStats_Faction",
                schema: "Tigl",
                table: "AsyncPlayerMatchStats");

            migrationBuilder.DropIndex(
                name: "IX_AsyncPlayerMatchStats_IsRankUpGame",
                schema: "Tigl",
                table: "AsyncPlayerMatchStats");

            migrationBuilder.DropIndex(
                name: "IX_AsyncPlayerMatchStats_Season",
                schema: "Tigl",
                table: "AsyncPlayerMatchStats");

            migrationBuilder.DropIndex(
                name: "IX_AsyncPlayerMatchStats_StartTimestamp",
                schema: "Tigl",
                table: "AsyncPlayerMatchStats");

            migrationBuilder.DropIndex(
                name: "IX_AsyncPlayerMatchStats_Winner",
                schema: "Tigl",
                table: "AsyncPlayerMatchStats");

            migrationBuilder.DropIndex(
                name: "IX_AsyncGameStats_Setup_Ended",
                schema: "Statistics",
                table: "AsyncGameStats");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "d3f1c4e2-3f4a-4e2b-8f4e-2c3b5e6d7f89");

            migrationBuilder.DeleteData(
                schema: "Tigl",
                table: "Parameters",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                schema: "Tigl",
                table: "Parameters",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                schema: "Tigl",
                table: "Parameters",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                schema: "Tigl",
                table: "Parameters",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                schema: "Tigl",
                table: "Parameters",
                keyColumn: "Id",
                keyValue: 7);

            migrationBuilder.DeleteData(
                schema: "Tigl",
                table: "Parameters",
                keyColumn: "Id",
                keyValue: 8);

            migrationBuilder.DeleteData(
                schema: "Tigl",
                table: "Parameters",
                keyColumn: "Id",
                keyValue: 9);

            migrationBuilder.DeleteData(
                schema: "Tigl",
                table: "Parameters",
                keyColumn: "Id",
                keyValue: 10);

            migrationBuilder.DeleteData(
                schema: "Tigl",
                table: "Parameters",
                keyColumn: "Id",
                keyValue: 11);

            migrationBuilder.DropColumn(
                name: "ProphecyOfKingsRank",
                schema: "Tigl",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "ShatteredRank",
                schema: "Tigl",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "ThundersEdgeRank",
                schema: "Tigl",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "EndTimestamp",
                schema: "Tigl",
                table: "TrueSkillPlayerMatchStats");

            migrationBuilder.DropColumn(
                name: "ForcedReset",
                schema: "Tigl",
                table: "TrueSkillPlayerMatchStats");

            migrationBuilder.DropColumn(
                name: "EmperorId",
                schema: "Tigl",
                table: "Seasons");

            migrationBuilder.DropColumn(
                name: "IsWinner",
                schema: "Tigl",
                table: "PlayerResults");

            migrationBuilder.DropColumn(
                name: "EndTimestamp",
                schema: "Tigl",
                table: "MatchReports");

            migrationBuilder.DropColumn(
                name: "Events",
                schema: "Tigl",
                table: "MatchReports");

            migrationBuilder.DropColumn(
                name: "League",
                schema: "Tigl",
                table: "MatchReports");

            migrationBuilder.DropColumn(
                name: "PlayerCount",
                schema: "Tigl",
                table: "MatchReports");

            migrationBuilder.DropColumn(
                name: "Round",
                schema: "Tigl",
                table: "MatchReports");

            migrationBuilder.DropColumn(
                name: "Score",
                schema: "Tigl",
                table: "MatchReports");

            migrationBuilder.DropColumn(
                name: "Season",
                schema: "Tigl",
                table: "MatchReports");

            migrationBuilder.DropColumn(
                name: "EndTimestamp",
                schema: "Tigl",
                table: "GlickoPlayerMatchStats");

            migrationBuilder.DropColumn(
                name: "ForcedReset",
                schema: "Tigl",
                table: "GlickoPlayerMatchStats");

            migrationBuilder.DropColumn(
                name: "EndTimestamp",
                schema: "Tigl",
                table: "AsyncPlayerMatchStats");

            migrationBuilder.DropColumn(
                name: "ForcedReset",
                schema: "Tigl",
                table: "AsyncPlayerMatchStats");

            migrationBuilder.RenameColumn(
                name: "StartTimestamp",
                schema: "Tigl",
                table: "TrueSkillPlayerMatchStats",
                newName: "Timestamp");

            migrationBuilder.RenameColumn(
                name: "TiglUserId",
                schema: "Tigl",
                table: "PlayerResults",
                newName: "TiglUser");

            migrationBuilder.RenameColumn(
                name: "StartTimestamp",
                schema: "Tigl",
                table: "MatchReports",
                newName: "Timestamp");

            migrationBuilder.RenameColumn(
                name: "StartTimestamp",
                schema: "Tigl",
                table: "GlickoPlayerMatchStats",
                newName: "Timestmap");

            migrationBuilder.RenameColumn(
                name: "StartTimestamp",
                schema: "Tigl",
                table: "AsyncPlayerMatchStats",
                newName: "Timestamp");

            migrationBuilder.AlterColumn<bool>(
                name: "TiglUserNameChanged",
                schema: "Tigl",
                table: "Users",
                type: "bit",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(10)",
                oldMaxLength: 10)
                .OldAnnotation("Relational:ColumnOrder", 7);

            migrationBuilder.AlterColumn<DateOnly>(
                name: "LastUserNameChange",
                schema: "Tigl",
                table: "Users",
                type: "date",
                nullable: false,
                oldClrType: typeof(DateOnly),
                oldType: "date")
                .OldAnnotation("Relational:ColumnOrder", 8);

            migrationBuilder.AddColumn<string>(
                name: "Rank",
                schema: "Tigl",
                table: "TrueSkillStats",
                type: "varchar(50)",
                nullable: false,
                defaultValue: "")
                .Annotation("Relational:ColumnOrder", 4);

            migrationBuilder.AlterColumn<decimal>(
                name: "SigmaOld",
                schema: "Tigl",
                table: "TrueSkillPlayerMatchStats",
                type: "decimal(18,10)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,10)")
                .Annotation("Relational:ColumnOrder", 10)
                .OldAnnotation("Relational:ColumnOrder", 9);

            migrationBuilder.AlterColumn<decimal>(
                name: "SigmaNew",
                schema: "Tigl",
                table: "TrueSkillPlayerMatchStats",
                type: "decimal(18,10)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,10)")
                .Annotation("Relational:ColumnOrder", 11)
                .OldAnnotation("Relational:ColumnOrder", 10);

            migrationBuilder.AlterColumn<int>(
                name: "Season",
                schema: "Tigl",
                table: "TrueSkillPlayerMatchStats",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int")
                .Annotation("Relational:ColumnOrder", 13)
                .OldAnnotation("Relational:ColumnOrder", 12);

            migrationBuilder.AlterColumn<decimal>(
                name: "OpponentAvgRating",
                schema: "Tigl",
                table: "TrueSkillPlayerMatchStats",
                type: "decimal(18,10)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,10)")
                .Annotation("Relational:ColumnOrder", 12)
                .OldAnnotation("Relational:ColumnOrder", 11);

            migrationBuilder.AlterColumn<decimal>(
                name: "MuOld",
                schema: "Tigl",
                table: "TrueSkillPlayerMatchStats",
                type: "decimal(18,10)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,10)")
                .Annotation("Relational:ColumnOrder", 8)
                .OldAnnotation("Relational:ColumnOrder", 7);

            migrationBuilder.AlterColumn<decimal>(
                name: "MuNew",
                schema: "Tigl",
                table: "TrueSkillPlayerMatchStats",
                type: "decimal(18,10)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,10)")
                .Annotation("Relational:ColumnOrder", 9)
                .OldAnnotation("Relational:ColumnOrder", 8);

            migrationBuilder.AlterColumn<string>(
                name: "Faction",
                schema: "Tigl",
                table: "TrueSkillPlayerMatchStats",
                type: "varchar(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(100)",
                oldMaxLength: 100)
                .Annotation("Relational:ColumnOrder", 14)
                .OldAnnotation("Relational:ColumnOrder", 13);

            migrationBuilder.AlterColumn<long>(
                name: "Timestamp",
                schema: "Tigl",
                table: "TrueSkillPlayerMatchStats",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint")
                .Annotation("Relational:ColumnOrder", 15)
                .OldAnnotation("Relational:ColumnOrder", 14);

            migrationBuilder.AddColumn<decimal>(
                name: "Performance",
                schema: "Tigl",
                table: "TrueSkillPlayerMatchStats",
                type: "decimal(18,10)",
                nullable: false,
                defaultValue: 0m)
                .Annotation("Relational:ColumnOrder", 7);

            migrationBuilder.AlterColumn<string>(
                name: "Faction",
                schema: "Tigl",
                table: "PlayerResults",
                type: "varchar(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(100)",
                oldMaxLength: 100)
                .Annotation("Relational:ColumnOrder", 3)
                .OldAnnotation("Relational:ColumnOrder", 4);

            migrationBuilder.AlterColumn<string>(
                name: "State",
                schema: "Tigl",
                table: "MatchReports",
                type: "varchar(50)",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(50)",
                oldMaxLength: 50)
                .Annotation("Relational:ColumnOrder", 4)
                .OldAnnotation("Relational:ColumnOrder", 11);

            migrationBuilder.AddColumn<string>(
                name: "Rank",
                schema: "Tigl",
                table: "GlickoStats",
                type: "varchar(50)",
                nullable: false,
                defaultValue: "")
                .Annotation("Relational:ColumnOrder", 4);

            migrationBuilder.AlterColumn<decimal>(
                name: "VolatilityOld",
                schema: "Tigl",
                table: "GlickoPlayerMatchStats",
                type: "decimal(18,10)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,10)")
                .Annotation("Relational:ColumnOrder", 12)
                .OldAnnotation("Relational:ColumnOrder", 11);

            migrationBuilder.AlterColumn<decimal>(
                name: "VolatilityNew",
                schema: "Tigl",
                table: "GlickoPlayerMatchStats",
                type: "decimal(18,10)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,10)")
                .Annotation("Relational:ColumnOrder", 13)
                .OldAnnotation("Relational:ColumnOrder", 12);

            migrationBuilder.AlterColumn<int>(
                name: "Season",
                schema: "Tigl",
                table: "GlickoPlayerMatchStats",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int")
                .Annotation("Relational:ColumnOrder", 15)
                .OldAnnotation("Relational:ColumnOrder", 14);

            migrationBuilder.AlterColumn<decimal>(
                name: "RdOld",
                schema: "Tigl",
                table: "GlickoPlayerMatchStats",
                type: "decimal(18,10)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,10)")
                .Annotation("Relational:ColumnOrder", 10)
                .OldAnnotation("Relational:ColumnOrder", 9);

            migrationBuilder.AlterColumn<decimal>(
                name: "RdNew",
                schema: "Tigl",
                table: "GlickoPlayerMatchStats",
                type: "decimal(18,10)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,10)")
                .Annotation("Relational:ColumnOrder", 11)
                .OldAnnotation("Relational:ColumnOrder", 10);

            migrationBuilder.AlterColumn<decimal>(
                name: "RatingOld",
                schema: "Tigl",
                table: "GlickoPlayerMatchStats",
                type: "decimal(18,10)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,10)")
                .Annotation("Relational:ColumnOrder", 8)
                .OldAnnotation("Relational:ColumnOrder", 7);

            migrationBuilder.AlterColumn<decimal>(
                name: "RatingNew",
                schema: "Tigl",
                table: "GlickoPlayerMatchStats",
                type: "decimal(18,10)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,10)")
                .Annotation("Relational:ColumnOrder", 9)
                .OldAnnotation("Relational:ColumnOrder", 8);

            migrationBuilder.AlterColumn<decimal>(
                name: "OpponentAvgRating",
                schema: "Tigl",
                table: "GlickoPlayerMatchStats",
                type: "decimal(18,10)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,10)")
                .Annotation("Relational:ColumnOrder", 14)
                .OldAnnotation("Relational:ColumnOrder", 13);

            migrationBuilder.AlterColumn<string>(
                name: "Faction",
                schema: "Tigl",
                table: "GlickoPlayerMatchStats",
                type: "varchar(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(100)",
                oldMaxLength: 100)
                .Annotation("Relational:ColumnOrder", 16)
                .OldAnnotation("Relational:ColumnOrder", 15);

            migrationBuilder.AlterColumn<long>(
                name: "Timestmap",
                schema: "Tigl",
                table: "GlickoPlayerMatchStats",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint")
                .Annotation("Relational:ColumnOrder", 17)
                .OldAnnotation("Relational:ColumnOrder", 16);

            migrationBuilder.AddColumn<decimal>(
                name: "Performance",
                schema: "Tigl",
                table: "GlickoPlayerMatchStats",
                type: "decimal(18,10)",
                nullable: false,
                defaultValue: 0m)
                .Annotation("Relational:ColumnOrder", 7);

            migrationBuilder.AddColumn<string>(
                name: "Rank",
                schema: "Tigl",
                table: "AsyncStats",
                type: "varchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "")
                .Annotation("Relational:ColumnOrder", 4);

            migrationBuilder.AlterColumn<string>(
                name: "OldRank",
                schema: "Tigl",
                table: "AsyncPlayerMatchStats",
                type: "varchar(50)",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(50)",
                oldMaxLength: 50)
                .Annotation("Relational:ColumnOrder", 17)
                .OldAnnotation("Relational:ColumnOrder", 18);

            migrationBuilder.AlterColumn<string>(
                name: "NewRank",
                schema: "Tigl",
                table: "AsyncPlayerMatchStats",
                type: "varchar(50)",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(50)",
                oldMaxLength: 50)
                .Annotation("Relational:ColumnOrder", 18)
                .OldAnnotation("Relational:ColumnOrder", 19);

            migrationBuilder.AlterColumn<string>(
                name: "IsRankUpGame",
                schema: "Tigl",
                table: "AsyncPlayerMatchStats",
                type: "varchar(10)",
                maxLength: 10,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(10)",
                oldMaxLength: 10)
                .Annotation("Relational:ColumnOrder", 16)
                .OldAnnotation("Relational:ColumnOrder", 17);

            migrationBuilder.UpdateData(
                schema: "Tigl",
                table: "Seasons",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "EndDate", "Name", "StartDate" },
                values: new object[] { new DateOnly(2023, 12, 31), "", new DateOnly(2023, 1, 1) });

            migrationBuilder.CreateIndex(
                name: "IX_TrueSkillStats_TiglUserId",
                schema: "Tigl",
                table: "TrueSkillStats",
                column: "TiglUserId");

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
                name: "IX_AsyncStats_TiglUserId",
                schema: "Tigl",
                table: "AsyncStats",
                column: "TiglUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_GlickoStats_GlickoRatings_GlickoRatingId",
                schema: "Tigl",
                table: "GlickoStats",
                column: "GlickoRatingId",
                principalSchema: "Tigl",
                principalTable: "GlickoRatings",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
