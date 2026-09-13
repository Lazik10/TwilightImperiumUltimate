namespace TwilightImperiumUltimate.Web.Components.Factions.Header;

public partial class FactionComplexity
{
    private string _imagePath = string.Empty;

    [Inject]
    private IFactionProvider FactionProvider { get; set; } = default!;

    [Inject]
    private IPathProvider PathProvider { get; set; } = default!;

    protected override void OnInitialized()
    {
        var faction = FactionProvider.CurrentFaction;
        var complexityRating = faction?.ComplexityRating ?? ComplexityRating.Low;
        _imagePath = PathProvider.GetComplexityIconPath(complexityRating);
    }
}
