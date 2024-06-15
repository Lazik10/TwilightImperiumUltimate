using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TwilightImperiumUltimate.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
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
                name: "News");

            migrationBuilder.EnsureSchema(
                name: "Galaxy");

            migrationBuilder.EnsureSchema(
                name: "Player");

            migrationBuilder.EnsureSchema(
                name: "Rule");

            migrationBuilder.EnsureSchema(
                name: "Technology");

            migrationBuilder.EnsureSchema(
                name: "Unit");

            migrationBuilder.EnsureSchema(
                name: "User");

            migrationBuilder.CreateTable(
                name: "ActionCards",
                schema: "Card",
                columns: table => new
                {
                    ActionCardName = table.Column<int>(type: "integer", nullable: false),
                    ActionCardWindow = table.Column<int>(type: "integer", nullable: false),
                    Id = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Text = table.Column<string>(type: "text", nullable: false),
                    Type = table.Column<int>(type: "integer", nullable: false),
                    ImagePath = table.Column<string>(type: "text", nullable: false),
                    GameVersion = table.Column<int>(type: "integer", nullable: false),
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ActionCards", x => x.ActionCardName);
                });

            migrationBuilder.CreateTable(
                name: "AgendaCards",
                schema: "Card",
                columns: table => new
                {
                    AgendaCardName = table.Column<int>(type: "integer", nullable: false),
                    AgendaCardType = table.Column<int>(type: "integer", nullable: false),
                    Id = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Text = table.Column<string>(type: "text", nullable: false),
                    Type = table.Column<int>(type: "integer", nullable: false),
                    ImagePath = table.Column<string>(type: "text", nullable: false),
                    GameVersion = table.Column<int>(type: "integer", nullable: false),
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AgendaCards", x => x.AgendaCardName);
                });

            migrationBuilder.CreateTable(
                name: "ExplorationCards",
                schema: "Card",
                columns: table => new
                {
                    ExplorationCardName = table.Column<int>(type: "integer", nullable: false),
                    ExplorationPlanetTrait = table.Column<int>(type: "integer", nullable: false),
                    Id = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Text = table.Column<string>(type: "text", nullable: false),
                    Type = table.Column<int>(type: "integer", nullable: false),
                    ImagePath = table.Column<string>(type: "text", nullable: false),
                    GameVersion = table.Column<int>(type: "integer", nullable: false),
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExplorationCards", x => x.ExplorationCardName);
                });

            migrationBuilder.CreateTable(
                name: "FactionColorImportance",
                schema: "Faction",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FactionName = table.Column<int>(type: "integer", nullable: false),
                    Color = table.Column<int>(type: "integer", nullable: false),
                    Importance = table.Column<int>(type: "int", nullable: false),
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FactionColorImportance", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Factions",
                schema: "Faction",
                columns: table => new
                {
                    FactionName = table.Column<int>(type: "integer", nullable: false),
                    Id = table.Column<int>(type: "int", nullable: false),
                    HomeSystem = table.Column<int>(type: "int", nullable: false),
                    Commodities = table.Column<int>(type: "int", nullable: false),
                    ComplexityRating = table.Column<int>(type: "int", nullable: false),
                    Action = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PromissaryNote = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    History = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Quote = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SystemStats = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SystemInfo = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    GameVersion = table.Column<int>(type: "int", nullable: false),
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
                    FrontierCardName = table.Column<int>(type: "integer", nullable: false),
                    Id = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Text = table.Column<string>(type: "text", nullable: false),
                    Type = table.Column<int>(type: "integer", nullable: false),
                    ImagePath = table.Column<string>(type: "text", nullable: false),
                    GameVersion = table.Column<int>(type: "integer", nullable: false),
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FrontierCards", x => x.FrontierCardName);
                });

            migrationBuilder.CreateTable(
                name: "ObjectiveCards",
                schema: "Card",
                columns: table => new
                {
                    ObjectiveCardName = table.Column<int>(type: "integer", nullable: false),
                    ObjectiveCardType = table.Column<int>(type: "integer", nullable: false),
                    Id = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Text = table.Column<string>(type: "text", nullable: false),
                    Type = table.Column<int>(type: "integer", nullable: false),
                    ImagePath = table.Column<string>(type: "text", nullable: false),
                    GameVersion = table.Column<int>(type: "integer", nullable: false),
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ObjectiveCards", x => x.ObjectiveCardName);
                });

            migrationBuilder.CreateTable(
                name: "Players",
                schema: "Player",
                columns: table => new
                {
                    Name = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    Id = table.Column<int>(type: "int", nullable: false),
                    Color = table.Column<int>(type: "integer", nullable: false),
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
                    PromissaryNoteCardName = table.Column<int>(type: "integer", nullable: false),
                    Faction = table.Column<int>(type: "int", nullable: false),
                    Id = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Text = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Type = table.Column<int>(type: "int", nullable: false),
                    ImagePath = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    GameVersion = table.Column<int>(type: "int", nullable: false),
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PromissaryNoteCards", x => x.PromissaryNoteCardName);
                });

            migrationBuilder.CreateTable(
                name: "RelicCards",
                schema: "Card",
                columns: table => new
                {
                    RelicCardName = table.Column<int>(type: "integer", nullable: false),
                    Id = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Text = table.Column<string>(type: "text", nullable: false),
                    Type = table.Column<int>(type: "integer", nullable: false),
                    ImagePath = table.Column<string>(type: "text", nullable: false),
                    GameVersion = table.Column<int>(type: "integer", nullable: false),
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RelicCards", x => x.RelicCardName);
                });

            migrationBuilder.CreateTable(
                name: "Rules",
                schema: "Rule",
                columns: table => new
                {
                    RuleCategory = table.Column<int>(type: "integer", nullable: false),
                    Id = table.Column<int>(type: "int", nullable: false),
                    Content = table.Column<string>(type: "nvarchar(MAX)", nullable: false),
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
                    StrategyCardName = table.Column<int>(type: "integer", nullable: false),
                    InitiativeOrder = table.Column<int>(type: "integer", nullable: false),
                    PrimaryAbilityText = table.Column<string>(type: "text", nullable: false),
                    SecondaryAbilityText = table.Column<string>(type: "text", nullable: false),
                    Id = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Text = table.Column<string>(type: "text", nullable: false),
                    Type = table.Column<int>(type: "integer", nullable: false),
                    ImagePath = table.Column<string>(type: "text", nullable: false),
                    GameVersion = table.Column<int>(type: "integer", nullable: false),
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StrategyCards", x => x.StrategyCardName);
                });

            migrationBuilder.CreateTable(
                name: "SystemTiles",
                schema: "Galaxy",
                columns: table => new
                {
                    SystemTileName = table.Column<int>(type: "integer", nullable: false),
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TileCategory = table.Column<int>(type: "integer", nullable: false),
                    FactionName = table.Column<int>(type: "integer", nullable: false),
                    Anomaly = table.Column<int>(type: "integer", nullable: false),
                    GameVersion = table.Column<int>(type: "integer", nullable: false),
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
                    TechnologyName = table.Column<int>(type: "integer", nullable: false),
                    Id = table.Column<int>(type: "int", nullable: false),
                    Type = table.Column<int>(type: "integer", nullable: false),
                    Level = table.Column<int>(type: "integer", nullable: false),
                    IsFactionTechnology = table.Column<bool>(type: "bit", nullable: false),
                    FactionName = table.Column<int>(type: "integer", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Text = table.Column<string>(type: "nvarchar(50)", nullable: false),
                    ImagePath = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    GameVersion = table.Column<int>(type: "integer", nullable: false),
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
                    UnitName = table.Column<int>(type: "integer", nullable: false),
                    Id = table.Column<int>(type: "int", nullable: false),
                    UnitType = table.Column<int>(type: "integer", nullable: false),
                    Cost = table.Column<int>(type: "integer", nullable: false),
                    Combat = table.Column<int>(type: "integer", nullable: false),
                    Move = table.Column<int>(type: "integer", nullable: false),
                    Capacity = table.Column<int>(type: "integer", nullable: false),
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Units", x => x.UnitName);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                schema: "User",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nickname = table.Column<string>(type: "nvarchar(30)", nullable: false),
                    FirstName = table.Column<string>(type: "nvarchar(50)", nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(50)", nullable: false),
                    Age = table.Column<int>(type: "integer", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(30)", nullable: false),
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id)
                        .Annotation("SqlServer:Clustered", true);
                });

            migrationBuilder.CreateTable(
                name: "Planets",
                schema: "Galaxy",
                columns: table => new
                {
                    PlanetName = table.Column<int>(type: "integer", nullable: false),
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Resources = table.Column<int>(type: "integer", nullable: false),
                    Influence = table.Column<int>(type: "integer", nullable: false),
                    IsLegendary = table.Column<int>(type: "integer", nullable: false),
                    TechnologySkip = table.Column<int>(type: "integer", nullable: false),
                    PlanetTrait = table.Column<int>(type: "integer", nullable: false),
                    SystemTileName = table.Column<int>(type: "integer", nullable: false),
                    GameVersion = table.Column<int>(type: "integer", nullable: false),
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
                    WormholeName = table.Column<int>(type: "int", nullable: false),
                    SystemTileName = table.Column<int>(type: "int", nullable: false),
                    GameVersion = table.Column<int>(type: "int", nullable: false),
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
                    FactionName = table.Column<int>(type: "integer", nullable: false),
                    TechnologyName = table.Column<int>(type: "integer", nullable: false),
                    StartingTechnology = table.Column<int>(type: "integer", nullable: false),
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
                    UnitName = table.Column<int>(type: "integer", nullable: false),
                    FactionName = table.Column<int>(type: "integer", nullable: false),
                    Count = table.Column<int>(type: "int", nullable: false),
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
                    UserId = table.Column<int>(type: "integer", nullable: false),
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NewsArticles", x => x.Id)
                        .Annotation("SqlServer:Clustered", true);
                    table.ForeignKey(
                        name: "FK_NewsArticles_Users_UserId",
                        column: x => x.UserId,
                        principalSchema: "User",
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

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
                name: "ExplorationCards",
                schema: "Card");

            migrationBuilder.DropTable(
                name: "FactionColorImportance",
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
                name: "Wormholes",
                schema: "Galaxy");

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
                name: "Users",
                schema: "User");

            migrationBuilder.DropTable(
                name: "SystemTiles",
                schema: "Galaxy");
        }
    }
}
