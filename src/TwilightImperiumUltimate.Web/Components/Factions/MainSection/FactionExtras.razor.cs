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
        FactionName.TheGhostsOfCreuss => SystemTiles.SingleOrDefault(x => x.SystemTileName == SystemTileName.Tile17),
        FactionName.TheEmbersOfMuaat => SystemTiles.SingleOrDefault(x => x.SystemTileName == SystemTileName.Tile81),
        FactionName.TheCrimsonRebellion => SystemTiles.SingleOrDefault(x => x.SystemTileName == SystemTileName.TileTE94),
        _ => null,
    };

    private static IReadOnlyList<IReadOnlyList<SpecialComponentCardModel>> ChunkByColumns(IReadOnlyList<SpecialComponentCardModel> cards, int columns)
    {
        var rows = new List<IReadOnlyList<SpecialComponentCardModel>>();

        for (var i = 0; i < cards.Count; i += columns)
            rows.Add(cards.Skip(i).Take(columns).ToList());

        return rows;
    }

    private static IReadOnlyList<IReadOnlyList<SpecialComponentCardModel>> ChunkByRowSizes(IReadOnlyList<SpecialComponentCardModel> cards, IReadOnlyList<int> rowSizes)
    {
        var rows = new List<IReadOnlyList<SpecialComponentCardModel>>();
        var index = 0;

        foreach (var rowSize in rowSizes)
        {
            if (index >= cards.Count)
                break;

            var take = Math.Min(rowSize, cards.Count - index);
            rows.Add(cards.Skip(index).Take(take).ToList());
            index += take;
        }

        // Safety net if there are more cards than the configured row sizes account for.
        if (index < cards.Count)
            rows.Add(cards.Skip(index).ToList());

        return rows;
    }

    private IReadOnlyList<SpecialComponentCardModel> GetCardComponents() => SpecialComponentCards
        .Where(x => x.SpecialType is not SpecialComponentType.Token
            and not SpecialComponentType.PlanetToken
            and not SpecialComponentType.FactionUnit
            and not SpecialComponentType.Plot
            and not SpecialComponentType.Ocean)
        .ToList();

    // Chunks GetCardComponents() into grid rows sized so the LAST row's column count always
    // equals its own item count (not a fixed 3), so a short/leftover row spans full width evenly
    // instead of sitting left-aligned in a mostly-empty 3-column track. Some factions need an
    // explicit row-size sequence instead of even chunking (e.g. LastBastion wants 2 then 3, not
    // the greedy 3-then-2 that even chunking by 3 columns would produce).
    private IReadOnlyList<IReadOnlyList<SpecialComponentCardModel>> GetCardComponentRows()
    {
        var cards = GetCardComponents();

        return FactionName switch
        {
            FactionName.LastBastion => ChunkByRowSizes(cards, [2, 3]),
            FactionName.TheOlradinLeague => ChunkByColumns(cards, 2),
            FactionName.TheShipwrightsofAxis => ChunkByColumns(cards, 2),
            _ => ChunkByColumns(cards, 3),
        };
    }

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
