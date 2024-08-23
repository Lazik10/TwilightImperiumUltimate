namespace TwilightImperiumUltimate.Web.Models.SliceGenerators;

public class SliceModel
{
    public int Id { get; set; }

    public IReadOnlyList<SystemTileModel> SystemTiles { get; set; } = new List<SystemTileModel>();

    public int Resources => SystemTiles.Sum(x => x.Resources);

    public int Influence => SystemTiles.Sum(x => x.Influence);

    public float OptimalResources { get; set; }

    public float OptimalInfluence { get; set; }
}
