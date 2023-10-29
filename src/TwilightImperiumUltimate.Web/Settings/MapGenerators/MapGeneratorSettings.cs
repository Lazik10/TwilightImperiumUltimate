using TwilightImperiumUltimate.Web.Enums;

namespace TwilightImperiumUltimate.Web.Settings.MapGenerators;

public static class MapGeneratorSettings
{
    public static readonly int MinimumNumberOfPlayers = 3;

    public static readonly int MaximumNumberOfPlayers = 8;

    public static readonly int NumberOfPlayers = 6;

    public static readonly int DefaultTileId = 18;

    public static readonly int MaxTilePositions = 81;

    public static readonly MapTemplate MapTemplate = MapTemplate.MediumMapSixPlayers;

    public static readonly PlacementStyle PlacementStyle = PlacementStyle.Slice;

    public static readonly SystemWeight SystemWeight = SystemWeight.Balanced;

    public static readonly GameVersion GameVersion = GameVersion.ProphecyOfKing;
}
