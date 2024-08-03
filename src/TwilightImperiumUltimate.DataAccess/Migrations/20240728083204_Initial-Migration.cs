using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

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
                name: "Relationships");

            migrationBuilder.EnsureSchema(
                name: "Galaxy");

            migrationBuilder.EnsureSchema(
                name: "News");

            migrationBuilder.EnsureSchema(
                name: "Player");

            migrationBuilder.EnsureSchema(
                name: "Rule");

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
                name: "MapRedPositions",
                schema: "Galaxy",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MapTemplate = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false),
                    Count = table.Column<int>(type: "integer", nullable: false),
                    Positions = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MapRedPositions", x => x.Id)
                        .Annotation("SqlServer:Clustered", true);
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
                name: "FactionTechnology",
                schema: "Relationships");

            migrationBuilder.DropTable(
                name: "FactionUnit",
                schema: "Relationships");

            migrationBuilder.DropTable(
                name: "FrontierCards",
                schema: "Card");

            migrationBuilder.DropTable(
                name: "MapRedPositions",
                schema: "Galaxy");

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
                name: "StrategyCards",
                schema: "Card");

            migrationBuilder.DropTable(
                name: "Websites",
                schema: "Website");

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
                name: "AspNetUsers");

            migrationBuilder.DropTable(
                name: "SystemTiles",
                schema: "Galaxy");
        }
    }
}
