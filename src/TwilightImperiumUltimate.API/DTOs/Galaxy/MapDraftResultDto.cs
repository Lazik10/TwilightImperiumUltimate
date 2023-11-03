namespace TwilightImperiumUltimate.API.DTOs.Galaxy;

public class MapDraftResultDto
{
    public Dictionary<int, SystemTileDto> MapTiles { get; set; } = new Dictionary<int, SystemTileDto>();
}
