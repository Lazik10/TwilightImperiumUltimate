using System.Globalization;
using TwilightImperiumUltimate.Web.Helpers.Enums;
using TwilightImperiumUltimate.Web.Helpers.Factions;

namespace TwilightImperiumUltimate.Web.Pages.Game;

public partial class Factions
{
    private FactionTemplateGrid? _factionTemplateGridRef;

    private FactionInfoType _selectedInfoType = FactionInfoType.Ability;

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

    private string PageHeading => string.Format(CultureInfo.CurrentCulture, Strings.Page_Factions_PageTitle, FactionProvider.CurrentFactionName.GetFactionUIText(FactionResourceType.Title));

    private string MetaDescription => string.Format(CultureInfo.CurrentCulture, Strings.Page_Factions_MetaDescription, FactionProvider.CurrentFactionName.GetFactionUIText(FactionResourceType.Title));

    protected override async Task OnInitializedAsync()
    {
        await FactionProvider.InitializeFactions();
    }

    protected override async Task OnParametersSetAsync()
    {
        await ResolveSourceAndFaction();
        _selectedInfoType = ResolveFactionInfoType();
        _factionTemplateGridRef?.Refresh();
    }

    private static List<KeyValuePair<FactionSource, string>> GetFactionSourceOptions() =>
        Enum.GetValues<FactionSource>()
            .Select(source => new KeyValuePair<FactionSource, string>(source, source.GetDisplayName()))
            .ToList();

    private FactionInfoType ResolveFactionInfoType()
    {
        if (string.Equals(Info, "faq", StringComparison.OrdinalIgnoreCase))
            return FactionInfoType.Rules;

        return Enum.TryParse<FactionInfoType>(Info, ignoreCase: true, out var infoType) ? infoType : FactionInfoType.Ability;
    }

    private void OnSourceChanged(FactionSource newSource)
    {
        NavigationManager.NavigateTo($"/game/factions?source={newSource}");
    }

    private async Task ResolveSourceAndFaction()
    {
        var source = FactionSource.Official;
        var factionName = FactionName.TheArborec;

        // Try to load correct faction and source based on route parameter
        if (!string.IsNullOrEmpty(FactionOrSource))
        {
            if (FactionSourceAliasResolver.TryResolve(FactionOrSource, out var sourceFromRoute))
            {
                source = sourceFromRoute;
                factionName = source.GetFactionSourceDefaultFaction();
            }
            else if (FactionNameAliasResolver.TryResolve(FactionOrSource, out var factionFromRoute))
            {
                factionName = factionFromRoute;
                source = factionFromRoute.GetFactionSource();
            }

            FactionProvider.UpdateSourceAndFaction(source, factionName);
            return;
        }

        // If route parameter didn't resolve, try to load based on query parameters
        // If both source and faction are provided, use them. If only source is provided, use the default faction for that source. If only faction is provided, use the source associated with that faction.
        if (!string.IsNullOrEmpty(SourceQuery) && FactionSourceAliasResolver.TryResolve(SourceQuery, out var sourceFromQuery))
        {
            if (!string.IsNullOrEmpty(FactionQuery) && FactionNameAliasResolver.TryResolve(FactionQuery, out var factionFromQuery))
            {
                source = sourceFromQuery;
                factionName = factionFromQuery;
            }
            else
            {
                source = sourceFromQuery;
                factionName = source.GetFactionSourceDefaultFaction();
            }

            FactionProvider.UpdateSourceAndFaction(source, factionName);
            return;
        }
        else if (!string.IsNullOrEmpty(FactionQuery) && FactionNameAliasResolver.TryResolve(FactionQuery, out var factionFromQuery))
        {
            source = factionFromQuery.GetFactionSource();
            factionName = factionFromQuery;

            FactionProvider.UpdateSourceAndFaction(source, factionName);
            return;
        }

        FactionProvider.UpdateSourceAndFaction(source, factionName);
    }
}
