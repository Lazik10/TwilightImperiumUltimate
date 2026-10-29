using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TwilightImperiumUltimate.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class RenameBlueReverie : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            UpdateGameVersion(migrationBuilder, "BlueRiverie", "BlueReverie");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            UpdateGameVersion(migrationBuilder, "BlueReverie", "BlueRiverie");
        }

        private static void UpdateGameVersion(MigrationBuilder migrationBuilder, string currentValue, string replacementValue)
        {
            ArgumentNullException.ThrowIfNull(migrationBuilder);

            var tables = new[]
            {
                (Schema: "Card", Table: "BreakthroughCards"),
                (Schema: "Faction", Table: "Factions"),
                (Schema: "Card", Table: "FlagshipCards"),
                (Schema: "Galaxy", Table: "Planets"),
                (Schema: "Card", Table: "PromissaryNoteCards"),
                (Schema: "Card", Table: "SpecialComponentCards"),
                (Schema: "Galaxy", Table: "SystemTiles"),
                (Schema: "Technology", Table: "Technologies"),
            };

            foreach (var (schema, table) in tables)
            {
                migrationBuilder.Sql($"UPDATE [{schema}].[{table}] SET [GameVersion] = N'{replacementValue}' WHERE [GameVersion] = N'{currentValue}';");
            }
        }
    }
}