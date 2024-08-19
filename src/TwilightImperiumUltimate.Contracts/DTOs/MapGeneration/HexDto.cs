using TwilightImperiumUltimate.Contracts.DTOs.Galaxy;

namespace TwilightImperiumUltimate.Contracts.DTOs.MapGeneration;

public class HexDto()
{
    public int X { get; set; }

    public int Y { get; set; }

    public string Name { get; set; } = string.Empty;

    public SystemTileDto? SystemTile { get; set; }

    public string PlayerName { get; set; } = string.Empty;
}