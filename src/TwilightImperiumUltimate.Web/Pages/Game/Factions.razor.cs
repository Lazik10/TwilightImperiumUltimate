using System.Globalization;
using TwilightImperiumUltimate.Web.Helpers.Enums;
using TwilightImperiumUltimate.Web.Helpers.Factions;
using TwilightImperiumUltimate.Web.Services.Factions;

namespace TwilightImperiumUltimate.Web.Pages.Game;

public partial class Factions
{
    private FactionTemplateGrid? _factionTemplateGridRef;

    private FactionSource _selectedSource = FactionSource.Official;

    [Parameter]
    public string? FactionOrSource { get; set; }

    [Parameter]
    [SupplyParameterFromQuery(Name = "faction")]
    public string? FactionQuery { get; set; }

    [Parameter]
    [SupplyParameterFromQuery(Name = "source")]
    public string? SourceQuery { get; set; }

    [Parameter]
    [SupplyParameterFromQuery(Name = "info")]
    public string Info { get; set; } = string.Empty;

    [Inject]
    private IFactionProvider FactionProvider { get; set; } = default!;

    [Inject]
    private NavigationManager NavigationManager { get; set; } = default!;

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

    private void OnSourceChanged(FactionSource newSource)
    {
        NavigationManager.NavigateTo($"/game/factions?source={newSource}");
    }

    private void UpdateFaction(FactionModel faction)
    {
        _factionTemplateGridRef?.UpdateSelectedFaction(faction);
    }

    private FactionSource ResolveSource()
    {
        var source = FactionSource.Official;

        if (FactionSourceAliasResolver.TryResolve(SourceQuery, out var sourceFromQuery))
            source = sourceFromQuery;
        else if (FactionSourceAliasResolver.TryResolve(FactionOrSource, out var sourceFromRoute))
            source = sourceFromRoute;
        else if (FactionNameAliasResolver.TryResolve(FactionQuery, out var factionFromQuery))
            source = factionFromQuery.GetFactionSource();
        else if (FactionNameAliasResolver.TryResolve(FactionOrSource, out var factionFromRoute))
            source = factionFromRoute.GetFactionSource();

        FactionProvider.ClearSource();
        FactionProvider.SetSource(source);

        return source;
    }
}
