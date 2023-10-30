using TwilightImperiumUltimate.Web.Enums;

namespace TwilightImperiumUltimate.Web.Settings.MapGenerators;

public static class MapGeneratorSettings
{
    public static readonly int MinScale = 30;

    public static readonly int MaxScale = 100;

    public static readonly int DefaultScale = 100;

    public static readonly int ScaleIncrement = 5;

    public static readonly int DefaultTileId = 18;

    public static readonly int MaxTilePositions = 81;

    public static readonly MapTemplate MapTemplate = MapTemplate.MediumMapSixPlayers;

    public static readonly PlacementStyle PlacementStyle = PlacementStyle.Slice;

    public static readonly SystemWeight SystemWeight = SystemWeight.Balanced;

    public static readonly GameVersion GameVersion = GameVersion.ProphecyOfKing;
}
