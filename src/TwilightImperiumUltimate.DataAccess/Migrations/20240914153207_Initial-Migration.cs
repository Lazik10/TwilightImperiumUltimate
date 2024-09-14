using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace TwilightImperiumUltimate.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class InitialMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "Card");

            migrationBuilder.EnsureSchema(
                name: "Faction");

            migrationBuilder.EnsureSchema(
                name: "Statistics");

            migrationBuilder.EnsureSchema(
                name: "Relationships");

            migrationBuilder.EnsureSchema(
                name: "Rule");

            migrationBuilder.EnsureSchema(
                name: "Galaxy");

            migrationBuilder.EnsureSchema(
                name: "News");

            migrationBuilder.EnsureSchema(
                name: "Player");

            migrationBuilder.EnsureSchema(
                name: "Technology");

            migrationBuilder.EnsureSchema(
                name: "Unit");

            migrationBuilder.EnsureSchema(
                name: "Website");

            migrationBuilder.CreateTable(
                name: "ActionCards",
                schema: "Card",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EnumName = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false),
                    Type = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: false),
                    TimingWindow = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: false),
                    Text = table.Column<string>(type: "text", nullable: false),
                    GameVersion = table.Column<string>(type: "varchar(20)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ActionCards", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AgendaCards",
                schema: "Card",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EnumName = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false),
                    Type = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: false),
                    AgendaCardType = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: false),
                    Text = table.Column<string>(type: "text", nullable: false),
                    GameVersion = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AgendaCards", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetRoles",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUsers",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    FirstName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Age = table.Column<int>(type: "int", nullable: true),
                    FavoriteFaction = table.Column<int>(type: "int", nullable: false),
                    UserInfo = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DiscordId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SteamId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedUserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    Email = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedEmail = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    EmailConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SecurityStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    TwoFactorEnabled = table.Column<bool>(type: "bit", nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    LockoutEnabled = table.Column<bool>(type: "bit", nullable: false),
                    AccessFailedCount = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUsers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ExplorationCards",
                schema: "Card",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EnumName = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false),
                    Type = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: false),
                    ExplorationPlanetTrait = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: false),
                    Text = table.Column<string>(type: "text", nullable: false),
                    GameVersion = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExplorationCards", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "FactionColorImportances",
                schema: "Faction",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FactionName = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false),
                    Color = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: false),
                    Importance = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FactionColorImportances", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "FactionRoundStatistics",
                schema: "Statistics",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    GameId = table.Column<string>(type: "varchar(100)", nullable: false),
                    Round = table.Column<int>(type: "integer", nullable: false),
                    FactionName = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false),
                    Score = table.Column<int>(type: "integer", nullable: false),
                    StrategyCardName = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FactionRoundStatistics", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Factions",
                schema: "Faction",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    FactionName = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false),
                    ComplexityRating = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: false),
                    HomeSystem = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false),
                    Commodities = table.Column<int>(type: "integer", nullable: false),
                    Action = table.Column<string>(type: "nvarchar(MAX)", nullable: false),
                    History = table.Column<string>(type: "nvarchar(MAX)", nullable: false),
                    Quote = table.Column<string>(type: "nvarchar(MAX)", nullable: false),
                    SystemStats = table.Column<string>(type: "nvarchar(MAX)", nullable: false),
                    SystemInfo = table.Column<string>(type: "nvarchar(MAX)", nullable: false),
                    GameVersion = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Factions", x => x.FactionName);
                });

            migrationBuilder.CreateTable(
                name: "Faq",
                schema: "Rule",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ComponentName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    QuestionEnglish = table.Column<string>(type: "nvarchar(MAX)", nullable: false),
                    AnswerEnglish = table.Column<string>(type: "nvarchar(MAX)", nullable: false),
                    QuestionCzech = table.Column<string>(type: "nvarchar(MAX)", nullable: false),
                    AnswerCzech = table.Column<string>(type: "nvarchar(MAX)", nullable: false),
                    FaqStatus = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Faq", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "FrontierCards",
                schema: "Card",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EnumName = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false),
                    Type = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: false),
                    Text = table.Column<string>(type: "text", nullable: false),
                    GameVersion = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FrontierCards", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "GameStatistics",
                schema: "Statistics",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    GameId = table.Column<string>(type: "varchar(100)", nullable: false),
                    MaxPoints = table.Column<int>(type: "integer", nullable: false),
                    NumberOfPlayers = table.Column<int>(type: "integer", nullable: false),
                    Round = table.Column<int>(type: "integer", nullable: false),
                    Time = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false),
                    Winner = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GameStatistics", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ObjectiveCards",
                schema: "Card",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EnumName = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false),
                    Type = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: false),
                    ObjectiveCardType = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: false),
                    TimingWindow = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: false),
                    Text = table.Column<string>(type: "text", nullable: false),
                    GameVersion = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ObjectiveCards", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Players",
                schema: "Player",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    Color = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Players", x => x.Name);
                });

            migrationBuilder.CreateTable(
                name: "PromissaryNoteCards",
                schema: "Card",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EnumName = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false),
                    Type = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: false),
                    Faction = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false),
                    Text = table.Column<string>(type: "varchar(MAX)", nullable: false),
                    GameVersion = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PromissaryNoteCards", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "RelicCards",
                schema: "Card",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EnumName = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false),
                    Type = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: false),
                    Text = table.Column<string>(type: "text", nullable: false),
                    GameVersion = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RelicCards", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Rules",
                schema: "Rule",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RuleCategory = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: false),
                    Content = table.Column<string>(type: "nvarchar(MAX)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Rules", x => x.RuleCategory);
                });

            migrationBuilder.CreateTable(
                name: "StrategyCards",
                schema: "Card",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EnumName = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false),
                    Type = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: false),
                    InitiativeOrder = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: false),
                    PrimaryAbilityText = table.Column<string>(type: "text", nullable: false),
                    SecondaryAbilityText = table.Column<string>(type: "text", nullable: false),
                    Text = table.Column<string>(type: "text", nullable: false),
                    GameVersion = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StrategyCards", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SystemTiles",
                schema: "Galaxy",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SystemTileName = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false),
                    SystemTileCode = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: false),
                    TileCategory = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: false),
                    FactionName = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false),
                    Anomaly = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: false),
                    GameVersion = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SystemTiles", x => x.SystemTileName)
                        .Annotation("SqlServer:Clustered", true);
                });

            migrationBuilder.CreateTable(
                name: "Technologies",
                schema: "Technology",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TechnologyName = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false),
                    Type = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: false),
                    Level = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: false),
                    FactionName = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false),
                    IsFactionTechnology = table.Column<string>(type: "varchar(5)", nullable: false),
                    Text = table.Column<string>(type: "nvarchar(MAX)", nullable: false),
                    GameVersion = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Technologies", x => x.TechnologyName);
                });

            migrationBuilder.CreateTable(
                name: "Units",
                schema: "Unit",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UnitName = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: false),
                    UnitType = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: false),
                    Cost = table.Column<int>(type: "integer", nullable: false),
                    Combat = table.Column<int>(type: "integer", nullable: false),
                    Move = table.Column<int>(type: "integer", nullable: false),
                    Capacity = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Units", x => x.UnitName);
                });

            migrationBuilder.CreateTable(
                name: "Websites",
                schema: "Website",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "varchar(500)", maxLength: 500, nullable: false),
                    WebsitePath = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Websites", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "WebsiteStatistics",
                schema: "Statistics",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Visitors = table.Column<int>(type: "integer", nullable: false),
                    MapsGenerated = table.Column<int>(type: "integer", nullable: false),
                    SlicesGenerated = table.Column<int>(type: "integer", nullable: false),
                    MapsArchived = table.Column<int>(type: "integer", nullable: false),
                    SlicesArchived = table.Column<int>(type: "integer", nullable: false),
                    GamesPlayed = table.Column<int>(type: "integer", nullable: false),
                    FactionsDrafted = table.Column<int>(type: "integer", nullable: false),
                    ColorsDrafted = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WebsiteStatistics", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetRoleClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoleId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoleClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetRoleClaims_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetUserClaims_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserLogins",
                columns: table => new
                {
                    LoginProvider = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProviderKey = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProviderDisplayName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserLogins", x => new { x.LoginProvider, x.ProviderKey });
                    table.ForeignKey(
                        name: "FK_AspNetUserLogins_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserRoles",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    RoleId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserRoles", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserTokens",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    LoginProvider = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Value = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserTokens", x => new { x.UserId, x.LoginProvider, x.Name });
                    table.ForeignKey(
                        name: "FK_AspNetUserTokens_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Maps",
                schema: "Galaxy",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "varchar(256)", maxLength: 256, nullable: false),
                    EventName = table.Column<string>(type: "varchar(256)", maxLength: 256, nullable: false),
                    Description = table.Column<string>(type: "varchar(MAX)", nullable: false),
                    MapTemplate = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false),
                    TtsString = table.Column<string>(type: "varchar(MAX)", nullable: false),
                    MapGeneratorLink = table.Column<string>(type: "varchar(MAX)", nullable: false),
                    MapArchiveLink = table.Column<string>(type: "varchar(MAX)", nullable: false),
                    UserName = table.Column<string>(type: "varchar(256)", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Maps", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Maps_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "NewsArticles",
                schema: "News",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(255)", nullable: false),
                    Content = table.Column<string>(type: "nvarchar(MAX)", nullable: false),
                    CreatedAt = table.Column<DateOnly>(type: "date", nullable: false),
                    UpdatedAt = table.Column<DateOnly>(type: "date", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NewsArticles", x => x.Id)
                        .Annotation("SqlServer:Clustered", true);
                    table.ForeignKey(
                        name: "FK_NewsArticles_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SliceDrafts",
                schema: "Galaxy",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "varchar(256)", maxLength: 256, nullable: false),
                    EventName = table.Column<string>(type: "varchar(256)", maxLength: 256, nullable: false),
                    Description = table.Column<string>(type: "varchar(MAX)", nullable: false),
                    SliceDraftString = table.Column<string>(type: "varchar(MAX)", nullable: false),
                    SliceNames = table.Column<string>(type: "varchar(MAX)", nullable: false),
                    SliceCount = table.Column<int>(type: "integer", nullable: false),
                    SliceDraftGeneratorLink = table.Column<string>(type: "varchar(MAX)", nullable: false),
                    SliceDraftArchiveLink = table.Column<string>(type: "varchar(MAX)", nullable: false),
                    UserName = table.Column<string>(type: "varchar(256)", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SliceDrafts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SliceDrafts_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Planets",
                schema: "Galaxy",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PlanetName = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false),
                    Resources = table.Column<int>(type: "integer", nullable: false),
                    Influence = table.Column<int>(type: "integer", nullable: false),
                    IsLegendary = table.Column<string>(type: "varchar(5)", nullable: false),
                    TechnologySkip = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: false),
                    PlanetTrait = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: false),
                    SystemTileName = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false),
                    GameVersion = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Planets", x => x.PlanetName)
                        .Annotation("SqlServer:Clustered", true);
                    table.ForeignKey(
                        name: "FK_Planets_SystemTiles_SystemTileName",
                        column: x => x.SystemTileName,
                        principalSchema: "Galaxy",
                        principalTable: "SystemTiles",
                        principalColumn: "SystemTileName",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Wormholes",
                schema: "Galaxy",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    WormholeName = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: false),
                    SystemTileName = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false),
                    GameVersion = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Wormholes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Wormholes_SystemTiles_SystemTileName",
                        column: x => x.SystemTileName,
                        principalSchema: "Galaxy",
                        principalTable: "SystemTiles",
                        principalColumn: "SystemTileName",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "FactionTechnology",
                schema: "Relationships",
                columns: table => new
                {
                    FactionName = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false),
                    TechnologyName = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false),
                    StartingTechnology = table.Column<string>(type: "varchar(5)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FactionTechnology", x => new { x.FactionName, x.TechnologyName });
                    table.ForeignKey(
                        name: "FK_FactionTechnology_Factions_FactionName",
                        column: x => x.FactionName,
                        principalSchema: "Faction",
                        principalTable: "Factions",
                        principalColumn: "FactionName",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FactionTechnology_Technologies_TechnologyName",
                        column: x => x.TechnologyName,
                        principalSchema: "Technology",
                        principalTable: "Technologies",
                        principalColumn: "TechnologyName",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "FactionUnit",
                schema: "Relationships",
                columns: table => new
                {
                    FactionName = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false),
                    UnitName = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: false),
                    Count = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FactionUnit", x => new { x.FactionName, x.UnitName });
                    table.ForeignKey(
                        name: "FK_FactionUnit_Factions_FactionName",
                        column: x => x.FactionName,
                        principalSchema: "Faction",
                        principalTable: "Factions",
                        principalColumn: "FactionName",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FactionUnit_Units_UnitName",
                        column: x => x.UnitName,
                        principalSchema: "Unit",
                        principalTable: "Units",
                        principalColumn: "UnitName",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MapRating",
                schema: "Relationships",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MapId = table.Column<int>(type: "integer", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Rating = table.Column<float>(type: "real", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MapRating", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MapRating_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_MapRating_Maps_MapId",
                        column: x => x.MapId,
                        principalSchema: "Galaxy",
                        principalTable: "Maps",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SliceDraftRating",
                schema: "Relationships",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SliceDraftId = table.Column<int>(type: "integer", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Rating = table.Column<float>(type: "real", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SliceDraftRating", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SliceDraftRating_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_SliceDraftRating_SliceDrafts_SliceDraftId",
                        column: x => x.SliceDraftId,
                        principalSchema: "Galaxy",
                        principalTable: "SliceDrafts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                schema: "Card",
                table: "ActionCards",
                columns: new[] { "Id", "EnumName", "GameVersion", "Text", "TimingWindow", "Type" },
                values: new object[,]
                {
                    { 1, "AncientBurialSites", "BaseGame", "", "AgendaPhase", "Action" },
                    { 2, "ArchaeologicalExpedition", "ProphecyOfKings", "", "Action", "Action" },
                    { 3, "AssassinateRepresentative", "BaseGame", "", "AgendaPhase", "Action" },
                    { 4, "Blitz", "CodexOrdinian", "", "Combat", "Action" },
                    { 5, "Bribery", "BaseGame", "", "AgendaPhase", "Action" },
                    { 6, "Bunker", "BaseGame", "", "Combat", "Action" },
                    { 7, "ConfoundingLegalText", "ProphecyOfKings", "", "AgendaPhase", "Action" },
                    { 8, "ConfusingLegalText", "BaseGame", "", "AgendaPhase", "Action" },
                    { 9, "ConstructionRider", "BaseGame", "", "AgendaPhase", "Action" },
                    { 10, "Counterstroke", "CodexOrdinian", "", "Reaction", "Action" },
                    { 11, "CoupDetat", "ProphecyOfKings", "", "Reaction", "Action" },
                    { 12, "CourageousToTheEnd", "BaseGame", "", "Combat", "Action" },
                    { 13, "CrippleDefenses", "BaseGame", "", "Action", "Action" },
                    { 14, "DeadlyPlot", "ProphecyOfKings", "", "AgendaPhase", "Action" },
                    { 15, "DecoyOperation", "ProphecyOfKings", "", "TacticalAction", "Action" },
                    { 16, "DiplomacyRider", "BaseGame", "", "AgendaPhase", "Action" },
                    { 17, "DiplomaticPressure", "ProphecyOfKings", "", "AgendaPhase", "Action" },
                    { 18, "DirectHit", "BaseGame", "", "Combat", "Action" },
                    { 19, "Disable", "BaseGame", "", "Combat", "Action" },
                    { 20, "DistinguishedCouncilor", "BaseGame", "", "AgendaPhase", "Action" },
                    { 21, "DivertFunding", "ProphecyOfKings", "", "Action", "Action" },
                    { 22, "EconomicInitiative", "BaseGame", "", "Action", "Action" },
                    { 23, "EmergencyRepairs", "BaseGame", "", "Combat", "Action" },
                    { 24, "ExperimentalBattlestation", "BaseGame", "", "TacticalAction", "Action" },
                    { 25, "ExplorationProbe", "ProphecyOfKings", "", "Action", "Action" },
                    { 26, "FighterConscription", "CodexOrdinian", "", "Action", "Action" },
                    { 27, "FighterPrototype", "BaseGame", "", "Combat", "Action" },
                    { 28, "FireTeam", "BaseGame", "", "Combat", "Action" },
                    { 29, "FlankSpeed", "BaseGame", "", "TacticalAction", "Action" },
                    { 30, "FocusedResearch", "BaseGame", "", "Action", "Action" },
                    { 31, "ForwardSupplyBase", "CodexOrdinian", "", "Reaction", "Action" },
                    { 32, "FrontlineDeployment", "BaseGame", "", "Action", "Action" },
                    { 33, "GhostShip", "BaseGame", "", "Action", "Action" },
                    { 34, "GhostSquad", "CodexOrdinian", "", "Combat", "Action" },
                    { 35, "HackElection", "CodexOrdinian", "", "AgendaPhase", "Action" },
                    { 36, "HarnessEnergy", "CodexOrdinian", "", "TacticalAction", "Action" },
                    { 37, "ImperialRider", "BaseGame", "", "AgendaPhase", "Action" },
                    { 38, "Impersonation", "CodexOrdinian", "", "Action", "Action" },
                    { 39, "InTheSilenceOfSpace", "BaseGame", "", "TacticalAction", "Action" },
                    { 40, "IndustrialInitiative", "BaseGame", "", "Action", "Action" },
                    { 41, "Infiltrate", "BaseGame", "", "Combat", "Action" },
                    { 42, "InsiderInformation", "CodexOrdinian", "", "AgendaPhase", "Action" },
                    { 43, "Insubordination", "BaseGame", "", "Action", "Action" },
                    { 44, "Intercept", "BaseGame", "", "Combat", "Action" },
                    { 45, "LeadershipRider", "BaseGame", "", "AgendaPhase", "Action" },
                    { 46, "LostStarChart", "BaseGame", "", "TacticalAction", "Action" },
                    { 47, "LuckyShot", "BaseGame", "", "Action", "Action" },
                    { 48, "ManeuveringJets", "BaseGame", "", "Combat", "Action" },
                    { 49, "ManipulateInvestments", "ProphecyOfKings", "", "StrategyPhase", "Action" },
                    { 50, "MasterPlan", "CodexOrdinian", "", "TacticalAction", "Action" },
                    { 51, "MiningInitiative", "BaseGame", "", "Action", "Action" },
                    { 52, "MoraleBoost", "BaseGame", "", "Combat", "Action" },
                    { 53, "NavSuite", "ProphecyOfKings", "", "TacticalAction", "Action" },
                    { 54, "Parley", "BaseGame", "", "Combat", "Action" },
                    { 55, "Plagiarize", "CodexOrdinian", "", "Action", "Action" },
                    { 56, "Plague", "BaseGame", "", "Action", "Action" },
                    { 57, "PoliticalStability", "BaseGame", "", "StatusPhase", "Action" },
                    { 58, "PoliticsRider", "BaseGame", "", "AgendaPhase", "Action" },
                    { 59, "PublicDisgrace", "BaseGame", "", "StrategyPhase", "Action" },
                    { 60, "Rally", "CodexOrdinian", "", "TacticalAction", "Action" },
                    { 61, "ReactorMeltdown", "BaseGame", "", "Action", "Action" },
                    { 62, "RefitTroops", "ProphecyOfKings", "", "Action", "Action" },
                    { 63, "ReflectiveShielding", "CodexOrdinian", "", "Combat", "Action" },
                    { 64, "Reparations", "BaseGame", "", "TacticalAction", "Action" },
                    { 65, "RepealLaw", "BaseGame", "", "Action", "Action" },
                    { 66, "RevealPrototype", "ProphecyOfKings", "", "Combat", "Action" },
                    { 67, "ReverseEngineer", "ProphecyOfKings", "", "Reaction", "Action" },
                    { 68, "RiseOfAMessiah", "BaseGame", "", "Action", "Action" },
                    { 69, "Rout", "ProphecyOfKings", "", "Combat", "Action" },
                    { 70, "Sabotage", "BaseGame", "", "Reaction", "Action" },
                    { 71, "Sanction", "CodexOrdinian", "", "AgendaPhase", "Action" },
                    { 72, "ScrambleFrequency", "CodexOrdinian", "", "Combat", "Action" },
                    { 73, "Scuttle", "ProphecyOfKings", "", "Action", "Action" },
                    { 74, "SeizeArtifact", "ProphecyOfKings", "", "Action", "Action" },
                    { 75, "ShieldsHolding", "BaseGame", "", "Combat", "Action" },
                    { 76, "SignalJamming", "BaseGame", "", "Action", "Action" },
                    { 77, "SkilledRetreat", "BaseGame", "", "Combat", "Action" },
                    { 78, "SolarFlare", "CodexOrdinian", "", "TacticalAction", "Action" },
                    { 79, "Spy", "BaseGame", "", "Action", "Action" },
                    { 80, "Summit", "BaseGame", "", "StrategyPhase", "Action" },
                    { 81, "TacticalBombardment", "BaseGame", "", "Action", "Action" },
                    { 82, "TechnologyRider", "BaseGame", "", "AgendaPhase", "Action" },
                    { 83, "TradeRider", "BaseGame", "", "AgendaPhase", "Action" },
                    { 84, "UnexpectedAction", "BaseGame", "", "Action", "Action" },
                    { 85, "UnstablePlanet", "BaseGame", "", "Action", "Action" },
                    { 86, "Upgrade", "BaseGame", "", "TacticalAction", "Action" },
                    { 87, "Uprising", "BaseGame", "", "Action", "Action" },
                    { 88, "Veto", "BaseGame", "", "AgendaPhase", "Action" },
                    { 89, "WarMachine", "CodexOrdinian", "", "TacticalAction", "Action" },
                    { 90, "WarfareRider", "BaseGame", "", "AgendaPhase", "Action" },
                    { 91, "Waylay", "ProphecyOfKings", "", "Combat", "Action" },
                    { 92, "AbyssalStarpaths", "UnchartedSpace", "", "TacticalAction", "Action" },
                    { 93, "AggressiveBroker", "UnchartedSpace", "", "Reaction", "Action" },
                    { 94, "AIAugury", "UnchartedSpace", "", "TacticalAction", "Action" },
                    { 95, "Arbitrage", "UnchartedSpace", "", "TacticalAction", "Action" },
                    { 96, "BountyContracts", "UnchartedSpace", "", "StatusPhase", "Action" },
                    { 97, "CarapacePlating", "UnchartedSpace", "", "Combat", "Action" },
                    { 98, "ClassifiedWeapons", "UnchartedSpace", "", "Combat", "Action" },
                    { 99, "ConfusedSage", "UnchartedSpace", "", "Combat", "Action" },
                    { 100, "Contagion", "UnchartedSpace", "", "Combat", "Action" },
                    { 101, "EchoShielding", "UnchartedSpace", "", "Combat", "Action" },
                    { 102, "EmergencyMeeting", "UnchartedSpace", "", "AgendaPhase", "Action" },
                    { 103, "EscapeClause", "UnchartedSpace", "", "Reaction", "Action" },
                    { 104, "ForbiddenKnowledge", "UnchartedSpace", "", "Reaction", "Action" },
                    { 105, "FreeTradeInitiative", "UnchartedSpace", "", "Action", "Action" },
                    { 106, "FreedomFighters", "UnchartedSpace", "", "Combat", "Action" },
                    { 107, "FulfillmentProtocols", "UnchartedSpace", "", "Reaction", "Action" },
                    { 108, "HostileWorld", "UnchartedSpace", "", "Combat", "Action" },
                    { 109, "IllusoryDuplication", "UnchartedSpace", "", "Combat", "Action" },
                    { 110, "MicrometeoroidSwarm", "UnchartedSpace", "", "Action", "Action" },
                    { 111, "NeuralHammer", "UnchartedSpace", "", "Action", "Action" },
                    { 112, "PersonnelWrit", "UnchartedSpace", "", "Action", "Action" },
                    { 113, "PlanetaryRigs", "UnchartedSpace", "", "Action", "Action" },
                    { 114, "Preparation", "UnchartedSpace", "", "Action", "Action" },
                    { 115, "ProfessionalArchaeologists", "UnchartedSpace", "", "Action", "Action" },
                    { 116, "RemnantCollection", "UnchartedSpace", "", "Action", "Action" },
                    { 117, "Renegotiation", "UnchartedSpace", "", "Reaction", "Action" },
                    { 118, "SafetyOverrides", "UnchartedSpace", "", "Combat", "Action" },
                    { 119, "SecuredTrove", "UnchartedSpace", "", "Action", "Action" },
                    { 120, "SingularityCharge", "UnchartedSpace", "", "Combat", "Action" },
                    { 121, "SpecialSession", "UnchartedSpace", "", "AgendaPhase", "Action" }
                });

            migrationBuilder.InsertData(
                schema: "Card",
                table: "AgendaCards",
                columns: new[] { "Id", "AgendaCardType", "EnumName", "GameVersion", "Text", "Type" },
                values: new object[,]
                {
                    { 1, "Law", "AntiIntellectualRevolution", "BaseGame", "", "Agenda" },
                    { 2, "Directive", "ArchivedSecret", "BaseGame", "", "Agenda" },
                    { 3, "Directive", "ArmsReduction", "BaseGame", "", "Agenda" },
                    { 4, "Law", "ClassifiedDocumentLeaks", "BaseGame", "", "Agenda" },
                    { 5, "Directive", "ColonialRedistribution", "BaseGame", "", "Agenda" },
                    { 6, "Law", "CommitteeFormation", "BaseGame", "", "Agenda" },
                    { 7, "Directive", "CompensatedDisarmament", "BaseGame", "", "Agenda" },
                    { 8, "Law", "ConventionsOfWar", "BaseGame", "", "Agenda" },
                    { 9, "Law", "CoreMining", "Deprecated", "", "Agenda" },
                    { 10, "Law", "DemilitarizedZone", "Deprecated", "", "Agenda" },
                    { 11, "Directive", "EconomicEquality", "BaseGame", "", "Agenda" },
                    { 12, "Law", "EnforcedTravelBan", "BaseGame", "", "Agenda" },
                    { 13, "Law", "ExecutiveSanctions", "BaseGame", "", "Agenda" },
                    { 14, "Law", "FleetRegulations", "BaseGame", "", "Agenda" },
                    { 15, "Law", "HolyPlanetOfIxth", "Deprecated", "", "Agenda" },
                    { 16, "Law", "HomelandDefenseAct", "BaseGame", "", "Agenda" },
                    { 17, "Law", "ImperialArbiter", "BaseGame", "", "Agenda" },
                    { 18, "Directive", "IncentiveProgram", "BaseGame", "", "Agenda" },
                    { 19, "Directive", "IxthianArtifact", "BaseGame", "", "Agenda" },
                    { 20, "Directive", "JudicialAbolishment", "BaseGame", "", "Agenda" },
                    { 21, "Law", "MinisterOfCommerce", "BaseGame", "", "Agenda" },
                    { 22, "Law", "MinisterOfExploration", "BaseGame", "", "Agenda" },
                    { 23, "Law", "MinisterOfIndustry", "BaseGame", "", "Agenda" },
                    { 24, "Law", "MinisterOfPeace", "BaseGame", "", "Agenda" },
                    { 25, "Law", "MinisterOfPolicy", "BaseGame", "", "Agenda" },
                    { 26, "Law", "MinisterOfSciences", "BaseGame", "", "Agenda" },
                    { 27, "Law", "MinisterOfWar", "BaseGame", "", "Agenda" },
                    { 28, "Directive", "MiscountDisclosed", "BaseGame", "", "Agenda" },
                    { 29, "Directive", "Mutiny", "BaseGame", "", "Agenda" },
                    { 30, "Directive", "NewConstitution", "BaseGame", "", "Agenda" },
                    { 31, "Law", "ProphecyOfIxth", "BaseGame", "", "Agenda" },
                    { 32, "Directive", "PublicExecution", "BaseGame", "", "Agenda" },
                    { 33, "Law", "PublicizeWeaponSchematics", "BaseGame", "", "Agenda" },
                    { 34, "Law", "RegulatedConscription", "BaseGame", "", "Agenda" },
                    { 35, "Law", "RepresentativeGovernment", "Deprecated", "", "Agenda" },
                    { 36, "Law", "ResearchTeamBiotic", "Deprecated", "", "Agenda" },
                    { 37, "Law", "ResearchTeamCybernetic", "Deprecated", "", "Agenda" },
                    { 38, "Law", "ResearchTeamPropulsion", "Deprecated", "", "Agenda" },
                    { 39, "Law", "ResearchTeamWarfare", "Deprecated", "", "Agenda" },
                    { 40, "Directive", "SeedOfAnEmpire", "BaseGame", "", "Agenda" },
                    { 41, "Law", "SenateSanctuary", "Deprecated", "", "Agenda" },
                    { 42, "Law", "ShardOfTheThrone", "Deprecated", "", "Agenda" },
                    { 43, "Law", "SharedResearch", "BaseGame", "", "Agenda" },
                    { 44, "Directive", "SwordsToPlowshares", "BaseGame", "", "Agenda" },
                    { 45, "Law", "TerraformingInitiative", "Deprecated", "", "Agenda" },
                    { 46, "Law", "TheCrownOfEmphidia", "Deprecated", "", "Agenda" },
                    { 47, "Law", "TheCrownOfThalnos", "Deprecated", "", "Agenda" },
                    { 48, "Directive", "UnconventionalMeasures", "BaseGame", "", "Agenda" },
                    { 49, "Law", "WormholeReconstruction", "BaseGame", "", "Agenda" },
                    { 50, "Directive", "WormholeResearch", "BaseGame", "", "Agenda" },
                    { 51, "Directive", "ArmedForcesStandardization", "ProphecyOfKings", "", "Agenda" },
                    { 52, "Law", "ArticlesOfWar", "ProphecyOfKings", "", "Agenda" },
                    { 53, "Law", "ChecksAndBalances", "ProphecyOfKings", "", "Agenda" },
                    { 54, "Directive", "ClandestineOperations", "ProphecyOfKings", "", "Agenda" },
                    { 55, "Directive", "CovertLegislation", "ProphecyOfKings", "", "Agenda" },
                    { 56, "Directive", "GalacticCrisisPact", "ProphecyOfKings", "", "Agenda" },
                    { 57, "Directive", "MinisterOfAntiquities", "ProphecyOfKings", "", "Agenda" },
                    { 58, "Law", "NexusSovereignty", "ProphecyOfKings", "", "Agenda" },
                    { 59, "Law", "PoliticalCensure", "ProphecyOfKings", "", "Agenda" },
                    { 60, "Directive", "RearmamentAgreement", "ProphecyOfKings", "", "Agenda" },
                    { 61, "Law", "RepresentativeGovernmentProphecyOfKings", "ProphecyOfKings", "", "Agenda" },
                    { 62, "Directive", "ResearchGrantReallocation", "ProphecyOfKings", "", "Agenda" },
                    { 63, "Law", "SearchWarrant", "ProphecyOfKings", "", "Agenda" }
                });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "2147411d-19b7-4936-800a-b8d815271d00", null, "Admin", "ADMIN" },
                    { "5b2bee5c-e5ce-4472-a141-bff7e040ac78", null, "Moderator", "MODERATOR" },
                    { "cc4089b0-22e9-47df-b7c5-a4734b4423f4", null, "User", "USER" }
                });

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "Age", "ConcurrencyStamp", "DiscordId", "Email", "EmailConfirmed", "FavoriteFaction", "FirstName", "LastName", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "SteamId", "TwoFactorEnabled", "UserInfo", "UserName" },
                values: new object[] { "1", 0, null, "b3f1533b-9a81-4b4e-9717-4ed06275b455", "", "Test@user.cz", true, 17, "Test", "User", false, null, "TEST@USER.CZ", "ADMIN", "AQAAAAIAAYagAAAAEBR3YUVjbEJ90H/W4UJvhWxkzFGKDcfkvuXqznAhS6vj0p8vBUgFBKepfxoHMw2wmA==", null, false, "9ccae6b7-e015-4b21-8b07-6d4d85b6192b", "", false, "First seeded test user", "Admin" });

            migrationBuilder.InsertData(
                schema: "Card",
                table: "ExplorationCards",
                columns: new[] { "Id", "EnumName", "ExplorationPlanetTrait", "GameVersion", "Text", "Type" },
                values: new object[,]
                {
                    { 1, "CulturalRelicFragment", "Cultural", "ProphecyOfKings", "", "Exploration" },
                    { 2, "DemilitarizedZone", "Cultural", "ProphecyOfKings", "", "Exploration" },
                    { 3, "DysonSphere", "Cultural", "ProphecyOfKings", "", "Exploration" },
                    { 4, "Freelancers", "Cultural", "ProphecyOfKings", "", "Exploration" },
                    { 5, "GammaWormhole", "Cultural", "ProphecyOfKings", "", "Exploration" },
                    { 6, "MercenaryOutfit", "Cultural", "ProphecyOfKings", "", "Exploration" },
                    { 7, "ParadiseWorld", "Cultural", "ProphecyOfKings", "", "Exploration" },
                    { 8, "TombOfEmphidia", "Cultural", "ProphecyOfKings", "", "Exploration" },
                    { 9, "CouncilPreserve", "Cultural", "UnchartedSpace", "", "Exploration" },
                    { 10, "DesertedTradeStation", "Cultural", "UnchartedSpace", "", "Exploration" },
                    { 11, "DistinguishedAdmiral", "Cultural", "UnchartedSpace", "", "Exploration" },
                    { 12, "StarChartCultural", "Cultural", "UnchartedSpace", "", "Exploration" },
                    { 13, "CoreMine", "Hazardous", "ProphecyOfKings", "", "Exploration" },
                    { 14, "Expedition", "Hazardous", "ProphecyOfKings", "", "Exploration" },
                    { 15, "HazardousRelicFragment", "Hazardous", "ProphecyOfKings", "", "Exploration" },
                    { 16, "LazaxSurvivors", "Hazardous", "ProphecyOfKings", "", "Exploration" },
                    { 17, "MiningWorld", "Hazardous", "ProphecyOfKings", "", "Exploration" },
                    { 18, "RichWorld", "Hazardous", "ProphecyOfKings", "", "Exploration" },
                    { 19, "VolatileFuelSource", "Hazardous", "ProphecyOfKings", "", "Exploration" },
                    { 20, "WarfareResearchFacility", "Hazardous", "ProphecyOfKings", "", "Exploration" },
                    { 21, "ArcaneCitadel", "Hazardous", "UnchartedSpace", "", "Exploration" },
                    { 22, "ScorchedDepot", "Hazardous", "UnchartedSpace", "", "Exploration" },
                    { 23, "SeedySpaceport", "Hazardous", "UnchartedSpace", "", "Exploration" },
                    { 24, "StarChartHazardous", "Hazardous", "UnchartedSpace", "", "Exploration" },
                    { 25, "AbandonedWarehouses", "Industrial", "ProphecyOfKings", "", "Exploration" },
                    { 26, "BioticResearchFacility", "Industrial", "ProphecyOfKings", "", "Exploration" },
                    { 27, "CyberneticResearchFacility", "Industrial", "ProphecyOfKings", "", "Exploration" },
                    { 28, "FunctioningBase", "Industrial", "ProphecyOfKings", "", "Exploration" },
                    { 29, "IndustrialRelicFragment", "Industrial", "ProphecyOfKings", "", "Exploration" },
                    { 30, "LocalFabricators", "Industrial", "ProphecyOfKings", "", "Exploration" },
                    { 31, "PropulsionResearchFacility", "Industrial", "ProphecyOfKings", "", "Exploration" },
                    { 32, "AncientShipyard", "Industrial", "UnchartedSpace", "", "Exploration" },
                    { 33, "HiddenLab", "Industrial", "UnchartedSpace", "", "Exploration" },
                    { 34, "OrbitalFoundries", "Industrial", "UnchartedSpace", "", "Exploration" },
                    { 35, "StarChartIndustrial", "Industrial", "UnchartedSpace", "", "Exploration" }
                });

            migrationBuilder.InsertData(
                schema: "Faction",
                table: "FactionColorImportances",
                columns: new[] { "Id", "Color", "FactionName", "Importance" },
                values: new object[,]
                {
                    { 1, "Red", "TheArborec", 1 },
                    { 2, "Orange", "TheArborec", 2 },
                    { 3, "Yellow", "TheArborec", 6 },
                    { 4, "Green", "TheArborec", 10 },
                    { 5, "Blue", "TheArborec", 0 },
                    { 6, "Purple", "TheArborec", 1 },
                    { 7, "Pink", "TheArborec", 0 },
                    { 8, "Black", "TheArborec", 2 },
                    { 9, "Red", "TheArgentFlight", 2 },
                    { 10, "Orange", "TheArgentFlight", 10 },
                    { 11, "Yellow", "TheArgentFlight", 2 },
                    { 12, "Green", "TheArgentFlight", 7 },
                    { 13, "Blue", "TheArgentFlight", 2 },
                    { 14, "Purple", "TheArgentFlight", 2 },
                    { 15, "Pink", "TheArgentFlight", 0 },
                    { 16, "Black", "TheArgentFlight", 1 },
                    { 17, "Red", "TheBaronyOfLetnev", 8 },
                    { 18, "Orange", "TheBaronyOfLetnev", 2 },
                    { 19, "Yellow", "TheBaronyOfLetnev", 0 },
                    { 20, "Green", "TheBaronyOfLetnev", 1 },
                    { 21, "Blue", "TheBaronyOfLetnev", 4 },
                    { 22, "Purple", "TheBaronyOfLetnev", 0 },
                    { 23, "Pink", "TheBaronyOfLetnev", 0 },
                    { 24, "Black", "TheBaronyOfLetnev", 9 },
                    { 25, "Red", "TheClanOfSaar", 1 },
                    { 26, "Orange", "TheClanOfSaar", 9 },
                    { 27, "Yellow", "TheClanOfSaar", 8 },
                    { 28, "Green", "TheClanOfSaar", 1 },
                    { 29, "Blue", "TheClanOfSaar", 0 },
                    { 30, "Purple", "TheClanOfSaar", 1 },
                    { 31, "Pink", "TheClanOfSaar", 0 },
                    { 32, "Black", "TheClanOfSaar", 2 },
                    { 33, "Red", "TheEmbersOfMuaat", 8 },
                    { 34, "Orange", "TheEmbersOfMuaat", 9 },
                    { 35, "Yellow", "TheEmbersOfMuaat", 4 },
                    { 36, "Green", "TheEmbersOfMuaat", 0 },
                    { 37, "Blue", "TheEmbersOfMuaat", 0 },
                    { 38, "Purple", "TheEmbersOfMuaat", 0 },
                    { 39, "Pink", "TheEmbersOfMuaat", 0 },
                    { 40, "Black", "TheEmbersOfMuaat", 4 },
                    { 41, "Red", "TheEmiratesOfHacan", 4 },
                    { 42, "Orange", "TheEmiratesOfHacan", 8 },
                    { 43, "Yellow", "TheEmiratesOfHacan", 9 },
                    { 44, "Green", "TheEmiratesOfHacan", 0 },
                    { 45, "Blue", "TheEmiratesOfHacan", 0 },
                    { 46, "Purple", "TheEmiratesOfHacan", 0 },
                    { 47, "Pink", "TheEmiratesOfHacan", 0 },
                    { 48, "Black", "TheEmiratesOfHacan", 1 },
                    { 49, "Red", "TheEmpyrean", 7 },
                    { 50, "Orange", "TheEmpyrean", 4 },
                    { 51, "Yellow", "TheEmpyrean", 1 },
                    { 52, "Green", "TheEmpyrean", 0 },
                    { 53, "Blue", "TheEmpyrean", 2 },
                    { 54, "Purple", "TheEmpyrean", 9 },
                    { 55, "Pink", "TheEmpyrean", 3 },
                    { 56, "Black", "TheEmpyrean", 5 },
                    { 57, "Red", "TheFederationOfSol", 2 },
                    { 58, "Orange", "TheFederationOfSol", 0 },
                    { 59, "Yellow", "TheFederationOfSol", 8 },
                    { 60, "Green", "TheFederationOfSol", 2 },
                    { 61, "Blue", "TheFederationOfSol", 9 },
                    { 62, "Purple", "TheFederationOfSol", 0 },
                    { 63, "Pink", "TheFederationOfSol", 0 },
                    { 64, "Black", "TheFederationOfSol", 1 },
                    { 65, "Red", "TheGhostsOfCreuss", 0 },
                    { 66, "Orange", "TheGhostsOfCreuss", 1 },
                    { 67, "Yellow", "TheGhostsOfCreuss", 0 },
                    { 68, "Green", "TheGhostsOfCreuss", 1 },
                    { 69, "Blue", "TheGhostsOfCreuss", 10 },
                    { 70, "Purple", "TheGhostsOfCreuss", 2 },
                    { 71, "Pink", "TheGhostsOfCreuss", 1 },
                    { 72, "Black", "TheGhostsOfCreuss", 7 },
                    { 73, "Red", "TheL1z1xMindnet", 8 },
                    { 74, "Orange", "TheL1z1xMindnet", 1 },
                    { 75, "Yellow", "TheL1z1xMindnet", 0 },
                    { 76, "Green", "TheL1z1xMindnet", 1 },
                    { 77, "Blue", "TheL1z1xMindnet", 9 },
                    { 78, "Purple", "TheL1z1xMindnet", 0 },
                    { 79, "Pink", "TheL1z1xMindnet", 0 },
                    { 80, "Black", "TheL1z1xMindnet", 8 },
                    { 81, "Red", "TheMahactGeneSorcerers", 1 },
                    { 82, "Orange", "TheMahactGeneSorcerers", 0 },
                    { 83, "Yellow", "TheMahactGeneSorcerers", 10 },
                    { 84, "Green", "TheMahactGeneSorcerers", 0 },
                    { 85, "Blue", "TheMahactGeneSorcerers", 0 },
                    { 86, "Purple", "TheMahactGeneSorcerers", 7 },
                    { 87, "Pink", "TheMahactGeneSorcerers", 2 },
                    { 88, "Black", "TheMahactGeneSorcerers", 1 },
                    { 89, "Red", "TheMentakCoalition", 0 },
                    { 90, "Orange", "TheMentakCoalition", 9 },
                    { 91, "Yellow", "TheMentakCoalition", 9 },
                    { 92, "Green", "TheMentakCoalition", 0 },
                    { 93, "Blue", "TheMentakCoalition", 0 },
                    { 94, "Purple", "TheMentakCoalition", 4 },
                    { 95, "Pink", "TheMentakCoalition", 0 },
                    { 96, "Black", "TheMentakCoalition", 8 },
                    { 97, "Red", "TheNaaluCollective", 0 },
                    { 98, "Orange", "TheNaaluCollective", 9 },
                    { 99, "Yellow", "TheNaaluCollective", 9 },
                    { 100, "Green", "TheNaaluCollective", 8 },
                    { 101, "Blue", "TheNaaluCollective", 0 },
                    { 102, "Purple", "TheNaaluCollective", 1 },
                    { 103, "Pink", "TheNaaluCollective", 0 },
                    { 104, "Black", "TheNaaluCollective", 1 },
                    { 105, "Red", "TheNaazRokhaAlliance", 1 },
                    { 106, "Orange", "TheNaazRokhaAlliance", 0 },
                    { 107, "Yellow", "TheNaazRokhaAlliance", 7 },
                    { 108, "Green", "TheNaazRokhaAlliance", 10 },
                    { 109, "Blue", "TheNaazRokhaAlliance", 0 },
                    { 110, "Purple", "TheNaazRokhaAlliance", 1 },
                    { 111, "Pink", "TheNaazRokhaAlliance", 0 },
                    { 112, "Black", "TheNaazRokhaAlliance", 2 },
                    { 113, "Red", "TheNekroVirus", 10 },
                    { 114, "Orange", "TheNekroVirus", 1 },
                    { 115, "Yellow", "TheNekroVirus", 1 },
                    { 116, "Green", "TheNekroVirus", 0 },
                    { 117, "Blue", "TheNekroVirus", 1 },
                    { 118, "Purple", "TheNekroVirus", 1 },
                    { 119, "Pink", "TheNekroVirus", 0 },
                    { 120, "Black", "TheNekroVirus", 4 },
                    { 121, "Red", "TheNomad", 1 },
                    { 122, "Orange", "TheNomad", 1 },
                    { 123, "Yellow", "TheNomad", 2 },
                    { 124, "Green", "TheNomad", 1 },
                    { 125, "Blue", "TheNomad", 8 },
                    { 126, "Purple", "TheNomad", 8 },
                    { 127, "Pink", "TheNomad", 7 },
                    { 128, "Black", "TheNomad", 4 },
                    { 129, "Red", "SardakkNorr", 9 },
                    { 130, "Orange", "SardakkNorr", 0 },
                    { 131, "Yellow", "SardakkNorr", 2 },
                    { 132, "Green", "SardakkNorr", 4 },
                    { 133, "Blue", "SardakkNorr", 0 },
                    { 134, "Purple", "SardakkNorr", 0 },
                    { 135, "Pink", "SardakkNorr", 0 },
                    { 136, "Black", "SardakkNorr", 8 },
                    { 137, "Red", "TheTitansOfUl", 0 },
                    { 138, "Orange", "TheTitansOfUl", 0 },
                    { 139, "Yellow", "TheTitansOfUl", 1 },
                    { 140, "Green", "TheTitansOfUl", 1 },
                    { 141, "Blue", "TheTitansOfUl", 1 },
                    { 142, "Purple", "TheTitansOfUl", 4 },
                    { 143, "Pink", "TheTitansOfUl", 10 },
                    { 144, "Black", "TheTitansOfUl", 1 },
                    { 145, "Red", "TheUniversitiesOfJolNar", 0 },
                    { 146, "Orange", "TheUniversitiesOfJolNar", 0 },
                    { 147, "Yellow", "TheUniversitiesOfJolNar", 1 },
                    { 148, "Green", "TheUniversitiesOfJolNar", 2 },
                    { 149, "Blue", "TheUniversitiesOfJolNar", 8 },
                    { 150, "Purple", "TheUniversitiesOfJolNar", 9 },
                    { 151, "Pink", "TheUniversitiesOfJolNar", 4 },
                    { 152, "Black", "TheUniversitiesOfJolNar", 1 },
                    { 153, "Red", "TheVuilRaithCabal", 10 },
                    { 154, "Orange", "TheVuilRaithCabal", 6 },
                    { 155, "Yellow", "TheVuilRaithCabal", 1 },
                    { 156, "Green", "TheVuilRaithCabal", 0 },
                    { 157, "Blue", "TheVuilRaithCabal", 0 },
                    { 158, "Purple", "TheVuilRaithCabal", 0 },
                    { 159, "Pink", "TheVuilRaithCabal", 0 },
                    { 160, "Black", "TheVuilRaithCabal", 6 },
                    { 161, "Red", "TheWinnu", 6 },
                    { 162, "Orange", "TheWinnu", 8 },
                    { 163, "Yellow", "TheWinnu", 8 },
                    { 164, "Green", "TheWinnu", 2 },
                    { 165, "Blue", "TheWinnu", 0 },
                    { 166, "Purple", "TheWinnu", 9 },
                    { 167, "Pink", "TheWinnu", 1 },
                    { 168, "Black", "TheWinnu", 1 },
                    { 169, "Red", "TheXxchaKingdom", 0 },
                    { 170, "Orange", "TheXxchaKingdom", 1 },
                    { 171, "Yellow", "TheXxchaKingdom", 4 },
                    { 172, "Green", "TheXxchaKingdom", 10 },
                    { 173, "Blue", "TheXxchaKingdom", 6 },
                    { 174, "Purple", "TheXxchaKingdom", 1 },
                    { 175, "Pink", "TheXxchaKingdom", 0 },
                    { 176, "Black", "TheXxchaKingdom", 2 },
                    { 177, "Red", "TheYinBrotherhood", 0 },
                    { 178, "Orange", "TheYinBrotherhood", 1 },
                    { 179, "Yellow", "TheYinBrotherhood", 9 },
                    { 180, "Green", "TheYinBrotherhood", 1 },
                    { 181, "Blue", "TheYinBrotherhood", 2 },
                    { 182, "Purple", "TheYinBrotherhood", 0 },
                    { 183, "Pink", "TheYinBrotherhood", 7 },
                    { 184, "Black", "TheYinBrotherhood", 1 },
                    { 185, "Red", "TheYssarilTribes", 1 },
                    { 186, "Orange", "TheYssarilTribes", 0 },
                    { 187, "Yellow", "TheYssarilTribes", 0 },
                    { 188, "Green", "TheYssarilTribes", 9 },
                    { 189, "Blue", "TheYssarilTribes", 0 },
                    { 190, "Purple", "TheYssarilTribes", 3 },
                    { 191, "Pink", "TheYssarilTribes", 1 },
                    { 192, "Black", "TheYssarilTribes", 2 },
                    { 193, "Red", "TheCouncilKeleres", 0 },
                    { 194, "Orange", "TheCouncilKeleres", 6 },
                    { 195, "Yellow", "TheCouncilKeleres", 6 },
                    { 196, "Green", "TheCouncilKeleres", 0 },
                    { 197, "Blue", "TheCouncilKeleres", 5 },
                    { 198, "Purple", "TheCouncilKeleres", 8 },
                    { 199, "Pink", "TheCouncilKeleres", 7 },
                    { 200, "Black", "TheCouncilKeleres", 0 },
                    { 201, "Red", "TheAugursOfIlyxum", 7 },
                    { 202, "Orange", "TheAugursOfIlyxum", 8 },
                    { 203, "Yellow", "TheAugursOfIlyxum", 3 },
                    { 204, "Green", "TheAugursOfIlyxum", 0 },
                    { 205, "Blue", "TheAugursOfIlyxum", 0 },
                    { 206, "Purple", "TheAugursOfIlyxum", 9 },
                    { 207, "Pink", "TheAugursOfIlyxum", 0 },
                    { 208, "Black", "TheAugursOfIlyxum", 0 },
                    { 209, "Red", "TheCeldauriTradeConfederation", 0 },
                    { 210, "Orange", "TheCeldauriTradeConfederation", 4 },
                    { 211, "Yellow", "TheCeldauriTradeConfederation", 9 },
                    { 212, "Green", "TheCeldauriTradeConfederation", 0 },
                    { 213, "Blue", "TheCeldauriTradeConfederation", 7 },
                    { 214, "Purple", "TheCeldauriTradeConfederation", 0 },
                    { 215, "Pink", "TheCeldauriTradeConfederation", 0 },
                    { 216, "Black", "TheCeldauriTradeConfederation", 2 },
                    { 217, "Red", "TheDihMohnFlotilla", 6 },
                    { 218, "Orange", "TheDihMohnFlotilla", 0 },
                    { 219, "Yellow", "TheDihMohnFlotilla", 0 },
                    { 220, "Green", "TheDihMohnFlotilla", 0 },
                    { 221, "Blue", "TheDihMohnFlotilla", 2 },
                    { 222, "Purple", "TheDihMohnFlotilla", 10 },
                    { 223, "Pink", "TheDihMohnFlotilla", 8 },
                    { 224, "Black", "TheDihMohnFlotilla", 1 },
                    { 225, "Red", "TheFlorzenProfiteers", 0 },
                    { 226, "Orange", "TheFlorzenProfiteers", 5 },
                    { 227, "Yellow", "TheFlorzenProfiteers", 0 },
                    { 228, "Green", "TheFlorzenProfiteers", 7 },
                    { 229, "Blue", "TheFlorzenProfiteers", 9 },
                    { 230, "Purple", "TheFlorzenProfiteers", 3 },
                    { 231, "Pink", "TheFlorzenProfiteers", 2 },
                    { 232, "Black", "TheFlorzenProfiteers", 0 },
                    { 233, "Red", "TheFreeSystemsCompact", 0 },
                    { 234, "Orange", "TheFreeSystemsCompact", 0 },
                    { 235, "Yellow", "TheFreeSystemsCompact", 0 },
                    { 236, "Green", "TheFreeSystemsCompact", 5 },
                    { 237, "Blue", "TheFreeSystemsCompact", 2 },
                    { 238, "Purple", "TheFreeSystemsCompact", 8 },
                    { 239, "Pink", "TheFreeSystemsCompact", 7 },
                    { 240, "Black", "TheFreeSystemsCompact", 3 },
                    { 241, "Red", "TheGheminaRaiders", 0 },
                    { 242, "Orange", "TheGheminaRaiders", 0 },
                    { 243, "Yellow", "TheGheminaRaiders", 0 },
                    { 244, "Green", "TheGheminaRaiders", 4 },
                    { 245, "Blue", "TheGheminaRaiders", 10 },
                    { 246, "Purple", "TheGheminaRaiders", 5 },
                    { 247, "Pink", "TheGheminaRaiders", 0 },
                    { 248, "Black", "TheGheminaRaiders", 8 },
                    { 249, "Red", "TheGlimmerOfMortheus", 5 },
                    { 250, "Orange", "TheGlimmerOfMortheus", 0 },
                    { 251, "Yellow", "TheGlimmerOfMortheus", 0 },
                    { 252, "Green", "TheGlimmerOfMortheus", 0 },
                    { 253, "Blue", "TheGlimmerOfMortheus", 7 },
                    { 254, "Purple", "TheGlimmerOfMortheus", 2 },
                    { 255, "Pink", "TheGlimmerOfMortheus", 9 },
                    { 256, "Black", "TheGlimmerOfMortheus", 1 },
                    { 257, "Red", "TheKolleccSociety", 5 },
                    { 258, "Orange", "TheKolleccSociety", 9 },
                    { 259, "Yellow", "TheKolleccSociety", 0 },
                    { 260, "Green", "TheKolleccSociety", 2 },
                    { 261, "Blue", "TheKolleccSociety", 3 },
                    { 262, "Purple", "TheKolleccSociety", 0 },
                    { 263, "Pink", "TheKolleccSociety", 0 },
                    { 264, "Black", "TheKolleccSociety", 7 },
                    { 265, "Red", "TheKortaliTribunal", 0 },
                    { 266, "Orange", "TheKortaliTribunal", 2 },
                    { 267, "Yellow", "TheKortaliTribunal", 0 },
                    { 268, "Green", "TheKortaliTribunal", 3 },
                    { 269, "Blue", "TheKortaliTribunal", 0 },
                    { 270, "Purple", "TheKortaliTribunal", 8 },
                    { 271, "Pink", "TheKortaliTribunal", 0 },
                    { 272, "Black", "TheKortaliTribunal", 10 },
                    { 273, "Red", "TheLiZhoDynasty", 2 },
                    { 274, "Orange", "TheLiZhoDynasty", 7 },
                    { 275, "Yellow", "TheLiZhoDynasty", 5 },
                    { 276, "Green", "TheLiZhoDynasty", 0 },
                    { 277, "Blue", "TheLiZhoDynasty", 0 },
                    { 278, "Purple", "TheLiZhoDynasty", 0 },
                    { 279, "Pink", "TheLiZhoDynasty", 0 },
                    { 280, "Black", "TheLiZhoDynasty", 9 },
                    { 281, "Red", "TheLTokkKhrask", 0 },
                    { 282, "Orange", "TheLTokkKhrask", 7 },
                    { 283, "Yellow", "TheLTokkKhrask", 2 },
                    { 284, "Green", "TheLTokkKhrask", 9 },
                    { 285, "Blue", "TheLTokkKhrask", 5 },
                    { 286, "Purple", "TheLTokkKhrask", 0 },
                    { 287, "Pink", "TheLTokkKhrask", 0 },
                    { 288, "Black", "TheLTokkKhrask", 0 },
                    { 289, "Red", "TheMirvedaProtectorate", 0 },
                    { 290, "Orange", "TheMirvedaProtectorate", 0 },
                    { 291, "Yellow", "TheMirvedaProtectorate", 0 },
                    { 292, "Green", "TheMirvedaProtectorate", 0 },
                    { 293, "Blue", "TheMirvedaProtectorate", 8 },
                    { 294, "Purple", "TheMirvedaProtectorate", 7 },
                    { 295, "Pink", "TheMirvedaProtectorate", 10 },
                    { 296, "Black", "TheMirvedaProtectorate", 2 },
                    { 297, "Red", "TheMykoMentori", 0 },
                    { 298, "Orange", "TheMykoMentori", 7 },
                    { 299, "Yellow", "TheMykoMentori", 0 },
                    { 300, "Green", "TheMykoMentori", 9 },
                    { 301, "Blue", "TheMykoMentori", 3 },
                    { 302, "Purple", "TheMykoMentori", 0 },
                    { 303, "Pink", "TheMykoMentori", 8 },
                    { 304, "Black", "TheMykoMentori", 0 },
                    { 305, "Red", "TheNivynStarKings", 0 },
                    { 306, "Orange", "TheNivynStarKings", 0 },
                    { 307, "Yellow", "TheNivynStarKings", 8 },
                    { 308, "Green", "TheNivynStarKings", 4 },
                    { 309, "Blue", "TheNivynStarKings", 2 },
                    { 310, "Purple", "TheNivynStarKings", 0 },
                    { 311, "Pink", "TheNivynStarKings", 0 },
                    { 312, "Black", "TheNivynStarKings", 9 },
                    { 313, "Red", "TheOlradinLeague", 0 },
                    { 314, "Orange", "TheOlradinLeague", 10 },
                    { 315, "Yellow", "TheOlradinLeague", 8 },
                    { 316, "Green", "TheOlradinLeague", 0 },
                    { 317, "Blue", "TheOlradinLeague", 6 },
                    { 318, "Purple", "TheOlradinLeague", 0 },
                    { 319, "Pink", "TheOlradinLeague", 0 },
                    { 320, "Black", "TheOlradinLeague", 2 },
                    { 321, "Red", "RohDhnaMechatronics", 0 },
                    { 322, "Orange", "RohDhnaMechatronics", 0 },
                    { 323, "Yellow", "RohDhnaMechatronics", 0 },
                    { 324, "Green", "RohDhnaMechatronics", 0 },
                    { 325, "Blue", "RohDhnaMechatronics", 9 },
                    { 326, "Purple", "RohDhnaMechatronics", 6 },
                    { 327, "Pink", "RohDhnaMechatronics", 3 },
                    { 328, "Black", "RohDhnaMechatronics", 8 },
                    { 329, "Red", "TheSavagesOfCymiae", 8 },
                    { 330, "Orange", "TheSavagesOfCymiae", 9 },
                    { 331, "Yellow", "TheSavagesOfCymiae", 0 },
                    { 332, "Green", "TheSavagesOfCymiae", 3 },
                    { 333, "Blue", "TheSavagesOfCymiae", 5 },
                    { 334, "Purple", "TheSavagesOfCymiae", 0 },
                    { 335, "Pink", "TheSavagesOfCymiae", 0 },
                    { 336, "Black", "TheSavagesOfCymiae", 0 },
                    { 337, "Red", "TheShipwrightsofAxis", 8 },
                    { 338, "Orange", "TheShipwrightsofAxis", 0 },
                    { 339, "Yellow", "TheShipwrightsofAxis", 6 },
                    { 340, "Green", "TheShipwrightsofAxis", 2 },
                    { 341, "Blue", "TheShipwrightsofAxis", 4 },
                    { 342, "Purple", "TheShipwrightsofAxis", 0 },
                    { 343, "Pink", "TheShipwrightsofAxis", 0 },
                    { 344, "Black", "TheShipwrightsofAxis", 10 },
                    { 345, "Red", "TheTnelisSyndicate", 5 },
                    { 346, "Orange", "TheTnelisSyndicate", 7 },
                    { 347, "Yellow", "TheTnelisSyndicate", 0 },
                    { 348, "Green", "TheTnelisSyndicate", 9 },
                    { 349, "Blue", "TheTnelisSyndicate", 2 },
                    { 350, "Purple", "TheTnelisSyndicate", 0 },
                    { 351, "Pink", "TheTnelisSyndicate", 0 },
                    { 352, "Black", "TheTnelisSyndicate", 0 },
                    { 353, "Red", "TheVadenBankingClans", 0 },
                    { 354, "Orange", "TheVadenBankingClans", 0 },
                    { 355, "Yellow", "TheVadenBankingClans", 7 },
                    { 356, "Green", "TheVadenBankingClans", 9 },
                    { 357, "Blue", "TheVadenBankingClans", 5 },
                    { 358, "Purple", "TheVadenBankingClans", 0 },
                    { 359, "Pink", "TheVadenBankingClans", 0 },
                    { 360, "Black", "TheVadenBankingClans", 2 },
                    { 361, "Red", "TheVaylerianScourge", 3 },
                    { 362, "Orange", "TheVaylerianScourge", 0 },
                    { 363, "Yellow", "TheVaylerianScourge", 0 },
                    { 364, "Green", "TheVaylerianScourge", 8 },
                    { 365, "Blue", "TheVaylerianScourge", 9 },
                    { 366, "Purple", "TheVaylerianScourge", 0 },
                    { 367, "Pink", "TheVaylerianScourge", 0 },
                    { 368, "Black", "TheVaylerianScourge", 1 },
                    { 369, "Red", "TheVeldyrSovereignty", 0 },
                    { 370, "Orange", "TheVeldyrSovereignty", 2 },
                    { 371, "Yellow", "TheVeldyrSovereignty", 0 },
                    { 372, "Green", "TheVeldyrSovereignty", 6 },
                    { 373, "Blue", "TheVeldyrSovereignty", 9 },
                    { 374, "Purple", "TheVeldyrSovereignty", 0 },
                    { 375, "Pink", "TheVeldyrSovereignty", 0 },
                    { 376, "Black", "TheVeldyrSovereignty", 8 },
                    { 377, "Red", "TheZealotsOfRhodun", 0 },
                    { 378, "Orange", "TheZealotsOfRhodun", 6 },
                    { 379, "Yellow", "TheZealotsOfRhodun", 0 },
                    { 380, "Green", "TheZealotsOfRhodun", 2 },
                    { 381, "Blue", "TheZealotsOfRhodun", 8 },
                    { 382, "Purple", "TheZealotsOfRhodun", 0 },
                    { 383, "Pink", "TheZealotsOfRhodun", 0 },
                    { 384, "Black", "TheZealotsOfRhodun", 9 },
                    { 385, "Red", "TheZelianPurifier", 10 },
                    { 386, "Orange", "TheZelianPurifier", 6 },
                    { 387, "Yellow", "TheZelianPurifier", 2 },
                    { 388, "Green", "TheZelianPurifier", 0 },
                    { 389, "Blue", "TheZelianPurifier", 0 },
                    { 390, "Purple", "TheZelianPurifier", 0 },
                    { 391, "Pink", "TheZelianPurifier", 0 },
                    { 392, "Black", "TheZelianPurifier", 8 },
                    { 393, "Red", "TheBentorConglomerate", 0 },
                    { 394, "Orange", "TheBentorConglomerate", 0 },
                    { 395, "Yellow", "TheBentorConglomerate", 0 },
                    { 396, "Green", "TheBentorConglomerate", 5 },
                    { 397, "Blue", "TheBentorConglomerate", 7 },
                    { 398, "Purple", "TheBentorConglomerate", 8 },
                    { 399, "Pink", "TheBentorConglomerate", 3 },
                    { 400, "Black", "TheBentorConglomerate", 0 },
                    { 401, "Red", "TheCheiranHordes", 6 },
                    { 402, "Orange", "TheCheiranHordes", 9 },
                    { 403, "Yellow", "TheCheiranHordes", 2 },
                    { 404, "Green", "TheCheiranHordes", 0 },
                    { 405, "Blue", "TheCheiranHordes", 7 },
                    { 406, "Purple", "TheCheiranHordes", 0 },
                    { 407, "Pink", "TheCheiranHordes", 0 },
                    { 408, "Black", "TheCheiranHordes", 0 },
                    { 409, "Red", "TheEdynMandate", 1 },
                    { 410, "Orange", "TheEdynMandate", 5 },
                    { 411, "Yellow", "TheEdynMandate", 7 },
                    { 412, "Green", "TheEdynMandate", 0 },
                    { 413, "Blue", "TheEdynMandate", 0 },
                    { 414, "Purple", "TheEdynMandate", 2 },
                    { 415, "Pink", "TheEdynMandate", 0 },
                    { 416, "Black", "TheEdynMandate", 9 },
                    { 417, "Red", "TheGhotiWayfarers", 0 },
                    { 418, "Orange", "TheGhotiWayfarers", 6 },
                    { 419, "Yellow", "TheGhotiWayfarers", 0 },
                    { 420, "Green", "TheGhotiWayfarers", 9 },
                    { 421, "Blue", "TheGhotiWayfarers", 10 },
                    { 422, "Purple", "TheGhotiWayfarers", 2 },
                    { 423, "Pink", "TheGhotiWayfarers", 1 },
                    { 424, "Black", "TheGhotiWayfarers", 0 },
                    { 425, "Red", "TheGledgeUnion", 0 },
                    { 426, "Orange", "TheGledgeUnion", 9 },
                    { 427, "Yellow", "TheGledgeUnion", 5 },
                    { 428, "Green", "TheGledgeUnion", 8 },
                    { 429, "Blue", "TheGledgeUnion", 2 },
                    { 430, "Purple", "TheGledgeUnion", 0 },
                    { 431, "Pink", "TheGledgeUnion", 0 },
                    { 432, "Black", "TheGledgeUnion", 0 },
                    { 433, "Red", "TheBerserkersOfKjalengard", 7 },
                    { 434, "Orange", "TheBerserkersOfKjalengard", 1 },
                    { 435, "Yellow", "TheBerserkersOfKjalengard", 0 },
                    { 436, "Green", "TheBerserkersOfKjalengard", 0 },
                    { 437, "Blue", "TheBerserkersOfKjalengard", 9 },
                    { 438, "Purple", "TheBerserkersOfKjalengard", 8 },
                    { 439, "Pink", "TheBerserkersOfKjalengard", 2 },
                    { 440, "Black", "TheBerserkersOfKjalengard", 0 },
                    { 441, "Red", "TheMonksOfKolume", 0 },
                    { 442, "Orange", "TheMonksOfKolume", 8 },
                    { 443, "Yellow", "TheMonksOfKolume", 0 },
                    { 444, "Green", "TheMonksOfKolume", 0 },
                    { 445, "Blue", "TheMonksOfKolume", 5 },
                    { 446, "Purple", "TheMonksOfKolume", 2 },
                    { 447, "Pink", "TheMonksOfKolume", 1 },
                    { 448, "Black", "TheMonksOfKolume", 9 },
                    { 449, "Red", "TheKyroSodality", 1 },
                    { 450, "Orange", "TheKyroSodality", 2 },
                    { 451, "Yellow", "TheKyroSodality", 7 },
                    { 452, "Green", "TheKyroSodality", 9 },
                    { 453, "Blue", "TheKyroSodality", 3 },
                    { 454, "Purple", "TheKyroSodality", 0 },
                    { 455, "Pink", "TheKyroSodality", 0 },
                    { 456, "Black", "TheKyroSodality", 0 },
                    { 457, "Red", "TheLanefirRemnants", 0 },
                    { 458, "Orange", "TheLanefirRemnants", 8 },
                    { 459, "Yellow", "TheLanefirRemnants", 1 },
                    { 460, "Green", "TheLanefirRemnants", 9 },
                    { 461, "Blue", "TheLanefirRemnants", 4 },
                    { 462, "Purple", "TheLanefirRemnants", 0 },
                    { 463, "Pink", "TheLanefirRemnants", 0 },
                    { 464, "Black", "TheLanefirRemnants", 2 },
                    { 465, "Red", "TheNokarSellships", 0 },
                    { 466, "Orange", "TheNokarSellships", 9 },
                    { 467, "Yellow", "TheNokarSellships", 2 },
                    { 468, "Green", "TheNokarSellships", 5 },
                    { 469, "Blue", "TheNokarSellships", 8 },
                    { 470, "Purple", "TheNokarSellships", 0 },
                    { 471, "Pink", "TheNokarSellships", 0 },
                    { 472, "Black", "TheNokarSellships", 0 }
                });

            migrationBuilder.InsertData(
                schema: "Faction",
                table: "Factions",
                columns: new[] { "FactionName", "Action", "Commodities", "ComplexityRating", "GameVersion", "History", "HomeSystem", "Id", "Quote", "SystemInfo", "SystemStats" },
                values: new object[,]
                {
                    { "RohDhnaMechatronics", "RohDhnaMechatronics_Action", 4, "Low", "DiscordantStars", "RohDhnaMechatronics_History", "Tile1026", 51, "RohDhnaMechatronics_Quote", "RohDhnaMechatronics_SystemInfo", "RohDhnaMechatronics_SystemStats" },
                    { "SardakkNorr", "SardakkNorr_Action", 3, "Medium", "BaseGame", "SardakkNorr_History", "Tile13", 12, "SardakkNorr_Quote", "SardakkNorr_SystemInfo", "SardakkNorr_SystemStats" },
                    { "TheArborec", "TheArborec_Action", 3, "High", "BaseGame", "TheArborec_History", "Tile05", 1, "TheArborec_Quote", "TheArborec_SystemInfo", "TheArborec_SystemStats" },
                    { "TheArgentFlight", "TheArgentFlight_Action", 3, "Low", "ProphecyOfKings", "TheArgentFlight_History", "Tile58", 18, "TheArgentFlight_Quote", "TheArgentFlight_SystemInfo", "TheArgentFlight_SystemStats" },
                    { "TheAugursOfIlyxum", "TheAugursOfIlyxum_Action", 3, "High", "DiscordantStars", "TheAugursOfIlyxum_History", "Tile1001", 26, "TheAugursOfIlyxum_Quote", "TheAugursOfIlyxum_SystemInfo", "TheAugursOfIlyxum_SystemStats" },
                    { "TheBaronyOfLetnev", "TheBaronyOfLetnev_Action", 2, "Low", "BaseGame", "TheBaronyOfLetnev_History", "Tile10", 2, "TheBaronyOfLetnev_Quote", "TheBaronyOfLetnev_SystemInfo", "TheBaronyOfLetnev_SystemStats" },
                    { "TheBentorConglomerate", "TheBentorConglomerate_Action", 2, "Low", "DiscordantStars", "TheBentorConglomerate_History", "Tile1002", 27, "TheBentorConglomerate_Quote", "TheBentorConglomerate_SystemInfo", "TheBentorConglomerate_SystemStats" },
                    { "TheBerserkersOfKjalengard", "TheBerserkersOfKjalengard_Action", 3, "High", "DiscordantStars", "TheBerserkersOfKjalengard_History", "Tile1003", 28, "TheBerserkersOfKjalengard_Quote", "TheBerserkersOfKjalengard_SystemInfo", "TheBerserkersOfKjalengard_SystemStats" },
                    { "TheCeldauriTradeConfederation", "TheCeldauriTradeConfederation_Action", 4, "Medium", "DiscordantStars", "TheCeldauriTradeConfederation_History", "Tile1004", 29, "TheCeldauriTradeConfederation_Quote", "TheCeldauriTradeConfederation_SystemInfo", "TheCeldauriTradeConfederation_SystemStats" },
                    { "TheClanOfSaar", "TheClanOfSaar_Action", 3, "Medium", "BaseGame", "TheClanOfSaar_History", "Tile11", 3, "TheClanOfSaar_Quote", "TheClanOfSaar_SystemInfo", "TheClanOfSaar_SystemStats" },
                    { "TheCouncilKeleres", "TheCouncilKeleres_Action", 2, "Medium", "CodexVigil", "TheCouncilKeleres_History", "Tile92", 25, "TheCouncilKeleres_Quote", "TheCouncilKeleres_SystemInfo", "TheCouncilKeleres_SystemStats" },
                    { "TheDihMohnFlotilla", "TheDihMohnFlotilla_Action", 2, "Medium", "DiscordantStars", "TheDihMohnFlotilla_History", "Tile1006", 31, "TheDihMohnFlotilla_Quote", "TheDihMohnFlotilla_SystemInfo", "TheDihMohnFlotilla_SystemStats" },
                    { "TheEdynMandate", "TheEdynMandate_Action", 3, "Medium", "DiscordantStars", "TheEdynMandate_History", "Tile1007", 32, "TheEdynMandate_Quote", "TheEdynMandate_SystemInfo", "TheEdynMandate_SystemStats" },
                    { "TheEmbersOfMuaat", "TheEmbersOfMuaat_Action", 4, "High", "BaseGame", "TheEmbersOfMuaat_History", "Tile04", 4, "TheEmbersOfMuaat_Quote", "TheEmbersOfMuaat_SystemInfo", "TheEmbersOfMuaat_SystemStats" },
                    { "TheEmiratesOfHacan", "TheEmiratesOfHacan_Action", 6, "Low", "BaseGame", "TheEmiratesOfHacan_History", "Tile16", 5, "TheEmiratesOfHacan_Quote", "TheEmiratesOfHacan_SystemInfo", "TheEmiratesOfHacan_SystemStats" },
                    { "TheEmpyrean", "TheEmpyrean_Action", 4, "Low", "ProphecyOfKings", "TheEmpyrean_History", "Tile56", 19, "TheEmpyrean_Quote", "TheEmpyrean_SystemInfo", "TheEmpyrean_SystemStats" },
                    { "TheFederationOfSol", "TheFederationOfSol_Action", 4, "Low", "BaseGame", "TheFederationOfSol_History", "Tile01", 6, "TheFederationOfSol_Quote", "TheFederationOfSol_SystemInfo", "TheFederationOfSol_SystemStats" },
                    { "TheFlorzenProfiteers", "TheFlorzenProfiteers_Action", 4, "Medium", "DiscordantStars", "TheFlorzenProfiteers_History", "Tile1008", 33, "TheFlorzenProfiteers_Quote", "TheFlorzenProfiteers_SystemInfo", "TheFlorzenProfiteers_SystemStats" },
                    { "TheFreeSystemsCompact", "TheFreeSystemsCompact_Action", 4, "Medium", "DiscordantStars", "TheFreeSystemsCompact_History", "Tile1009", 34, "TheFreeSystemsCompact_Quote", "TheFreeSystemsCompact_SystemInfo", "TheFreeSystemsCompact_SystemStats" },
                    { "TheGheminaRaiders", "TheGheminaRaiders_Action", 2, "Low", "DiscordantStars", "TheGheminaRaiders_History", "Tile1010", 35, "TheGheminaRaiders_Quote", "TheGheminaRaiders_SystemInfo", "TheGheminaRaiders_SystemStats" },
                    { "TheGhostsOfCreuss", "TheGhostsOfCreuss_Action", 4, "Medium", "BaseGame", "TheGhostsOfCreuss_History", "Tile17", 7, "TheGhostsOfCreuss_Quote", "TheGhostsOfCreuss_SystemInfo", "TheGhostsOfCreuss_SystemStats" },
                    { "TheGhotiWayfarers", "TheGhotiWayfarers_Action", 4, "Medium", "DiscordantStars", "TheGhotiWayfarers_History", "Tile1011", 36, "TheGhotiWayfarers_Quote", "TheGhotiWayfarers_SystemInfo", "TheGhotiWayfarers_SystemStats" },
                    { "TheGledgeUnion", "TheGledgeUnion_Action", 2, "Medium", "DiscordantStars", "TheGledgeUnion_History", "Tile1012", 37, "TheGledgeUnion_Quote", "TheGledgeUnion_SystemInfo", "TheGledgeUnion_SystemStats" },
                    { "TheGlimmerOfMortheus", "TheGlimmerOfMortheus_Action", 3, "High", "DiscordantStars", "TheGlimmerOfMortheus_History", "Tile1013", 38, "TheGlimmerOfMortheus_Quote", "TheGlimmerOfMortheus_SystemInfo", "TheGlimmerOfMortheus_SystemStats" },
                    { "TheCheiranHordes", "TheCheiranHordes_Action", 2, "Low", "DiscordantStars", "TheCheiranHordes_History", "Tile1005", 30, "TheCheiranHordes_Quote", "TheCheiranHordes_SystemInfo", "TheCheiranHordes_SystemStats" },
                    { "TheKolleccSociety", "TheKolleccSociety_Action", 3, "High", "DiscordantStars", "TheKolleccSociety_History", "Tile1014", 39, "TheKolleccSociety_Quote", "TheKolleccSociety_SystemInfo", "TheKolleccSociety_SystemStats" },
                    { "TheKortaliTribunal", "TheKortaliTribunal_Action", 3, "Low", "DiscordantStars", "TheKortaliTribunal_History", "Tile1015", 40, "TheKortaliTribunal_Quote", "TheKortaliTribunal_SystemInfo", "TheKortaliTribunal_SystemStats" },
                    { "TheKyroSodality", "TheKyroSodality_Action", 2, "Low", "DiscordantStars", "TheKyroSodality_History", "Tile1016", 41, "TheKyroSodality_Quote", "TheKyroSodality_SystemInfo", "TheKyroSodality_SystemStats" },
                    { "TheL1z1xMindnet", "TheL1z1xMindnet_Action", 2, "Low", "BaseGame", "TheL1z1xMindnet_History", "Tile06", 8, "TheL1z1xMindnet_Quote", "TheL1z1xMindnet_SystemInfo", "TheL1z1xMindnet_SystemStats" },
                    { "TheLanefirRemnants", "TheLanefirRemnants_Action", 2, "Low", "DiscordantStars", "TheLanefirRemnants_History", "Tile1017", 42, "TheLanefirRemnants_Quote", "TheLanefirRemnants_SystemInfo", "TheLanefirRemnants_SystemStats" },
                    { "TheLiZhoDynasty", "TheLiZhoDynasty_Action", 3, "High", "DiscordantStars", "TheLiZhoDynasty_History", "Tile1018", 43, "TheLiZhoDynasty_Quote", "TheLiZhoDynasty_SystemInfo", "TheLiZhoDynasty_SystemStats" },
                    { "TheLTokkKhrask", "TheLTokkKhrask_Action", 2, "Medium", "DiscordantStars", "TheLTokkKhrask_History", "Tile1019", 44, "TheLTokkKhrask_Quote", "TheLTokkKhrask_SystemInfo", "TheLTokkKhrask_SystemStats" },
                    { "TheMahactGeneSorcerers", "TheMahactGeneSorcerers_Action", 3, "High", "ProphecyOfKings", "TheMahactGeneSorcerers_History", "Tile52", 20, "TheMahactGeneSorcerers_Quote", "TheMahactGeneSorcerers_SystemInfo", "TheMahactGeneSorcerers_SystemStats" },
                    { "TheMentakCoalition", "TheMentakCoalition_Action", 2, "High", "BaseGame", "TheMentakCoalition_History", "Tile02", 9, "TheMentakCoalition_Quote", "TheMentakCoalition_SystemInfo", "TheMentakCoalition_SystemStats" },
                    { "TheMirvedaProtectorate", "TheMirvedaProtectorate_Action", 3, "Medium", "DiscordantStars", "TheMirvedaProtectorate_History", "Tile1020", 45, "TheMirvedaProtectorate_Quote", "TheMirvedaProtectorate_SystemInfo", "TheMirvedaProtectorate_SystemStats" },
                    { "TheMonksOfKolume", "TheMonksOfKolume_Action", 3, "High", "DiscordantStars", "TheMonksOfKolume_History", "Tile1021", 46, "TheMonksOfKolume_Quote", "TheMonksOfKolume_SystemInfo", "TheMonksOfKolume_SystemStats" },
                    { "TheMykoMentori", "TheMykoMentori_Action", 1, "Low", "DiscordantStars", "TheMykoMentori_History", "Tile1022", 47, "TheMykoMentori_Quote", "TheMykoMentori_SystemInfo", "TheMykoMentori_SystemStats" },
                    { "TheNaaluCollective", "TheNaaluCollective_Action", 3, "Medium", "BaseGame", "TheNaaluCollective_History", "Tile09", 10, "TheNaaluCollective_Quote", "TheNaaluCollective_SystemInfo", "TheNaaluCollective_SystemStats" },
                    { "TheNaazRokhaAlliance", "TheNaazRokhaAlliance_Action", 3, "Low", "ProphecyOfKings", "TheNaazRokhaAlliance_History", "Tile57", 21, "TheNaazRokhaAlliance_Quote", "TheNaazRokhaAlliance_SystemInfo", "TheNaazRokhaAlliance_SystemStats" },
                    { "TheNekroVirus", "TheNekroVirus_Action", 3, "High", "BaseGame", "TheNekroVirus_History", "Tile08", 11, "TheNekroVirus_Quote", "TheNekroVirus_SystemInfo", "TheNekroVirus_SystemStats" },
                    { "TheNivynStarKings", "TheNivynStarKings_Action", 3, "Medium", "DiscordantStars", "TheNivynStarKings_History", "Tile1023", 48, "TheNivynStarKings_Quote", "TheNivynStarKings_SystemInfo", "TheNivynStarKings_SystemStats" },
                    { "TheNokarSellships", "TheNokarSellships_Action", 4, "Medium", "DiscordantStars", "TheNokarSellships_History", "Tile1024", 49, "TheNokarSellships_Quote", "TheNokarSellships_SystemInfo", "TheNokarSellships_SystemStats" },
                    { "TheNomad", "TheNomad_Action", 4, "Low", "ProphecyOfKings", "TheNomad_History", "Tile53", 22, "TheNomad_Quote", "TheNomad_SystemInfo", "TheNomad_SystemStats" },
                    { "TheOlradinLeague", "TheOlradinLeague_Action", 3, "High", "DiscordantStars", "TheOlradinLeague_History", "Tile1025", 50, "TheOlradinLeague_Quote", "TheOlradinLeague_SystemInfo", "TheOlradinLeague_SystemStats" },
                    { "TheSavagesOfCymiae", "TheSavagesOfCymiae_Action", 3, "Medium", "DiscordantStars", "TheSavagesOfCymiae_History", "Tile1027", 52, "TheSavagesOfCymiae_Quote", "TheSavagesOfCymiae_SystemInfo", "TheSavagesOfCymiae_SystemStats" },
                    { "TheShipwrightsofAxis", "TheShipwrightsofAxis_Action", 5, "Medium", "DiscordantStars", "TheShipwrightsofAxis_History", "Tile1028", 53, "TheShipwrightsofAxis_Quote", "TheShipwrightsofAxis_SystemInfo", "TheShipwrightsofAxis_SystemStats" },
                    { "TheTitansOfUl", "TheTitansOfUl_Action", 2, "Medium", "ProphecyOfKings", "TheTitansOfUl_History", "Tile55", 23, "TheTitansOfUl_Quote", "TheTitansOfUl_SystemInfo", "TheTitansOfUl_SystemStats" },
                    { "TheTnelisSyndicate", "TheTnelisSyndicate_Action", 2, "Medium", "DiscordantStars", "TheTnelisSyndicate_History", "Tile1029", 54, "TheTnelisSyndicate_Quote", "TheTnelisSyndicate_SystemInfo", "TheTnelisSyndicate_SystemStats" },
                    { "TheUniversitiesOfJolNar", "TheUniversitiesOfJolNar_Action", 4, "Low", "BaseGame", "TheUniversitiesOfJolNar_History", "Tile12", 13, "TheUniversitiesOfJolNar_Quote", "TheUniversitiesOfJolNar_SystemInfo", "TheUniversitiesOfJolNar_SystemStats" },
                    { "TheVadenBankingClans", "TheVadenBankingClans_Action", 3, "Low", "DiscordantStars", "TheVadenBankingClans_History", "Tile1030", 55, "TheVadenBankingClans_Quote", "TheVadenBankingClans_SystemInfo", "TheVadenBankingClans_SystemStats" },
                    { "TheVaylerianScourge", "TheVaylerianScourge_Action", 2, "Low", "DiscordantStars", "TheVaylerianScourge_History", "Tile1031", 56, "TheVaylerianScourge_Quote", "TheVaylerianScourge_SystemInfo", "TheVaylerianScourge_SystemStats" },
                    { "TheVeldyrSovereignty", "TheVeldyrSovereignty_Action", 4, "Low", "DiscordantStars", "TheVeldyrSovereignty_History", "Tile1032", 57, "TheVeldyrSovereignty_Quote", "TheVeldyrSovereignty_SystemInfo", "TheVeldyrSovereignty_SystemStats" },
                    { "TheVuilRaithCabal", "TheVuilRaithCabal_Action", 2, "High", "ProphecyOfKings", "TheVuilRaithCabal_History", "Tile54", 24, "TheVuilRaithCabal_Quote", "TheVuilRaithCabal_SystemInfo", "TheVuilRaithCabal_SystemStats" },
                    { "TheWinnu", "TheWinnu_Action", 3, "Medium", "BaseGame", "TheWinnu_History", "Tile07", 14, "TheWinnu_Quote", "TheWinnu_SystemInfo", "TheWinnu_SystemStats" },
                    { "TheXxchaKingdom", "TheXxchaKingdom_Action", 4, "Low", "BaseGame", "TheXxchaKingdom_History", "Tile14", 15, "TheXxchaKingdom_Quote", "TheXxchaKingdom_SystemInfo", "TheXxchaKingdom_SystemStats" },
                    { "TheYinBrotherhood", "TheYinBrotherhood_Action", 2, "Low", "BaseGame", "TheYinBrotherhood_History", "Tile03", 16, "TheYinBrotherhood_Quote", "TheYinBrotherhood_SystemInfo", "TheYinBrotherhood_SystemStats" },
                    { "TheYssarilTribes", "TheYssarilTribes_Action", 3, "Low", "BaseGame", "TheYssarilTribes_History", "Tile15", 17, "TheYssarilTribes_Quote", "TheYssarilTribes_SystemInfo", "TheYssarilTribes_SystemStats" },
                    { "TheZealotsOfRhodun", "TheZealotsOfRhodun_Action", 3, "High", "DiscordantStars", "TheZealotsOfRhodun_History", "Tile1033", 58, "TheZealotsOfRhodun_Quote", "TheZealotsOfRhodun_SystemInfo", "TheZealotsOfRhodun_SystemStats" },
                    { "TheZelianPurifier", "TheZelianPurifier_Action", 2, "Medium", "DiscordantStars", "TheZelianPurifier_History", "Tile1034", 59, "TheZelianPurifier_Quote", "TheZelianPurifier_SystemInfo", "TheZelianPurifier_SystemStats" }
                });

            migrationBuilder.InsertData(
                schema: "Rule",
                table: "Faq",
                columns: new[] { "Id", "AnswerCzech", "AnswerEnglish", "ComponentName", "FaqStatus", "QuestionCzech", "QuestionEnglish" },
                values: new object[] { 1, "Answer in czech", "Answer in english", "TheArborec", 0, "Question in czech", "Question in english" });

            migrationBuilder.InsertData(
                schema: "Card",
                table: "FrontierCards",
                columns: new[] { "Id", "EnumName", "GameVersion", "Text", "Type" },
                values: new object[,]
                {
                    { 1, "DeadWorld", "CodexVigil", "", "Frontier" },
                    { 2, "DerelictVessel", "ProphecyOfKings", "", "Frontier" },
                    { 3, "EnigmaticDevice", "ProphecyOfKings", "", "Frontier" },
                    { 4, "EntropicField", "CodexVigil", "", "Frontier" },
                    { 5, "GammaRelay", "ProphecyOfKings", "", "Frontier" },
                    { 6, "IonStorm", "ProphecyOfKings", "", "Frontier" },
                    { 7, "KeleresShip", "CodexVigil", "", "Frontier" },
                    { 8, "LostCrew", "ProphecyOfKings", "", "Frontier" },
                    { 9, "MajorEntropicField", "CodexVigil", "", "Frontier" },
                    { 10, "MerchantStation", "ProphecyOfKings", "", "Frontier" },
                    { 11, "MinorEntropicField", "CodexVigil", "", "Frontier" },
                    { 12, "Mirage", "ProphecyOfKings", "", "Frontier" },
                    { 13, "UnknownRelicFragment", "ProphecyOfKings", "", "Frontier" },
                    { 14, "DarkVisions", "UnchartedSpace", "", "Frontier" },
                    { 15, "FoldedSpace", "UnchartedSpace", "", "Frontier" },
                    { 16, "StarChart", "UnchartedSpace", "", "Frontier" },
                    { 17, "SuspiciousWreckage", "UnchartedSpace", "", "Frontier" }
                });

            migrationBuilder.InsertData(
                schema: "Card",
                table: "ObjectiveCards",
                columns: new[] { "Id", "EnumName", "GameVersion", "ObjectiveCardType", "Text", "TimingWindow", "Type" },
                values: new object[,]
                {
                    { 1, "AMassWealth", "ProphecyOfKings", "StageOne", "", "StatusPhase", "Objective" },
                    { 2, "BuildDefenses", "ProphecyOfKings", "StageOne", "", "StatusPhase", "Objective" },
                    { 3, "CornerTheMarket", "BaseGame", "StageOne", "", "StatusPhase", "Objective" },
                    { 4, "DevelopWeaponry", "BaseGame", "StageOne", "", "StatusPhase", "Objective" },
                    { 5, "DiscoverLostOutposts", "ProphecyOfKings", "StageOne", "", "StatusPhase", "Objective" },
                    { 6, "DiversifyResearch", "BaseGame", "StageOne", "", "StatusPhase", "Objective" },
                    { 7, "EngineerAMarvel", "ProphecyOfKings", "StageOne", "", "StatusPhase", "Objective" },
                    { 8, "ErectAMonument", "BaseGame", "StageOne", "", "StatusPhase", "Objective" },
                    { 9, "ExpandBorders", "BaseGame", "StageOne", "", "StatusPhase", "Objective" },
                    { 10, "ExploreDeepSpace", "ProphecyOfKings", "StageOne", "", "StatusPhase", "Objective" },
                    { 11, "FoundResearchOutposts", "BaseGame", "StageOne", "", "StatusPhase", "Objective" },
                    { 12, "ImproveInfrastructure", "ProphecyOfKings", "StageOne", "", "StatusPhase", "Objective" },
                    { 13, "IntimidateCouncil", "BaseGame", "StageOne", "", "StatusPhase", "Objective" },
                    { 14, "LeadFromTheFront", "BaseGame", "StageOne", "", "StatusPhase", "Objective" },
                    { 15, "MakeHistory", "ProphecyOfKings", "StageOne", "", "StatusPhase", "Objective" },
                    { 16, "NegotiateTradeRoutes", "BaseGame", "StageOne", "", "StatusPhase", "Objective" },
                    { 17, "PopulateTheOuterRim", "ProphecyOfKings", "StageOne", "", "StatusPhase", "Objective" },
                    { 18, "PushBoundaries", "ProphecyOfKings", "StageOne", "", "StatusPhase", "Objective" },
                    { 19, "RaiseAFleet", "ProphecyOfKings", "StageOne", "", "StatusPhase", "Objective" },
                    { 20, "SwayTheCouncil", "BaseGame", "StageOne", "", "StatusPhase", "Objective" },
                    { 21, "AchieveSupremacy", "ProphecyOfKings", "StageTwo", "", "StatusPhase", "Objective" },
                    { 22, "BecomeALegend", "ProphecyOfKings", "StageTwo", "", "StatusPhase", "Objective" },
                    { 23, "CentralizeGalacticTrade", "BaseGame", "StageTwo", "", "StatusPhase", "Objective" },
                    { 24, "CommandAnArmada", "ProphecyOfKings", "StageTwo", "", "StatusPhase", "Objective" },
                    { 25, "ConquerTheWeak", "BaseGame", "StageTwo", "", "StatusPhase", "Objective" },
                    { 26, "ConstructMassiveCities", "ProphecyOfKings", "StageTwo", "", "StatusPhase", "Objective" },
                    { 27, "ControlTheBorderlands", "ProphecyOfKings", "StageTwo", "", "StatusPhase", "Objective" },
                    { 28, "FormGalacticBrainTrust", "BaseGame", "StageTwo", "", "StatusPhase", "Objective" },
                    { 29, "FoundAGoldenAge", "BaseGame", "StageTwo", "", "StatusPhase", "Objective" },
                    { 30, "GalvanizeThePeople", "BaseGame", "StageTwo", "", "StatusPhase", "Objective" },
                    { 31, "HoldVastReserves", "ProphecyOfKings", "StageTwo", "", "StatusPhase", "Objective" },
                    { 32, "ManipulateGalacticLaw", "BaseGame", "StageTwo", "", "StatusPhase", "Objective" },
                    { 33, "MasterTheSciences", "BaseGame", "StageTwo", "", "StatusPhase", "Objective" },
                    { 34, "PatrolVastTerritories", "ProphecyOfKings", "StageTwo", "", "StatusPhase", "Objective" },
                    { 35, "ProtectTheBorder", "ProphecyOfKings", "StageTwo", "", "StatusPhase", "Objective" },
                    { 36, "ReclaimAncientMonuments", "ProphecyOfKings", "StageTwo", "", "StatusPhase", "Objective" },
                    { 37, "RevolutionizeWarfare", "BaseGame", "StageTwo", "", "StatusPhase", "Objective" },
                    { 38, "RuleDistantLands", "ProphecyOfKings", "StageTwo", "", "StatusPhase", "Objective" },
                    { 39, "SubdueTheGalaxy", "BaseGame", "StageTwo", "", "StatusPhase", "Objective" },
                    { 40, "UnifyTheColonies", "BaseGame", "StageTwo", "", "StatusPhase", "Objective" },
                    { 41, "AdaptNewStrategies", "BaseGame", "Secret", "", "StatusPhase", "Objective" },
                    { 42, "BecomeAMartyr", "ProphecyOfKings", "Secret", "", "Action", "Objective" },
                    { 43, "BecomeTheGatekeeper", "BaseGame", "Secret", "", "StatusPhase", "Objective" },
                    { 44, "BetrayAFriend", "ProphecyOfKings", "Secret", "", "Action", "Objective" },
                    { 45, "BraveTheVoid", "ProphecyOfKings", "Secret", "", "Action", "Objective" },
                    { 46, "ControlTheRegion", "BaseGame", "Secret", "", "StatusPhase", "Objective" },
                    { 47, "CutSupplyLines", "BaseGame", "Secret", "", "StatusPhase", "Objective" },
                    { 48, "DarkenTheSkies", "ProphecyOfKings", "Secret", "", "Action", "Objective" },
                    { 49, "DefySpaceAndTime", "ProphecyOfKings", "Secret", "", "StatusPhase", "Objective" },
                    { 50, "DemonstrateYourPower", "ProphecyOfKings", "Secret", "", "Action", "Objective" },
                    { 51, "DestroyHereticalWorks", "ProphecyOfKings", "Secret", "", "StatusPhase", "Objective" },
                    { 52, "DestroyTheirGreatestShip", "BaseGame", "Secret", "", "Action", "Objective" },
                    { 53, "DictatePolicy", "ProphecyOfKings", "Secret", "", "StrategyPhase", "Objective" },
                    { 54, "DriveTheDebate", "ProphecyOfKings", "Secret", "", "StrategyPhase", "Objective" },
                    { 55, "EstablishAPerimeter", "BaseGame", "Secret", "", "StatusPhase", "Objective" },
                    { 56, "EstablishHegemony", "ProphecyOfKings", "Secret", "", "StatusPhase", "Objective" },
                    { 57, "FightWithPrecision", "Deprecated", "Secret", "", "Action", "Objective" },
                    { 58, "FightWithPrecisionOmega", "CodexVigil", "Secret", "", "Action", "Objective" },
                    { 59, "ForgeAnAlliance", "BaseGame", "Secret", "", "StatusPhase", "Objective" },
                    { 60, "FormASpyNetwork", "BaseGame", "Secret", "", "StatusPhase", "Objective" },
                    { 61, "FosterCohesion", "ProphecyOfKings", "Secret", "", "StatusPhase", "Objective" },
                    { 62, "FuelTheWarMachine", "BaseGame", "Secret", "", "StatusPhase", "Objective" },
                    { 63, "GatherAMightyFleet", "BaseGame", "Secret", "", "StatusPhase", "Objective" },
                    { 64, "HoardRawMaterials", "ProphecyOfKings", "Secret", "", "StatusPhase", "Objective" },
                    { 65, "LearnTheSecretsOfTheCosmos", "BaseGame", "Secret", "", "StatusPhase", "Objective" },
                    { 66, "MakeAnExampleOfTheirWorld", "Deprecated", "Secret", "", "Action", "Objective" },
                    { 67, "MakeAnExampleOfTheirWorldOmega", "CodexVigil", "Secret", "", "Action", "Objective" },
                    { 68, "MasterTheLawsOfPhysics", "BaseGame", "Secret", "", "StatusPhase", "Objective" },
                    { 69, "MechanizeTheMilitary", "ProphecyOfKings", "Secret", "", "StatusPhase", "Objective" },
                    { 70, "MineRareMinerals", "BaseGame", "Secret", "", "StatusPhase", "Objective" },
                    { 71, "MonopolizeProduction", "BaseGame", "Secret", "", "StatusPhase", "Objective" },
                    { 72, "OccupyTheFringe", "ProphecyOfKings", "Secret", "", "StatusPhase", "Objective" },
                    { 73, "OccupyTheSeatOfTheEmpire", "BaseGame", "Secret", "", "StatusPhase", "Objective" },
                    { 74, "ProduceEnMasse", "ProphecyOfKings", "Secret", "", "StatusPhase", "Objective" },
                    { 75, "ProveEndurance", "ProphecyOfKings", "Secret", "", "Action", "Objective" },
                    { 76, "SeizeAnIcon", "ProphecyOfKings", "Secret", "", "StatusPhase", "Objective" },
                    { 77, "SparkARebellion", "BaseGame", "Secret", "", "Action", "Objective" },
                    { 78, "StakeYourClaim", "ProphecyOfKings", "Secret", "", "StatusPhase", "Objective" },
                    { 79, "StrengthenBonds", "ProphecyOfKings", "Secret", "", "StatusPhase", "Objective" },
                    { 80, "ThreatenEnemies", "BaseGame", "Secret", "", "StatusPhase", "Objective" },
                    { 81, "TurnTheirFleetsToDust", "Deprecated", "Secret", "", "Action", "Objective" },
                    { 82, "TurnTheirFleetsToDustOmega", "CodexVigil", "Secret", "", "Action", "Objective" },
                    { 83, "UnveilFlagship", "BaseGame", "Secret", "", "Action", "Objective" }
                });

            migrationBuilder.InsertData(
                schema: "Card",
                table: "PromissaryNoteCards",
                columns: new[] { "Id", "EnumName", "Faction", "GameVersion", "Text", "Type" },
                values: new object[,]
                {
                    { 1, "Ceasefire", "None", "BaseGame", "", "Promissary" },
                    { 2, "TradeAgreement", "None", "BaseGame", "", "Promissary" },
                    { 3, "SupportForTheThrone", "None", "BaseGame", "", "Promissary" },
                    { 4, "PoliticalSecret", "None", "BaseGame", "", "Promissary" },
                    { 5, "Alliance", "None", "ProphecyOfKings", "", "Promissary" },
                    { 6, "Stymie", "TheArborec", "Deprecated", "", "Promissary" },
                    { 7, "StymieOmega", "TheArborec", "CodexOrdinian", "", "Promissary" },
                    { 8, "WarFunding", "TheBaronyOfLetnev", "Deprecated", "", "Promissary" },
                    { 9, "WarFundingOmega", "TheBaronyOfLetnev", "CodexOrdinian", "", "Promissary" },
                    { 10, "RaghsCall", "TheClanOfSaar", "BaseGame", "", "Promissary" },
                    { 11, "FiresOfTheGashlai", "TheEmbersOfMuaat", "BaseGame", "", "Promissary" },
                    { 12, "TradeConvoys", "TheEmiratesOfHacan", "BaseGame", "", "Promissary" },
                    { 13, "MilitarySupport", "TheFederationOfSol", "BaseGame", "", "Promissary" },
                    { 14, "CreussIff", "TheGhostsOfCreuss", "BaseGame", "", "Promissary" },
                    { 15, "CyberneticEnhancements", "TheL1z1xMindnet", "Deprecated", "", "Promissary" },
                    { 16, "CyberneticEnhancementsOmega", "TheL1z1xMindnet", "CodexOrdinian", "", "Promissary" },
                    { 17, "PromiseOfProtection", "TheMentakCoalition", "BaseGame", "", "Promissary" },
                    { 18, "GiftOfPrescience", "TheNaaluCollective", "BaseGame", "", "Promissary" },
                    { 19, "Antivirus", "TheNekroVirus", "BaseGame", "", "Promissary" },
                    { 20, "TekklarLegion", "SardakkNorr", "BaseGame", "", "Promissary" },
                    { 21, "ResearchAgreement", "TheUniversitiesOfJolNar", "BaseGame", "", "Promissary" },
                    { 22, "Acquiescence", "TheWinnu", "Deprecated", "", "Promissary" },
                    { 23, "AcquiescenceOmega", "TheWinnu", "CodexOrdinian", "", "Promissary" },
                    { 24, "PoliticalFavor", "TheXxchaKingdom", "BaseGame", "", "Promissary" },
                    { 25, "GreyfireMutagen", "TheYinBrotherhood", "Deprecated", "", "Promissary" },
                    { 26, "GreyfireMutagenOmega", "TheYinBrotherhood", "CodexOrdinian", "", "Promissary" },
                    { 27, "SpyNet", "TheYssarilTribes", "BaseGame", "", "Promissary" },
                    { 28, "StrikeWingAmbuscade", "TheArgentFlight", "ProphecyOfKings", "", "Promissary" },
                    { 29, "DarkPact", "TheEmpyrean", "ProphecyOfKings", "", "Promissary" },
                    { 30, "BloodPact", "TheEmpyrean", "ProphecyOfKings", "", "Promissary" },
                    { 31, "ScepterOfDominion", "TheMahactGeneSorcerers", "ProphecyOfKings", "", "Promissary" },
                    { 32, "BlackMarketForgery", "TheNaazRokhaAlliance", "ProphecyOfKings", "", "Promissary" },
                    { 33, "TheCavalry", "TheNomad", "ProphecyOfKings", "", "Promissary" },
                    { 34, "Terraform", "TheTitansOfUl", "ProphecyOfKings", "", "Promissary" },
                    { 35, "Crucible", "TheVuilRaithCabal", "ProphecyOfKings", "", "Promissary" },
                    { 36, "KeleresRider", "TheCouncilKeleres", "CodexVigil", "", "Promissary" },
                    { 37, "AiSurvey", "TheKolleccSociety", "DiscordantStars", "", "Promissary" },
                    { 38, "AlgorithmicReplication", "TheSavagesOfCymiae", "DiscordantStars", "", "Promissary" },
                    { 39, "Automatons", "RohDhnaMechatronics", "DiscordantStars", "", "Promissary" },
                    { 40, "BlessingOfTheQueens", "TheKortaliTribunal", "DiscordantStars", "", "Promissary" },
                    { 41, "BroadcastTeams", "TheFreeSystemsCompact", "DiscordantStars", "", "Promissary" },
                    { 42, "Carcinisation", "TheCheiranHordes", "DiscordantStars", "", "Promissary" },
                    { 43, "ClansFavor", "TheVaylerianScourge", "DiscordantStars", "", "Promissary" },
                    { 44, "CombatDrills", "TheDihMohnFlotilla", "DiscordantStars", "", "Promissary" },
                    { 45, "CombinatorialBypass", "TheMonksOfKolume", "DiscordantStars", "", "Promissary" },
                    { 46, "EdynRider", "TheEdynMandate", "DiscordantStars", "", "Promissary" },
                    { 47, "EncryptionKey", "TheBentorConglomerate", "DiscordantStars", "", "Promissary" },
                    { 48, "FavorOfRhodun", "TheZealotsOfRhodun", "DiscordantStars", "", "Promissary" },
                    { 49, "GhotiRelay", "TheGhotiWayfarers", "DiscordantStars", "", "Promissary" },
                    { 50, "GiftOfInsight", "TheMykoMentori", "DiscordantStars", "", "Promissary" },
                    { 51, "GledgeBase", "TheGledgeUnion", "DiscordantStars", "", "Promissary" },
                    { 52, "InciteRevolution", "TheOlradinLeague", "DiscordantStars", "", "Promissary" },
                    { 53, "IndustrySecrets", "TheShipwrightsofAxis", "DiscordantStars", "", "Promissary" },
                    { 54, "KyroRider", "TheKyroSodality", "DiscordantStars", "", "Promissary" },
                    { 55, "NivynGuidance", "TheNivynStarKings", "DiscordantStars", "", "Promissary" },
                    { 56, "NokarNavigator", "TheNokarSellships", "DiscordantStars", "", "Promissary" },
                    { 57, "PlotsWithinPlots", "TheTnelisSyndicate", "DiscordantStars", "", "Promissary" },
                    { 58, "RaidLeaders", "TheGheminaRaiders", "DiscordantStars", "", "Promissary" },
                    { 59, "RapidExcavation", "TheMirvedaProtectorate", "DiscordantStars", "", "Promissary" },
                    { 60, "ReadTheFates", "TheAugursOfIlyxum", "DiscordantStars", "", "Promissary" },
                    { 61, "SecretsOfTheWeave", "TheGlimmerOfMortheus", "DiscordantStars", "", "Promissary" },
                    { 62, "SpilsOfWar", "TheLanefirRemnants", "DiscordantStars", "", "Promissary" },
                    { 63, "StoneSpeakers", "TheLTokkKhrask", "DiscordantStars", "", "Promissary" },
                    { 64, "TradeAlliance", "TheCeldauriTradeConfederation", "DiscordantStars", "", "Promissary" },
                    { 65, "TrustedCounselor", "TheLiZhoDynasty", "DiscordantStars", "", "Promissary" },
                    { 66, "UndergroundMarket", "TheFlorzenProfiteers", "DiscordantStars", "", "Promissary" },
                    { 67, "VadenHandshake", "TheVadenBankingClans", "DiscordantStars", "", "Promissary" },
                    { 68, "Vassalage", "TheBerserkersOfKjalengard", "DiscordantStars", "", "Promissary" },
                    { 69, "BranchOfficeTaxHaven", "TheVeldyrSovereignty", "DiscordantStars", "", "Promissary" },
                    { 70, "BranchOfficeBroadcastHub", "TheVeldyrSovereignty", "DiscordantStars", "", "Promissary" },
                    { 71, "BranchOfficeReserveBank", "TheVeldyrSovereignty", "DiscordantStars", "", "Promissary" },
                    { 72, "BranchOfficeOrbitalShipyard", "TheVeldyrSovereignty", "DiscordantStars", "", "Promissary" },
                    { 73, "HyperkineticOrdinance", "TheZelianPurifier", "DiscordantStars", "", "Promissary" }
                });

            migrationBuilder.InsertData(
                schema: "Card",
                table: "RelicCards",
                columns: new[] { "Id", "EnumName", "GameVersion", "Text", "Type" },
                values: new object[,]
                {
                    { 1, "DominusOrb", "ProphecyOfKings", "", "Relic" },
                    { 2, "MawOfWorlds", "ProphecyOfKings", "", "Relic" },
                    { 3, "ScepterOfEmelpar", "ProphecyOfKings", "", "Relic" },
                    { 4, "ShardOfTheThrone", "ProphecyOfKings", "", "Relic" },
                    { 5, "StellarConverter", "ProphecyOfKings", "", "Relic" },
                    { 6, "TheCodex", "ProphecyOfKings", "", "Relic" },
                    { 7, "TheCrownOfEmphidia", "ProphecyOfKings", "", "Relic" },
                    { 8, "TheCrownOfThalnos", "ProphecyOfKings", "", "Relic" },
                    { 9, "TheObsidian", "ProphecyOfKings", "", "Relic" },
                    { 10, "TheProphetsTears", "ProphecyOfKings", "", "Relic" },
                    { 11, "DynamisCore", "CodexAffinity", "", "Relic" },
                    { 12, "JRXS455OLostTitanPrototypeAgent", "CodexAffinity", "", "Relic" },
                    { 13, "NanoForge", "CodexAffinity", "", "Relic" },
                    { 14, "StarfallArray", "UnchartedSpace", "", "Relic" },
                    { 15, "AccretionEngine", "UnchartedSpace", "", "Relic" },
                    { 16, "EyeOfVogul", "UnchartedSpace", "", "Relic" },
                    { 17, "AzdelsKey", "UnchartedSpace", "", "Relic" },
                    { 18, "TwilightMirror", "UnchartedSpace", "", "Relic" },
                    { 19, "ESixGoNetwork", "UnchartedSpace", "", "Relic" },
                    { 20, "FotgottenThrone", "UnchartedSpace", "", "Relic" }
                });

            migrationBuilder.InsertData(
                schema: "Rule",
                table: "Rules",
                columns: new[] { "RuleCategory", "Content" },
                values: new object[,]
                {
                    { "Abilities", "RuleCategory_Abilities" },
                    { "ActionCards", "RuleCategory_ActionCards" },
                    { "ActionPhase", "RuleCategory_ActionPhase" },
                    { "ActivePlayer", "RuleCategory_ActivePlayer" },
                    { "ActiveSystem", "RuleCategory_ActiveSystem" },
                    { "Adjacency", "RuleCategory_Adjacency" },
                    { "AgendaCard", "RuleCategory_AgendaCard" },
                    { "AgendaPhase", "RuleCategory_AgendaPhase" },
                    { "Anomalies", "RuleCategory_Anomalies" },
                    { "AntiFighterBarrage", "RuleCategory_AntiFighterBarrage" },
                    { "AsteroidField", "RuleCategory_AsteroidField" },
                    { "Attacker", "RuleCategory_Attacker" },
                    { "Attach", "RuleCategory_Attach" },
                    { "Blockaded", "RuleCategory_Blockaded" },
                    { "Bombardment", "RuleCategory_Bombardment" },
                    { "Capacity", "RuleCategory_Capacity" },
                    { "Capture", "RuleCategory_Capture" },
                    { "Combat", "RuleCategory_Combat" },
                    { "CommandSheet", "RuleCategory_CommandSheet" },
                    { "CommandTokens", "RuleCategory_CommandTokens" },
                    { "Commodities", "RuleCategory_Commodities" },
                    { "ComponentAction", "RuleCategory_ComponentAction" },
                    { "ComponentLimitations", "RuleCategory_ComponentLimitations" },
                    { "Construction", "RuleCategory_Construction" },
                    { "Control", "RuleCategory_Control" },
                    { "Cost", "RuleCategory_Cost" },
                    { "CustodiansToken", "RuleCategory_CustodiansToken" },
                    { "Deals", "RuleCategory_Deals" },
                    { "Defender", "RuleCategory_Defender" },
                    { "Deploy", "RuleCategory_Deploy" },
                    { "Destroyed", "RuleCategory_Destroyed" },
                    { "Diplomacy", "RuleCategory_Diplomacy" },
                    { "Elimination", "RuleCategory_Elimination" },
                    { "Exhausted", "RuleCategory_Exhausted" },
                    { "Exploration", "RuleCategory_Exploration" },
                    { "FighterTokens", "RuleCategory_FighterTokens" },
                    { "FleetPool", "RuleCategory_FleetPool" },
                    { "FrontierTokens", "RuleCategory_FrontierTokens" },
                    { "GameBoard", "RuleCategory_GameBoard" },
                    { "GameRound", "RuleCategory_GameRound" },
                    { "GravityRift", "RuleCategory_GravityRift" },
                    { "GroundCombat", "RuleCategory_GroundCombat" },
                    { "GroundForces", "RuleCategory_GroundForces" },
                    { "Hyperlanes", "RuleCategory_Hyperlanes" },
                    { "Imperial", "RuleCategory_Imperial" },
                    { "InfantryTokens", "RuleCategory_InfantryTokens" },
                    { "Influence", "RuleCategory_Influence" },
                    { "InitiativeOrder", "RuleCategory_InitiativeOrder" },
                    { "Invasion", "RuleCategory_Invasion" },
                    { "Leaders", "RuleCategory_Leaders" },
                    { "LeaderSheet", "RuleCategory_LeaderSheet" },
                    { "Leadership", "RuleCategory_Leadership" },
                    { "LegendaryPlanets", "RuleCategory_LegendaryPlanets" },
                    { "MecatolRex", "RuleCategory_MecatolRex" },
                    { "Mechs", "RuleCategory_Mechs" },
                    { "Modifiers", "RuleCategory_Modifiers" },
                    { "Move", "RuleCategory_Move" },
                    { "Movement", "RuleCategory_Movement" },
                    { "Nebula", "RuleCategory_Nebula" },
                    { "Neighbors", "RuleCategory_Neighbors" },
                    { "None", "RuleCategory_None" },
                    { "ObjectiveCards", "RuleCategory_ObjectiveCards" },
                    { "Opponent", "RuleCategory_Opponent" },
                    { "Pds", "RuleCategory_Pds" },
                    { "PlanetaryShield", "RuleCategory_PlanetaryShield" },
                    { "Planets", "RuleCategory_Planets" },
                    { "Politics", "RuleCategory_Politics" },
                    { "ProducingUnits", "RuleCategory_ProducingUnits" },
                    { "Production", "RuleCategory_Production" },
                    { "PromissoryNotes", "RuleCategory_PromissoryNotes" },
                    { "Purge", "RuleCategory_Purge" },
                    { "Readied", "RuleCategory_Readied" },
                    { "Reinforcements", "RuleCategory_Reinforcements" },
                    { "Relics", "RuleCategory_Relics" },
                    { "Rerolls", "RuleCategory_Rerolls" },
                    { "Resources", "RuleCategory_Resources" },
                    { "Ships", "RuleCategory_Ships" },
                    { "SpaceCannon", "RuleCategory_SpaceCannon" },
                    { "SpaceCombat", "RuleCategory_SpaceCombat" },
                    { "SpaceDock", "RuleCategory_SpaceDock" },
                    { "Speaker", "RuleCategory_Speaker" },
                    { "StatusPhase", "RuleCategory_StatusPhase" },
                    { "StrategicAction", "RuleCategory_StrategicAction" },
                    { "StrategyCard", "RuleCategory_StrategyCard" },
                    { "StrategyPhase", "RuleCategory_StrategyPhase" },
                    { "Structures", "RuleCategory_Structures" },
                    { "Supernova", "RuleCategory_Supernova" },
                    { "SustainDamage", "RuleCategory_SustainDamage" },
                    { "SystemTiles", "RuleCategory_SystemTiles" },
                    { "TacticalAction", "RuleCategory_TacticalAction" },
                    { "Technology", "RuleCategory_Technology" },
                    { "TechnologySystem", "RuleCategory_TechnologySystem" },
                    { "Trade", "RuleCategory_Trade" },
                    { "TradeGoods", "RuleCategory_TradeGoods" },
                    { "Transactions", "RuleCategory_Transactions" },
                    { "Transport", "RuleCategory_Transport" },
                    { "Units", "RuleCategory_Units" },
                    { "UnitUpgrades", "RuleCategory_UnitUpgrades" },
                    { "VictoryPoints", "RuleCategory_VictoryPoints" },
                    { "Warfare", "RuleCategory_Warfare" },
                    { "WormholeNexus", "RuleCategory_WormholeNexus" },
                    { "Wormholes", "RuleCategory_Wormholes" }
                });

            migrationBuilder.InsertData(
                schema: "Card",
                table: "StrategyCards",
                columns: new[] { "Id", "EnumName", "GameVersion", "InitiativeOrder", "PrimaryAbilityText", "SecondaryAbilityText", "Text", "Type" },
                values: new object[,]
                {
                    { 1, "Leadership", "BaseGame", "First", "", "", "", "Strategy" },
                    { 2, "Diplomacy", "Deprecated", "Second", "", "", "", "Strategy" },
                    { 3, "Politics", "BaseGame", "Third", "", "", "", "Strategy" },
                    { 4, "Construction", "Deprecated", "Fourth", "", "", "", "Strategy" },
                    { 5, "Trade", "BaseGame", "Fifth", "", "", "", "Strategy" },
                    { 6, "Warfare", "BaseGame", "Sixth", "", "", "", "Strategy" },
                    { 7, "Technology", "BaseGame", "Seventh", "", "", "", "Strategy" },
                    { 8, "Imperial", "BaseGame", "Eighth", "", "", "", "Strategy" },
                    { 9, "DiplomacyCodexOrdinian", "CodexOrdinian", "Second", "", "", "", "Strategy" },
                    { 10, "ConstructionProphecyOfKings", "ProphecyOfKings", "Fourth", "", "", "", "Strategy" }
                });

            migrationBuilder.InsertData(
                schema: "Galaxy",
                table: "SystemTiles",
                columns: new[] { "SystemTileName", "Anomaly", "FactionName", "GameVersion", "Id", "SystemTileCode", "TileCategory" },
                values: new object[,]
                {
                    { "Tile01", "None", "TheFederationOfSol", "BaseGame", 6, "1", "Green" },
                    { "Tile02", "None", "TheMentakCoalition", "BaseGame", 7, "2", "Green" },
                    { "Tile03", "None", "TheYinBrotherhood", "BaseGame", 8, "3", "Green" },
                    { "Tile04", "None", "TheEmbersOfMuaat", "BaseGame", 9, "4", "Green" },
                    { "Tile05", "None", "TheArborec", "BaseGame", 10, "5", "Green" },
                    { "Tile06", "None", "TheL1z1xMindnet", "BaseGame", 11, "6", "Green" },
                    { "Tile07", "None", "TheWinnu", "BaseGame", 12, "7", "Green" },
                    { "Tile08", "None", "TheNekroVirus", "BaseGame", 13, "8", "Green" },
                    { "Tile09", "None", "TheNaaluCollective", "BaseGame", 14, "9", "Green" },
                    { "Tile10", "None", "TheBaronyOfLetnev", "BaseGame", 15, "10", "Green" },
                    { "Tile1001", "None", "TheAugursOfIlyxum", "DiscordantStars", 110, "1001", "Green" },
                    { "Tile1002", "None", "TheBentorConglomerate", "DiscordantStars", 111, "1002", "Green" },
                    { "Tile1003", "None", "TheBerserkersOfKjalengard", "DiscordantStars", 112, "1003", "Green" },
                    { "Tile1004", "None", "TheCeldauriTradeConfederation", "DiscordantStars", 113, "1004", "Green" },
                    { "Tile1005", "None", "TheCheiranHordes", "DiscordantStars", 114, "1005", "Green" },
                    { "Tile1006", "None", "TheDihMohnFlotilla", "DiscordantStars", 115, "1006", "Green" },
                    { "Tile1007", "None", "TheEdynMandate", "DiscordantStars", 116, "1007", "Green" },
                    { "Tile1008", "None", "TheFlorzenProfiteers", "DiscordantStars", 117, "1008", "Green" },
                    { "Tile1009", "None", "TheFreeSystemsCompact", "DiscordantStars", 118, "1009", "Green" },
                    { "Tile1010", "None", "TheGheminaRaiders", "DiscordantStars", 119, "1010", "Green" },
                    { "Tile1011", "None", "TheGhotiWayfarers", "DiscordantStars", 120, "1011", "Green" },
                    { "Tile1012", "None", "TheGledgeUnion", "DiscordantStars", 121, "1012", "Green" },
                    { "Tile1013", "Nebula", "TheGlimmerOfMortheus", "DiscordantStars", 122, "1013", "Green" },
                    { "Tile1014", "None", "TheKolleccSociety", "DiscordantStars", 123, "1014", "Green" },
                    { "Tile1015", "None", "TheKortaliTribunal", "DiscordantStars", 124, "1015", "Green" },
                    { "Tile1016", "None", "TheKyroSodality", "DiscordantStars", 125, "1016", "Green" },
                    { "Tile1017", "None", "TheLanefirRemnants", "DiscordantStars", 126, "1017", "Green" },
                    { "Tile1018", "None", "TheLiZhoDynasty", "DiscordantStars", 127, "1018", "Green" },
                    { "Tile1019", "None", "TheLTokkKhrask", "DiscordantStars", 128, "1019", "Green" },
                    { "Tile1020", "None", "TheMirvedaProtectorate", "DiscordantStars", 129, "1020", "Green" },
                    { "Tile1021", "None", "TheMonksOfKolume", "DiscordantStars", 130, "1021", "Green" },
                    { "Tile1022", "None", "TheMykoMentori", "DiscordantStars", 131, "1022", "Green" },
                    { "Tile1023", "GravityRift", "TheNivynStarKings", "DiscordantStars", 132, "1023", "Green" },
                    { "Tile1024", "None", "TheNokarSellships", "DiscordantStars", 133, "1024", "Green" },
                    { "Tile1025", "None", "TheOlradinLeague", "DiscordantStars", 134, "1025", "Green" },
                    { "Tile1026", "None", "RohDhnaMechatronics", "DiscordantStars", 135, "1026", "Green" },
                    { "Tile1027", "None", "TheSavagesOfCymiae", "DiscordantStars", 136, "1027", "Green" },
                    { "Tile1028", "None", "TheShipwrightsofAxis", "DiscordantStars", 137, "1028", "Green" },
                    { "Tile1029", "None", "TheTnelisSyndicate", "DiscordantStars", 138, "1029", "Green" },
                    { "Tile1030", "None", "TheVadenBankingClans", "DiscordantStars", 139, "1030", "Green" },
                    { "Tile1031", "None", "TheVaylerianScourge", "DiscordantStars", 140, "1031", "Green" },
                    { "Tile1032", "None", "TheVeldyrSovereignty", "DiscordantStars", 141, "1032", "Green" },
                    { "Tile1033", "None", "TheZealotsOfRhodun", "DiscordantStars", 142, "1033", "Green" },
                    { "Tile1034", "None", "TheZelianPurifier", "DiscordantStars", 143, "1034", "Green" },
                    { "Tile1035A", "AsteroidField", "TheMykoMentori", "DiscordantStars", 144, "1035A", "Red" },
                    { "Tile1035B", "Nebula", "TheMykoMentori", "DiscordantStars", 145, "1035B", "Red" },
                    { "Tile1036", "AsteroidField", "TheZelianPurifier", "DiscordantStars", 146, "1036", "Red" },
                    { "Tile11", "None", "TheClanOfSaar", "BaseGame", 16, "11", "Green" },
                    { "Tile12", "None", "TheUniversitiesOfJolNar", "BaseGame", 17, "12", "Green" },
                    { "Tile13", "None", "SardakkNorr", "BaseGame", 18, "13", "Green" },
                    { "Tile14", "None", "TheXxchaKingdom", "BaseGame", 19, "14", "Green" },
                    { "Tile15", "None", "TheYssarilTribes", "BaseGame", 20, "15", "Green" },
                    { "Tile16", "None", "TheEmiratesOfHacan", "BaseGame", 21, "16", "Green" },
                    { "Tile17", "None", "TheGhostsOfCreuss", "BaseGame", 22, "17", "Green" },
                    { "Tile18", "None", "None", "BaseGame", 23, "18", "MecatolRex" },
                    { "Tile19", "None", "None", "BaseGame", 24, "19", "Blue" },
                    { "Tile20", "None", "None", "BaseGame", 25, "20", "Blue" },
                    { "Tile21", "None", "None", "BaseGame", 26, "21", "Blue" },
                    { "Tile22", "None", "None", "BaseGame", 27, "22", "Blue" },
                    { "Tile23", "None", "None", "BaseGame", 28, "23", "Blue" },
                    { "Tile24", "None", "None", "BaseGame", 29, "24", "Blue" },
                    { "Tile25", "None", "None", "BaseGame", 30, "25", "Blue" },
                    { "Tile26", "None", "None", "BaseGame", 31, "26", "Blue" },
                    { "Tile27", "None", "None", "BaseGame", 32, "27", "Blue" },
                    { "Tile28", "None", "None", "BaseGame", 33, "28", "Blue" },
                    { "Tile29", "None", "None", "BaseGame", 34, "29", "Blue" },
                    { "Tile30", "None", "None", "BaseGame", 35, "30", "Blue" },
                    { "Tile3001A", "None", "None", "TtsHyperlines", 171, "3001A", "Hyperlane" },
                    { "Tile3002A", "None", "None", "TtsHyperlines", 172, "3002A", "Hyperlane" },
                    { "Tile3003A", "None", "None", "TtsHyperlines", 173, "3003A", "Hyperlane" },
                    { "Tile3004A", "None", "None", "TtsHyperlines", 174, "3004A", "Hyperlane" },
                    { "Tile3005A", "None", "None", "TtsHyperlines", 175, "3005A", "Hyperlane" },
                    { "Tile3006A", "None", "None", "TtsHyperlines", 176, "3006A", "Hyperlane" },
                    { "Tile3007A", "None", "None", "TtsHyperlines", 177, "3007A", "Hyperlane" },
                    { "Tile3008A", "None", "None", "TtsHyperlines", 178, "3008A", "Hyperlane" },
                    { "Tile3009A", "None", "None", "TtsHyperlines", 179, "3009A", "Hyperlane" },
                    { "Tile3010A", "None", "None", "TtsHyperlines", 180, "3010A", "Hyperlane" },
                    { "Tile3011A", "None", "None", "TtsHyperlines", 181, "3011A", "Hyperlane" },
                    { "Tile3012A", "None", "None", "TtsHyperlines", 182, "3012A", "Hyperlane" },
                    { "Tile3013A", "None", "None", "TtsHyperlines", 183, "3013A", "Hyperlane" },
                    { "Tile3014A", "None", "None", "TtsHyperlines", 184, "3014A", "Hyperlane" },
                    { "Tile3015A", "None", "None", "TtsHyperlines", 185, "3015A", "Hyperlane" },
                    { "Tile3016A", "None", "None", "TtsHyperlines", 186, "3016A", "Hyperlane" },
                    { "Tile3017A", "None", "None", "TtsHyperlines", 187, "3017A", "Hyperlane" },
                    { "Tile3018A", "None", "None", "TtsHyperlines", 188, "3018A", "Hyperlane" },
                    { "Tile3019A", "None", "None", "TtsHyperlines", 189, "3019A", "Hyperlane" },
                    { "Tile3020A", "None", "None", "TtsHyperlines", 190, "3020A", "Hyperlane" },
                    { "Tile31", "None", "None", "BaseGame", 36, "31", "Blue" },
                    { "Tile32", "None", "None", "BaseGame", 37, "32", "Blue" },
                    { "Tile33", "None", "None", "BaseGame", 38, "33", "Blue" },
                    { "Tile34", "None", "None", "BaseGame", 39, "34", "Blue" },
                    { "Tile35", "None", "None", "BaseGame", 40, "35", "Blue" },
                    { "Tile36", "None", "None", "BaseGame", 41, "36", "Blue" },
                    { "Tile37", "None", "None", "BaseGame", 42, "37", "Blue" },
                    { "Tile38", "None", "None", "BaseGame", 43, "38", "Blue" },
                    { "Tile39", "None", "None", "BaseGame", 44, "39", "Red" },
                    { "Tile40", "None", "None", "BaseGame", 45, "40", "Red" },
                    { "Tile41", "GravityRift", "None", "BaseGame", 46, "41", "Red" },
                    { "Tile42", "Nebula", "None", "BaseGame", 47, "42", "Red" },
                    { "Tile4253", "None", "None", "UnchartedSpace", 147, "4253", "Blue" },
                    { "Tile4254", "None", "None", "UnchartedSpace", 148, "4254", "Blue" },
                    { "Tile4255", "None", "None", "UnchartedSpace", 149, "4255", "Blue" },
                    { "Tile4256", "None", "None", "UnchartedSpace", 150, "4256", "Blue" },
                    { "Tile4257", "None", "None", "UnchartedSpace", 151, "4257", "Blue" },
                    { "Tile4258", "None", "None", "UnchartedSpace", 152, "4258", "Blue" },
                    { "Tile4259", "None", "None", "UnchartedSpace", 153, "4259", "Blue" },
                    { "Tile4260", "None", "None", "UnchartedSpace", 154, "4260", "Blue" },
                    { "Tile4261", "None", "None", "UnchartedSpace", 155, "4261", "Blue" },
                    { "Tile4262", "None", "None", "UnchartedSpace", 156, "4262", "Blue" },
                    { "Tile4263", "None", "None", "UnchartedSpace", 157, "4263", "Blue" },
                    { "Tile4264", "None", "None", "UnchartedSpace", 158, "4264", "Blue" },
                    { "Tile4265", "None", "None", "UnchartedSpace", 159, "4265", "Blue" },
                    { "Tile4266", "None", "None", "UnchartedSpace", 160, "4266", "Blue" },
                    { "Tile4267", "None", "None", "UnchartedSpace", 161, "4267", "Blue" },
                    { "Tile4268", "None", "None", "UnchartedSpace", 162, "4268", "Blue" },
                    { "Tile4269", "Nebula", "None", "UnchartedSpace", 163, "4269", "Red" },
                    { "Tile4270", "None", "None", "UnchartedSpace", 164, "4270", "Red" },
                    { "Tile4271", "None", "None", "UnchartedSpace", 165, "4271", "Red" },
                    { "Tile4272", "Nebula", "None", "UnchartedSpace", 166, "4272", "Red" },
                    { "Tile4273", "AsteroidField", "None", "UnchartedSpace", 167, "4273", "Red" },
                    { "Tile4274", "GravityRift", "None", "UnchartedSpace", 168, "4274", "Red" },
                    { "Tile4275", "GravityRift", "None", "UnchartedSpace", 169, "4275", "Red" },
                    { "Tile4276", "Supernova", "None", "UnchartedSpace", 170, "4276", "Red" },
                    { "Tile43", "Supernova", "None", "BaseGame", 48, "43", "Red" },
                    { "Tile44", "AsteroidField", "None", "BaseGame", 49, "44", "Red" },
                    { "Tile45", "AsteroidField", "None", "BaseGame", 50, "45", "Red" },
                    { "Tile46", "None", "None", "BaseGame", 51, "46", "Red" },
                    { "Tile47", "None", "None", "BaseGame", 52, "47", "Red" },
                    { "Tile48", "None", "None", "BaseGame", 53, "48", "Red" },
                    { "Tile49", "None", "None", "BaseGame", 54, "49", "Red" },
                    { "Tile50", "None", "None", "BaseGame", 55, "50", "Red" },
                    { "Tile51", "None", "TheGhostsOfCreuss", "BaseGame", 56, "51", "ExternalMapTile" },
                    { "Tile52", "None", "TheMahactGeneSorcerers", "ProphecyOfKings", 57, "52", "Green" },
                    { "Tile53", "None", "TheNomad", "ProphecyOfKings", 58, "53", "Green" },
                    { "Tile54", "None", "TheVuilRaithCabal", "ProphecyOfKings", 59, "54", "Green" },
                    { "Tile55", "None", "TheTitansOfUl", "ProphecyOfKings", 60, "55", "Green" },
                    { "Tile56", "None", "TheEmpyrean", "ProphecyOfKings", 61, "56", "Green" },
                    { "Tile57", "None", "TheNaazRokhaAlliance", "ProphecyOfKings", 62, "57", "Green" },
                    { "Tile58", "None", "TheArgentFlight", "ProphecyOfKings", 63, "58", "Green" },
                    { "Tile59", "None", "None", "ProphecyOfKings", 64, "59", "Blue" },
                    { "Tile60", "None", "None", "ProphecyOfKings", 65, "60", "Blue" },
                    { "Tile61", "None", "None", "ProphecyOfKings", 66, "61", "Blue" },
                    { "Tile62", "None", "None", "ProphecyOfKings", 67, "62", "Blue" },
                    { "Tile63", "None", "None", "ProphecyOfKings", 68, "63", "Blue" },
                    { "Tile64", "None", "None", "ProphecyOfKings", 69, "64", "Blue" },
                    { "Tile65", "None", "None", "ProphecyOfKings", 70, "65", "Blue" },
                    { "Tile66", "None", "None", "ProphecyOfKings", 71, "66", "Blue" },
                    { "Tile67", "GravityRift", "None", "ProphecyOfKings", 72, "67", "Red" },
                    { "Tile68", "Nebula", "None", "ProphecyOfKings", 73, "68", "Red" },
                    { "Tile69", "None", "None", "ProphecyOfKings", 74, "69", "Blue" },
                    { "Tile70", "None", "None", "ProphecyOfKings", 75, "70", "Blue" },
                    { "Tile71", "None", "None", "ProphecyOfKings", 76, "71", "Blue" },
                    { "Tile72", "None", "None", "ProphecyOfKings", 77, "72", "Blue" },
                    { "Tile73", "None", "None", "ProphecyOfKings", 78, "73", "Blue" },
                    { "Tile74", "None", "None", "ProphecyOfKings", 79, "74", "Blue" },
                    { "Tile75", "None", "None", "ProphecyOfKings", 80, "75", "Blue" },
                    { "Tile76", "None", "None", "ProphecyOfKings", 81, "76", "Blue" },
                    { "Tile77", "None", "None", "ProphecyOfKings", 82, "77", "Red" },
                    { "Tile78", "None", "None", "ProphecyOfKings", 83, "78", "Red" },
                    { "Tile79", "AsteroidField", "None", "ProphecyOfKings", 84, "79", "Red" },
                    { "Tile80", "Supernova", "None", "ProphecyOfKings", 85, "80", "Red" },
                    { "Tile81", "Supernova", "TheEmbersOfMuaat", "ProphecyOfKings", 86, "81", "Red" },
                    { "Tile82A", "None", "None", "ProphecyOfKings", 87, "82A", "ExternalMapTile" },
                    { "Tile82B", "None", "None", "ProphecyOfKings", 88, "82B", "ExternalMapTile" },
                    { "Tile83A", "None", "None", "ProphecyOfKings", 89, "83A", "Hyperlane" },
                    { "Tile83B", "None", "None", "ProphecyOfKings", 90, "83B", "Hyperlane" },
                    { "Tile84A", "None", "None", "ProphecyOfKings", 91, "84A", "Hyperlane" },
                    { "Tile84B", "None", "None", "ProphecyOfKings", 92, "84B", "Hyperlane" },
                    { "Tile85A", "None", "None", "ProphecyOfKings", 93, "85A", "Hyperlane" },
                    { "Tile85B", "None", "None", "ProphecyOfKings", 94, "85B", "Hyperlane" },
                    { "Tile86A", "None", "None", "ProphecyOfKings", 95, "86A", "Hyperlane" },
                    { "Tile86B", "None", "None", "ProphecyOfKings", 96, "86B", "Hyperlane" },
                    { "Tile87A", "None", "None", "ProphecyOfKings", 97, "87A", "Hyperlane" },
                    { "Tile87B", "None", "None", "ProphecyOfKings", 98, "87B", "Hyperlane" },
                    { "Tile88A", "None", "None", "ProphecyOfKings", 99, "88A", "Hyperlane" },
                    { "Tile88B", "None", "None", "ProphecyOfKings", 100, "88B", "Hyperlane" },
                    { "Tile89A", "None", "None", "ProphecyOfKings", 101, "89A", "Hyperlane" },
                    { "Tile89B", "None", "None", "ProphecyOfKings", 102, "89B", "Hyperlane" },
                    { "Tile90A", "None", "None", "ProphecyOfKings", 103, "90A", "Hyperlane" },
                    { "Tile90B", "None", "None", "ProphecyOfKings", 104, "90B", "Hyperlane" },
                    { "Tile91A", "None", "None", "ProphecyOfKings", 105, "91A", "Hyperlane" },
                    { "Tile91B", "None", "None", "ProphecyOfKings", 106, "91B", "Hyperlane" },
                    { "Tile92", "None", "TheCouncilKeleres", "CodexVigil", 107, "92", "Green" },
                    { "Tile93", "None", "TheCouncilKeleres", "CodexVigil", 108, "93", "Green" },
                    { "Tile94", "None", "TheCouncilKeleres", "CodexVigil", 109, "94", "Green" },
                    { "TileBlackFrame", "None", "None", "Custom", 3, "", "None" },
                    { "TileBlueFrame", "None", "None", "Custom", 4, "", "None" },
                    { "TileEmpty", "None", "None", "Custom", 2, "", "None" },
                    { "TileEronous01", "None", "None", "AscendantSun", 191, "Er01", "Blue" },
                    { "TileEronous02", "None", "None", "AscendantSun", 192, "Er02", "Blue" },
                    { "TileEronous03", "None", "None", "AscendantSun", 193, "Er03", "Blue" },
                    { "TileEronous04", "None", "None", "AscendantSun", 194, "Er04", "Blue" },
                    { "TileEronous05", "None", "None", "AscendantSun", 195, "Er05", "Blue" },
                    { "TileEronous06", "None", "None", "AscendantSun", 196, "Er06", "Blue" },
                    { "TileEronous07", "None", "None", "AscendantSun", 197, "Er07", "Blue" },
                    { "TileEronous08", "None", "None", "AscendantSun", 198, "Er08", "Blue" },
                    { "TileEronous09", "None", "None", "AscendantSun", 199, "Er09", "None" },
                    { "TileEronous10", "None", "None", "AscendantSun", 200, "Er10", "Blue" },
                    { "TileEronous100", "None", "None", "AscendantSun", 290, "Er100", "Blue" },
                    { "TileEronous101", "None", "None", "AscendantSun", 291, "Er101", "Blue" },
                    { "TileEronous102", "None", "None", "AscendantSun", 292, "Er102", "Red" },
                    { "TileEronous103", "GravityRift", "None", "AscendantSun", 293, "Er103", "Red" },
                    { "TileEronous104", "Nebula", "None", "AscendantSun", 294, "Er104", "Red" },
                    { "TileEronous105", "AsteroidField", "None", "AscendantSun", 295, "Er105", "Red" },
                    { "TileEronous106", "None", "None", "AscendantSun", 296, "Er106", "Blue" },
                    { "TileEronous107", "None", "None", "AscendantSun", 297, "Er107", "Blue" },
                    { "TileEronous108", "None", "None", "AscendantSun", 298, "Er108", "Red" },
                    { "TileEronous109", "GravityRift", "None", "AscendantSun", 299, "Er109", "Red" },
                    { "TileEronous11", "None", "None", "AscendantSun", 201, "Er11", "Blue" },
                    { "TileEronous110", "Nebula", "None", "AscendantSun", 300, "Er110", "Red" },
                    { "TileEronous111", "AsteroidField", "None", "AscendantSun", 301, "Er111", "Red" },
                    { "TileEronous112", "None", "None", "AscendantSun", 302, "Er112", "Blue" },
                    { "TileEronous113", "None", "None", "AscendantSun", 303, "Er113", "Blue" },
                    { "TileEronous114", "None", "None", "AscendantSun", 304, "Er114", "Red" },
                    { "TileEronous115", "GravityRift", "None", "AscendantSun", 305, "Er115", "Red" },
                    { "TileEronous116", "Nebula", "None", "AscendantSun", 306, "Er116", "Red" },
                    { "TileEronous117", "AsteroidField", "None", "AscendantSun", 307, "Er117", "Red" },
                    { "TileEronous118", "None", "None", "AscendantSun", 308, "Er118", "Blue" },
                    { "TileEronous119", "None", "None", "AscendantSun", 309, "Er119", "Blue" },
                    { "TileEronous12", "None", "None", "AscendantSun", 202, "Er12", "Blue" },
                    { "TileEronous120", "None", "None", "AscendantSun", 310, "Er120", "Red" },
                    { "TileEronous121", "GravityRift", "None", "AscendantSun", 311, "Er121", "Red" },
                    { "TileEronous122", "Nebula", "None", "AscendantSun", 312, "Er122", "Red" },
                    { "TileEronous123", "AsteroidField", "None", "AscendantSun", 313, "Er123", "Red" },
                    { "TileEronous124", "None", "None", "AscendantSun", 314, "Er124", "Blue" },
                    { "TileEronous125", "None", "None", "AscendantSun", 315, "Er125", "Blue" },
                    { "TileEronous126", "None", "None", "AscendantSun", 316, "Er126", "Red" },
                    { "TileEronous127", "None", "None", "AscendantSun", 317, "Er127", "Red" },
                    { "TileEronous128", "None", "None", "AscendantSun", 318, "Er128", "Red" },
                    { "TileEronous13", "None", "None", "AscendantSun", 203, "Er013", "Blue" },
                    { "TileEronous14", "None", "None", "AscendantSun", 204, "Er14", "Blue" },
                    { "TileEronous15", "None", "None", "AscendantSun", 205, "Er15", "None" },
                    { "TileEronous16", "None", "None", "AscendantSun", 206, "Er16", "None" },
                    { "TileEronous17", "None", "None", "AscendantSun", 207, "Er17", "None" },
                    { "TileEronous18", "Nebula", "None", "AscendantSun", 208, "Er18", "Red" },
                    { "TileEronous19", "GravityRift", "None", "AscendantSun", 209, "Er19", "Red" },
                    { "TileEronous20", "GravityRift", "None", "AscendantSun", 210, "Er20", "Red" },
                    { "TileEronous21", "Supernova", "None", "AscendantSun", 211, "Er21", "Red" },
                    { "TileEronous22", "Nebula", "None", "AscendantSun", 212, "Er22", "Red" },
                    { "TileEronous23", "GravityRift", "None", "AscendantSun", 213, "Er23", "Red" },
                    { "TileEronous24", "Supernova", "None", "AscendantSun", 214, "Er24", "Red" },
                    { "TileEronous25", "None", "None", "AscendantSun", 215, "Er25", "Blue" },
                    { "TileEronous26", "None", "None", "AscendantSun", 216, "Er26", "Blue" },
                    { "TileEronous27", "None", "None", "AscendantSun", 217, "Er27", "Blue" },
                    { "TileEronous28", "None", "None", "AscendantSun", 218, "Er28", "Blue" },
                    { "TileEronous29", "None", "None", "AscendantSun", 219, "Er29", "Blue" },
                    { "TileEronous30", "None", "None", "AscendantSun", 220, "Er30", "Blue" },
                    { "TileEronous31", "None", "None", "AscendantSun", 221, "Er31", "Blue" },
                    { "TileEronous32", "None", "None", "AscendantSun", 222, "Er32", "Blue" },
                    { "TileEronous33", "None", "None", "AscendantSun", 223, "Er33", "Blue" },
                    { "TileEronous34", "None", "None", "AscendantSun", 224, "Er34", "Blue" },
                    { "TileEronous35", "None", "None", "AscendantSun", 225, "Er35", "Blue" },
                    { "TileEronous36", "None", "None", "AscendantSun", 226, "Er36", "Blue" },
                    { "TileEronous37", "None", "None", "AscendantSun", 227, "Er37", "Blue" },
                    { "TileEronous38", "None", "None", "AscendantSun", 228, "Er38", "Blue" },
                    { "TileEronous39", "None", "None", "AscendantSun", 229, "Er39", "Blue" },
                    { "TileEronous40", "None", "None", "AscendantSun", 230, "Er40", "Blue" },
                    { "TileEronous41", "None", "None", "AscendantSun", 231, "Er41", "Blue" },
                    { "TileEronous42", "None", "None", "AscendantSun", 232, "Er42", "Blue" },
                    { "TileEronous43", "None", "None", "AscendantSun", 233, "Er43", "Blue" },
                    { "TileEronous44", "None", "None", "AscendantSun", 234, "Er44", "Blue" },
                    { "TileEronous45", "None", "None", "AscendantSun", 235, "Er45", "Blue" },
                    { "TileEronous46", "None", "None", "AscendantSun", 236, "Er46", "Blue" },
                    { "TileEronous47", "None", "None", "AscendantSun", 237, "Er47", "Blue" },
                    { "TileEronous48", "None", "None", "AscendantSun", 238, "Er48", "Blue" },
                    { "TileEronous49", "AsteroidField", "None", "AscendantSun", 239, "Er49", "Red" },
                    { "TileEronous50", "Nebula", "None", "AscendantSun", 240, "Er50", "Red" },
                    { "TileEronous51", "None", "None", "AscendantSun", 241, "Er51", "Blue" },
                    { "TileEronous52", "None", "None", "AscendantSun", 242, "Er52", "Blue" },
                    { "TileEronous53", "GravityRift", "None", "AscendantSun", 243, "Er53", "Red" },
                    { "TileEronous54", "None", "None", "AscendantSun", 244, "Er54", "Blue" },
                    { "TileEronous55", "None", "None", "AscendantSun", 245, "Er55", "Blue" },
                    { "TileEronous56", "None", "None", "AscendantSun", 246, "Er56", "Blue" },
                    { "TileEronous57", "None", "None", "AscendantSun", 247, "Er57", "Blue" },
                    { "TileEronous58", "None", "None", "AscendantSun", 248, "Er58", "Blue" },
                    { "TileEronous59", "None", "None", "AscendantSun", 249, "Er59", "Blue" },
                    { "TileEronous60", "None", "None", "AscendantSun", 250, "Er60", "Blue" },
                    { "TileEronous61", "None", "None", "AscendantSun", 251, "Er61", "Blue" },
                    { "TileEronous62", "None", "None", "AscendantSun", 252, "Er62", "Blue" },
                    { "TileEronous63", "None", "None", "AscendantSun", 253, "Er63", "Blue" },
                    { "TileEronous64", "None", "None", "AscendantSun", 254, "Er64", "Blue" },
                    { "TileEronous65", "None", "None", "AscendantSun", 255, "Er65", "Red" },
                    { "TileEronous66", "None", "None", "AscendantSun", 256, "Er66", "Red" },
                    { "TileEronous67", "None", "None", "AscendantSun", 257, "Er67", "Red" },
                    { "TileEronous68", "None", "None", "AscendantSun", 258, "Er68", "Red" },
                    { "TileEronous69", "None", "None", "AscendantSun", 259, "Er69", "Red" },
                    { "TileEronous70", "None", "None", "AscendantSun", 260, "Er70", "Red" },
                    { "TileEronous71", "None", "None", "AscendantSun", 261, "Er71", "Red" },
                    { "TileEronous72", "AsteroidField", "None", "AscendantSun", 262, "Er72", "Red" },
                    { "TileEronous73", "AsteroidField", "None", "AscendantSun", 263, "Er72", "Red" },
                    { "TileEronous74", "Nebula", "None", "AscendantSun", 264, "Er72", "Red" },
                    { "TileEronous75", "Nebula", "None", "AscendantSun", 265, "Er75", "Red" },
                    { "TileEronous76", "GravityRift", "None", "AscendantSun", 266, "Er76", "Red" },
                    { "TileEronous77", "GravityRift", "None", "AscendantSun", 267, "Er77", "Red" },
                    { "TileEronous78", "GravityRift", "None", "AscendantSun", 268, "Er78", "Red" },
                    { "TileEronous79", "Supernova", "None", "AscendantSun", 269, "Er79", "Red" },
                    { "TileEronous80", "Supernova", "None", "AscendantSun", 270, "Er80", "Red" },
                    { "TileEronous81", "AsteroidField", "None", "AscendantSun", 271, "Er81", "Red" },
                    { "TileEronous82", "AsteroidField", "None", "AscendantSun", 272, "Er82", "Red" },
                    { "TileEronous83", "AsteroidField", "None", "AscendantSun", 273, "Er83", "Red" },
                    { "TileEronous84", "GravityRift", "None", "AscendantSun", 274, "Er84", "Red" },
                    { "TileEronous85", "Nebula", "None", "AscendantSun", 275, "Er85", "Red" },
                    { "TileEronous86", "Supernova", "None", "AscendantSun", 276, "Er86", "Red" },
                    { "TileEronous87", "None", "None", "AscendantSun", 277, "Er87", "Red" },
                    { "TileEronous88", "GravityRift", "None", "AscendantSun", 278, "Er88", "Red" },
                    { "TileEronous89", "Nebula", "None", "AscendantSun", 279, "Er89", "Red" },
                    { "TileEronous90", "AsteroidField", "None", "AscendantSun", 280, "Er90", "Red" },
                    { "TileEronous91", "None", "None", "AscendantSun", 281, "Er91", "Blue" },
                    { "TileEronous92", "None", "None", "AscendantSun", 282, "Er92", "Blue" },
                    { "TileEronous93", "None", "None", "AscendantSun", 283, "Er93", "Blue" },
                    { "TileEronous94", "Nebula", "None", "AscendantSun", 284, "Er94", "Red" },
                    { "TileEronous95", "GravityRift", "None", "AscendantSun", 285, "Er95", "Red" },
                    { "TileEronous96", "None", "None", "AscendantSun", 286, "Er96", "Red" },
                    { "TileEronous97", "GravityRift", "None", "AscendantSun", 287, "Er97", "Red" },
                    { "TileEronous98", "Nebula", "None", "AscendantSun", 288, "Er98", "Red" },
                    { "TileEronous99", "AsteroidField", "None", "AscendantSun", 289, "Er99", "Red" },
                    { "TileHome", "None", "None", "Custom", 1, "", "None" },
                    { "TileTransparent", "None", "None", "Custom", 5, "T", "None" }
                });

            migrationBuilder.InsertData(
                schema: "Technology",
                table: "Technologies",
                columns: new[] { "TechnologyName", "FactionName", "GameVersion", "Id", "IsFactionTechnology", "Level", "Text", "Type" },
                values: new object[,]
                {
                    { "AdvancedCarrier", "TheFederationOfSol", "BaseGame", 36, "true", "Level2", "", "UnitUpgrade" },
                    { "AegisTwo", "TheDihMohnFlotilla", "DiscordantStars", 94, "true", "Level3", "", "UnitUpgrade" },
                    { "AerieHololattice", "TheArgentFlight", "ProphecyOfKings", 67, "true", "Level1", "", "Cybernetic" },
                    { "Aetherstream", "TheEmpyrean", "BaseGame", 63, "true", "Level2", "", "Propulsion" },
                    { "AgencySupplyNetwork", "TheCouncilKeleres", "CodexVigil", 77, "true", "Level2", "", "Cybernetic" },
                    { "AIDevelopmentAlgorithm", "None", "ProphecyOfKings", 21, "false", "Level0", "", "Warfare" },
                    { "AntimassDeflectors", "None", "BaseGame", 8, "false", "Level0", "", "Propulsion" },
                    { "AppliedBiothermics", "TheMonksOfKolume", "DiscordantStars", 149, "true", "Level2", "", "Biotic" },
                    { "AssaultCannon", "None", "BaseGame", 26, "false", "Level3", "", "Warfare" },
                    { "AtsArmaments", "TheLanefirRemnants", "DiscordantStars", 154, "true", "Level2", "", "Warfare" },
                    { "Bioplasmosis", "TheArborec", "BaseGame", 54, "true", "Level2", "", "Biotic" },
                    { "BioStims", "None", "ProphecyOfKings", 4, "false", "Level1", "", "Biotic" },
                    { "BlackmailPrograms", "TheFlorzenProfiteers", "DiscordantStars", 95, "true", "Level2", "", "Biotic" },
                    { "BlockadeRunnerTwo", "TheTnelisSyndicate", "DiscordantStars", 126, "true", "Level2", "", "UnitUpgrade" },
                    { "BrokerNetwork", "TheBentorConglomerate", "DiscordantStars", 137, "true", "Level1", "", "Biotic" },
                    { "BroodPod", "TheCheiranHordes", "DiscordantStars", 139, "true", "Level2", "", "Warfare" },
                    { "CarrierTwo", "None", "BaseGame", 35, "false", "Level2", "", "UnitUpgrade" },
                    { "CombatTransportTwo", "TheGheminaRaiders", "DiscordantStars", 100, "true", "Level2", "", "UnitUpgrade" },
                    { "ContractualObligations", "RohDhnaMechatronics", "DiscordantStars", 119, "true", "Level2", "", "Cybernetic" },
                    { "CorsairTwo", "TheFlorzenProfiteers", "DiscordantStars", 96, "true", "Level2", "", "UnitUpgrade" },
                    { "CovertStrikeTeams", "TheFreeSystemsCompact", "DiscordantStars", 98, "true", "Level2", "", "Cybernetic" },
                    { "CrimsonLegionnaire", "TheMahactGeneSorcerers", "BaseGame", 30, "true", "Level2", "", "UnitUpgrade" },
                    { "CruiserTwo", "None", "BaseGame", 37, "false", "Level3", "", "UnitUpgrade" },
                    { "DacxiveAnimators", "None", "BaseGame", 3, "false", "Level1", "", "Biotic" },
                    { "DaedalonFlightSystem", "TheTnelisSyndicate", "DiscordantStars", 125, "true", "Level1", "", "Cybernetic" },
                    { "DarkEnergyTap", "None", "ProphecyOfKings", 9, "false", "Level0", "", "Propulsion" },
                    { "DeliveranceEngine", "TheKortaliTribunal", "DiscordantStars", 106, "true", "Level2", "", "Warfare" },
                    { "DestroyerTwo", "None", "BaseGame", 33, "false", "Level2", "", "UnitUpgrade" },
                    { "DimensionalSplicer", "TheGhostsOfCreuss", "BaseGame", 80, "true", "Level1", "", "Warfare" },
                    { "DimensionalTear", "TheVuilRaithCabal", "BaseGame", 47, "true", "Level2", "", "UnitUpgrade" },
                    { "DreadnoughtTwo", "None", "BaseGame", 39, "false", "Level3", "", "UnitUpgrade" },
                    { "DuraniumArmor", "None", "BaseGame", 25, "false", "Level2", "", "Warfare" },
                    { "EmergencyDeployment", "TheShipwrightsofAxis", "DiscordantStars", 124, "true", "Level3", "", "Cybernetic" },
                    { "EmergencyMobilizationProtocols", "TheCeldauriTradeConfederation", "DiscordantStars", 91, "true", "Level2", "", "Warfare" },
                    { "EncryptedTradeHub", "TheEdynMandate", "DiscordantStars", 142, "true", "Level2", "", "Cybernetic" },
                    { "EnvoyNetwork", "TheFreeSystemsCompact", "DiscordantStars", 97, "true", "Level1", "", "Biotic" },
                    { "EResSiphons", "TheUniversitiesOfJolNar", "BaseGame", 73, "true", "Level2", "", "Cybernetic" },
                    { "Exotrireme", "SardakkNorr", "BaseGame", 41, "true", "Level2", "", "UnitUpgrade" },
                    { "FabricationGrid", "TheGlimmerOfMortheus", "DiscordantStars", 102, "true", "Level2", "", "Cybernetic" },
                    { "FalseFlagOperations", "TheOlradinLeague", "DiscordantStars", 117, "true", "Level1", "", "Warfare" },
                    { "FighterTwo", "None", "BaseGame", 31, "false", "Level2", "", "UnitUpgrade" },
                    { "FleetLogistics", "None", "BaseGame", 12, "false", "Level2", "", "Propulsion" },
                    { "FloatingFactory", "TheClanOfSaar", "BaseGame", 46, "true", "Level2", "", "UnitUpgrade" },
                    { "FractalPlating", "TheGlimmerOfMortheus", "DiscordantStars", 101, "true", "Level2", "", "Warfare" },
                    { "GaussCannonTwo", "TheMirvedaProtectorate", "DiscordantStars", 112, "true", "Level2", "", "UnitUpgrade" },
                    { "GeneticRecombination", "TheMahactGeneSorcerers", "BaseGame", 53, "true", "Level1", "", "Biotic" },
                    { "GeosympathicImpeller", "TheOlradinLeague", "DiscordantStars", 118, "true", "Level1", "", "Propulsion" },
                    { "GravitonLaserSystem", "None", "BaseGame", 16, "false", "Level1", "", "Cybernetic" },
                    { "GravityDrive", "None", "BaseGame", 10, "false", "Level1", "", "Propulsion" },
                    { "HeavyBomberTwo", "TheLiZhoDynasty", "DiscordantStars", 108, "true", "Level2", "", "UnitUpgrade" },
                    { "HegemonicTradePolicy", "TheWinnu", "BaseGame", 74, "true", "Level2", "", "Cybernetic" },
                    { "HelTitan", "TheTitansOfUl", "BaseGame", 49, "true", "Level2", "", "UnitUpgrade" },
                    { "HybridCrystalFighter", "TheNaaluCollective", "BaseGame", 32, "true", "Level2", "", "UnitUpgrade" },
                    { "HyperMetabolism", "None", "BaseGame", 5, "false", "Level2", "", "Biotic" },
                    { "ChaosMapping", "TheClanOfSaar", "BaseGame", 61, "true", "Level1", "", "Propulsion" },
                    { "ChitinHulkTwo", "TheCheiranHordes", "DiscordantStars", 140, "true", "Level3", "", "UnitUpgrade" },
                    { "IihqModernization", "TheCouncilKeleres", "CodexVigil", 70, "true", "Level1", "", "Cybernetic" },
                    { "ImpactorTwo", "TheZelianPurifier", "DiscordantStars", 136, "true", "Level2", "", "UnitUpgrade" },
                    { "ImpressmentPrograms", "TheDihMohnFlotilla", "DiscordantStars", 93, "true", "Level2", "", "Cybernetic" },
                    { "ImpulseCore", "TheYinBrotherhood", "BaseGame", 76, "true", "Level2", "", "Cybernetic" },
                    { "IndoctrinationTeam", "TheKyroSodality", "DiscordantStars", 151, "true", "Level2", "", "Biotic" },
                    { "InfantryTwo", "None", "BaseGame", 27, "false", "Level2", "", "UnitUpgrade" },
                    { "InheritanceSystems", "TheL1z1xMindnet", "BaseGame", 72, "true", "Level2", "", "Cybernetic" },
                    { "InstrinctTraining", "TheXxchaKingdom", "BaseGame", 51, "true", "Level1", "", "Biotic" },
                    { "IntegratedEconomy", "None", "BaseGame", 19, "false", "Level3", "", "Cybernetic" },
                    { "KrovozStrikeTeams", "TheVadenBankingClans", "DiscordantStars", 128, "true", "Level2", "", "Cybernetic" },
                    { "L4Disruptors", "TheBaronyOfLetnev", "BaseGame", 68, "true", "Level1", "", "Cybernetic" },
                    { "LancerDreadnoughtTwo", "TheVeldyrSovereignty", "DiscordantStars", 132, "true", "Level3", "", "UnitUpgrade" },
                    { "LazaxGateFolding", "TheWinnu", "BaseGame", 66, "true", "Level2", "", "Propulsion" },
                    { "LetaniWarrior", "TheArborec", "BaseGame", 29, "true", "Level2", "", "UnitUpgrade" },
                    { "LightningDrives", "TheGledgeUnion", "DiscordantStars", 145, "true", "Level3", "", "Propulsion" },
                    { "LightWaveDeflector", "None", "BaseGame", 13, "false", "Level3", "", "Propulsion" },
                    { "LocalContracts", "TheNokarSellships", "DiscordantStars", 155, "true", "Level2", "", "Cybernetic" },
                    { "MagenDefenseGrid", "None", "Deprecated", 22, "false", "Level1", "", "Warfare" },
                    { "MagenDefenseGridOmega", "None", "BaseGame", 23, "false", "Level1", "", "Warfare" },
                    { "MageonImplants", "TheYssarilTribes", "BaseGame", 60, "true", "Level3", "", "Biotic" },
                    { "MagmusReactor", "TheEmbersOfMuaat", "Deprecated", 84, "true", "Level2", "", "Warfare" },
                    { "MagmusReactorOmega", "TheEmbersOfMuaat", "CodexOrdinian", 85, "true", "Level2", "", "Warfare" },
                    { "Memoria", "TheNomad", "BaseGame", 42, "true", "Level3", "", "UnitUpgrade" },
                    { "MergedReplicators", "TheBentorConglomerate", "DiscordantStars", 138, "true", "Level2", "", "Cybernetic" },
                    { "MidasTurbine", "TheVadenBankingClans", "DiscordantStars", 127, "true", "Level1", "", "Propulsion" },
                    { "MirrorComputing", "TheMentakCoalition", "BaseGame", 79, "true", "Level3", "", "Cybernetic" },
                    { "MyceliumRingTwo", "TheMykoMentori", "DiscordantStars", 114, "true", "Level2", "", "UnitUpgrade" },
                    { "NetworkedCommand", "TheGhotiWayfarers", "DiscordantStars", 143, "true", "Level1", "", "Biotic" },
                    { "NeuralMotivator", "None", "BaseGame", 1, "false", "Level0", "", "Biotic" },
                    { "Neuroglaive", "TheNaaluCollective", "BaseGame", 58, "true", "Level3", "", "Biotic" },
                    { "NonEuclidianShielding", "TheBaronyOfLetnev", "BaseGame", 83, "true", "Level2", "", "Warfare" },
                    { "NullificationField", "TheXxchaKingdom", "BaseGame", 75, "true", "Level2", "", "Cybernetic" },
                    { "OmniscienceField", "TheMonksOfKolume", "DiscordantStars", 150, "true", "Level3", "", "Warfare" },
                    { "OrbitalDefenseGrid", "TheMirvedaProtectorate", "DiscordantStars", 111, "true", "Level2", "", "Warfare" },
                    { "OrionPlatformTwo", "TheGledgeUnion", "DiscordantStars", 146, "true", "Level2", "", "UnitUpgrade" },
                    { "ParallelProduction", "TheGhotiWayfarers", "DiscordantStars", 144, "true", "Level1", "", "Cybernetic" },
                    { "PdsTwo", "None", "BaseGame", 48, "false", "Level2", "", "UnitUpgrade" },
                    { "PilgrimageBeacons", "TheZealotsOfRhodun", "DiscordantStars", 134, "true", "Level2", "", "Propulsion" },
                    { "PlasmaScoring", "None", "BaseGame", 20, "false", "Level0", "", "Warfare" },
                    { "PredictiveIntelligence", "None", "ProphecyOfKings", 17, "false", "Level1", "", "Cybernetic" },
                    { "PreFabArcologies", "TheNaazRokhaAlliance", "BaseGame", 59, "true", "Level3", "", "Biotic" },
                    { "ProductionBiomes", "TheEmiratesOfHacan", "BaseGame", 55, "true", "Level2", "", "Biotic" },
                    { "PrototypeWarSun", "TheEmbersOfMuaat", "BaseGame", 44, "true", "Level4", "", "UnitUpgrade" },
                    { "PsychoactiveArmaments", "TheMykoMentori", "DiscordantStars", 113, "true", "Level2", "", "Biotic" },
                    { "Psychoarchaeology", "None", "ProphecyOfKings", 2, "false", "Level0", "", "Biotic" },
                    { "Psychographics", "TheAugursOfIlyxum", "DiscordantStars", 89, "true", "Level3", "", "Biotic" },
                    { "QuantumDatahubNode", "TheEmiratesOfHacan", "BaseGame", 78, "true", "Level3", "", "Cybernetic" },
                    { "RaiderTwo", "TheVaylerianScourge", "DiscordantStars", 130, "true", "Level3", "", "UnitUpgrade" },
                    { "RecursiveWorm", "TheSavagesOfCymiae", "DiscordantStars", 121, "true", "Level1", "", "Cybernetic" },
                    { "RiftEngines", "TheShipwrightsofAxis", "DiscordantStars", 123, "true", "Level1", "", "Propulsion" },
                    { "SabreTwo", "TheNokarSellships", "DiscordantStars", 156, "true", "Level2", "", "UnitUpgrade" },
                    { "SalvageOperations", "TheMentakCoalition", "BaseGame", 71, "true", "Level2", "", "Cybernetic" },
                    { "SanctificationField", "TheZealotsOfRhodun", "DiscordantStars", 133, "true", "Level3", "", "Cybernetic" },
                    { "SarweenTools", "None", "BaseGame", 14, "false", "Level0", "", "Cybernetic" },
                    { "SaturnEngine", "TheTitansOfUl", "BaseGame", 38, "true", "Level3", "", "UnitUpgrade" },
                    { "ScanlinkDroneNetwork", "None", "ProphecyOfKings", 15, "false", "Level0", "", "Cybernetic" },
                    { "ScavengerExos", "TheVaylerianScourge", "DiscordantStars", 129, "true", "Level1", "", "Warfare" },
                    { "SeekerDrones", "TheKolleccSociety", "DiscordantStars", 103, "true", "Level2", "", "Cybernetic" },
                    { "SeidrProject", "TheVeldyrSovereignty", "DiscordantStars", 131, "true", "Level2", "", "Warfare" },
                    { "SelfAssemblyRoutines", "None", "ProphecyOfKings", 24, "false", "Level1", "", "Warfare" },
                    { "SentientDatapool", "TheAugursOfIlyxum", "DiscordantStars", 90, "true", "Level2", "", "Cybernetic" },
                    { "ShardVolley", "TheZelianPurifier", "DiscordantStars", 135, "true", "Level1", "", "Warfare" },
                    { "ShatteredSkyTwo", "TheLTokkKhrask", "DiscordantStars", 110, "true", "Level3", "", "UnitUpgrade" },
                    { "ShroudedSkirmishers", "TheKolleccSociety", "DiscordantStars", 104, "true", "Level1", "", "Propulsion" },
                    { "SlingRelay", "None", "ProphecyOfKings", 11, "false", "Level1", "", "Propulsion" },
                    { "SpaceDockTwo", "None", "BaseGame", 45, "false", "Level2", "", "UnitUpgrade" },
                    { "SpacialConduitCylinder", "TheUniversitiesOfJolNar", "BaseGame", 62, "true", "Level2", "", "Propulsion" },
                    { "SparkThrusters", "TheLanefirRemnants", "DiscordantStars", 153, "true", "Level2", "", "Propulsion" },
                    { "SpecOps", "TheFederationOfSol", "BaseGame", 28, "true", "Level2", "", "UnitUpgrade" },
                    { "StarDragonTwo", "TheBerserkersOfKjalengard", "DiscordantStars", 148, "true", "Level2", "", "UnitUpgrade" },
                    { "StonesEmbrace", "TheLTokkKhrask", "DiscordantStars", 109, "true", "Level2", "", "Biotic" },
                    { "StrikeWingAlpha", "TheArgentFlight", "BaseGame", 34, "true", "Level2", "", "UnitUpgrade" },
                    { "SuperDreadnought", "TheL1z1xMindnet", "BaseGame", 40, "true", "Level3", "", "UnitUpgrade" },
                    { "Supercharge", "TheNaazRokhaAlliance", "ProphecyOfKings", 81, "true", "Level1", "", "Warfare" },
                    { "TempestDrive", "TheKortaliTribunal", "DiscordantStars", 105, "true", "Level2", "", "Biotic" },
                    { "TemporalCommandSuite", "TheNomad", "ProphecyOfKings", 69, "true", "Level1", "", "Cybernetic" },
                    { "TerrafactoryTwo", "RohDhnaMechatronics", "DiscordantStars", 120, "true", "Level4", "", "UnitUpgrade" },
                    { "TradePortTwo", "TheCeldauriTradeConfederation", "DiscordantStars", 92, "true", "Level2", "", "UnitUpgrade" },
                    { "TransitDiodes", "None", "BaseGame", 18, "false", "Level2", "", "Cybernetic" },
                    { "TransparasteelPlating", "TheYssarilTribes", "BaseGame", 52, "true", "Level1", "", "Biotic" },
                    { "UnholyAbominationTwo", "TheSavagesOfCymiae", "DiscordantStars", 122, "true", "Level2", "", "UnitUpgrade" },
                    { "UnityAlgorithm", "TheEdynMandate", "DiscordantStars", 141, "true", "Level3", "", "Biotic" },
                    { "ValefarAssimilatorX", "TheNekroVirus", "BaseGame", 87, "true", "Level0", "", "Faction" },
                    { "ValefarAssimilatorY", "TheNekroVirus", "BaseGame", 88, "true", "Level0", "", "Faction" },
                    { "VectorProgram", "TheKyroSodality", "DiscordantStars", 152, "true", "Level1", "", "Cybernetic" },
                    { "VoidflareWardenTwo", "TheNivynStarKings", "DiscordantStars", 116, "true", "Level3", "", "UnitUpgrade" },
                    { "VoidwakeMissiles", "TheNivynStarKings", "DiscordantStars", 115, "true", "Level1", "", "Cybernetic" },
                    { "Voidwatch", "TheEmpyrean", "BaseGame", 50, "true", "Level1", "", "Biotic" },
                    { "Vortex", "TheVuilRaithCabal", "ProphecyOfKings", 82, "true", "Level1", "", "Warfare" },
                    { "WalkyrieParticleWeave", "SardakkNorr", "BaseGame", 86, "true", "Level2", "", "Warfare" },
                    { "WarSongImplants", "TheGheminaRaiders", "DiscordantStars", 99, "true", "Level3", "", "Biotic" },
                    { "WarSun", "None", "BaseGame", 43, "false", "Level4", "", "UnitUpgrade" },
                    { "WormholeGenerator", "TheGhostsOfCreuss", "Deprecated", 64, "true", "Level2", "", "Propulsion" },
                    { "WormholeGeneratorOmega", "TheGhostsOfCreuss", "CodexOrdinian", 65, "true", "Level2", "", "Propulsion" },
                    { "WraithEngine", "TheLiZhoDynasty", "DiscordantStars", 107, "true", "Level2", "", "Propulsion" },
                    { "X89BacterialWeapon", "None", "Deprecated", 6, "false", "Level3", "", "Biotic" },
                    { "X89BacterialWeaponOmega", "None", "BaseGame", 7, "false", "Level3", "", "Biotic" },
                    { "YinSpinner", "TheYinBrotherhood", "Deprecated", 56, "true", "Level2", "", "Biotic" },
                    { "YinSpinnerOmega", "TheYinBrotherhood", "CodexOrdinian", 57, "true", "Level2", "", "Biotic" },
                    { "ZhrgarStimulants", "TheBerserkersOfKjalengard", "DiscordantStars", 147, "true", "Level1", "", "Biotic" }
                });

            migrationBuilder.InsertData(
                schema: "Unit",
                table: "Units",
                columns: new[] { "UnitName", "Capacity", "Combat", "Cost", "Id", "Move", "UnitType" },
                values: new object[,]
                {
                    { "Carrier", -1, -1, -1, 7, -1, "Ship" },
                    { "Cruiser", -1, -1, -1, 8, -1, "Ship" },
                    { "Destroyer", -1, -1, -1, 6, -1, "Ship" },
                    { "Dreadnought", -1, -1, -1, 9, -1, "Ship" },
                    { "Fighter", -1, -1, -1, 5, -1, "Ship" },
                    { "Flagship", -1, -1, -1, 10, -1, "Ship" },
                    { "Infantry", -1, -1, -1, 3, -1, "GroundUnit" },
                    { "Mech", -1, -1, -1, 4, -1, "GroundUnit" },
                    { "Pds", -1, -1, -1, 2, -1, "Structure" },
                    { "SpaceDock", -1, -1, -1, 1, -1, "Structure" },
                    { "WarSun", -1, -1, -1, 11, -1, "Ship" }
                });

            migrationBuilder.InsertData(
                schema: "Statistics",
                table: "WebsiteStatistics",
                columns: new[] { "Id", "ColorsDrafted", "FactionsDrafted", "GamesPlayed", "MapsArchived", "MapsGenerated", "SlicesArchived", "SlicesGenerated", "Visitors" },
                values: new object[] { 1, 0, 0, 0, 0, 0, 0, 0, 0 });

            migrationBuilder.InsertData(
                schema: "Website",
                table: "Websites",
                columns: new[] { "Id", "Description", "Title", "WebsitePath" },
                values: new object[,]
                {
                    { 1, "", "Fantasy Flight Games", "https://www.fantasyflightgames.com/en/products/#/universe/twilight-imperium-universe" },
                    { 2, "", "Twilight Imperium Wiki", "https://twilight-imperium.fandom.com/wiki/Twilight_Imperium_Wiki" },
                    { 3, "", "Twilight Imperium 4th Edition Rules Reference", "https://www.tirules.com" },
                    { 4, "", "Board Game Geek", "https://boardgamegeek.com/boardgame/233078/twilight-imperium-fourth-edition" },
                    { 5, "", "Reddit", "https://www.reddit.com/r/twilightimperium/" },
                    { 6, "", "SCPT Podcasts", "https://www.podbean.com/user-mEJPMtyOuVj" },
                    { 7, "", "Twilight Wars", "https://www.twilightwars.com/games" },
                    { 8, "", "Twilight Imperium Assistant", "https://ti-assistant.com" },
                    { 9, "", "Online Match Stats", "https://lookerstudio.google.com/reporting/3b435bf2-2100-488c-a424-130f1d22ebb0/page/pE58B" },
                    { 10, "", "Keegan Map Generator", "https://keeganw.github.io/ti4/" },
                    { 11, "", "TI4 Lab", "https://ti4-lab.fly.dev/" },
                    { 12, "", "Twilight Imperium 4 Balanced Map Generator", "https://ti4-map-generator.derekpeterson.ca" },
                    { 13, "", "TI4 Map Lab", "https://joepinion.github.io/ti4-map-lab/" },
                    { 14, "", "Milty Draft Generator", "https://milty.shenanigans.be" },
                    { 15, "", "Milty Draft Map Generator", "https://miltydraft.com/goodtimes" },
                    { 16, "", "TI4 Cartographer", "https://acodcha.github.io/ti4-cartographer/" },
                    { 17, "", "Color Picker", "https://www.ti.vetinari.net" },
                    { 18, "", "Twilight Imperium 4th Color Assigner", "https://brilleslange.github.io" },
                    { 19, "", "Card Generator", "https://ti4-card-images.appspot.com/static/card.html" }
                });

            migrationBuilder.InsertData(
                schema: "Relationships",
                table: "FactionTechnology",
                columns: new[] { "FactionName", "TechnologyName", "StartingTechnology" },
                values: new object[,]
                {
                    { "RohDhnaMechatronics", "Psychoarchaeology", "true" },
                    { "RohDhnaMechatronics", "SarweenTools", "true" },
                    { "TheArborec", "MagenDefenseGridOmega", "true" },
                    { "TheArgentFlight", "NeuralMotivator", "true" },
                    { "TheArgentFlight", "PlasmaScoring", "true" },
                    { "TheArgentFlight", "SarweenTools", "true" },
                    { "TheAugursOfIlyxum", "AIDevelopmentAlgorithm", "true" },
                    { "TheAugursOfIlyxum", "ScanlinkDroneNetwork", "true" },
                    { "TheBaronyOfLetnev", "AntimassDeflectors", "true" },
                    { "TheBaronyOfLetnev", "PlasmaScoring", "true" },
                    { "TheBentorConglomerate", "DarkEnergyTap", "true" },
                    { "TheBentorConglomerate", "Psychoarchaeology", "true" },
                    { "TheBentorConglomerate", "SarweenTools", "true" },
                    { "TheBerserkersOfKjalengard", "CarrierTwo", "true" },
                    { "TheBerserkersOfKjalengard", "CruiserTwo", "true" },
                    { "TheBerserkersOfKjalengard", "DestroyerTwo", "true" },
                    { "TheBerserkersOfKjalengard", "DreadnoughtTwo", "true" },
                    { "TheBerserkersOfKjalengard", "FighterTwo", "true" },
                    { "TheBerserkersOfKjalengard", "InfantryTwo", "true" },
                    { "TheBerserkersOfKjalengard", "PdsTwo", "true" },
                    { "TheBerserkersOfKjalengard", "SpaceDockTwo", "true" },
                    { "TheCeldauriTradeConfederation", "AntimassDeflectors", "true" },
                    { "TheCeldauriTradeConfederation", "PlasmaScoring", "true" },
                    { "TheCeldauriTradeConfederation", "SarweenTools", "true" },
                    { "TheClanOfSaar", "AntimassDeflectors", "true" },
                    { "TheDihMohnFlotilla", "DarkEnergyTap", "true" },
                    { "TheDihMohnFlotilla", "ScanlinkDroneNetwork", "true" },
                    { "TheEdynMandate", "AIDevelopmentAlgorithm", "true" },
                    { "TheEdynMandate", "AntimassDeflectors", "true" },
                    { "TheEdynMandate", "DarkEnergyTap", "true" },
                    { "TheEdynMandate", "NeuralMotivator", "true" },
                    { "TheEdynMandate", "PlasmaScoring", "true" },
                    { "TheEdynMandate", "Psychoarchaeology", "true" },
                    { "TheEdynMandate", "SarweenTools", "true" },
                    { "TheEdynMandate", "ScanlinkDroneNetwork", "true" },
                    { "TheEmbersOfMuaat", "PlasmaScoring", "true" },
                    { "TheEmiratesOfHacan", "AntimassDeflectors", "true" },
                    { "TheEmiratesOfHacan", "SarweenTools", "true" },
                    { "TheEmpyrean", "DarkEnergyTap", "true" },
                    { "TheFederationOfSol", "AntimassDeflectors", "true" },
                    { "TheFederationOfSol", "NeuralMotivator", "true" },
                    { "TheFlorzenProfiteers", "NeuralMotivator", "true" },
                    { "TheFlorzenProfiteers", "ScanlinkDroneNetwork", "true" },
                    { "TheFreeSystemsCompact", "Psychoarchaeology", "true" },
                    { "TheGheminaRaiders", "DarkEnergyTap", "true" },
                    { "TheGheminaRaiders", "Psychoarchaeology", "true" },
                    { "TheGhostsOfCreuss", "GravityDrive", "true" },
                    { "TheGhotiWayfarers", "GravityDrive", "true" },
                    { "TheGhotiWayfarers", "SlingRelay", "true" },
                    { "TheGledgeUnion", "AIDevelopmentAlgorithm", "true" },
                    { "TheGledgeUnion", "Psychoarchaeology", "true" },
                    { "TheGledgeUnion", "ScanlinkDroneNetwork", "true" },
                    { "TheGlimmerOfMortheus", "DarkEnergyTap", "true" },
                    { "TheGlimmerOfMortheus", "SarweenTools", "true" },
                    { "TheCheiranHordes", "MagenDefenseGridOmega", "true" },
                    { "TheCheiranHordes", "SelfAssemblyRoutines", "true" },
                    { "TheKolleccSociety", "ScanlinkDroneNetwork", "true" },
                    { "TheKortaliTribunal", "PlasmaScoring", "true" },
                    { "TheKortaliTribunal", "Psychoarchaeology", "true" },
                    { "TheKyroSodality", "BioStims", "true" },
                    { "TheKyroSodality", "DacxiveAnimators", "true" },
                    { "TheL1z1xMindnet", "NeuralMotivator", "true" },
                    { "TheL1z1xMindnet", "PlasmaScoring", "true" },
                    { "TheLanefirRemnants", "AIDevelopmentAlgorithm", "true" },
                    { "TheLanefirRemnants", "DarkEnergyTap", "true" },
                    { "TheLanefirRemnants", "ScanlinkDroneNetwork", "true" },
                    { "TheLiZhoDynasty", "AntimassDeflectors", "true" },
                    { "TheLiZhoDynasty", "Psychoarchaeology", "true" },
                    { "TheLTokkKhrask", "PlasmaScoring", "true" },
                    { "TheLTokkKhrask", "ScanlinkDroneNetwork", "true" },
                    { "TheMahactGeneSorcerers", "BioStims", "true" },
                    { "TheMahactGeneSorcerers", "PredictiveIntelligence", "true" },
                    { "TheMentakCoalition", "PlasmaScoring", "true" },
                    { "TheMentakCoalition", "SarweenTools", "true" },
                    { "TheMirvedaProtectorate", "AIDevelopmentAlgorithm", "true" },
                    { "TheMonksOfKolume", "GravitonLaserSystem", "true" },
                    { "TheMonksOfKolume", "PredictiveIntelligence", "true" },
                    { "TheMykoMentori", "PredictiveIntelligence", "true" },
                    { "TheNaaluCollective", "NeuralMotivator", "true" },
                    { "TheNaaluCollective", "SarweenTools", "true" },
                    { "TheNaazRokhaAlliance", "AIDevelopmentAlgorithm", "true" },
                    { "TheNaazRokhaAlliance", "Psychoarchaeology", "true" },
                    { "TheNekroVirus", "DacxiveAnimators", "true" },
                    { "TheNekroVirus", "ValefarAssimilatorX", "true" },
                    { "TheNekroVirus", "ValefarAssimilatorY", "true" },
                    { "TheNivynStarKings", "DarkEnergyTap", "true" },
                    { "TheNivynStarKings", "PlasmaScoring", "true" },
                    { "TheNokarSellships", "AIDevelopmentAlgorithm", "true" },
                    { "TheNokarSellships", "DarkEnergyTap", "true" },
                    { "TheNokarSellships", "Psychoarchaeology", "true" },
                    { "TheNomad", "SlingRelay", "true" },
                    { "TheOlradinLeague", "Psychoarchaeology", "true" },
                    { "TheOlradinLeague", "ScanlinkDroneNetwork", "true" },
                    { "TheSavagesOfCymiae", "AIDevelopmentAlgorithm", "true" },
                    { "TheSavagesOfCymiae", "NeuralMotivator", "true" },
                    { "TheShipwrightsofAxis", "AIDevelopmentAlgorithm", "true" },
                    { "TheShipwrightsofAxis", "SarweenTools", "true" },
                    { "TheTitansOfUl", "AntimassDeflectors", "true" },
                    { "TheTitansOfUl", "ScanlinkDroneNetwork", "true" },
                    { "TheTnelisSyndicate", "AntimassDeflectors", "true" },
                    { "TheTnelisSyndicate", "NeuralMotivator", "true" },
                    { "TheTnelisSyndicate", "PlasmaScoring", "true" },
                    { "TheUniversitiesOfJolNar", "AntimassDeflectors", "true" },
                    { "TheUniversitiesOfJolNar", "NeuralMotivator", "true" },
                    { "TheUniversitiesOfJolNar", "PlasmaScoring", "true" },
                    { "TheUniversitiesOfJolNar", "SarweenTools", "true" },
                    { "TheVadenBankingClans", "AntimassDeflectors", "true" },
                    { "TheVadenBankingClans", "NeuralMotivator", "true" },
                    { "TheVadenBankingClans", "SarweenTools", "true" },
                    { "TheVaylerianScourge", "DarkEnergyTap", "true" },
                    { "TheVaylerianScourge", "NeuralMotivator", "true" },
                    { "TheVeldyrSovereignty", "AIDevelopmentAlgorithm", "true" },
                    { "TheVeldyrSovereignty", "DarkEnergyTap", "true" },
                    { "TheVuilRaithCabal", "SelfAssemblyRoutines", "true" },
                    { "TheWinnu", "AIDevelopmentAlgorithm", "true" },
                    { "TheWinnu", "AntimassDeflectors", "true" },
                    { "TheWinnu", "DarkEnergyTap", "true" },
                    { "TheWinnu", "NeuralMotivator", "true" },
                    { "TheWinnu", "PlasmaScoring", "true" },
                    { "TheWinnu", "Psychoarchaeology", "true" },
                    { "TheWinnu", "SarweenTools", "true" },
                    { "TheWinnu", "ScanlinkDroneNetwork", "true" },
                    { "TheXxchaKingdom", "GravitonLaserSystem", "true" },
                    { "TheYinBrotherhood", "SarweenTools", "true" },
                    { "TheYssarilTribes", "NeuralMotivator", "true" },
                    { "TheZealotsOfRhodun", "BioStims", "true" },
                    { "TheZelianPurifier", "AIDevelopmentAlgorithm", "true" },
                    { "TheZelianPurifier", "AntimassDeflectors", "true" }
                });

            migrationBuilder.InsertData(
                schema: "Relationships",
                table: "FactionUnit",
                columns: new[] { "FactionName", "UnitName", "Count" },
                values: new object[,]
                {
                    { "RohDhnaMechatronics", "Carrier", 2 },
                    { "RohDhnaMechatronics", "Destroyer", 1 },
                    { "RohDhnaMechatronics", "Fighter", 3 },
                    { "RohDhnaMechatronics", "Infantry", 3 },
                    { "RohDhnaMechatronics", "SpaceDock", 1 },
                    { "SardakkNorr", "Carrier", 2 },
                    { "SardakkNorr", "Cruiser", 1 },
                    { "SardakkNorr", "Infantry", 5 },
                    { "SardakkNorr", "Pds", 1 },
                    { "SardakkNorr", "SpaceDock", 1 },
                    { "TheArborec", "Carrier", 1 },
                    { "TheArborec", "Cruiser", 1 },
                    { "TheArborec", "Fighter", 2 },
                    { "TheArborec", "Infantry", 4 },
                    { "TheArborec", "Pds", 1 },
                    { "TheArborec", "SpaceDock", 1 },
                    { "TheArgentFlight", "Carrier", 1 },
                    { "TheArgentFlight", "Destroyer", 2 },
                    { "TheArgentFlight", "Fighter", 2 },
                    { "TheArgentFlight", "Infantry", 5 },
                    { "TheArgentFlight", "Pds", 1 },
                    { "TheArgentFlight", "SpaceDock", 1 },
                    { "TheAugursOfIlyxum", "Carrier", 1 },
                    { "TheAugursOfIlyxum", "Destroyer", 2 },
                    { "TheAugursOfIlyxum", "Fighter", 2 },
                    { "TheAugursOfIlyxum", "Infantry", 4 },
                    { "TheAugursOfIlyxum", "Pds", 1 },
                    { "TheAugursOfIlyxum", "SpaceDock", 1 },
                    { "TheBaronyOfLetnev", "Carrier", 1 },
                    { "TheBaronyOfLetnev", "Destroyer", 1 },
                    { "TheBaronyOfLetnev", "Dreadnought", 1 },
                    { "TheBaronyOfLetnev", "Fighter", 1 },
                    { "TheBaronyOfLetnev", "Infantry", 3 },
                    { "TheBaronyOfLetnev", "SpaceDock", 1 },
                    { "TheBentorConglomerate", "Carrier", 1 },
                    { "TheBentorConglomerate", "Cruiser", 2 },
                    { "TheBentorConglomerate", "Fighter", 3 },
                    { "TheBentorConglomerate", "Infantry", 4 },
                    { "TheBentorConglomerate", "Pds", 1 },
                    { "TheBentorConglomerate", "SpaceDock", 1 },
                    { "TheBerserkersOfKjalengard", "Carrier", 2 },
                    { "TheBerserkersOfKjalengard", "Destroyer", 1 },
                    { "TheBerserkersOfKjalengard", "Fighter", 4 },
                    { "TheBerserkersOfKjalengard", "Infantry", 4 },
                    { "TheBerserkersOfKjalengard", "Pds", 1 },
                    { "TheBerserkersOfKjalengard", "SpaceDock", 1 },
                    { "TheCeldauriTradeConfederation", "Carrier", 2 },
                    { "TheCeldauriTradeConfederation", "Destroyer", 1 },
                    { "TheCeldauriTradeConfederation", "Fighter", 4 },
                    { "TheCeldauriTradeConfederation", "Infantry", 3 },
                    { "TheCeldauriTradeConfederation", "Pds", 1 },
                    { "TheCeldauriTradeConfederation", "SpaceDock", 1 },
                    { "TheClanOfSaar", "Carrier", 2 },
                    { "TheClanOfSaar", "Cruiser", 1 },
                    { "TheClanOfSaar", "Fighter", 2 },
                    { "TheClanOfSaar", "Infantry", 4 },
                    { "TheClanOfSaar", "SpaceDock", 1 },
                    { "TheCouncilKeleres", "Carrier", 2 },
                    { "TheCouncilKeleres", "Cruiser", 1 },
                    { "TheCouncilKeleres", "Fighter", 2 },
                    { "TheCouncilKeleres", "Infantry", 2 },
                    { "TheCouncilKeleres", "SpaceDock", 1 },
                    { "TheDihMohnFlotilla", "Destroyer", 2 },
                    { "TheDihMohnFlotilla", "Dreadnought", 2 },
                    { "TheDihMohnFlotilla", "Fighter", 2 },
                    { "TheDihMohnFlotilla", "Infantry", 2 },
                    { "TheDihMohnFlotilla", "Mech", 1 },
                    { "TheDihMohnFlotilla", "SpaceDock", 1 },
                    { "TheEdynMandate", "Carrier", 1 },
                    { "TheEdynMandate", "Destroyer", 2 },
                    { "TheEdynMandate", "Fighter", 4 },
                    { "TheEdynMandate", "Infantry", 3 },
                    { "TheEdynMandate", "Pds", 1 },
                    { "TheEdynMandate", "SpaceDock", 1 },
                    { "TheEmbersOfMuaat", "Fighter", 2 },
                    { "TheEmbersOfMuaat", "Infantry", 4 },
                    { "TheEmbersOfMuaat", "SpaceDock", 1 },
                    { "TheEmbersOfMuaat", "WarSun", 1 },
                    { "TheEmiratesOfHacan", "Carrier", 2 },
                    { "TheEmiratesOfHacan", "Cruiser", 1 },
                    { "TheEmiratesOfHacan", "Fighter", 2 },
                    { "TheEmiratesOfHacan", "Infantry", 4 },
                    { "TheEmiratesOfHacan", "SpaceDock", 1 },
                    { "TheEmpyrean", "Carrier", 2 },
                    { "TheEmpyrean", "Destroyer", 1 },
                    { "TheEmpyrean", "Fighter", 2 },
                    { "TheEmpyrean", "Infantry", 4 },
                    { "TheEmpyrean", "SpaceDock", 1 },
                    { "TheFederationOfSol", "Carrier", 2 },
                    { "TheFederationOfSol", "Destroyer", 1 },
                    { "TheFederationOfSol", "Fighter", 3 },
                    { "TheFederationOfSol", "Infantry", 5 },
                    { "TheFederationOfSol", "SpaceDock", 1 },
                    { "TheFlorzenProfiteers", "Carrier", 2 },
                    { "TheFlorzenProfiteers", "Fighter", 4 },
                    { "TheFlorzenProfiteers", "Infantry", 4 },
                    { "TheFlorzenProfiteers", "SpaceDock", 1 },
                    { "TheFreeSystemsCompact", "Carrier", 1 },
                    { "TheFreeSystemsCompact", "Cruiser", 2 },
                    { "TheFreeSystemsCompact", "Fighter", 2 },
                    { "TheFreeSystemsCompact", "Infantry", 4 },
                    { "TheFreeSystemsCompact", "Pds", 1 },
                    { "TheFreeSystemsCompact", "SpaceDock", 1 },
                    { "TheGheminaRaiders", "Carrier", 2 },
                    { "TheGheminaRaiders", "Destroyer", 1 },
                    { "TheGheminaRaiders", "Fighter", 3 },
                    { "TheGheminaRaiders", "Infantry", 3 },
                    { "TheGheminaRaiders", "SpaceDock", 2 },
                    { "TheGhostsOfCreuss", "Carrier", 1 },
                    { "TheGhostsOfCreuss", "Destroyer", 2 },
                    { "TheGhostsOfCreuss", "Fighter", 2 },
                    { "TheGhostsOfCreuss", "Infantry", 4 },
                    { "TheGhostsOfCreuss", "SpaceDock", 1 },
                    { "TheGhotiWayfarers", "Cruiser", 1 },
                    { "TheGhotiWayfarers", "Fighter", 2 },
                    { "TheGhotiWayfarers", "Flagship", 1 },
                    { "TheGhotiWayfarers", "Infantry", 3 },
                    { "TheGledgeUnion", "Carrier", 1 },
                    { "TheGledgeUnion", "Destroyer", 1 },
                    { "TheGledgeUnion", "Dreadnought", 1 },
                    { "TheGledgeUnion", "Fighter", 3 },
                    { "TheGledgeUnion", "Infantry", 2 },
                    { "TheGledgeUnion", "Mech", 1 },
                    { "TheGledgeUnion", "SpaceDock", 1 },
                    { "TheGlimmerOfMortheus", "Carrier", 1 },
                    { "TheGlimmerOfMortheus", "Cruiser", 1 },
                    { "TheGlimmerOfMortheus", "Dreadnought", 1 },
                    { "TheGlimmerOfMortheus", "Fighter", 2 },
                    { "TheGlimmerOfMortheus", "Infantry", 3 },
                    { "TheGlimmerOfMortheus", "SpaceDock", 1 },
                    { "TheCheiranHordes", "Carrier", 1 },
                    { "TheCheiranHordes", "Destroyer", 1 },
                    { "TheCheiranHordes", "Dreadnought", 1 },
                    { "TheCheiranHordes", "Fighter", 2 },
                    { "TheCheiranHordes", "Infantry", 2 },
                    { "TheCheiranHordes", "Mech", 1 },
                    { "TheCheiranHordes", "SpaceDock", 1 },
                    { "TheKolleccSociety", "Carrier", 2 },
                    { "TheKolleccSociety", "Cruiser", 1 },
                    { "TheKolleccSociety", "Fighter", 2 },
                    { "TheKolleccSociety", "Infantry", 4 },
                    { "TheKolleccSociety", "SpaceDock", 1 },
                    { "TheKortaliTribunal", "Carrier", 2 },
                    { "TheKortaliTribunal", "Cruiser", 1 },
                    { "TheKortaliTribunal", "Fighter", 2 },
                    { "TheKortaliTribunal", "Infantry", 4 },
                    { "TheKortaliTribunal", "Pds", 1 },
                    { "TheKortaliTribunal", "SpaceDock", 1 },
                    { "TheKyroSodality", "Carrier", 1 },
                    { "TheKyroSodality", "Destroyer", 1 },
                    { "TheKyroSodality", "Dreadnought", 1 },
                    { "TheKyroSodality", "Infantry", 3 },
                    { "TheKyroSodality", "SpaceDock", 1 },
                    { "TheL1z1xMindnet", "Carrier", 1 },
                    { "TheL1z1xMindnet", "Dreadnought", 1 },
                    { "TheL1z1xMindnet", "Fighter", 3 },
                    { "TheL1z1xMindnet", "Infantry", 5 },
                    { "TheL1z1xMindnet", "Pds", 1 },
                    { "TheL1z1xMindnet", "SpaceDock", 1 },
                    { "TheLanefirRemnants", "Carrier", 2 },
                    { "TheLanefirRemnants", "Destroyer", 1 },
                    { "TheLanefirRemnants", "Fighter", 2 },
                    { "TheLanefirRemnants", "Infantry", 3 },
                    { "TheLanefirRemnants", "Pds", 1 },
                    { "TheLanefirRemnants", "SpaceDock", 1 },
                    { "TheLiZhoDynasty", "Carrier", 2 },
                    { "TheLiZhoDynasty", "Destroyer", 1 },
                    { "TheLiZhoDynasty", "Fighter", 3 },
                    { "TheLiZhoDynasty", "Infantry", 4 },
                    { "TheLiZhoDynasty", "Pds", 1 },
                    { "TheLiZhoDynasty", "SpaceDock", 1 },
                    { "TheLTokkKhrask", "Cruiser", 3 },
                    { "TheLTokkKhrask", "Fighter", 1 },
                    { "TheLTokkKhrask", "Infantry", 3 },
                    { "TheLTokkKhrask", "Pds", 1 },
                    { "TheLTokkKhrask", "SpaceDock", 1 },
                    { "TheMahactGeneSorcerers", "Carrier", 1 },
                    { "TheMahactGeneSorcerers", "Cruiser", 1 },
                    { "TheMahactGeneSorcerers", "Dreadnought", 1 },
                    { "TheMahactGeneSorcerers", "Fighter", 2 },
                    { "TheMahactGeneSorcerers", "Infantry", 3 },
                    { "TheMahactGeneSorcerers", "SpaceDock", 1 },
                    { "TheMentakCoalition", "Carrier", 1 },
                    { "TheMentakCoalition", "Cruiser", 2 },
                    { "TheMentakCoalition", "Fighter", 3 },
                    { "TheMentakCoalition", "Infantry", 4 },
                    { "TheMentakCoalition", "Pds", 1 },
                    { "TheMentakCoalition", "SpaceDock", 1 },
                    { "TheMirvedaProtectorate", "Carrier", 2 },
                    { "TheMirvedaProtectorate", "Cruiser", 1 },
                    { "TheMirvedaProtectorate", "Fighter", 5 },
                    { "TheMirvedaProtectorate", "Infantry", 2 },
                    { "TheMirvedaProtectorate", "Pds", 1 },
                    { "TheMirvedaProtectorate", "SpaceDock", 1 },
                    { "TheMonksOfKolume", "Carrier", 2 },
                    { "TheMonksOfKolume", "Cruiser", 1 },
                    { "TheMonksOfKolume", "Fighter", 2 },
                    { "TheMonksOfKolume", "Infantry", 4 },
                    { "TheMonksOfKolume", "SpaceDock", 1 },
                    { "TheMykoMentori", "Carrier", 2 },
                    { "TheMykoMentori", "Cruiser", 1 },
                    { "TheMykoMentori", "Fighter", 1 },
                    { "TheMykoMentori", "Infantry", 6 },
                    { "TheMykoMentori", "SpaceDock", 1 },
                    { "TheNaaluCollective", "Carrier", 1 },
                    { "TheNaaluCollective", "Cruiser", 1 },
                    { "TheNaaluCollective", "Destroyer", 1 },
                    { "TheNaaluCollective", "Fighter", 3 },
                    { "TheNaaluCollective", "Infantry", 4 },
                    { "TheNaaluCollective", "Pds", 1 },
                    { "TheNaaluCollective", "SpaceDock", 1 },
                    { "TheNaazRokhaAlliance", "Carrier", 2 },
                    { "TheNaazRokhaAlliance", "Destroyer", 1 },
                    { "TheNaazRokhaAlliance", "Fighter", 2 },
                    { "TheNaazRokhaAlliance", "Infantry", 3 },
                    { "TheNaazRokhaAlliance", "Mech", 1 },
                    { "TheNaazRokhaAlliance", "SpaceDock", 1 },
                    { "TheNekroVirus", "Carrier", 1 },
                    { "TheNekroVirus", "Cruiser", 1 },
                    { "TheNekroVirus", "Dreadnought", 1 },
                    { "TheNekroVirus", "Fighter", 2 },
                    { "TheNekroVirus", "Infantry", 2 },
                    { "TheNekroVirus", "SpaceDock", 1 },
                    { "TheNivynStarKings", "Carrier", 1 },
                    { "TheNivynStarKings", "Cruiser", 1 },
                    { "TheNivynStarKings", "Dreadnought", 1 },
                    { "TheNivynStarKings", "Fighter", 3 },
                    { "TheNivynStarKings", "Infantry", 3 },
                    { "TheNivynStarKings", "Mech", 1 },
                    { "TheNivynStarKings", "SpaceDock", 1 },
                    { "TheNokarSellships", "Carrier", 1 },
                    { "TheNokarSellships", "Cruiser", 1 },
                    { "TheNokarSellships", "Destroyer", 1 },
                    { "TheNokarSellships", "Fighter", 2 },
                    { "TheNokarSellships", "Infantry", 4 },
                    { "TheNokarSellships", "Pds", 1 },
                    { "TheNokarSellships", "SpaceDock", 1 },
                    { "TheNomad", "Carrier", 1 },
                    { "TheNomad", "Destroyer", 1 },
                    { "TheNomad", "Fighter", 3 },
                    { "TheNomad", "Flagship", 1 },
                    { "TheNomad", "Infantry", 4 },
                    { "TheNomad", "SpaceDock", 1 },
                    { "TheOlradinLeague", "Carrier", 2 },
                    { "TheOlradinLeague", "Cruiser", 1 },
                    { "TheOlradinLeague", "Fighter", 3 },
                    { "TheOlradinLeague", "Infantry", 4 },
                    { "TheOlradinLeague", "SpaceDock", 1 },
                    { "TheSavagesOfCymiae", "Carrier", 1 },
                    { "TheSavagesOfCymiae", "Destroyer", 1 },
                    { "TheSavagesOfCymiae", "Dreadnought", 1 },
                    { "TheSavagesOfCymiae", "Fighter", 2 },
                    { "TheSavagesOfCymiae", "Infantry", 3 },
                    { "TheSavagesOfCymiae", "SpaceDock", 1 },
                    { "TheShipwrightsofAxis", "Carrier", 1 },
                    { "TheShipwrightsofAxis", "Destroyer", 1 },
                    { "TheShipwrightsofAxis", "Dreadnought", 1 },
                    { "TheShipwrightsofAxis", "Fighter", 2 },
                    { "TheShipwrightsofAxis", "Infantry", 3 },
                    { "TheShipwrightsofAxis", "SpaceDock", 1 },
                    { "TheTitansOfUl", "Cruiser", 2 },
                    { "TheTitansOfUl", "Dreadnought", 1 },
                    { "TheTitansOfUl", "Fighter", 2 },
                    { "TheTitansOfUl", "Infantry", 3 },
                    { "TheTitansOfUl", "SpaceDock", 1 },
                    { "TheTnelisSyndicate", "Carrier", 1 },
                    { "TheTnelisSyndicate", "Destroyer", 2 },
                    { "TheTnelisSyndicate", "Fighter", 2 },
                    { "TheTnelisSyndicate", "Infantry", 4 },
                    { "TheTnelisSyndicate", "Pds", 1 },
                    { "TheTnelisSyndicate", "SpaceDock", 1 },
                    { "TheUniversitiesOfJolNar", "Carrier", 2 },
                    { "TheUniversitiesOfJolNar", "Dreadnought", 1 },
                    { "TheUniversitiesOfJolNar", "Fighter", 1 },
                    { "TheUniversitiesOfJolNar", "Infantry", 2 },
                    { "TheUniversitiesOfJolNar", "Pds", 2 },
                    { "TheUniversitiesOfJolNar", "SpaceDock", 1 },
                    { "TheVadenBankingClans", "Carrier", 1 },
                    { "TheVadenBankingClans", "Cruiser", 1 },
                    { "TheVadenBankingClans", "Dreadnought", 1 },
                    { "TheVadenBankingClans", "Fighter", 2 },
                    { "TheVadenBankingClans", "Infantry", 3 },
                    { "TheVadenBankingClans", "SpaceDock", 1 },
                    { "TheVaylerianScourge", "Carrier", 1 },
                    { "TheVaylerianScourge", "Cruiser", 1 },
                    { "TheVaylerianScourge", "Destroyer", 1 },
                    { "TheVaylerianScourge", "Fighter", 3 },
                    { "TheVaylerianScourge", "Infantry", 3 },
                    { "TheVaylerianScourge", "Pds", 1 },
                    { "TheVaylerianScourge", "SpaceDock", 1 },
                    { "TheVeldyrSovereignty", "Carrier", 1 },
                    { "TheVeldyrSovereignty", "Destroyer", 1 },
                    { "TheVeldyrSovereignty", "Dreadnought", 1 },
                    { "TheVeldyrSovereignty", "Fighter", 3 },
                    { "TheVeldyrSovereignty", "Infantry", 4 },
                    { "TheVeldyrSovereignty", "Pds", 1 },
                    { "TheVeldyrSovereignty", "SpaceDock", 1 },
                    { "TheVuilRaithCabal", "Carrier", 1 },
                    { "TheVuilRaithCabal", "Cruiser", 1 },
                    { "TheVuilRaithCabal", "Dreadnought", 1 },
                    { "TheVuilRaithCabal", "Fighter", 3 },
                    { "TheVuilRaithCabal", "Infantry", 3 },
                    { "TheVuilRaithCabal", "SpaceDock", 1 },
                    { "TheWinnu", "Carrier", 1 },
                    { "TheWinnu", "Cruiser", 1 },
                    { "TheWinnu", "Fighter", 2 },
                    { "TheWinnu", "Infantry", 2 },
                    { "TheWinnu", "Pds", 1 },
                    { "TheWinnu", "SpaceDock", 1 },
                    { "TheXxchaKingdom", "Carrier", 1 },
                    { "TheXxchaKingdom", "Cruiser", 2 },
                    { "TheXxchaKingdom", "Fighter", 3 },
                    { "TheXxchaKingdom", "Infantry", 4 },
                    { "TheXxchaKingdom", "Pds", 1 },
                    { "TheXxchaKingdom", "SpaceDock", 1 },
                    { "TheYinBrotherhood", "Carrier", 2 },
                    { "TheYinBrotherhood", "Destroyer", 1 },
                    { "TheYinBrotherhood", "Fighter", 4 },
                    { "TheYinBrotherhood", "Infantry", 4 },
                    { "TheYinBrotherhood", "SpaceDock", 1 },
                    { "TheYssarilTribes", "Carrier", 2 },
                    { "TheYssarilTribes", "Cruiser", 1 },
                    { "TheYssarilTribes", "Fighter", 2 },
                    { "TheYssarilTribes", "Infantry", 5 },
                    { "TheYssarilTribes", "Pds", 1 },
                    { "TheYssarilTribes", "SpaceDock", 1 },
                    { "TheZealotsOfRhodun", "Carrier", 1 },
                    { "TheZealotsOfRhodun", "Cruiser", 1 },
                    { "TheZealotsOfRhodun", "Destroyer", 1 },
                    { "TheZealotsOfRhodun", "Fighter", 3 },
                    { "TheZealotsOfRhodun", "Infantry", 4 },
                    { "TheZealotsOfRhodun", "SpaceDock", 1 },
                    { "TheZelianPurifier", "Carrier", 1 },
                    { "TheZelianPurifier", "Destroyer", 1 },
                    { "TheZelianPurifier", "Dreadnought", 1 },
                    { "TheZelianPurifier", "Fighter", 1 },
                    { "TheZelianPurifier", "Infantry", 5 },
                    { "TheZelianPurifier", "Pds", 1 },
                    { "TheZelianPurifier", "SpaceDock", 1 }
                });

            migrationBuilder.InsertData(
                schema: "Galaxy",
                table: "Maps",
                columns: new[] { "Id", "Description", "EventName", "MapArchiveLink", "MapGeneratorLink", "MapTemplate", "Name", "TtsString", "UserId", "UserName" },
                values: new object[,]
                {
                    { 1, "Three-Player galaxy setup map suggestion in Learn to Play by Fantasy Flight Games.", "Learn to Play", "https://ti4ultimate.com/community/maps-archive/map/1", "https://ti4ultimate.com/tools/map-generator?template=ThreePlayersMediumTriangleMap&tiles=NDIgMjQgNDUgMjIgNDQgMTkgMzMgNDcgMjcgMjYgMzAgNDAgMzUgMjMgMzYgMzkgMzggMjUgMCAzNCAwIDAgMCAyOCAwIDIxIDAgMCAwIDUwIDAgMzEgMCAwIDAgMjk=", "ThreePlayersMediumTriangleMap", "Three-Player Galaxy Setup", "42 24 45 22 44 19 33 47 27 26 30 40 35 23 36 39 38 25 0 34 0 0 0 28 0 21 0 0 0 50 0 31 0 0 0 29", "1", "Admin" },
                    { 2, "Four-Player galaxy setup map suggestion in Learn to Play by Fantasy Flight Games.", "Learn to Play", "https://ti4ultimate.com/community/maps-archive/map/2", "https://ti4ultimate.com/tools/map-generator?template=FourPlayersMediumMap&tiles=NDggNDQgMzggNDIgMjggNDUgMjMgMjYgMzIgMzQgNDkgMzMgMzYgMzkgMjcgMzAgNDcgMzUgMjQgNDEgNTAgMzEgMCAyMCAyNSAzNyAwIDIyIDQzIDI5IDQ2IDAgMTkgNDAgMjEgMA==", "FourPlayersMediumMap", "Four-Player Galaxy Setup", "48 44 38 42 28 45 23 26 32 34 49 33 36 39 27 30 47 35 24 41 50 31 0 20 25 37 0 22 43 29 46 0 19 40 21 0", "1", "Admin" },
                    { 3, "Five-Player galaxy setup map suggestion in Learn to Play by Fantasy Flight Games.", "Learn to Play", "https://ti4ultimate.com/community/maps-archive/map/3", "https://ti4ultimate.com/tools/map-generator?template=FivePlayersMediumMap&tiles=NDUgMjIgNDQgMTkgNDIgMjQgMzAgNDAgMzUgMjMgMzYgMzkgMzggMjUgMzMgNDcgMjAgMjYgMjggNDkgMCAzMiA0MyA1MCAwIDMxIDQ4IDAgMzcgMjkgMCAzNCAyNyA0NiAwIDIx", "FivePlayersMediumMap", "Five-Player Galaxy Setup", "45 22 44 19 42 24 30 40 35 23 36 39 38 25 33 47 20 26 28 49 0 32 43 50 0 31 48 0 37 29 0 34 27 46 0 21", "1", "Admin" },
                    { 4, "Six-Player galaxy setup map suggestion in Learn to Play by Fantasy Flight Games.", "Learn to Play", "https://ti4ultimate.com/community/maps-archive/map/4", "https://ti4ultimate.com/tools/map-generator?template=SixPlayersMediumMap&tiles=MjIgNDQgMTkgNDIgMjQgNDUgMzUgMjMgMzYgMzkgMzggMjUgMzMgNDcgMjcgMjYgMzAgNDAgMCAzMiA1MCAwIDMxIDQ4IDAgMzcgMjkgMCAzNCAyMCAwIDQ5IDI4IDAgMjEgNDM=", "SixPlayersMediumMap", "Six-Player Galaxy Setup", "22 44 19 42 24 45 35 23 36 39 38 25 33 47 27 26 30 40 0 32 50 0 31 48 0 37 29 0 34 20 0 49 28 0 21 43", "1", "Admin" },
                    { 5, "Three-Player galaxy setup map suggestion in Prophecy of Kings rulebook by Fantasy Flight Games.", "PoK Rulebook", "https://ti4ultimate.com/community/maps-archive/map/5", "https://ti4ultimate.com/tools/map-generator?template=ThreePlayersMediumTriangleMap&tiles=NDggNzIgNDkgMzEgNDYgNzQgMzYgNDQgNzEgNjIgNzAgNzcgNjkgMjQgMjggNDEgMzUgNjMgMCA0MCAwIDAgMCAyNSAwIDc5IDAgMCAwIDY0IDAgMzkgMCAwIDAgMjY=", "ThreePlayersMediumTriangleMap", "Three-Player Galaxy Setup", "48 72 49 31 46 74 36 44 71 62 70 77 69 24 28 41 35 63 0 40 0 0 0 25 0 79 0 0 0 64 0 39 0 0 0 26", "1", "Admin" },
                    { 6, "Four-Player galaxy setup map suggestion in Prophecy of Kings rulebook by Fantasy Flight Games.", "PoK Rulebook", "https://ti4ultimate.com/community/maps-archive/map/6", "https://ti4ultimate.com/tools/map-generator?template=FourPlayersMediumMap&tiles=MjYgNzYgNjcgNjQgNzAgNDUgNzEgNjYgNjggNjAgMzkgNjkgNDcgNjUgNzggNjIgNzUgNjMgNDIgNDAgNzcgNzMgMCA0OCA3NCA0MSAwIDU5IDUwIDc5IDcyIDAgODAgMjAgNjEgMA==", "FourPlayersMediumMap", "Four-Player Galaxy Setup", "26 76 67 64 70 45 71 66 68 60 39 69 47 65 78 62 75 63 42 40 77 73 0 48 74 41 0 59 50 79 72 0 80 20 61 0", "1", "Admin" },
                    { 7, "Five-Player galaxy setup map suggestion in Prophecy of Kings rulebook by Fantasy Flight Games.", "PoK Rulebook", "https://ti4ultimate.com/community/maps-archive/map/7", "https://ti4ultimate.com/tools/map-generator?template=FivePlayersMediumHyperlineMap&tiles=NzcgNDAgNDEgODVBIDM5IDY1IDY3IDYwIDcwIDY2IDc0IDg4QSA3NSA4N0EgNjggNzIgNzMgNjkgMCA2MiA3NiAwIDc5IDcxIDAgODAgODNBIDg2QSA4NEEgNjMgMCA2NCA3OCAwIDYxIDU5", "FivePlayersMediumHyperlineMap", "Five-Player Galaxy Setup", "77 40 41 85A 39 65 67 60 70 66 74 88A 75 87A 68 72 73 69 0 62 76 0 79 71 0 80 83A 86A 84A 63 0 64 78 0 61 59", "1", "Admin" },
                    { 8, "Six-Player galaxy setup map suggestion in Prophecy of Kings rulebook by Fantasy Flight Games.", "PoK Rulebook", "https://ti4ultimate.com/community/maps-archive/map/8", "https://ti4ultimate.com/tools/map-generator?template=SixPlayersMediumMap&tiles=MjYgNjEgNjQgNTkgNjIgNjMgNzEgNDUgNjkgNDEgNjUgNjggNzIgNzkgNjYgODAgNzUgNjcgMCA3MyA0MCAwIDc0IDQ2IDAgMzcgNDkgMCA2MCA3NyAwIDc2IDQ3IDAgNzAgNzg=", "SixPlayersMediumMap", "Six-Player Galaxy Setup", "26 61 64 59 62 63 71 45 69 41 65 68 72 79 66 80 75 67 0 73 40 0 74 46 0 37 49 0 60 77 0 76 47 0 70 78", "1", "Admin" },
                    { 9, "Seven-Player galaxy setup map suggestion in Prophecy of Kings rulebook by Fantasy Flight Games.", "PoK Rulebook", "https://ti4ultimate.com/community/maps-archive/map/9", "https://ti4ultimate.com/tools/map-generator?template=SevenPlayersLargeWarpMap&tiles=ODVCMCA3MiAyMyA4NEIwIDkwQjAgNzYgNjQgNjUgNzUgNzggNjIgMzcgNzkgNjEgNDMgNzAgMjYgNjcgNzQgODhCMCA0MiAwIDM5IDY4IDAgNDUgODZCMCA3MyA0NyA2NiA2MyA0OCA4M0IyIDQ0IDY5IDcxIDAgNjAgMCAwIDAgMCAwIDAgMCAwIDAgMzUgMCA0MCA0MSAwIDAgMCAwIDI1IDAgMCA1OSA3Nw==", "SevenPlayersLargeWarpMap", "Seven-Player Galaxy Setup", "85B0 72 23 84B0 90B0 76 64 65 75 78 62 37 79 61 43 70 26 67 74 88B0 42 0 39 68 0 45 86B0 73 47 66 63 48 83B2 44 69 71 0 60 0 0 0 0 0 0 0 0 0 35 0 40 41 0 0 0 0 25 0 0 59 77", "1", "Admin" },
                    { 10, "Eight-Player galaxy setup map suggestion in Prophecy of Kings rulebook by Fantasy Flight Games.", "PoK Rulebook", "https://ti4ultimate.com/community/maps-archive/map/10", "https://ti4ultimate.com/tools/map-generator?template=EightPlayersLargeWarpMap&tiles=ODdBMSA5MEIzIDY1IDg4QTIgODlCMCA2NiAxOSAyMSA0NSAzMSA0NiA2NCAyMiA3MyA0NCAzNCA0OSAyNCAyNyA1MCA3MiA0MCA1OSA4NUIyIDYyIDc2IDQxIDM3IDQzIDM1IDQ4IDYzIDgzQjIgNzQgMzYgNzkgMCA0MiAzMCAwIDAgMCAwIDY4IDAgMCA0NyA3MSAwIDM5IDYxIDAgMCAwIDAgNjcgMCAwIDc4IDI4", "EightPlayersLargeWarpMap", "Eight-Player Galaxy Setup", "87A1 90B3 65 88A2 89B0 66 19 21 45 31 46 64 22 73 44 34 49 24 27 50 72 40 59 85B2 62 76 41 37 43 35 48 63 83B2 74 36 79 0 42 30 0 0 0 0 68 0 0 47 71 0 39 61 0 0 0 0 67 0 0 78 28", "1", "Admin" }
                });

            migrationBuilder.InsertData(
                schema: "News",
                table: "NewsArticles",
                columns: new[] { "Id", "Content", "CreatedAt", "Title", "UpdatedAt", "UserId" },
                values: new object[,]
                {
                    { 1, "Welcome to the all-new 'Twilight Imperium: Ultimate' website<br/><br/>As an avid fan and a dedicated developer, I'm developing this website to elevate your Twilight Imperium gaming experience. Here, you'll find a host of new features, game-enhancing tools, and community-driven sections tailor-made for fans, by a fans.<br/><br/>While the site is a cosmic work in progress, I'm committed to constantly evolving it with more functionality and user-friendly enhancements. Your feedback is crucial in this journey! So, if you have any cool ideas, spot pesky bugs, or just want to share your thoughts, reach out through my socials. I'm all ears (and antennas!).<br/><br/>Stay tuned for regular updates on new releases and features. And, most importantly, I hope this site becomes a valuable part of your galactic conquests. May your strategies be sharp, and your gaming sessions epic!<br/><br/>Happy gaming,<br/>Lazik", new DateOnly(2023, 6, 15), "New Era - Twilight Imperium: Ultimate!", new DateOnly(2023, 6, 15), "1" },
                    { 2, "Introducing the Game Section<br/><br/><p>Exciting news for all Twilight Imperium enthusiasts! The first section of our website is now live, dedicated to the mesmerizing universe of Twilight Imperium 4th Edition, the PoK Expansion, and Codices 1-3.</p><p>Here's what you can explore:</p><ul>  <li><strong>Faction Insights:</strong> Dive deep into each faction, understanding their unique strengths and strategies.</li>  <li><strong>Card Compendium:</strong> Scroll through an extensive collection of cards, from political agendas to technological advancements.</li></ul><p>Your feedback is vital for our journey through the stars! If you have suggestions for enhancements, or if you encounter any cosmic bugs, your insights are invaluable. Help us improve by reporting issues directly on our GitHub:</p><p><a href=\"https://github.com/Lazik10/TwilightImperiumUltimate/issues\" style=\"color: yellow\">GitHub Repository</a></p>", new DateOnly(2023, 8, 24), "First Steps - Launching Our First Feature!", new DateOnly(2023, 8, 24), "1" },
                    { 3, "Discover the New 'Tools' Section<br/><br/><p>Great news for Twilight Imperium Commanders! I am thrilled to announce the launch of the 'Tools' section, a brand-new feature designed to enhance your galactic conquests. This section is tailor-made to add a new level of depth and customization to your Twilight Imperium experience.</p><p>Here’s what awaits you in the 'Tools' section:</p><ul>  <li><strong>Faction Color Drafting:</strong> Choose your faction color with new drafting tool, adding a personal touch to your empire.</li>  <li><strong>Faction Drafting:</strong> Select from all 25 factions using intuitive drafting tool, ensuring a balanced and exciting game setup.</li>  <li><strong>Custom Map Generator:</strong> Create unique galactic maps for your games, with varied configurations to keep every session fresh and challenging.</li>  <li><strong>Custom Card Creation:</strong> Unleash your creativity by designing your own custom cards, adding a personalized flair to your game.</li></ul><p>Your strategic insights are invaluable to me! As always if you have any suggestions for improving the 'Tools' section, or if you discover any anomalies in the vast reaches of my digital galaxy, please let me know. Your feedback helps me continuously improve and expand our universe.</p><p>Contribute your ideas and report issues on our GitHub:</p><p><a href=\"https://github.com/Lazik10/TwilightImperiumUltimate/issues\" style=\"color: yellow\">GitHub Repository</a></p>", new DateOnly(2023, 11, 5), "Exciting Update - Introducing the 'Tools' Section!", new DateOnly(2023, 11, 5), "1" },
                    { 4, "Embarking on a New Journey: TI: Ultimate Test Site Goes Live!<br/><br/><p>I am thrilled to announce a giant leap in my developing Twilight Imperium website journey – the launch of the TI: Ultimate test website! This is a pivotal moment, marking the first time my platform is open for interstellar explorers – like you.</p><p><strong>What’s in Store?</strong></p><ul>  <li>Experience the website’s core functionalities in their initial form.</li>  <li>Explore sections that are already operational, offering a glimpse into what the final universe will look like.</li></ul><p>This is just the beginning! The test site is your playground to experiment, explore, and provide feedback. Your insights and suggestions are more than just valuable – they are the fuel that will drive me forward in this cosmic journey.</p><p>As one of my first volunteers, you have the unique opportunity to shape the future of the TI: Ultimate website. Encounter a bug? Have a suggestion? Your feedback is crucial to navigating this new frontier. Let’s create the ultimate Twilight Imperium digital experience together!</p><p>Join me in this adventure! Looking forward to your feedback and participation!</p>", new DateOnly(2023, 11, 14), "First Milestone - TI: Ultimate Test Website Launch!", new DateOnly(2023, 11, 14), "1" },
                    { 5, "Unveiling the Technology Cards Compendium<br/><br/><p>Dear Twilight Imperium Commanders, it's time to unveil our latest addition to the TI: Ultimate website - the comprehensive Technology Cards Compendium! This new section is a treasure trove for players seeking in-depth knowledge and strategies around the game’s technological advancements.</p><ul>  <li><strong>Biotic:</strong> Explore the life-enhancing and ecosystem-related technologies that give your civilization a biological edge.</li>  <li><strong>Propulsion:</strong> Boost your fleets with advanced propulsion technologies, ensuring faster and more efficient interstellar travel.</li>  <li><strong>Cybernetic:</strong> Augment your capabilities with cybernetic enhancements, blending the best of technology and biology.</li>  <li><strong>Warfare:</strong> Gain insights into warfare technologies that can turn the tide of any battle in your favor.</li>  <li><strong>Unit Upgrades:</strong> Upgrade your units with cutting-edge advancements for superior performance in various aspects of the game.</li>  <li><strong>Faction Technologies:</strong> Discover unique technologies specific to each faction, adding a layer of depth and strategy to your gameplay.</li></ul><p>Your experience and feedback are pivotal in this galactic journey. If you have any suggestions or encounter any glitches in the cosmic matrix of my website, your input is invaluable. Together, let's continue enhancing the TI: Ultimate experience for every commander in the galaxy!</p><p>Explore, experiment, and enjoy the new Technology Cards Compendium, and don't forget to share your thoughts and feedback with us.</p><p>Here's to many more technological breakthroughs!</p>", new DateOnly(2023, 11, 19), "Tech Triumph - Launching the Technology Cards Compendium!", new DateOnly(2023, 11, 19), "1" },
                    { 6, "<p>I’m thrilled to announce the integration of a brand-new Cards Compendium into the TI: Ultimate website!</p></br>\r\n                        <p>This latest addition includes all the system tiles and planets.\r\n                        You can now scroll through the entire collection of system tiles and planets, making it easier than ever to strategize and plan your next moves.\r\n                        Whether you're new to the game or a seasoned veteran, this compendium is designed to help you dive deeper into the galaxy.</p></br>\r\n                        <p>Explore the compendium, familiarize yourself with all the system tiles and planets, and get ready to conquer the stars!</p>", new DateOnly(2024, 1, 2), "Explore the New System Tiles and Planets Compendium!", new DateOnly(2024, 1, 2), "1" },
                    { 7, "<p>I’m excited to announce that the TI: Ultimate website now features comprehensive information on all 25 factions from <em>Twilight Imperium</em>!\r\n                        Whether you're exploring the galaxy for the first time or you're a seasoned commander,\r\n                        you can now find detailed information about each faction, including:</p>\r\n                            <ul>\r\n                                <li><strong>Difficulty:</strong> Understand the complexity and challenge level of each faction.</li>\r\n                                <li><strong>Lore:</strong> Dive into the rich backstory and lore behind every faction.</li>\r\n                                <li><strong>Starting Components:</strong> Review the starting resources and units each faction brings to the table.</li>\r\n                                <li><strong>Faction Components:</strong> Get familiar with unique faction-specific components that define their strategy.</li>\r\n                                <li><strong>Abilities:</strong> Learn about the powerful abilities that set each faction apart.</li>\r\n                                <li><strong>FAQ:</strong> Find answers to common questions and clarifications specific to each faction.</li>\r\n                                <li><strong>Leaders:</strong> Discover the leaders that guide each faction and their special traits.</li>\r\n                            </ul>\r\n                        <p>This comprehensive resource is designed to help you master your favorite factions and explore new ones with confidence.\r\n                        I hope these additions enhance your gameplay and provide you with all the information you need to dominate the galaxy.</p>\r\n                        <p>Explore the factions, strategize, and lead your people to victory!</p>", new DateOnly(2024, 5, 7), "All 25 Twilight Imperium Factions Now Available!", new DateOnly(2024, 5, 7), "1" },
                    { 8, "<p>I'm thrilled to announce that we've expanded the TI: Ultimate website with an exciting new addition of 34 homebrew factions\r\n                        from the Discordant Stars community! These fan-created factions bring even more depth and variety to your <em>Twilight Imperium</em> experience.</p>\r\n                        <p>Dive into the creativity of the community and explore these unique factions, each with its own lore, abilities, and strategies. Whether you're\r\n                        looking to challenge yourself with something new or simply curious about what the community has to offer, these homebrew factions are sure to add\r\n                        fresh excitement to your games.</p>\r\n                        <p>Check them out, experiment with their playstyles, and discover how these new factions can reshape the galaxy!</p>\r\n                        <p>As always, your feedback is invaluable. Let us know your thoughts on these new additions and how they integrate into your games.</p>\r\n                        <p>Happy gaming, and may the stars align in your favor!</p>", new DateOnly(2024, 6, 2), "Introducing 34 New Homebrew Factions from Discordant Stars!", new DateOnly(2024, 6, 2), "1" },
                    { 9, "<p>I am excited to introduce a new FAQ section on the TI: Ultimate website, designed to enhance your gaming experience.\r\n                        Now, if you're logged in, you can contribute by adding FAQs to any component or faction in the game. This community-driven\r\n                        feature allows everyone to share knowledge and clarify any uncertainties that might arise during gameplay.</p>\r\n                        <p>The FAQ section also includes a powerful search functionality, making it easier than ever to find answers to your\r\n                        questions. Additionally, you can search through the Rules section, which contains all the latest rules updated\r\n                        with the Living Rule Reference (LRR). To further enrich the experience, the Rules section also features notes provided\r\n                        by other players, offering insights and interpretations that might help you navigate the complexities of the game.</p>\r\n                        <p>We hope this new feature becomes a valuable resource for the entire community. Dive in, explore the FAQs, contribute your own,\r\n                        and make use of the comprehensive rules search to ensure you're always playing at your best!</p>\r\n                        <p>Thank you for being a part of this galactic journey, and we look forward to your continued contributions.</p>\r\n                        <p>Happy gaming, and may your strategies be ever victorious!</p>", new DateOnly(2024, 6, 20), "Announcing the New FAQ and Rules Section with Community Contributions!", new DateOnly(2024, 6, 20), "1" },
                    { 10, "<p>We are thrilled to announce the addition of the Ascendant Sun (Eronous homebrew) System Tiles to the TI: Ultimate website!\r\n                        This new set includes an incredible 127 system tiles that significantly expand the possibilities when creating your game maps.</p>\r\n                        <p>Among these new tiles, you'll find:</p>\r\n                        <ul>\r\n                            <li><strong>Legendary Planets:</strong> New planets that offer unique abilities and strategic advantages.</li>\r\n                            <li><strong>Anomalies:</strong> Fresh anomalies that add complexity and challenge to your gameplay.</li>\r\n                            <li><strong>Wormhole Types:</strong> Innovative wormhole designs that change how you navigate the galaxy.</li>\r\n                            <li><strong>Numerous Planets:</strong> A variety of new planets to explore, each bringing its own flavor and strategic opportunities.</li>\r\n                        </ul>\r\n                        <p>These tiles are designed to further enrich your map creation experience, allowing for even more creative and dynamic setups.\r\n                        Whether you're looking to add new challenges or simply explore different gameplay scenarios, the Ascendant Sun tiles are sure to enhance your games.</p>\r\n                        <p>We hope you enjoy these new additions and look forward to seeing the exciting maps you create!</p>", new DateOnly(2024, 6, 29), "Introducing the Ascendant Sun (Eronous Homebrew) System Tiles!", new DateOnly(2024, 6, 29), "1" },
                    { 11, "<p>Today I would like to inform you about the latest addition to the TI: Ultimate website—the Game Tracker!\r\n                        This powerful new feature is designed to enhance your gaming experience by helping you keep track of various\r\n                        aspects of your <em>Twilight Imperium</em> games in real-time.</p>\r\n                        <p>With the Game Tracker, you can now:</p>\r\n                        <ul>\r\n                            <li><strong>Track Game Length:</strong> Monitor the total duration of your game, from start to finish.</li>\r\n                            <li><strong>Strategy Card Picks:</strong> Keep a record of which strategy cards each player has chosen during each round.</li>\r\n                            <li><strong>Individual Game Time:</strong> Track how much time each player spends on their turns, helping you identify pacing and efficiency.</li>\r\n                            <li><strong>Agenda Phase:</strong> Easily manage and track the progression of the agenda phase, ensuring smooth gameplay.</li>\r\n                            <li><strong>Score Tracking:</strong> Keep an updated tally of each player's score throughout the game.</li>\r\n                            <li><strong>Turn Indicator:</strong> Know exactly whose turn it is at any given moment, reducing confusion and keeping the game flowing.</li>\r\n                        </ul>\r\n                        <p>This feature is designed to streamline your games, making it easier to focus on strategy and enjoy the epic experience that is <em>Twilight Imperium</em>. We hope this tool enhances your gameplay and makes tracking every detail a breeze!</p>\r\n                        <p>As always, your feedback is invaluable—let us know how the Game Tracker is working for you and any additional features you’d like to see.</p>\r\n                        <p>Happy gaming, and may your empire thrive!</p>", new DateOnly(2024, 7, 13), "Introducing the New Game Tracker Feature!", new DateOnly(2024, 7, 13), "1" },
                    { 12, "<p>Dear <em>Twilight Imperium</em> Commanders,</p>\r\n                        <p>It’s hard to believe, but it’s been a year since the first line of code was written for the TI: Ultimate website.\r\n                        Today, I’m thrilled to announce that the site is now fully operational and ready for you to explore! After countless\r\n                        hours of development and testing, the website has been brought to the knowledge of players like you, and now,\r\n                        it's your turn to dive in and test all the features, compendiums, and resources available.</p>\r\n                        <p>This is a significant milestone, and I couldn’t be more excited to share it with the community.\r\n                        You can now explore everything the website has to offer, from the detailed faction and system tile compendiums\r\n                        to the advanced tools like the Game Tracker, Map Generators, and much more.</p>\r\n                        <p>Your feedback is crucial as we continue to improve and expand the site. I invite you to share your thoughts,\r\n                        ideas, and any areas where you think improvements could be made. Together, we can make the TI: Ultimate website an even\r\n                        better resource for all <em>Twilight Imperium</em> players.</p>\r\n                        <p>Thank you for being a part of this journey, and I can’t wait to see how the community helps shape the future of the website.\r\n                        Test, explore, and enjoy—and don’t hesitate to reach out with your feedback!</p>\r\n                        <p>Here’s to another year of galactic conquests and innovations!</p>", new DateOnly(2024, 9, 1), "Celebrating a Major Milestone: One Year of Development!", new DateOnly(2024, 9, 1), "1" }
                });

            migrationBuilder.InsertData(
                schema: "Galaxy",
                table: "Planets",
                columns: new[] { "PlanetName", "GameVersion", "Id", "Influence", "IsLegendary", "PlanetTrait", "Resources", "SystemTileName", "TechnologySkip" },
                values: new object[,]
                {
                    { "Abaddon", "ProphecyOfKings", 92, 0, "false", "Cultural", 1, "Tile75", "None" },
                    { "Abyssus", "DiscordantStars", 116, 2, "false", "None", 4, "Tile1006", "None" },
                    { "Abyz", "BaseGame", 57, 0, "false", "Hazardous", 3, "Tile38", "None" },
                    { "Accoen", "ProphecyOfKings", 80, 3, "false", "Industrial", 2, "Tile69", "None" },
                    { "Adoriah", "AscendantSun", 294, 1, "false", "Cultural", 1, "TileEronous107", "None" },
                    { "Adrian", "AscendantSun", 209, 0, "false", "Hazardous", 1, "TileEronous28", "None" },
                    { "Acheron", "ProphecyOfKings", 62, 0, "false", "None", 4, "Tile54", "None" },
                    { "Akhassi", "AscendantSun", 220, 2, "false", "Hazardous", 0, "TileEronous07", "Warfare" },
                    { "AkoDotFourDotOm", "AscendantSun", 234, 1, "true", "Hazardous", 1, "TileEronous53", "None" },
                    { "Aldra", "DiscordantStars", 141, 3, "false", "None", 2, "Tile1020", "None" },
                    { "Alesna", "DiscordantStars", 143, 0, "false", "None", 2, "Tile1021", "None" },
                    { "AlioPrima", "ProphecyOfKings", 85, 1, "false", "Cultural", 1, "Tile71", "None" },
                    { "Ang", "ProphecyOfKings", 72, 0, "false", "Industrial", 2, "Tile61", "Warfare" },
                    { "Aranndan", "AscendantSun", 284, 3, "false", "Hazardous", 3, "TileEronous100", "None" },
                    { "ArcPrime", "BaseGame", 11, 0, "false", "None", 4, "Tile10", "None" },
                    { "Arcturus", "ProphecyOfKings", 61, 4, "false", "None", 4, "Tile53", "None" },
                    { "Argenum", "AscendantSun", 241, 3, "false", "Hazardous", 3, "TileEronous37", "None" },
                    { "Arche", "DiscordantStars", 114, 2, "false", "None", 2, "Tile1005", "None" },
                    { "ArchonRen", "BaseGame", 19, 3, "false", "None", 2, "Tile14", "None" },
                    { "ArchonRenCouncilOfKeleres", "CodexVigil", 101, 3, "false", "None", 2, "Tile93", "None" },
                    { "ArchonTau", "BaseGame", 20, 1, "false", "None", 1, "Tile14", "None" },
                    { "ArchonTauCouncilOfKeleres", "CodexVigil", 102, 1, "false", "None", 1, "Tile93", "None" },
                    { "ArchonVail", "ProphecyOfKings", 70, 3, "false", "Hazardous", 1, "Tile59", "Propulsion" },
                    { "Arinam", "BaseGame", 55, 2, "false", "Industrial", 1, "Tile37", "None" },
                    { "Arnor", "BaseGame", 53, 1, "false", "Industrial", 2, "Tile36", "None" },
                    { "Arretze", "BaseGame", 23, 0, "false", "None", 2, "Tile16", "None" },
                    { "Ashtroth", "ProphecyOfKings", 94, 0, "false", "Hazardous", 2, "Tile75", "None" },
                    { "Atlas", "ProphecyOfKings", 75, 1, "false", "Hazardous", 3, "Tile64", "None" },
                    { "Auldane", "DiscordantStars", 113, 3, "false", "None", 1, "Tile1004", "None" },
                    { "Avar", "ProphecyOfKings", 69, 1, "false", "None", 1, "Tile58", "None" },
                    { "AvarCouncilOfKeleres", "CodexVigil", 105, 1, "false", "None", 1, "Tile94", "None" },
                    { "Avicenna", "DiscordantStars", 134, 0, "false", "None", 4, "Tile1016", "None" },
                    { "Axis", "DiscordantStars", 152, 0, "false", "None", 5, "Tile1028", "None" },
                    { "AysisRest", "DiscordantStars", 135, 3, "false", "None", 4, "Tile1017", "None" },
                    { "Azle", "DiscordantStars", 144, 0, "false", "None", 2, "Tile1021", "None" },
                    { "Bakal", "ProphecyOfKings", 84, 2, "false", "Industrial", 3, "Tile71", "None" },
                    { "Beata", "DiscordantStars", 142, 1, "false", "None", 2, "Tile1020", "None" },
                    { "Behjan", "AscendantSun", 289, 2, "false", "Cultural", 1, "TileEronous45", "None" },
                    { "Benc", "DiscordantStars", 108, 0, "false", "None", 2, "Tile1002", "None" },
                    { "Bereg", "BaseGame", 51, 1, "false", "Hazardous", 3, "Tile35", "None" },
                    { "Biaheo", "DiscordantStars", 129, 0, "false", "None", 3, "Tile1013", "None" },
                    { "BohlDhur", "DiscordantStars", 140, 4, "false", "None", 3, "Tile1019", "None" },
                    { "Breakpoint", "AscendantSun", 219, 3, "false", "Hazardous", 3, "TileEronous113", "None" },
                    { "Brilenci", "AscendantSun", 245, 1, "false", "Industrial", 1, "TileEronous93", "Cybernetic" },
                    { "Brthkul", "DiscordantStars", 133, 3, "false", "None", 1, "Tile1015", "None" },
                    { "Cahgaris", "AscendantSun", 280, 3, "false", "Hazardous", 1, "TileEronous124", "None" },
                    { "Cantris", "AscendantSun", 278, 4, "true", "Hazardous", 4, "TileEronous64", "None" },
                    { "Casibann", "AscendantSun", 236, 3, "true", "Cultural", 0, "TileEronous55", "None" },
                    { "Cealdri", "ProphecyOfKings", 88, 2, "false", "Cultural", 0, "Tile73", "Cybernetic" },
                    { "Centauri", "BaseGame", 49, 3, "false", "Cultural", 1, "Tile34", "None" },
                    { "Cerberus", "AscendantSun", 272, 1, "true", "Hazardous", 3, "TileEronous58", "None" },
                    { "Cormund", "ProphecyOfKings", 78, 0, "false", "Hazardous", 2, "Tile67", "None" },
                    { "Corneeq", "BaseGame", 47, 2, "false", "Industrial", 1, "Tile33", "None" },
                    { "Creuss", "BaseGame", 59, 2, "false", "None", 4, "Tile51", "None" },
                    { "Cymiae", "DiscordantStars", 151, 1, "false", "None", 3, "Tile1027", "None" },
                    { "Cyrra", "DiscordantStars", 124, 1, "false", "None", 0, "Tile1009", "None" },
                    { "DalBootha", "BaseGame", 45, 2, "false", "Cultural", 0, "Tile32", "None" },
                    { "Darien", "BaseGame", 3, 4, "false", "None", 4, "Tile03", "None" },
                    { "DeathsGate", "AscendantSun", 192, 1, "false", "Hazardous", 0, "TileEronous95", "None" },
                    { "Delmor", "DiscordantStars", 120, 1, "false", "None", 2, "Tile1008", "None" },
                    { "Demis", "DiscordantStars", 106, 2, "false", "None", 2, "Tile1001", "None" },
                    { "Derbrae", "UnchartedSpace", 173, 3, "false", "Cultural", 2, "Tile4262", "None" },
                    { "Detic", "UnchartedSpace", 180, 2, "false", "Cultural", 3, "Tile4266", "None" },
                    { "Discordia", "DiscordantStars", 153, 1, "false", "None", 4, "Tile1029", "None" },
                    { "Dognui", "AscendantSun", 247, 0, "false", "Hazardous", 1, "TileEronous22", "None" },
                    { "Domna", "UnchartedSpace", 188, 1, "true", "Hazardous", 2, "Tile4269", "None" },
                    { "Dorvok", "UnchartedSpace", 172, 2, "false", "Industrial", 1, "Tile4262", "Warfare" },
                    { "Drah", "DiscordantStars", 125, 2, "false", "None", 1, "Tile1010", "None" },
                    { "Druaa", "BaseGame", 10, 1, "false", "None", 3, "Tile09", "None" },
                    { "Dwuuit", "AscendantSun", 217, 1, "false", "Cultural", 1, "TileEronous31", "None" },
                    { "Edyn", "DiscordantStars", 118, 3, "false", "None", 3, "Tile1007", "None" },
                    { "Echo", "UnchartedSpace", 163, 2, "true", "Hazardous", 1, "Tile4254", "None" },
                    { "Ekko", "DiscordantStars", 117, 1, "false", "None", 0, "Tile1007", "None" },
                    { "ElansRest", "AscendantSun", 243, 2, "false", "Industrial", 1, "TileEronous38", "None" },
                    { "Ellas", "DiscordantStars", 146, 3, "false", "None", 3, "Tile1023", "None" },
                    { "ElokNu", "AscendantSun", 201, 1, "false", "Hazardous", 1, "TileEronous50", "None" },
                    { "ElokPhi", "AscendantSun", 200, 1, "false", "Hazardous", 0, "TileEronous50", "None" },
                    { "Elysium", "ProphecyOfKings", 63, 1, "false", "None", 4, "Tile55", "None" },
                    { "Empero", "DiscordantStars", 130, 3, "false", "None", 0, "Tile1013", "None" },
                    { "Erissiha", "AscendantSun", 226, 1, "false", "Hazardous", 1, "TileEronous34", "None" },
                    { "Erodius", "AscendantSun", 202, 1, "false", "Hazardous", 2, "TileEronous92", "None" },
                    { "Eshonia", "AscendantSun", 190, 1, "false", "Hazardous", 2, "TileEronous18", "None" },
                    { "EtirV", "UnchartedSpace", 167, 0, "false", "Hazardous", 4, "Tile4258", "None" },
                    { "Everra", "ProphecyOfKings", 79, 1, "false", "Cultural", 3, "Tile68", "None" },
                    { "Fakrenn", "UnchartedSpace", 169, 2, "false", "Hazardous", 2, "Tile4260", "None" },
                    { "Ferrust", "AscendantSun", 240, 0, "false", "Hazardous", 2, "TileEronous51", "None" },
                    { "Fria", "BaseGame", 58, 0, "false", "Hazardous", 2, "Tile38", "None" },
                    { "Fyrain", "AscendantSun", 248, 0, "false", "Cultural", 3, "TileEronous39", "None" },
                    { "Gen", "DiscordantStars", 160, 0, "false", "None", 2, "Tile1034", "None" },
                    { "GghurnTheta", "DiscordantStars", 115, 1, "false", "None", 2, "Tile1005", "None" },
                    { "Ghanis", "AscendantSun", 288, 0, "false", "Industrial", 2, "TileEronous45", "None" },
                    { "Ghoti", "DiscordantStars", 127, 3, "false", "None", 3, "Tile1011", "None" },
                    { "Gral", "BaseGame", 50, 1, "false", "Industrial", 1, "Tile34", "Propulsion" },
                    { "Grishinu", "AscendantSun", 295, 2, "false", "Cultural", 3, "TileEronous47", "None" },
                    { "Gryenorn", "AscendantSun", 296, 3, "false", "Industrial", 2, "TileEronous47", "None" },
                    { "Grywon", "AscendantSun", 211, 2, "false", "Cultural", 2, "TileEronous112", "None" },
                    { "Gwiyun", "UnchartedSpace", 177, 2, "false", "Hazardous", 2, "Tile4264", "None" },
                    { "Hau", "DiscordantStars", 109, 2, "false", "None", 1, "Tile1002", "None" },
                    { "HellsMaw", "AscendantSun", 191, 0, "false", "Hazardous", 3, "TileEronous95", "None" },
                    { "Hercant", "BaseGame", 24, 1, "false", "None", 1, "Tile16", "None" },
                    { "Hersey", "AscendantSun", 275, 5, "true", "Cultural", 0, "TileEronous61", "None" },
                    { "Heska", "AscendantSun", 228, 0, "false", "Industrial", 2, "TileEronous118", "Cybernetic" },
                    { "Hevahold", "AscendantSun", 231, 0, "false", "Cultural", 2, "TileEronous106", "None" },
                    { "HopesEnd", "ProphecyOfKings", 77, 0, "true", "Hazardous", 3, "Tile66", "None" },
                    { "HranCus", "AscendantSun", 285, 1, "false", "Cultural", 1, "TileEronous44", "None" },
                    { "Hulgade", "DiscordantStars", 111, 0, "false", "None", 1, "Tile1003", "None" },
                    { "Hurigati", "AscendantSun", 238, 1, "false", "Hazardous", 0, "TileEronous51", "Cybernetic" },
                    { "Char", "AscendantSun", 246, 0, "false", "Hazardous", 1, "TileEronous21", "None" },
                    { "Chrion", "DiscordantStars", 107, 3, "false", "None", 2, "Tile1001", "None" },
                    { "Idyn", "DiscordantStars", 123, 0, "false", "None", 1, "Tile1009", "None" },
                    { "IkrusThree", "AscendantSun", 212, 2, "false", "Industrial", 2, "TileEronous29", "None" },
                    { "IlVoshu", "AscendantSun", 203, 1, "false", "Cultural", 2, "TileEronous04", "None" },
                    { "Inan", "UnchartedSpace", 178, 2, "false", "Industrial", 1, "Tile4265", "None" },
                    { "Ixth", "ProphecyOfKings", 60, 5, "false", "None", 3, "Tile52", "None" },
                    { "Iynntani", "AscendantSun", 251, 3, "false", "Industrial", 1, "TileEronous17", "None" },
                    { "JeolIr", "ProphecyOfKings", 81, 3, "false", "Industrial", 2, "Tile69", "None" },
                    { "Jol", "BaseGame", 16, 2, "false", "None", 1, "Tile12", "None" },
                    { "Jord", "BaseGame", 1, 2, "false", "None", 4, "Tile01", "None" },
                    { "Kamdorn", "BaseGame", 25, 1, "false", "None", 0, "Tile16", "None" },
                    { "KanHis", "AscendantSun", 286, 3, "false", "Industrial", 0, "TileEronous44", "None" },
                    { "Kelgate", "AscendantSun", 235, 2, "true", "Industrial", 1, "TileEronous54", "None" },
                    { "Khjan", "AscendantSun", 197, 1, "false", "Hazardous", 2, "TileEronous26", "None" },
                    { "Kjalengard", "DiscordantStars", 110, 2, "false", "None", 3, "Tile1003", "None" },
                    { "KkitaUlin", "AscendantSun", 194, 2, "false", "Cultural", 0, "TileEronous03", "None" },
                    { "Kraag", "ProphecyOfKings", 82, 1, "false", "Hazardous", 2, "Tile70", "None" },
                    { "Kris", "AscendantSun", 232, 1, "false", "Cultural", 0, "TileEronous36", "None" },
                    { "Kroll", "DiscordantStars", 122, 1, "false", "None", 1, "Tile1009", "None" },
                    { "Kyd", "DiscordantStars", 121, 2, "false", "None", 1, "Tile1008", "None" },
                    { "Kyr", "DiscordantStars", 138, 0, "false", "None", 2, "Tile1018", "None" },
                    { "Kytos", "AscendantSun", 252, 1, "false", "Cultural", 2, "TileEronous48", "None" },
                    { "Larred", "UnchartedSpace", 183, 1, "false", "Industrial", 1, "Tile4267", "None" },
                    { "LastStop", "DiscordantStars", 128, 0, "false", "None", 3, "Tile1012", "None" },
                    { "Lazar", "BaseGame", 43, 0, "false", "Industrial", 1, "Tile31", "Cybernetic" },
                    { "Leonelli", "AscendantSun", 249, 1, "false", "Industrial", 2, "TileEronous39", "Cybernetic" },
                    { "Limbo", "AscendantSun", 270, 2, "true", "Hazardous", 0, "TileEronous56", "None" },
                    { "LirtaFour", "BaseGame", 52, 3, "false", "Hazardous", 2, "Tile35", "None" },
                    { "Lisis", "ProphecyOfKings", 86, 2, "false", "Industrial", 2, "Tile72", "None" },
                    { "LisisTwo", "BaseGame", 13, 0, "false", "None", 1, "Tile11", "None" },
                    { "Lliot", "UnchartedSpace", 181, 1, "false", "Cultural", 0, "Tile4266", "None" },
                    { "Lodor", "BaseGame", 34, 1, "false", "Cultural", 3, "Tile26", "None" },
                    { "Lodran", "UnchartedSpace", 171, 2, "false", "Hazardous", 0, "Tile4261", "Cybernetic" },
                    { "Loki", "ProphecyOfKings", 93, 2, "false", "Cultural", 1, "Tile75", "None" },
                    { "Lor", "BaseGame", 54, 2, "false", "Industrial", 1, "Tile36", "None" },
                    { "Louk", "DiscordantStars", 112, 1, "false", "None", 2, "Tile1004", "None" },
                    { "Lunerus", "AscendantSun", 292, 3, "false", "Hazardous", 2, "TileEronous46", "None" },
                    { "Lust", "AscendantSun", 271, 2, "true", "Cultural", 1, "TileEronous57", "None" },
                    { "Maaluuk", "BaseGame", 9, 2, "false", "None", 0, "Tile09", "None" },
                    { "Malbolge", "AscendantSun", 277, 3, "true", "Hazardous", 1, "TileEronous63", "None" },
                    { "Mallice", "ProphecyOfKings", 99, 3, "true", "Cultural", 0, "Tile82B", "None" },
                    { "MalliceInactive", "ProphecyOfKings", 98, 3, "true", "Cultural", 0, "Tile82A", "None" },
                    { "Mandle", "UnchartedSpace", 186, 1, "false", "Industrial", 1, "Tile4268", "None" },
                    { "MaonLor", "AscendantSun", 293, 2, "false", "Industrial", 3, "TileEronous101", "Biotic" },
                    { "Mayris", "AscendantSun", 229, 1, "false", "Industrial", 0, "TileEronous35", "None" },
                    { "Mecantor", "AscendantSun", 254, 1, "false", "Hazardous", 2, "TileEronous23", "None" },
                    { "MecatolRex", "BaseGame", 26, 6, "false", "None", 1, "Tile18", "None" },
                    { "Meer", "BaseGame", 56, 4, "false", "Hazardous", 0, "Tile37", "Warfare" },
                    { "MeharXull", "BaseGame", 32, 3, "false", "Hazardous", 1, "Tile24", "Warfare" },
                    { "MekoTwo", "AscendantSun", 214, 2, "false", "Cultural", 0, "TileEronous06", "Propulsion" },
                    { "Mellon", "BaseGame", 41, 2, "false", "Cultural", 0, "Tile30", "None" },
                    { "Meranna", "AscendantSun", 239, 1, "false", "Cultural", 0, "TileEronous51", "None" },
                    { "Merjae", "AscendantSun", 255, 2, "false", "Industrial", 2, "TileEronous11", "Propulsion" },
                    { "Migyro", "AscendantSun", 257, 2, "false", "Industrial", 1, "TileEronous40", "None" },
                    { "MollPrimus", "BaseGame", 2, 1, "false", "None", 4, "Tile02", "None" },
                    { "MollPrimusCouncilOfKeleres", "CodexVigil", 100, 1, "false", "None", 4, "Tile92", "None" },
                    { "Moln", "UnchartedSpace", 175, 0, "false", "Hazardous", 2, "Tile4263", "Biotic" },
                    { "MordaiTwo", "BaseGame", 8, 0, "false", "None", 4, "Tile08", "None" },
                    { "Mornn", "AscendantSun", 258, 3, "false", "Hazardous", 0, "TileEronous94", "None" },
                    { "MorRock", "AscendantSun", 256, 1, "false", "Cultural", 2, "TileEronous40", "None" },
                    { "Muaat", "BaseGame", 4, 1, "false", "None", 4, "Tile04", "None" },
                    { "Myrwater", "AscendantSun", 208, 3, "false", "Hazardous", 1, "TileEronous05", "Biotic" },
                    { "Naazir", "ProphecyOfKings", 65, 1, "false", "None", 2, "Tile57", "None" },
                    { "Nairb", "UnchartedSpace", 184, 1, "false", "Hazardous", 1, "Tile4267", "None" },
                    { "Nar", "BaseGame", 15, 3, "false", "None", 2, "Tile12", "None" },
                    { "Nestphar", "BaseGame", 5, 2, "false", "None", 3, "Tile05", "None" },
                    { "NewAlbion", "BaseGame", 35, 1, "false", "Industrial", 1, "Tile27", "Biotic" },
                    { "Nix", "AscendantSun", 260, 1, "false", "Industrial", 0, "TileEronous52", "None" },
                    { "Nokk", "DiscordantStars", 148, 1, "false", "None", 1, "Tile1024", "None" },
                    { "Nokturn", "AscendantSun", 259, 2, "false", "Hazardous", 0, "TileEronous52", "Warfare" },
                    { "Norrk", "AscendantSun", 264, 2, "false", "Industrial", 1, "TileEronous12", "None" },
                    { "Norvus", "DiscordantStars", 155, 2, "false", "None", 1, "Tile1030", "None" },
                    { "Ogdun", "DiscordantStars", 132, 0, "false", "None", 2, "Tile1015", "None" },
                    { "Okke", "DiscordantStars", 119, 1, "false", "None", 0, "Tile1007", "None" },
                    { "Orad", "DiscordantStars", 159, 1, "false", "None", 3, "Tile1033", "None" },
                    { "Orchard", "AscendantSun", 225, 3, "false", "Cultural", 2, "TileEronous08", "None" },
                    { "Pax", "DiscordantStars", 137, 2, "false", "None", 1, "Tile1018", "None" },
                    { "Perimeter", "ProphecyOfKings", 71, 1, "false", "Industrial", 2, "Tile60", "None" },
                    { "Perpetual", "AscendantSun", 265, 2, "false", "Hazardous", 0, "TileEronous24", "None" },
                    { "Phylo", "AscendantSun", 279, 2, "false", "Industrial", 2, "TileEronous119", "None" },
                    { "Plutus", "AscendantSun", 273, 0, "true", "Industrial", 1, "TileEronous59", "None" },
                    { "Poh", "DiscordantStars", 158, 0, "false", "None", 2, "Tile1033", "None" },
                    { "Primor", "ProphecyOfKings", 76, 1, "true", "Cultural", 2, "Tile65", "None" },
                    { "Prind", "DiscordantStars", 150, 3, "false", "None", 3, "Tile1026", "None" },
                    { "Prism", "UnchartedSpace", 165, 3, "true", "Industrial", 0, "Tile4256", "None" },
                    { "Prymis", "AscendantSun", 253, 2, "false", "Industrial", 1, "TileEronous48", "None" },
                    { "Qaak", "UnchartedSpace", 182, 1, "false", "Cultural", 1, "Tile4267", "None" },
                    { "Quann", "BaseGame", 33, 1, "false", "Cultural", 2, "Tile25", "None" },
                    { "Qucenn", "BaseGame", 39, 2, "false", "Industrial", 1, "Tile29", "None" },
                    { "Quinarra", "BaseGame", 18, 1, "false", "None", 3, "Tile13", "None" },
                    { "Quwon", "AscendantSun", 230, 2, "false", "Industrial", 1, "TileEronous35", "None" },
                    { "Ragh", "BaseGame", 14, 1, "false", "None", 2, "Tile11", "None" },
                    { "Rarron", "BaseGame", 40, 3, "false", "Cultural", 0, "Tile29", "None" },
                    { "RayonFive", "AscendantSun", 199, 2, "false", "Hazardous", 2, "TileEronous91", "None" },
                    { "Regnem", "UnchartedSpace", 187, 2, "false", "Hazardous", 0, "Tile4268", "None" },
                    { "Renhult", "AscendantSun", 227, 2, "false", "Industrial", 1, "TileEronous34", "None" },
                    { "Resculon", "BaseGame", 48, 0, "false", "Industrial", 2, "Tile33", "None" },
                    { "Retillion", "BaseGame", 21, 3, "false", "None", 2, "Tile15", "None" },
                    { "Rhune", "DiscordantStars", 157, 4, "false", "None", 3, "Tile1032", "None" },
                    { "Rhyah", "AscendantSun", 221, 0, "false", "Cultural", 2, "TileEronous32", "None" },
                    { "RialArchon", "AscendantSun", 189, 1, "false", "Hazardous", 1, "TileEronous01", "Propulsion" },
                    { "RigelOne", "ProphecyOfKings", 97, 1, "false", "Hazardous", 0, "Tile76", "None" },
                    { "RigelThree", "ProphecyOfKings", 96, 1, "false", "Industrial", 1, "Tile76", "Biotic" },
                    { "RigelTwo", "ProphecyOfKings", 95, 2, "false", "Industrial", 1, "Tile76", "None" },
                    { "Rokha", "ProphecyOfKings", 66, 2, "false", "None", 1, "Tile57", "None" },
                    { "RylFang", "AscendantSun", 262, 0, "false", "Cultural", 2, "TileEronous41", "None" },
                    { "Rysaa", "UnchartedSpace", 174, 2, "false", "Industrial", 1, "Tile4263", "Propulsion" },
                    { "Sakulag", "BaseGame", 44, 1, "false", "Hazardous", 2, "Tile31", "None" },
                    { "Salin", "UnchartedSpace", 176, 2, "false", "Hazardous", 1, "Tile4264", "None" },
                    { "Sanctuary", "DiscordantStars", 149, 4, "false", "None", 3, "Tile1025", "None" },
                    { "SanVit", "UnchartedSpace", 170, 1, "false", "Cultural", 3, "Tile4261", "None" },
                    { "Saudor", "BaseGame", 31, 2, "false", "Industrial", 2, "Tile23", "None" },
                    { "Sehnn", "AscendantSun", 207, 2, "false", "Industrial", 1, "TileEronous27", "None" },
                    { "SelenTu", "AscendantSun", 195, 1, "false", "Industrial", 2, "TileEronous25", "None" },
                    { "Semlore", "ProphecyOfKings", 73, 2, "false", "Cultural", 3, "Tile62", "Cybernetic" },
                    { "Sentuim", "AscendantSun", 282, 1, "false", "Cultural", 1, "TileEronous43", "None" },
                    { "Shalloq", "BaseGame", 22, 2, "false", "None", 1, "Tile15", "None" },
                    { "Shigonas", "AscendantSun", 233, 2, "false", "Hazardous", 0, "TileEronous36", "None" },
                    { "ShiHalaum", "DiscordantStars", 145, 0, "false", "None", 4, "Tile1022", "None" },
                    { "Shul", "AscendantSun", 198, 1, "false", "Cultural", 1, "TileEronous26", "Propulsion" },
                    { "Sierpen", "UnchartedSpace", 185, 0, "false", "Cultural", 2, "Tile4268", "None" },
                    { "Sigilus", "AscendantSun", 250, 4, "false", "Hazardous", 0, "TileEronous10", "None" },
                    { "Siig", "ProphecyOfKings", 83, 2, "false", "Hazardous", 0, "Tile70", "None" },
                    { "Silence", "UnchartedSpace", 162, 2, "true", "Industrial", 2, "Tile4253", "None" },
                    { "Sokaris", "AscendantSun", 291, 3, "false", "Cultural", 1, "TileEronous46", "None" },
                    { "SolinUo", "AscendantSun", 196, 3, "false", "Cultural", 1, "TileEronous25", "None" },
                    { "Solitude", "DiscordantStars", 136, 1, "false", "None", 0, "Tile1017", "None" },
                    { "Starpoint", "BaseGame", 36, 1, "false", "Hazardous", 3, "Tile27", "None" },
                    { "StationThreeZeroNine", "AscendantSun", 237, 2, "false", "Industrial", 3, "TileEronous09", "None" },
                    { "Stygain", "AscendantSun", 274, 0, "true", "Hazardous", 3, "TileEronous60", "None" },
                    { "Suprima", "AscendantSun", 193, 1, "false", "Cultural", 2, "TileEronous02", "None" },
                    { "Susuros", "DiscordantStars", 131, 4, "false", "None", 4, "Tile1014", "None" },
                    { "Swog", "UnchartedSpace", 179, 0, "false", "Industrial", 1, "Tile4265", "None" },
                    { "Syvian", "AscendantSun", 263, 3, "false", "Cultural", 0, "TileEronous41", "Biotic" },
                    { "TaalDorn", "AscendantSun", 222, 2, "false", "Industrial", 1, "TileEronous32", "Cybernetic" },
                    { "TarMann", "BaseGame", 30, 1, "false", "Industrial", 1, "Tile22", "Biotic" },
                    { "Tarrock", "UnchartedSpace", 164, 0, "true", "Industrial", 3, "Tile4255", "None" },
                    { "Telahas", "AscendantSun", 210, 0, "false", "Industrial", 2, "TileEronous28", "None" },
                    { "Tequran", "BaseGame", 37, 0, "false", "Hazardous", 2, "Tile28", "None" },
                    { "TethnSekus", "AscendantSun", 215, 1, "false", "Cultural", 1, "TileEronous30", "None" },
                    { "TethnTirs", "AscendantSun", 216, 2, "false", "Hazardous", 2, "TileEronous30", "None" },
                    { "TheDark", "ProphecyOfKings", 64, 4, "false", "None", 3, "Tile56", "None" },
                    { "Thenphase", "AscendantSun", 266, 1, "false", "Hazardous", 2, "TileEronous13", "Biotic" },
                    { "Thibah", "BaseGame", 29, 1, "false", "Industrial", 1, "Tile21", "Propulsion" },
                    { "Tir", "AscendantSun", 213, 2, "false", "Hazardous", 1, "TileEronous29", "None" },
                    { "Torkan", "BaseGame", 38, 3, "false", "Cultural", 0, "Tile28", "None" },
                    { "Trenlak", "BaseGame", 17, 0, "false", "None", 1, "Tile13", "None" },
                    { "Troac", "UnchartedSpace", 166, 4, "false", "Cultural", 0, "Tile4257", "None" },
                    { "Trykk", "DiscordantStars", 126, 1, "false", "None", 2, "Tile1010", "None" },
                    { "Uhott", "AscendantSun", 218, 3, "false", "Industrial", 0, "TileEronous31", "None" },
                    { "UlonGamma", "AscendantSun", 200, 1, "false", "Hazardous", 0, "TileEronous49", "None" },
                    { "UlonRho", "AscendantSun", 201, 1, "false", "Industrial", 1, "TileEronous49", "None" },
                    { "Ultimur", "AscendantSun", 281, 0, "false", "Cultural", 3, "TileEronous125", "None" },
                    { "Vadarian", "DiscordantStars", 154, 0, "false", "None", 3, "Tile1030", "None" },
                    { "Valk", "ProphecyOfKings", 68, 0, "false", "None", 2, "Tile58", "None" },
                    { "ValkCouncilOfKeleres", "CodexVigil", 104, 0, "false", "None", 2, "Tile94", "None" },
                    { "Vaylar", "DiscordantStars", 156, 2, "false", "None", 3, "Tile1031", "None" },
                    { "VefutTwo", "BaseGame", 28, 2, "false", "Hazardous", 2, "Tile20", "None" },
                    { "VegaMajor", "ProphecyOfKings", 90, 1, "false", "Cultural", 2, "Tile74", "None" },
                    { "VegaMinor", "ProphecyOfKings", 91, 2, "false", "Cultural", 1, "Tile74", "Propulsion" },
                    { "Velnor", "ProphecyOfKings", 87, 1, "false", "Industrial", 2, "Tile72", "Warfare" },
                    { "Venhalo", "AscendantSun", 224, 1, "false", "Industrial", 1, "TileEronous33", "None" },
                    { "Vent", "AscendantSun", 267, 2, "false", "Industrial", 2, "TileEronous14", "Cybernetic" },
                    { "Verdis", "AscendantSun", 244, 1, "false", "Industrial", 3, "TileEronous38", "None" },
                    { "Vernium", "AscendantSun", 242, 0, "false", "Hazardous", 1, "TileEronous37", "None" },
                    { "Vess", "DiscordantStars", 139, 1, "false", "None", 0, "Tile1018", "None" },
                    { "Veyhrune", "AscendantSun", 290, 2, "false", "Industrial", 2, "TileEronous16", "Cybernetic" },
                    { "Viliguard", "AscendantSun", 261, 0, "false", "Cultural", 1, "TileEronous52", "None" },
                    { "Violence", "AscendantSun", 276, 1, "true", "Industrial", 3, "TileEronous62", "None" },
                    { "Vioss", "UnchartedSpace", 168, 3, "false", "Cultural", 3, "Tile4259", "None" },
                    { "Volgan", "AscendantSun", 283, 2, "false", "Hazardous", 3, "TileEronous43", "None" },
                    { "Volra", "AscendantSun", 223, 1, "false", "Hazardous", 3, "TileEronous33", "None" },
                    { "Vorhal", "ProphecyOfKings", 74, 2, "false", "Cultural", 0, "Tile63", "Biotic" },
                    { "VygarTwo", "AscendantSun", 268, 0, "false", "Hazardous", 2, "TileEronous42", "None" },
                    { "Vylanua", "AscendantSun", 287, 2, "false", "Hazardous", 0, "TileEronous15", "Warfare" },
                    { "Wellon", "BaseGame", 27, 2, "false", "Industrial", 1, "Tile19", "Cybernetic" },
                    { "Winnu", "BaseGame", 7, 4, "false", "None", 3, "Tile07", "None" },
                    { "WrenTerra", "BaseGame", 12, 1, "false", "None", 2, "Tile10", "None" },
                    { "Xanhact", "ProphecyOfKings", 89, 1, "false", "Hazardous", 0, "Tile73", "None" },
                    { "Xxehan", "BaseGame", 46, 1, "false", "Cultural", 1, "Tile32", "None" },
                    { "Xyon", "AscendantSun", 204, 1, "false", "Hazardous", 2, "TileEronous19", "None" },
                    { "Ylir", "ProphecyOfKings", 67, 2, "false", "None", 0, "Tile58", "None" },
                    { "YlirCouncilOfKeleres", "CodexVigil", 103, 2, "false", "None", 0, "Tile94", "None" },
                    { "Yncranti", "AscendantSun", 269, 2, "false", "Industrial", 1, "TileEronous42", "None" },
                    { "Ynnis", "AscendantSun", 205, 3, "false", "Hazardous", 2, "TileEronous20", "None" },
                    { "Zarr", "DiscordantStars", 147, 1, "false", "None", 2, "Tile1024", "None" },
                    { "Zelian", "DiscordantStars", 161, 3, "false", "None", 3, "Tile1034", "None" },
                    { "ZeroDotZeroDotZeroDot", "BaseGame", 6, 0, "false", "None", 5, "Tile06", "None" },
                    { "Zhgen", "AscendantSun", 206, 2, "false", "Industrial", 0, "TileEronous27", "None" },
                    { "Zohbat", "BaseGame", 42, 1, "false", "Hazardous", 3, "Tile30", "None" }
                });

            migrationBuilder.InsertData(
                schema: "Galaxy",
                table: "SliceDrafts",
                columns: new[] { "Id", "Description", "EventName", "Name", "SliceCount", "SliceDraftArchiveLink", "SliceDraftGeneratorLink", "SliceDraftString", "SliceNames", "UserId", "UserName" },
                values: new object[,]
                {
                    { 1, "Qualifiers for the SCPT 2022 tournament.", "SCPT 2022", "Qualifiers", 6, "https://ti4ultimate.com/community/slices-archive/slice-draft/1", "https://ti4ultimate.com/tools/slice-generator?tiles=MzksMzUsNDEsNjYsNzQsCjQ3LDM3LDc2LDIwLDY4LAoyNiwzMCw1OSw2Nyw0OCwKNjUsMjQsNDksNzksMjgsCjI3LDY5LDc4LDY0LDQ1LAo0MiwyNSwyOSw3Nyw2MiwK", "39,35,41,66,74,\r\n47,37,76,20,68,\r\n26,30,59,67,48,\r\n65,24,49,79,28,\r\n27,69,78,64,45,\r\n42,25,29,77,62,\r\n", "Hope,Mama's Drama,Golden Corral,Antimassachusetts,Tom Hanks,Chili Dogs on the Beach", "1", "Admin" },
                    { 2, "Prelims for the SCPT 2022 tournament.", "SCPT 2022", "Prelims", 7, "https://ti4ultimate.com/community/slices-archive/slice-draft/2", "https://ti4ultimate.com/tools/slice-generator?tiles=NDIsNzUsNzgsNTksMjQsCjQ2LDcxLDYzLDMxLDI2LAozNyw2MCwzOSw1MCw2NywKNjgsNzMsNzksMjAsNjUsCjM0LDc3LDM2LDQxLDY0LAo3Niw2Niw0MCw2Miw0NCwKMjgsMTksMjUsODAsNDcsCg==", "42,75,78,59,24,\r\n46,71,63,31,26,\r\n37,60,39,50,67,\r\n68,73,79,20,65,\r\n34,77,36,41,64,\r\n76,66,40,62,44,\r\n28,19,25,80,47,\r\n", "Soft Launch,Winslayer's Dugout,Custodian's Gambit,Between a Rock and a Slow Place,The Glass House,Grand Opening,Twilight Imperium 4th Edition", "1", "Admin" },
                    { 3, "Semifinals for the SCPT 2022 tournament.", "SCPT 2022", "Semifinals", 6, "https://ti4ultimate.com/community/slices-archive/slice-draft/3", "https://ti4ultimate.com/tools/slice-generator?tiles=NDgsMjUsNzAsNjcsMjgsCjIzLDY0LDMzLDc4LDY1LAoyMSw0MCw2OSw1MCwzOCwKMzUsMzksMzQsNDQsNjEsCjQxLDI2LDMwLDc3LDczLAo3Miw3OSwzMiwyMiw2NiwK", "48,25,70,67,28,\r\n23,64,33,78,65,\r\n21,40,69,50,38,\r\n35,39,34,44,61,\r\n41,26,30,77,73,\r\n72,79,32,22,66,\r\n", "Rift of the Valkyries,Drunken Saudor,Jeol Ir: The Entertainer,Public Domain Smooth,Mellon Tell Lodorture,In the Hall of the Asteroid King", "1", "Admin" },
                    { 4, "Finals for the SCPT 2022 tournament.", "SCPT 2022", "Finals", 6, "https://ti4ultimate.com/community/slices-archive/slice-draft/4", "https://ti4ultimate.com/tools/slice-generator?tiles=NzksMzUsMzQsNjQsNzgsCjI3LDI5LDQ2LDQ0LDI1LAo0MCwzNywzMSw2NSw0MSwKMzksNjYsNjksNDcsNzQsCjQ4LDc2LDY4LDIwLDE5LAo2NywyOCw0OSw3MywyNiwK", "79,35,34,64,78,\r\n27,29,46,44,25,\r\n40,37,31,65,41,\r\n39,66,69,47,74,\r\n48,76,68,20,19,\r\n67,28,49,73,26,\r\n", "Final slice 1,Final slice 2,Final slice 3,Final slice 4,Final slice 5,Final slice 6", "1", "Admin" },
                    { 5, "Qualifiers for the SCPT 2023 tournament.", "SCPT 2023", "Qualifiers", 7, "https://ti4ultimate.com/community/slices-archive/slice-draft/5", "https://ti4ultimate.com/tools/slice-generator?tiles=MzAsNjMsNDYsNjcsNjEsCjM1LDc4LDQyLDI2LDcyLAoyNywyMyw0OCw3OSw2MiwKMjEsNjYsNjksNDAsODAsCjQ1LDc1LDI0LDY0LDUwLAozMSwzNyw0OSwyNSw0MSwKNjUsNDcsNTksMzksMzYsCg==", "30,63,46,67,61,\r\n35,78,42,26,72,\r\n27,23,48,79,62,\r\n21,66,69,40,80,\r\n45,75,24,64,50,\r\n31,37,49,25,41,\r\n65,47,59,39,36,\r\n", "Vorhallywood,Lirta IV: The Voyage Home,Synecdoche New Albion,No Country for Hope's End,Three Little Devils,Gravity's Blindside,More-d'Or", "1", "Admin" },
                    { 6, "Prelims for the SCPT 2023 tournament.", "SCPT 2023", "Prelims", 7, "https://ti4ultimate.com/community/slices-archive/slice-draft/6", "https://ti4ultimate.com/tools/slice-generator?tiles=NjMsNDAsNzIsNDYsNjgsCjM2LDI1LDI0LDUwLDQxLAozOSw2MSw1OSw0Myw3MSwKNDcsNzAsNjUsNDQsMTksCjQ1LDY0LDM0LDYyLDQ5LAo0OCwyMiw2Niw3OSwzMiwKNDIsMjYsNzMsNzgsMjEsCg==", "63,40,72,46,68,\r\n36,25,24,50,41,\r\n39,61,59,43,71,\r\n47,70,65,44,19,\r\n45,64,34,62,49,\r\n48,22,66,79,32,\r\n42,26,73,78,21,\r\n", "Gone Girl,DOOT! DOOT!,It's Finger,It's Pronounced Celery,Big-Lore, Not Four,Ginger As She Goes,It's Pronounced Kay All Dree", "1", "Admin" },
                    { 7, "Semifinals for the SCPT 2023 tournament.", "SCPT 2023", "Semifinals", 7, "https://ti4ultimate.com/community/slices-archive/slice-draft/7", "https://ti4ultimate.com/tools/slice-generator?tiles=NzcsMzQsMjYsNjcsMjcsCjY0LDUwLDI5LDQyLDcyLAozNSw0Nyw3Niw0Myw2NSwKNDUsMjQsMjgsMzgsNDAsCjc1LDU5LDc4LDY2LDM5LAoyNSwzNyw0OSw0MSw3MSwKNDgsNjksNzksMTksMzAsCg==", "77,34,26,67,27,\r\n64,50,29,42,72,\r\n35,47,76,43,65,\r\n45,24,28,38,40,\r\n75,59,78,66,39,\r\n25,37,49,41,71,\r\n48,69,79,19,30,\r\n", "Encounter at Starpoint,We'll Always Have Atlas,The Most of Best Worlds,Shades of Meh...Ar Xull,Hope's End Pursuits,Ba'Kal Good Things,Lickin' Good!", "1", "Admin" },
                    { 8, "Finals for the SCPT 2023 tournament.", "SCPT 2023", "Finals", 7, "https://ti4ultimate.com/community/slices-archive/slice-draft/8", "https://ti4ultimate.com/tools/slice-generator?tiles=NzcsMjIsNTksNjcsNjUsCjQ2LDI1LDI5LDMxLDQ1LAo3OCw2NCw3NCwzNyw0MSwKNjEsNDAsNzAsNjgsMzYsCjczLDM5LDI0LDgwLDcxLAo0MiwyNiw3MiwzNSw0NywKNDksNzksMjgsNjIsNzYsCg==", "77,22,59,67,65,\r\n46,25,29,31,45,\r\n78,64,74,37,41,\r\n61,40,70,68,36,\r\n73,39,24,80,71,\r\n42,26,72,35,47,\r\n49,79,28,62,76,\r\n", "Final Slice 1,Final Slice 2,Final Slice 3,Final Slice 4,Final Slice 5,Final Slice 6,Final Slice 7", "1", "Admin" },
                    { 9, "Qualifiers for the SCPT 2024 tournament.", "SCPT 2024", "Qualifiers", 6, "https://ti4ultimate.com/community/slices-archive/slice-draft/9", "https://ti4ultimate.com/tools/slice-generator?tiles=MzIsNjYsNjgsNjMsMzksCjI2LDc2LDQ5LDE5LDQxLAo2NCwzNSw2NSwyMiw3OSwKNTAsMzcsNDUsNjEsMzYsCjI1LDczLDc4LDU5LDYyLAo3Miw3NSw4MCwyMSw0MCwK", "32,66,68,63,39,\r\n26,76,49,19,41,\r\n64,35,65,22,79,\r\n50,37,45,61,36,\r\n25,73,78,59,62,\r\n72,75,80,21,40,\r\n", "101 Dal Boothas,Will,Tharma & Breg,Yellow Slice Because It Has Two Reds,Give Me Integrated or Give Me Death,Devil Went Down to Velnor", "1", "Admin" },
                    { 10, "Prelims for the SCPT 2024 tournament.", "SCPT 2024", "Prelims", 6, "https://ti4ultimate.com/community/slices-archive/slice-draft/10", "https://ti4ultimate.com/tools/slice-generator?tiles=MzMsNjIsNDEsMjUsMzIsCjQ0LDM2LDE5LDQwLDcyLAo0NSw3MCwzNSw2NCw3OCwKNTAsNzQsNjUsMjYsNjMsCjY5LDIxLDIzLDc5LDQ4LAozOCw1OSw0MiwzOSwyNCwK", "33,62,41,25,32,\r\n44,36,19,40,72,\r\n45,70,35,64,78,\r\n50,74,65,26,63,\r\n69,21,23,79,48,\r\n38,59,42,39,24,\r\n", "Corneeqticut,Lorxembourg,Siigney,Vorhalabama,Düssaudorf,New South Vails", "1", "Admin" },
                    { 11, "Semifinals for the SCPT 2024 tournament.", "SCPT 2024", "Semifinals", 6, "https://ti4ultimate.com/community/slices-archive/slice-draft/11", "https://ti4ultimate.com/tools/slice-generator?tiles=NjgsNDksMzYsMjUsNjMsCjIzLDc5LDc1LDYyLDUwLAo2NCwzMiw0Miw3MCw2NywKMzksNjYsMTksNDgsNzYsCjI2LDc0LDc4LDI0LDQzLAoyMCwyNyw1OSw0MCw0MSwK", "68,49,36,25,63,\r\n23,79,75,62,50,\r\n64,32,42,70,67,\r\n39,66,19,48,76,\r\n26,74,78,24,43,\r\n20,27,59,40,41,\r\n", "<name1>,<name2>,<name3>,<name4>,<name5>,<name6>", "1", "Admin" },
                    { 12, "Finals for the SCPT 2024 tournament.", "SCPT 2024", "Finals", 6, "https://ti4ultimate.com/community/slices-archive/slice-draft/12", "https://ti4ultimate.com/tools/slice-generator?tiles=MzQsMjIsNjcsNzcsNjYsCjQxLDMyLDQ3LDU5LDY5LAozNSwyNSw0NCw3Myw0OSwKNDAsNzUsNDIsMjQsMjYsCjM5LDc2LDYyLDQzLDY0LAoyNyw0MCw3Miw3OSw2NSwK", "34,22,67,77,66,\r\n41,32,47,59,69,\r\n35,25,44,73,49,\r\n40,75,42,24,26,\r\n39,76,62,43,64,\r\n27,40,72,79,65,\r\n", "Slice 1,Slice 2,Slice 3,Slice 4,Slice 5,Slice 6", "1", "Admin" }
                });

            migrationBuilder.InsertData(
                schema: "Galaxy",
                table: "Wormholes",
                columns: new[] { "Id", "GameVersion", "SystemTileName", "WormholeName" },
                values: new object[,]
                {
                    { 1, "BaseGame", "Tile17", "Delta" },
                    { 2, "BaseGame", "Tile25", "Beta" },
                    { 3, "BaseGame", "Tile26", "Alpha" },
                    { 4, "BaseGame", "Tile39", "Alpha" },
                    { 5, "BaseGame", "Tile40", "Beta" },
                    { 6, "BaseGame", "Tile51", "Delta" },
                    { 7, "ProphecyOfKings", "Tile64", "Beta" },
                    { 8, "ProphecyOfKings", "Tile79", "Alpha" },
                    { 9, "ProphecyOfKings", "Tile82A", "Gamma" },
                    { 10, "ProphecyOfKings", "Tile82B", "Alpha" },
                    { 11, "ProphecyOfKings", "Tile82B", "Beta" },
                    { 12, "ProphecyOfKings", "Tile82B", "Gamma" },
                    { 13, "UnchartedSpace", "Tile4260", "Alpha" },
                    { 14, "UnchartedSpace", "Tile4272", "Beta" },
                    { 15, "UnchartedSpace", "Tile4275", "Gamma" },
                    { 16, "UnchartedSpace", "Tile4276", "Alpha" },
                    { 17, "UnchartedSpace", "Tile4276", "Beta" },
                    { 18, "AscendantSun", "TileEronous17", "Beta" },
                    { 19, "AscendantSun", "TileEronous48", "Alpha" },
                    { 20, "AscendantSun", "TileEronous48", "Beta" },
                    { 21, "AscendantSun", "TileEronous81", "Beta" },
                    { 22, "AscendantSun", "TileEronous84", "Alpha" },
                    { 23, "AscendantSun", "TileEronous84", "Beta" },
                    { 24, "AscendantSun", "TileEronous85", "Alpha" },
                    { 25, "AscendantSun", "TileEronous85", "Beta" },
                    { 26, "AscendantSun", "TileEronous87", "Epsilon" },
                    { 27, "AscendantSun", "TileEronous88", "Epsilon" },
                    { 28, "AscendantSun", "TileEronous89", "Epsilon" },
                    { 29, "AscendantSun", "TileEronous90", "Epsilon" },
                    { 30, "AscendantSun", "TileEronous91", "Epsilon" },
                    { 31, "AscendantSun", "TileEronous92", "Epsilon" },
                    { 32, "AscendantSun", "TileEronous93", "Epsilon" },
                    { 33, "AscendantSun", "TileEronous94", "Epsilon" },
                    { 34, "AscendantSun", "TileEronous95", "Epsilon" },
                    { 35, "AscendantSun", "TileEronous96", "Zeta" },
                    { 36, "AscendantSun", "TileEronous97", "Zeta" },
                    { 37, "AscendantSun", "TileEronous98", "Zeta" },
                    { 38, "AscendantSun", "TileEronous99", "Zeta" },
                    { 39, "AscendantSun", "TileEronous100", "Zeta" },
                    { 40, "AscendantSun", "TileEronous101", "Zeta" },
                    { 41, "AscendantSun", "TileEronous102", "Eta" },
                    { 42, "AscendantSun", "TileEronous103", "Eta" },
                    { 43, "AscendantSun", "TileEronous104", "Eta" },
                    { 44, "AscendantSun", "TileEronous105", "Eta" },
                    { 45, "AscendantSun", "TileEronous106", "Eta" },
                    { 46, "AscendantSun", "TileEronous107", "Eta" },
                    { 47, "AscendantSun", "TileEronous108", "Theta" },
                    { 48, "AscendantSun", "TileEronous109", "Theta" },
                    { 49, "AscendantSun", "TileEronous110", "Theta" },
                    { 50, "AscendantSun", "TileEronous111", "Theta" },
                    { 51, "AscendantSun", "TileEronous112", "Theta" },
                    { 52, "AscendantSun", "TileEronous113", "Theta" },
                    { 53, "AscendantSun", "TileEronous114", "Iota" },
                    { 54, "AscendantSun", "TileEronous115", "Iota" },
                    { 55, "AscendantSun", "TileEronous116", "Iota" },
                    { 56, "AscendantSun", "TileEronous117", "Iota" },
                    { 57, "AscendantSun", "TileEronous118", "Iota" },
                    { 58, "AscendantSun", "TileEronous119", "Iota" },
                    { 59, "AscendantSun", "TileEronous120", "Kappa" },
                    { 60, "AscendantSun", "TileEronous121", "Kappa" },
                    { 61, "AscendantSun", "TileEronous122", "Kappa" },
                    { 62, "AscendantSun", "TileEronous123", "Kappa" },
                    { 63, "AscendantSun", "TileEronous124", "Kappa" },
                    { 64, "AscendantSun", "TileEronous125", "Kappa" },
                    { 65, "AscendantSun", "TileEronous126", "Zeta" },
                    { 66, "AscendantSun", "TileEronous126", "Epsilon" },
                    { 67, "AscendantSun", "TileEronous127", "Theta" },
                    { 68, "AscendantSun", "TileEronous127", "Eta" },
                    { 69, "AscendantSun", "TileEronous128", "Iota" },
                    { 70, "AscendantSun", "TileEronous128", "Kappa" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_AspNetRoleClaims_RoleId",
                table: "AspNetRoleClaims",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                table: "AspNetRoles",
                column: "NormalizedName",
                unique: true,
                filter: "[NormalizedName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserClaims_UserId",
                table: "AspNetUserClaims",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserLogins_UserId",
                table: "AspNetUserLogins",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserRoles_RoleId",
                table: "AspNetUserRoles",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                table: "AspNetUsers",
                column: "NormalizedEmail");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_UserName",
                table: "AspNetUsers",
                column: "UserName",
                unique: true,
                filter: "[UserName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                table: "AspNetUsers",
                column: "NormalizedUserName",
                unique: true,
                filter: "[NormalizedUserName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_FactionTechnology_TechnologyName",
                schema: "Relationships",
                table: "FactionTechnology",
                column: "TechnologyName");

            migrationBuilder.CreateIndex(
                name: "IX_FactionUnit_UnitName",
                schema: "Relationships",
                table: "FactionUnit",
                column: "UnitName");

            migrationBuilder.CreateIndex(
                name: "IX_MapRating_MapId_UserId",
                schema: "Relationships",
                table: "MapRating",
                columns: new[] { "MapId", "UserId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_MapRating_UserId",
                schema: "Relationships",
                table: "MapRating",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Maps_Name_EventName",
                schema: "Galaxy",
                table: "Maps",
                columns: new[] { "Name", "EventName" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Maps_UserId",
                schema: "Galaxy",
                table: "Maps",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_NewsArticles_UserId",
                schema: "News",
                table: "NewsArticles",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Planets_SystemTileName",
                schema: "Galaxy",
                table: "Planets",
                column: "SystemTileName");

            migrationBuilder.CreateIndex(
                name: "IX_SliceDraftRating_SliceDraftId_UserId",
                schema: "Relationships",
                table: "SliceDraftRating",
                columns: new[] { "SliceDraftId", "UserId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_SliceDraftRating_UserId",
                schema: "Relationships",
                table: "SliceDraftRating",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_SliceDrafts_Name_EventName",
                schema: "Galaxy",
                table: "SliceDrafts",
                columns: new[] { "Name", "EventName" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_SliceDrafts_UserId",
                schema: "Galaxy",
                table: "SliceDrafts",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Wormholes_SystemTileName",
                schema: "Galaxy",
                table: "Wormholes",
                column: "SystemTileName");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ActionCards",
                schema: "Card");

            migrationBuilder.DropTable(
                name: "AgendaCards",
                schema: "Card");

            migrationBuilder.DropTable(
                name: "AspNetRoleClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserLogins");

            migrationBuilder.DropTable(
                name: "AspNetUserRoles");

            migrationBuilder.DropTable(
                name: "AspNetUserTokens");

            migrationBuilder.DropTable(
                name: "ExplorationCards",
                schema: "Card");

            migrationBuilder.DropTable(
                name: "FactionColorImportances",
                schema: "Faction");

            migrationBuilder.DropTable(
                name: "FactionRoundStatistics",
                schema: "Statistics");

            migrationBuilder.DropTable(
                name: "FactionTechnology",
                schema: "Relationships");

            migrationBuilder.DropTable(
                name: "FactionUnit",
                schema: "Relationships");

            migrationBuilder.DropTable(
                name: "Faq",
                schema: "Rule");

            migrationBuilder.DropTable(
                name: "FrontierCards",
                schema: "Card");

            migrationBuilder.DropTable(
                name: "GameStatistics",
                schema: "Statistics");

            migrationBuilder.DropTable(
                name: "MapRating",
                schema: "Relationships");

            migrationBuilder.DropTable(
                name: "NewsArticles",
                schema: "News");

            migrationBuilder.DropTable(
                name: "ObjectiveCards",
                schema: "Card");

            migrationBuilder.DropTable(
                name: "Planets",
                schema: "Galaxy");

            migrationBuilder.DropTable(
                name: "Players",
                schema: "Player");

            migrationBuilder.DropTable(
                name: "PromissaryNoteCards",
                schema: "Card");

            migrationBuilder.DropTable(
                name: "RelicCards",
                schema: "Card");

            migrationBuilder.DropTable(
                name: "Rules",
                schema: "Rule");

            migrationBuilder.DropTable(
                name: "SliceDraftRating",
                schema: "Relationships");

            migrationBuilder.DropTable(
                name: "StrategyCards",
                schema: "Card");

            migrationBuilder.DropTable(
                name: "Websites",
                schema: "Website");

            migrationBuilder.DropTable(
                name: "WebsiteStatistics",
                schema: "Statistics");

            migrationBuilder.DropTable(
                name: "Wormholes",
                schema: "Galaxy");

            migrationBuilder.DropTable(
                name: "AspNetRoles");

            migrationBuilder.DropTable(
                name: "Technologies",
                schema: "Technology");

            migrationBuilder.DropTable(
                name: "Factions",
                schema: "Faction");

            migrationBuilder.DropTable(
                name: "Units",
                schema: "Unit");

            migrationBuilder.DropTable(
                name: "Maps",
                schema: "Galaxy");

            migrationBuilder.DropTable(
                name: "SliceDrafts",
                schema: "Galaxy");

            migrationBuilder.DropTable(
                name: "SystemTiles",
                schema: "Galaxy");

            migrationBuilder.DropTable(
                name: "AspNetUsers");
        }
    }
}
