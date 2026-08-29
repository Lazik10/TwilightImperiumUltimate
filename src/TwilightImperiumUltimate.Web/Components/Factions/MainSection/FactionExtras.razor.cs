namespace TwilightImperiumUltimate.Web.Components.Factions.MainSection;

public partial class FactionExtras
{
    private IReadOnlyList<SpecialComponentName> _largeTokens = [ SpecialComponentName.AvernusToken, SpecialComponentName.VoidTetherToken ];

    [Parameter]
    public FactionName FactionName { get; set; }

    [Parameter]
    [EditorRequired]
    public IReadOnlyList<SystemTileModel> SystemTiles { get; set; } = [];

    [Parameter]
    [EditorRequired]
    public IReadOnlyList<SpecialComponentCardModel> SpecialComponentCards { get; set; } = [];

    [Inject]
    private IPathProvider PathProvider { get; set; } = default!;

    private SystemTileModel? ExtraSystemTile => FactionName switch
    {
        FactionName.TheGhostsOfCreuss => SystemTiles.SingleOrDefault(x => x.SystemTileName == SystemTileName.Tile51),
        FactionName.TheEmbersOfMuaat => SystemTiles.SingleOrDefault(x => x.SystemTileName == SystemTileName.Tile81),
        _ => null,
    };

    private IReadOnlyList<SpecialComponentCardModel> GetCardComponents() => SpecialComponentCards
        .Where(x => x.SpecialType is not SpecialComponentType.Token
            and not SpecialComponentType.PlanetToken
            and not SpecialComponentType.FactionUnit
            and not SpecialComponentType.Plot
            and not SpecialComponentType.Ocean)
        .ToList();

    private IReadOnlyList<SpecialComponentCardModel> GetPlotComponents() => SpecialComponentCards
        .Where(x => x.SpecialType == SpecialComponentType.Plot)
        .ToList();

    private IReadOnlyList<SpecialComponentCardModel> GetOceanComponents() => SpecialComponentCards
        .Where(x => x.SpecialType == SpecialComponentType.Ocean)
        .ToList();

    private IReadOnlyList<SpecialComponentCardModel> GetTokenComponents() => SpecialComponentCards
        .Where(x => x.SpecialType is SpecialComponentType.Token or SpecialComponentType.PlanetToken)
        .ToList();

    private bool HasExtras() => GetCardComponents().Count > 0 || GetPlotComponents().Count > 0 || GetOceanComponents().Count > 0 || GetTokenComponents().Count > 0 || ExtraSystemTile is not null;
}
