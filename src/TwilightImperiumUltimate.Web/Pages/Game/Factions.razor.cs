using System.Globalization;
using TwilightImperiumUltimate.Web.Helpers.Enums;
using TwilightImperiumUltimate.Web.Helpers.Factions;

namespace TwilightImperiumUltimate.Web.Pages.Game;

public partial class Factions
{
    private FactionTemplateGrid? _factionTemplateGridRef;

    private FactionSource _selectedSource;

    /// <summary>
    /// Gets or sets the route segment identifying either a specific faction (e.g. "TheArborec")
    /// or a faction source/expansion (e.g. "DiscordantStars") so every faction can be reached
    /// through a crawlable, shareable URL. The "faction"/"source" query strings below take
    /// precedence when present.
    /// </summary>
    [Parameter]
    public string? FactionOrSource { get; set; }

    [Parameter]
    [SupplyParameterFromQuery(Name = "info")]
    public string Info { get; set; } = string.Empty;

    [Parameter]
    [SupplyParameterFromQuery(Name = "faction")]
    public string? FactionQuery { get; set; }

    [Parameter]
    [SupplyParameterFromQuery(Name = "source")]
    public string? SourceQuery { get; set; }

    [Inject]
    private NavigationManager NavigationManager { get; set; } = default!;

    private string SelectedFactionParameterValue =>
        !string.IsNullOrWhiteSpace(FactionQuery) ? FactionQuery! : FactionOrSource ?? string.Empty;

    private string PageHeading =>
        _factionTemplateGridRef?.SelectedFaction is { } selectedFaction
            ? string.Format(CultureInfo.CurrentCulture, Strings.Page_Factions_PageTitle, selectedFaction.FactionName.GetFactionUIText(FactionResourceType.Title))
            : Strings.Page_Factions_PageTitleDefault;

    private string MetaDescription =>
        _factionTemplateGridRef?.SelectedFaction is { } selectedFaction
            ? string.Format(CultureInfo.CurrentCulture, Strings.Page_Factions_MetaDescription, selectedFaction.FactionName.GetFactionUIText(FactionResourceType.Title))
            : Strings.Page_Factions_MetaDescriptionDefault;

    protected override void OnParametersSet()
    {
        _selectedSource = ResolveSource();
    }

    private static List<KeyValuePair<FactionSource, string>> GetFactionSourceOptions() =>
        Enum.GetValues<FactionSource>()
            .Select(source => new KeyValuePair<FactionSource, string>(source, source.GetDisplayName()))
            .ToList();

    private void UpdateSelectedFaction(FactionModel selectedFaction)
    {
        _factionTemplateGridRef?.UpdateSelectedFaction(selectedFaction);
        _factionTemplateGridRef?.SetFactionInfo(Info);
    }

    private void OnSourceChanged(FactionSource newSource)
    {
        NavigationManager.NavigateTo($"/game/factions?source={newSource}");
    }

    private FactionSource ResolveSource()
    {
        if (FactionSourceAliasResolver.TryResolve(SourceQuery, out var sourceFromQuery))
        {
            return sourceFromQuery;
        }

        if (FactionSourceAliasResolver.TryResolve(FactionOrSource, out var sourceFromRoute))
        {
            return sourceFromRoute;
        }

        if (FactionNameAliasResolver.TryResolve(FactionQuery, out var factionFromQuery))
        {
            return factionFromQuery.GetFactionSource();
        }

        if (FactionNameAliasResolver.TryResolve(FactionOrSource, out var factionFromRoute))
        {
            return factionFromRoute.GetFactionSource();
        }

        return FactionSource.Official;
    }
}
