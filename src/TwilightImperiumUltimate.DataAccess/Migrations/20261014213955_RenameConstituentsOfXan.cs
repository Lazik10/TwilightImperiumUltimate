using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace TwilightImperiumUltimate.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class RenameConstituentsOfXan : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                schema: "Relationships",
                table: "FactionTechnology",
                keyColumns: new[] { "FactionName", "TechnologyName" },
                keyValues: new object[] { "TheConstituantsOfXan", "AIDevelopmentAlgorithm" });

            migrationBuilder.DeleteData(
                schema: "Relationships",
                table: "FactionTechnology",
                keyColumns: new[] { "FactionName", "TechnologyName" },
                keyValues: new object[] { "TheConstituantsOfXan", "SarweenTools" });

            migrationBuilder.DeleteData(
                schema: "Relationships",
                table: "FactionUnit",
                keyColumns: new[] { "FactionName", "UnitName" },
                keyValues: new object[] { "TheConstituantsOfXan", "Carrier" });

            migrationBuilder.DeleteData(
                schema: "Relationships",
                table: "FactionUnit",
                keyColumns: new[] { "FactionName", "UnitName" },
                keyValues: new object[] { "TheConstituantsOfXan", "Destroyer" });

            migrationBuilder.DeleteData(
                schema: "Relationships",
                table: "FactionUnit",
                keyColumns: new[] { "FactionName", "UnitName" },
                keyValues: new object[] { "TheConstituantsOfXan", "Fighter" });

            migrationBuilder.DeleteData(
                schema: "Relationships",
                table: "FactionUnit",
                keyColumns: new[] { "FactionName", "UnitName" },
                keyValues: new object[] { "TheConstituantsOfXan", "Infantry" });

            migrationBuilder.DeleteData(
                schema: "Relationships",
                table: "FactionUnit",
                keyColumns: new[] { "FactionName", "UnitName" },
                keyValues: new object[] { "TheConstituantsOfXan", "SpaceDock" });

            migrationBuilder.DeleteData(
                schema: "Faction",
                table: "Factions",
                keyColumn: "FactionName",
                keyValue: "TheConstituantsOfXan");

            migrationBuilder.InsertData(
                schema: "Tigl",
                table: "Achievements",
                columns: new[] { "Id", "Category", "Faction", "Name" },
                values: new object[] { 44, "Wins", "None", "StayFirm" });

            migrationBuilder.UpdateData(
                schema: "Card",
                table: "BreakthroughCards",
                keyColumn: "Id",
                keyValue: 69,
                column: "FactionName",
                value: "TheConstituentsOfXan");

            migrationBuilder.UpdateData(
                schema: "Faction",
                table: "FactionColorImportances",
                keyColumn: "Id",
                keyValue: 521,
                column: "FactionName",
                value: "TheConstituentsOfXan");

            migrationBuilder.UpdateData(
                schema: "Faction",
                table: "FactionColorImportances",
                keyColumn: "Id",
                keyValue: 522,
                column: "FactionName",
                value: "TheConstituentsOfXan");

            migrationBuilder.UpdateData(
                schema: "Faction",
                table: "FactionColorImportances",
                keyColumn: "Id",
                keyValue: 523,
                column: "FactionName",
                value: "TheConstituentsOfXan");

            migrationBuilder.UpdateData(
                schema: "Faction",
                table: "FactionColorImportances",
                keyColumn: "Id",
                keyValue: 524,
                column: "FactionName",
                value: "TheConstituentsOfXan");

            migrationBuilder.UpdateData(
                schema: "Faction",
                table: "FactionColorImportances",
                keyColumn: "Id",
                keyValue: 525,
                column: "FactionName",
                value: "TheConstituentsOfXan");

            migrationBuilder.UpdateData(
                schema: "Faction",
                table: "FactionColorImportances",
                keyColumn: "Id",
                keyValue: 526,
                column: "FactionName",
                value: "TheConstituentsOfXan");

            migrationBuilder.UpdateData(
                schema: "Faction",
                table: "FactionColorImportances",
                keyColumn: "Id",
                keyValue: 527,
                column: "FactionName",
                value: "TheConstituentsOfXan");

            migrationBuilder.UpdateData(
                schema: "Faction",
                table: "FactionColorImportances",
                keyColumn: "Id",
                keyValue: 528,
                column: "FactionName",
                value: "TheConstituentsOfXan");

            migrationBuilder.InsertData(
                schema: "Faction",
                table: "Factions",
                columns: new[] { "FactionName", "Action", "Commodities", "ComplexityRating", "GameVersion", "History", "HomeSystem", "Id", "Quote", "SystemInfo", "SystemStats" },
                values: new object[] { "TheConstituentsOfXan", "TheConstituentsOfXan_Action", 4, "Medium", "WhispersFromTheVoid", "TheConstituentsOfXan_History", "TileWftV02", 66, "TheConstituentsOfXan_Quote", "TheConstituentsOfXan_SystemInfo", "TheConstituentsOfXan_SystemStats" });

            migrationBuilder.UpdateData(
                schema: "Card",
                table: "FlagshipCards",
                keyColumn: "Id",
                keyValue: 77,
                column: "FactionName",
                value: "TheConstituentsOfXan");

            migrationBuilder.UpdateData(
                schema: "Card",
                table: "PromissaryNoteCards",
                keyColumn: "Id",
                keyValue: 81,
                column: "Faction",
                value: "TheConstituentsOfXan");

            migrationBuilder.UpdateData(
                schema: "Card",
                table: "SpecialComponentCards",
                keyColumn: "Id",
                keyValue: 138,
                column: "FactionName",
                value: "TheConstituentsOfXan");

            migrationBuilder.UpdateData(
                schema: "Card",
                table: "SpecialComponentCards",
                keyColumn: "Id",
                keyValue: 139,
                column: "FactionName",
                value: "TheConstituentsOfXan");

            migrationBuilder.UpdateData(
                schema: "Galaxy",
                table: "SystemTiles",
                keyColumn: "SystemTileName",
                keyValue: "TileWftV02",
                column: "FactionName",
                value: "TheConstituentsOfXan");

            migrationBuilder.UpdateData(
                schema: "Technology",
                table: "Technologies",
                keyColumn: "TechnologyName",
                keyValue: "GalacticCitadelTwo",
                column: "FactionName",
                value: "TheConstituentsOfXan");

            migrationBuilder.UpdateData(
                schema: "Technology",
                table: "Technologies",
                keyColumn: "TechnologyName",
                keyValue: "StellarArchitectTwo",
                column: "FactionName",
                value: "TheConstituentsOfXan");

            migrationBuilder.InsertData(
                schema: "Relationships",
                table: "FactionTechnology",
                columns: new[] { "FactionName", "TechnologyName", "StartingTechnology" },
                values: new object[,]
                {
                    { "TheConstituentsOfXan", "AIDevelopmentAlgorithm", "true" },
                    { "TheConstituentsOfXan", "SarweenTools", "true" }
                });

            migrationBuilder.InsertData(
                schema: "Relationships",
                table: "FactionUnit",
                columns: new[] { "FactionName", "UnitName", "Count" },
                values: new object[,]
                {
                    { "TheConstituentsOfXan", "Carrier", 2 },
                    { "TheConstituentsOfXan", "Destroyer", 2 },
                    { "TheConstituentsOfXan", "Fighter", 2 },
                    { "TheConstituentsOfXan", "Infantry", 4 },
                    { "TheConstituentsOfXan", "SpaceDock", 1 }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                schema: "Tigl",
                table: "Achievements",
                keyColumn: "Id",
                keyValue: 44);

            migrationBuilder.DeleteData(
                schema: "Relationships",
                table: "FactionTechnology",
                keyColumns: new[] { "FactionName", "TechnologyName" },
                keyValues: new object[] { "TheConstituentsOfXan", "AIDevelopmentAlgorithm" });

            migrationBuilder.DeleteData(
                schema: "Relationships",
                table: "FactionTechnology",
                keyColumns: new[] { "FactionName", "TechnologyName" },
                keyValues: new object[] { "TheConstituentsOfXan", "SarweenTools" });

            migrationBuilder.DeleteData(
                schema: "Relationships",
                table: "FactionUnit",
                keyColumns: new[] { "FactionName", "UnitName" },
                keyValues: new object[] { "TheConstituentsOfXan", "Carrier" });

            migrationBuilder.DeleteData(
                schema: "Relationships",
                table: "FactionUnit",
                keyColumns: new[] { "FactionName", "UnitName" },
                keyValues: new object[] { "TheConstituentsOfXan", "Destroyer" });

            migrationBuilder.DeleteData(
                schema: "Relationships",
                table: "FactionUnit",
                keyColumns: new[] { "FactionName", "UnitName" },
                keyValues: new object[] { "TheConstituentsOfXan", "Fighter" });

            migrationBuilder.DeleteData(
                schema: "Relationships",
                table: "FactionUnit",
                keyColumns: new[] { "FactionName", "UnitName" },
                keyValues: new object[] { "TheConstituentsOfXan", "Infantry" });

            migrationBuilder.DeleteData(
                schema: "Relationships",
                table: "FactionUnit",
                keyColumns: new[] { "FactionName", "UnitName" },
                keyValues: new object[] { "TheConstituentsOfXan", "SpaceDock" });

            migrationBuilder.DeleteData(
                schema: "Faction",
                table: "Factions",
                keyColumn: "FactionName",
                keyValue: "TheConstituentsOfXan");

            migrationBuilder.UpdateData(
                schema: "Card",
                table: "BreakthroughCards",
                keyColumn: "Id",
                keyValue: 69,
                column: "FactionName",
                value: "TheConstituantsOfXan");

            migrationBuilder.UpdateData(
                schema: "Faction",
                table: "FactionColorImportances",
                keyColumn: "Id",
                keyValue: 521,
                column: "FactionName",
                value: "TheConstituantsOfXan");

            migrationBuilder.UpdateData(
                schema: "Faction",
                table: "FactionColorImportances",
                keyColumn: "Id",
                keyValue: 522,
                column: "FactionName",
                value: "TheConstituantsOfXan");

            migrationBuilder.UpdateData(
                schema: "Faction",
                table: "FactionColorImportances",
                keyColumn: "Id",
                keyValue: 523,
                column: "FactionName",
                value: "TheConstituantsOfXan");

            migrationBuilder.UpdateData(
                schema: "Faction",
                table: "FactionColorImportances",
                keyColumn: "Id",
                keyValue: 524,
                column: "FactionName",
                value: "TheConstituantsOfXan");

            migrationBuilder.UpdateData(
                schema: "Faction",
                table: "FactionColorImportances",
                keyColumn: "Id",
                keyValue: 525,
                column: "FactionName",
                value: "TheConstituantsOfXan");

            migrationBuilder.UpdateData(
                schema: "Faction",
                table: "FactionColorImportances",
                keyColumn: "Id",
                keyValue: 526,
                column: "FactionName",
                value: "TheConstituantsOfXan");

            migrationBuilder.UpdateData(
                schema: "Faction",
                table: "FactionColorImportances",
                keyColumn: "Id",
                keyValue: 527,
                column: "FactionName",
                value: "TheConstituantsOfXan");

            migrationBuilder.UpdateData(
                schema: "Faction",
                table: "FactionColorImportances",
                keyColumn: "Id",
                keyValue: 528,
                column: "FactionName",
                value: "TheConstituantsOfXan");

            migrationBuilder.InsertData(
                schema: "Faction",
                table: "Factions",
                columns: new[] { "FactionName", "Action", "Commodities", "ComplexityRating", "GameVersion", "History", "HomeSystem", "Id", "Quote", "SystemInfo", "SystemStats" },
                values: new object[] { "TheConstituantsOfXan", "TheConstituantsOfXan_Action", 4, "Medium", "WhispersFromTheVoid", "TheConstituantsOfXan_History", "TileWftV02", 66, "TheConstituantsOfXan_Quote", "TheConstituantsOfXan_SystemInfo", "TheConstituantsOfXan_SystemStats" });

            migrationBuilder.UpdateData(
                schema: "Card",
                table: "FlagshipCards",
                keyColumn: "Id",
                keyValue: 77,
                column: "FactionName",
                value: "TheConstituantsOfXan");

            migrationBuilder.UpdateData(
                schema: "Card",
                table: "PromissaryNoteCards",
                keyColumn: "Id",
                keyValue: 81,
                column: "Faction",
                value: "TheConstituantsOfXan");

            migrationBuilder.UpdateData(
                schema: "Card",
                table: "SpecialComponentCards",
                keyColumn: "Id",
                keyValue: 138,
                column: "FactionName",
                value: "TheConstituantsOfXan");

            migrationBuilder.UpdateData(
                schema: "Card",
                table: "SpecialComponentCards",
                keyColumn: "Id",
                keyValue: 139,
                column: "FactionName",
                value: "TheConstituantsOfXan");

            migrationBuilder.UpdateData(
                schema: "Galaxy",
                table: "SystemTiles",
                keyColumn: "SystemTileName",
                keyValue: "TileWftV02",
                column: "FactionName",
                value: "TheConstituantsOfXan");

            migrationBuilder.UpdateData(
                schema: "Technology",
                table: "Technologies",
                keyColumn: "TechnologyName",
                keyValue: "GalacticCitadelTwo",
                column: "FactionName",
                value: "TheConstituantsOfXan");

            migrationBuilder.UpdateData(
                schema: "Technology",
                table: "Technologies",
                keyColumn: "TechnologyName",
                keyValue: "StellarArchitectTwo",
                column: "FactionName",
                value: "TheConstituantsOfXan");

            migrationBuilder.InsertData(
                schema: "Relationships",
                table: "FactionTechnology",
                columns: new[] { "FactionName", "TechnologyName", "StartingTechnology" },
                values: new object[,]
                {
                    { "TheConstituantsOfXan", "AIDevelopmentAlgorithm", "true" },
                    { "TheConstituantsOfXan", "SarweenTools", "true" }
                });

            migrationBuilder.InsertData(
                schema: "Relationships",
                table: "FactionUnit",
                columns: new[] { "FactionName", "UnitName", "Count" },
                values: new object[,]
                {
                    { "TheConstituantsOfXan", "Carrier", 2 },
                    { "TheConstituantsOfXan", "Destroyer", 2 },
                    { "TheConstituantsOfXan", "Fighter", 2 },
                    { "TheConstituantsOfXan", "Infantry", 4 },
                    { "TheConstituantsOfXan", "SpaceDock", 1 }
                });
        }
    }
}
