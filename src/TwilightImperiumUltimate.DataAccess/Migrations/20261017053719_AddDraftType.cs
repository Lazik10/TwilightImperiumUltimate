using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TwilightImperiumUltimate.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class AddDraftType : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "DraftType",
                schema: "Draft",
                table: "DraftRooms",
                type: "varchar(64)",
                maxLength: 64,
                nullable: false,
                defaultValue: "FactionPick",
                oldClrType: typeof(string),
                oldType: "varchar(64)",
                oldMaxLength: 64);

            migrationBuilder.Sql("""
                UPDATE [Draft].[DraftRooms]
                SET [DraftType] = 'FactionPick'
                WHERE [DraftType] = '';

                IF EXISTS (
                    SELECT 1 FROM [Draft].[FactionPickSettings] AS [settings]
                    CROSS APPLY OPENJSON([settings].[GameVersions]) AS [version]
                    WHERE [version].[type] = 2
                        AND (TRY_CONVERT(int, [version].[value]) IS NULL
                            OR TRY_CONVERT(int, [version].[value]) NOT BETWEEN 0 AND 16))
                    THROW 51000, 'Faction Pick settings contain an unknown game version.', 1;

                UPDATE [settings]
                SET [GameVersions] = (
                    SELECT '[' + STRING_AGG(CONVERT(nvarchar(max),
                        '"' + CASE WHEN [version].[type] = 2 THEN
                            CASE [version].[value]
                                WHEN '0' THEN 'BaseGame'
                                WHEN '1' THEN 'ProphecyOfKings'
                                WHEN '2' THEN 'CodexRecolo'
                                WHEN '3' THEN 'CodexOrdinian'
                                WHEN '4' THEN 'CodexAffinity'
                                WHEN '5' THEN 'CodexVigil'
                                WHEN '6' THEN 'CodexLiberation'
                                WHEN '7' THEN 'ThundersEdge'
                                WHEN '8' THEN 'DiscordantStars'
                                WHEN '9' THEN 'BlueRiverie'
                                WHEN '10' THEN 'UnchartedSpace'
                                WHEN '11' THEN 'AscendantSun'
                                WHEN '12' THEN 'TwilightFall'
                                WHEN '13' THEN 'WhispersFromTheVoid'
                                WHEN '14' THEN 'TtsHyperlines'
                                WHEN '15' THEN 'Deprecated'
                                WHEN '16' THEN 'Custom'
                            END
                        ELSE [version].[value] END + '"'), ',')
                        WITHIN GROUP (ORDER BY CONVERT(int, [version].[key])) + ']'
                    FROM OPENJSON([settings].[GameVersions]) AS [version])
                FROM [Draft].[FactionPickSettings] AS [settings]
                WHERE EXISTS (SELECT 1 FROM OPENJSON([settings].[GameVersions]) WHERE [type] = 2);

                INSERT INTO [Draft].[FactionPickSettings]
                    ([RoomId], [GameVersions], [BansMode], [BansPerPlayer], [HostBanCount],
                     [PickOrderMode], [PicksPerPlayer], [Snake])
                SELECT [Id], '["BaseGame","ProphecyOfKings","ThundersEdge"]', 'None', 1, 0,
                    'Random', 1, 1
                FROM [Draft].[DraftRooms] AS [room]
                WHERE [DraftType] = 'FactionPick'
                    AND NOT EXISTS (
                        SELECT 1 FROM [Draft].[FactionPickSettings] AS [settings]
                        WHERE [settings].[RoomId] = [room].[Id]);
                """);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "DraftType",
                schema: "Draft",
                table: "DraftRooms",
                type: "varchar(64)",
                maxLength: 64,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(64)",
                oldMaxLength: 64,
                oldDefaultValue: "FactionPick");
        }
    }
}
