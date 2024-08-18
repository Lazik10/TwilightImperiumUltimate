using System.Globalization;
using System.Text;

namespace TwilightImperiumUltimate.Web.Services.MapGenerators;

public class MapToStringConverter(
    IMapGeneratorSettingsService mapGeneratorSettingsService,
    IMapGeneratorService mapGeneratorService,
    ILogger<MapToStringConverter> logger)
    : IMapToStringConverter
{
    private readonly IMapGeneratorSettingsService _mapGeneratorSettingsService = mapGeneratorSettingsService;
    private readonly IMapGeneratorService _mapGeneratorService = mapGeneratorService;
    private readonly ILogger<MapToStringConverter> _logger = logger;

    public async Task ConvertBase64StringToMap(MapTemplate mapTemplate, string base64String)
    {
        _mapGeneratorSettingsService.MapTemplate = mapTemplate;

        var bytes = Convert.FromBase64String(base64String);
        var decodedString = Encoding.UTF8.GetString(bytes);
        var systemTileCodes = decodedString.Split(' ').ToList();
        var tiUltimatePositions = TiUltimatePositionsFromTtsPositions(mapTemplate);

        await CreateMapFromTtsString(systemTileCodes, tiUltimatePositions);
    }

    public async Task<string> ConvertMapToBase64String()
    {
        var ttsString = await ConvertMapToTtsString();
        var base64string = Convert.ToBase64String(Encoding.UTF8.GetBytes(ttsString));
        return base64string;
    }

    public Task<string> ConvertMapToTtsString()
    {
        var template = _mapGeneratorSettingsService.MapTemplate;
        var ttsPositions = TiUltimatePositionsFromTtsPositions(template);
        var map = _mapGeneratorService.GeneratedPositionsWithSystemTiles;

        string ttsString = GenerateTtsString(ttsPositions, map);

        return Task.FromResult(ttsString);
    }

    public async Task ConvertTtsStringToMap(MapTemplate mapTemplate, string ttsString)
    {
        var ttsPositions = TiUltimatePositionsFromTtsPositions(mapTemplate);
        var importedTtsSystemTileCodes = ttsString.Split(' ').ToList();

        await CreateMapFromTtsString(importedTtsSystemTileCodes, ttsPositions);
    }

    private static string GenerateTtsString(List<int> ttsPositions, IReadOnlyDictionary<int, SystemTileModel> map)
    {
        var ttsString = new StringBuilder();

        foreach (int position in ttsPositions.Skip(1))
        {
            if (map.TryGetValue(position, out var systemTile) && systemTile is not null)
            {
                if (systemTile.SystemTileCategory is not SystemTileCategory.Hyperlane && (systemTile.SystemTileCategory is SystemTileCategory.None
                    || (systemTile.SystemTileCategory == SystemTileCategory.Green && systemTile.FactionName == FactionName.None)))
                {
                    ttsString.Append('0');
                }
                else
                {
                    ttsString.Append(systemTile.SystemTileCode);
                }
            }
            else if (systemTile is null)
            {
                ttsString.Append('0');
            }

            ttsString.Append(' ');
        }

        return ttsString.ToString().Trim();
    }

    private static List<int> TiUltimatePositionsFromTtsPositions(MapTemplate mapTemplate)
    {
        return mapTemplate switch
        {
            // In my converter mecatol is first, but it should be ignored when creating the map
            // I need to have it so I can place it in the correct position when converting back based on map template
            MapTemplate.ThreePlayersSmallMap
            => new List<int>
            {
                12, 7, 8, 13, 17, 11, 6, 2, 3, 9, 14, 19, 18, 21, 16, 15, 10, 5, 1,
            },

            MapTemplate.SixPlayersMediumMap or
            MapTemplate.SixPlayersMediumSpiralMap or
            MapTemplate.FivePlayersMediumMap or
            MapTemplate.FivePlayersMediumHyperlineMap or
            MapTemplate.FivePlayersMediumDiamondMap or
            MapTemplate.FourPlayersMediumMap or
            MapTemplate.FourPlayersMediumHorizontalMap or
            MapTemplate.FourPlayersMediumVerticalMap or
            MapTemplate.FourPlayersMediumGapsMap or
            MapTemplate.FourPlayersMediumWarpMap or
            MapTemplate.FourPlayersMediumMiniWarpMap or
            MapTemplate.FourPlayersMediumDoubleWarpMap
            => new List<int>
            {
                24, 17, 25, 32, 31, 30, 23, 10, 18, 19, 26, 33, 39, 38, 37, 29, 22, 15,
                16, 3, 11, 12, 20, 27, 34, 41, 40, 46, 45, 44, 36, 35, 28, 21, 14, 8, 9,
            },

            MapTemplate.EightPlayersLargeMap or
            MapTemplate.EightPlayersLargeWarpMap or
            MapTemplate.SevenPlayersLargeHyperlineMap or
            MapTemplate.SevenPlayersLargeWarpMap or
            MapTemplate.SixPlayersLargeMap or
            MapTemplate.FivePlayersLargeFlatMap
            => new List<int>
            {
                40, 31, 32, 41, 49, 39, 30, 22, 23, 33, 42, 51, 50, 58, 48, 47, 38, 29, 21, 13, 14,
                24, 25, 34, 43, 52, 60, 59, 67, 57, 56, 46, 37, 28, 19, 20, 12, 4, 5, 15, 16, 26,
                35, 44, 53, 62, 61, 69, 68, 74, 66, 65, 55, 54, 45, 36, 27, 18, 10, 11, 3,
            },

            _ => new List<int>(),
        };
    }

    private async Task CreateMapFromTtsString(List<string> systemTileCodes, List<int> tiUltimatePositions)
    {
        if (systemTileCodes.Count != tiUltimatePositions.Skip(1).Count())
            return;

        await _mapGeneratorService.GenerateMapAsync(true, default);
        var map = _mapGeneratorService.GeneratedPositionsWithSystemTiles.ToDictionary();
        await _mapGeneratorService.InitializeSystemTilesAsync(default);
        var allSystemTiles = _mapGeneratorService.AllSystemTiles;
        var tiUltimatePositionsWithoutMecatolRex = tiUltimatePositions.Skip(1).ToList();

        var mecatolRex = allSystemTiles
            .First(x => x.SystemTileCategory == SystemTileCategory.MecatolRex);

        var homePlaceholder = allSystemTiles
            .First(x => x.SystemTileCategory == SystemTileCategory.None
            && x.FactionName == FactionName.None
            && x.SystemTileName == SystemTileName.TileHome);

        // First assign Mecatol Rex
        map[tiUltimatePositions[0]] = mecatolRex;

        for (var i = 0; i < tiUltimatePositionsWithoutMecatolRex.Count; i++)
        {
            if (map.TryGetValue(tiUltimatePositionsWithoutMecatolRex[i], out var systemTile))
            {
                _logger.LogInformation(
                    "Handling position: {Position}, searching for system tile with Code: {SystemTileCode}",
                    tiUltimatePositionsWithoutMecatolRex[i],
                    systemTileCodes[i]);

                if (int.TryParse(systemTileCodes[i], out int number))
                {
                    if (number == 0)
                    {
                        map[tiUltimatePositionsWithoutMecatolRex[i]] = homePlaceholder!;
                        map[tiUltimatePositionsWithoutMecatolRex[i]].SystemTileCode = "0";
                    }
                    else
                    {
                        map[tiUltimatePositionsWithoutMecatolRex[i]] = allSystemTiles
                            .FirstOrDefault(x =>
                                x.SystemTileCode == systemTileCodes[i].ToString(CultureInfo.InvariantCulture)) ?? new SystemTileModel() { };
                    }
                }
                else
                {
                    // Need to handle hyperlanes
                    var stringNumber = systemTileCodes[i];
                    _logger.LogInformation("Handling hyperlane: {Hyperlane}", stringNumber);

                    if (stringNumber.Contains('A') || stringNumber.Contains('B'))
                    {
                        int startIndexA = stringNumber.IndexOf('A') + 1;
                        int startIndexB = stringNumber.IndexOf('B') + 1;

                        var test = stringNumber[..startIndexA];
                        var test1 = stringNumber[..startIndexB];

                        var index = startIndexA != 0 ? startIndexA : startIndexB;

                        var hyperlane = allSystemTiles
                            .First(x => x.SystemTileCode == stringNumber[..index]);

                        var rotation = stringNumber[index..];
                        hyperlane.SystemTileCode += rotation;

                        map[tiUltimatePositionsWithoutMecatolRex[i]] = hyperlane;
                    }
                }
            }
        }

        await _mapGeneratorService.InitializeMapFromLink(map);
    }
}


