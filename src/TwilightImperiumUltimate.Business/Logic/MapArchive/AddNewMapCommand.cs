namespace TwilightImperiumUltimate.Business.Logic.MapArchive;

public class AddNewMapCommand(MapDto map) : IRequest<bool>
{
    public MapDto Map { get; set; } = map;
}
