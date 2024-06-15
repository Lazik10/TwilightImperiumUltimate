using TwilightImperiumUltimate.Web.Enums;

namespace TwilightImperiumUltimate.Web.Options.MapGenerators;

public static class MapGeneratorOptions
{
    public static readonly int MinScale = 30;

    public static readonly int MaxScale = 100;

    public static readonly int DefaultScale = 100;

    public static readonly int ScaleIncrement = 5;

    public static readonly SystemTileName DefaultTileName = SystemTileName.Tile18;

    public static readonly int MaxTilePositions = 81;

    public static readonly MapTemplate MapTemplate = MapTemplate.MediumMapSixPlayers;

    public static readonly PlacementStyle PlacementStyle = PlacementStyle.Slice;

    public static readonly SystemWeight SystemWeight = SystemWeight.Balanced;

    public static readonly SystemTileOverlay SystemTileOverlay = SystemTileOverlay.None;

    public static readonly GameVersion GameVersion = GameVersion.ProphecyOfKing;
}
