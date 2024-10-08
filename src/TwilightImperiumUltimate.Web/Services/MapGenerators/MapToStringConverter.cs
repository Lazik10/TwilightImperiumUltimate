using System.Globalization;
using System.Text;
using TwilightImperiumUltimate.Web.Options.MapGenerators;

namespace TwilightImperiumUltimate.Web.Services.MapGenerators;

public class MapToStringConverter(
    IMapGeneratorSettingsService mapGeneratorSettingsService,
    IMapGeneratorService mapGeneratorService,
    ILogger<MapToStringConverter> logger,
    NavigationManager navigationManager)
    : IMapToStringConverter
{
    private readonly IMapGeneratorSettingsService _mapGeneratorSettingsService = mapGeneratorSettingsService;
    private readonly IMapGeneratorService _mapGeneratorService = mapGeneratorService;
    private readonly ILogger<MapToStringConverter> _logger = logger;
    private readonly NavigationManager _navigationManager = navigationManager;

    private Dictionary<int, SystemTileModel> _map = new();

    public IReadOnlyDictionary<int, SystemTileModel> Map => _map;

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

    public async Task<string> ConvertMapToBase64String(MapTemplate mapTemplate, IReadOnlyDictionary<int, SystemTileModel> map)
    {
        var ttsString = await ConvertMapToTtsString(mapTemplate, map);
        var base64string = Convert.ToBase64String(Encoding.UTF8.GetBytes(ttsString));
        return base64string;
    }

    public Task<string> ConvertMapToTtsString()
    {
        var template = _mapGeneratorSettingsService.MapTemplate;
        var ttsPositions = TiUltimatePositionsFromTtsPositions(template);
        var map = _mapGeneratorService.GeneratedPositionsWithSystemTiles;

        string ttsString = string.Empty;
        if (template != MapTemplate.CustomMap)
            ttsString = GenerateTtsString(ttsPositions, map);
        else
            ttsString = GenerateCustomMapTtsString(map);

        return Task.FromResult(ttsString);
    }

    public Task<string> ConvertMapToTtsString(MapTemplate mapTemplate, IReadOnlyDictionary<int, SystemTileModel> map)
    {
        var ttsPositions = TiUltimatePositionsFromTtsPositions(mapTemplate);
        string ttsString = GenerateTtsString(ttsPositions, map);

        return Task.FromResult(ttsString);
    }

    public async Task ConvertTtsStringToMap(MapTemplate mapTemplate, string ttsString)
    {
        var ttsPositions = TiUltimatePositionsFromTtsPositions(mapTemplate);
        var importedTtsSystemTileCodes = ttsString.Split(' ').ToList();

        await CreateMapFromTtsString(importedTtsSystemTileCodes, ttsPositions);
    }

    public Task<string> GenerateMapGeneratorLink(MapTemplate mapTemplate, string mapCode)
    {
        var baseAddress = _navigationManager.BaseUri;
        var mapUrl = $"{baseAddress}tools/map-generator?template={mapTemplate}&tiles={mapCode}";
        return Task.FromResult(mapUrl);
    }

    public Task<string> GenerateMapArchiveLink(string mapName, string mapEvent)
    {
        var baseAddress = _navigationManager.BaseUri;
        var mapUrl = $"{baseAddress}community/maps-archive/map/";
        return Task.FromResult(mapUrl);
    }

    private static string GenerateCustomMapTtsString(IReadOnlyDictionary<int, SystemTileModel> map)
    {
        var ttsString = new StringBuilder();

        foreach (var mapPosition in map.Keys)
        {
            if (map[mapPosition] is not null)
            {
                if (map[mapPosition].SystemTileName == SystemTileName.TileHome && map[mapPosition].SystemTileCategory == SystemTileCategory.None)
                {
                    ttsString.Append('0');
                }
                else if (map[mapPosition].SystemTileName == SystemTileName.TileEmpty
                    || map[mapPosition].SystemTileName == SystemTileName.TileTransparent
                    || map[mapPosition].SystemTileName == SystemTileName.TileBlackFrame
                    || map[mapPosition].SystemTileName == SystemTileName.TileBlueFrame)
                {
                    ttsString.Append('T');
                }
                else
                {
                    ttsString.Append(map[mapPosition].SystemTileCode);
                }

                ttsString.Append(' ');
            }
            else
            {
                ttsString.Append('N');
                ttsString.Append(' ');
            }
        }

        return ttsString.ToString().Trim();
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
            MapTemplate.ThreePlayersSmallMap or
            MapTemplate.ThreePlayersSmallAlternateMap
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
            MapTemplate.FourPlayersMediumHyperlineMap or
            MapTemplate.FourPlayersMediumGapsMap or
            MapTemplate.FourPlayersMediumWarpMap or
            MapTemplate.FourPlayersMediumMiniWarpMap or
            MapTemplate.FourPlayersMediumDoubleWarpMap or
            MapTemplate.ThreePlayersMediumHyperlineMap or
            MapTemplate.ThreePlayersMediumTriangleMap or
            MapTemplate.ThreePlayersMediumTriangleNarrowMap or
            MapTemplate.ThreePlayersMediumSnowflakeMap or
            MapTemplate.ThreePlayersMediumMantaRayMap or
            MapTemplate.ThreePlayersMediumTridentMap or
            MapTemplate.ThreePlayersMediumRexMap or
            MapTemplate.TwoPlayersMediumMap
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

            MapTemplate.CustomMap
            => Enumerable.Range(0, MapTemplateOptions.MaxTilePositionsCustomMap).ToList(),

            _ => new List<int>(),
        };
    }

    private async Task CreateMapFromTtsString(List<string> systemTileCodes, List<int> tiUltimatePositions)
    {
        var mapTemplate = _mapGeneratorSettingsService.MapTemplate;

        if (mapTemplate == MapTemplate.CustomMap && systemTileCodes.Count != tiUltimatePositions.Count)
            return;

        if (mapTemplate != MapTemplate.CustomMap && systemTileCodes.Count != tiUltimatePositions.Count - 1)
            return;

        await _mapGeneratorService.GenerateMapAsync(true, default);
        var map = _mapGeneratorService.GeneratedPositionsWithSystemTiles.ToDictionary();
        await _mapGeneratorService.InitializeSystemTilesAsync(default);
        var allSystemTiles = _mapGeneratorService.AllSystemTiles;

        var tiUltimatePositionsWithoutMecatolRex = tiUltimatePositions.Skip(mapTemplate == MapTemplate.CustomMap ? 0 : 1).ToList();

        var mecatolRex = allSystemTiles
            .First(x => x.SystemTileCategory == SystemTileCategory.MecatolRex);

        var homePlaceholder = allSystemTiles
            .First(x => x.SystemTileCategory == SystemTileCategory.None
            && x.FactionName == FactionName.None
            && x.SystemTileName == SystemTileName.TileHome);

        // First assign Mecatol Rex if it is not Custom Map
        if (mapTemplate != MapTemplate.CustomMap)
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
                                x.SystemTileCode == systemTileCodes[i].ToString(CultureInfo.InvariantCulture)) ?? new SystemTileModel() { SystemTileName = SystemTileName.TileBlackFrame };
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

                        var index = startIndexA != 0 ? startIndexA : startIndexB;

                        var hyperlane = allSystemTiles
                            .First(x => x.SystemTileCode == stringNumber[..index]);

                        var hyperlineCopy = hyperlane.Copy();
                        var rotation = stringNumber[index..];
                        hyperlineCopy.SystemTileCode += rotation;

                        map[tiUltimatePositionsWithoutMecatolRex[i]] = hyperlineCopy;
                    }
                    else if (stringNumber == "T")
                    {
                        var transparent = allSystemTiles.First(x => x.SystemTileName == SystemTileName.TileTransparent);
                        map[tiUltimatePositionsWithoutMecatolRex[i]] = transparent.Copy();
                    }
                    else
                    {
                        var transparent = allSystemTiles.First(x => x.SystemTileName == SystemTileName.TileBlackFrame);
                        var systemTileWithStringCode = allSystemTiles.FirstOrDefault(x => x.SystemTileCode == stringNumber);

                        map[tiUltimatePositionsWithoutMecatolRex[i]] = systemTileWithStringCode is not null ? systemTileWithStringCode.Copy() : transparent.Copy();
                    }
                }
            }
        }

        _map = map;
        await _mapGeneratorService.InitializeMapFromLink(map);
    }
}
