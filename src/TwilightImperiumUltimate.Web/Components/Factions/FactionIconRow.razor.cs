using TwilightImperiumUltimate.Web.Services.MapGenerators;

namespace TwilightImperiumUltimate.Web.Components.Factions;

public partial class FactionIconRow : TwilightImperiumBaseComponent
{
    private List<FactionModel>? _factions = new List<FactionModel>();

    [Parameter]
    public EventCallback<FactionModel> OnFactionClickGetFaction { get; set; }

    [Parameter]
    public EventCallback<IReadOnlyCollection<FactionModel>> OnInitializeGetFactions { get; set; }

    [Parameter]
    public bool EnableBanMode { get; set; } = false;

    [Parameter]
    public bool BanAllFactions { get; set; } = false;

    [Parameter]
    public bool ShowDiscordantStars { get; set; } = false;

    [Parameter]
    public bool ShowBaseGame { get; set; } = true;

    [Parameter]
    public List<FactionModel> ProvidedFactions { get; set; } = new List<FactionModel>();

    [Parameter]
    public string Faction { get; set; } = string.Empty;

    public IReadOnlyCollection<FactionModel>? Factions => _factions;

    [Inject]
    private IMapGeneratorSettingsService MapGeneratorSettingsService { get; set; } = default!;

    public void RefreshFactions()
    {
        _factions = MapGeneratorSettingsService.FactionsForMapGenerator;
        StateHasChanged();
    }

    public void SetAllFactionsBanStatus(bool banStatus)
    {
        _factions?.ForEach(x => x.Banned = banStatus);
    }

    protected override async Task OnInitializedAsync()
    {
        // This is a hack so I can use this component in the map generator,
        // unfortunatelly the componenet is initialized every time
        if (ProvidedFactions.Count != 0)
        {
            _factions = ProvidedFactions;

            await OnInitializeGetFactions.InvokeAsync(Factions);
            MapGeneratorSettingsService.FactionsForMapGenerator = ProvidedFactions;
            return;
        }

        await InitializeFactions();

        if (Factions is not null && Factions.Count != 0)
        {
            var initialFaction = ResolveInitialFaction(Faction);
            await OnFactionClickGetFaction.InvokeAsync(Factions.Single(x => x.FactionName == initialFaction));
        }
    }

    private void FactionClicked(FactionModel selectedFaction)
    {
        if (EnableBanMode)
            selectedFaction.Banned = !selectedFaction.Banned;

        OnFactionClickGetFaction.InvokeAsync(selectedFaction);
    }

    private FactionName ResolveInitialFaction(string factionName)
    {
        if (Enum.TryParse<FactionName>(factionName, out var faction))
        {
            if (ShowBaseGame && faction > FactionName.TheCouncilKeleres)
                return FactionName.TheArborec;

            if (ShowDiscordantStars && faction < FactionName.TheAugursOfIlyxum)
                return FactionName.TheAugursOfIlyxum;

            return faction;
        }

        if (!ShowBaseGame && ShowDiscordantStars)
        {
            return FactionName.TheAugursOfIlyxum;
        }
        else
        {
            return FactionName.TheArborec;
        }
    }

    private async Task InitializeFactions()
    {
        var (response, statusCode) = await HttpClient.GetAsync<ApiResponse<ItemListDto<FactionDto>>>(Paths.ApiPath_Factions);
        if (statusCode == HttpStatusCode.OK)
        {
            _factions = Mapper.Map<List<FactionModel>>(response!.Data!.Items);

            await OnInitializeGetFactions.InvokeAsync(Factions);

            if (BanAllFactions)
                SetAllFactionsBanStatus(true);
        }
    }

    private List<FactionModel> GetBaseGameFactions()
    {
        return _factions?.Where(x => x.GameVersion != GameVersion.DiscordantStars).ToList() ?? new List<FactionModel>();
    }

    private List<FactionModel> GetDiscordantStarsFactions()
    {
        return _factions?.Where(x => x.GameVersion == GameVersion.DiscordantStars).ToList() ?? new List<FactionModel>();
    }

    private bool UseSplitRowsLayout
    {
        get
        {
            var factions = GetBaseGameFactions();
            return ShowBaseGame && !ShowDiscordantStars && factions.Count >= 24;
        }
    }

    private IReadOnlyCollection<FactionModel> GetCompactFirstRowFactions()
    {
        var factions = GetBaseGameFactions();

        var firstRow = factions
            .Where(x => x.GameVersion == GameVersion.BaseGame)
            .Take(17)
            .ToList();

        if (firstRow.Count < 17)
        {
            var remainder = factions
                .Where(x => !firstRow.Contains(x))
                .Take(17 - firstRow.Count)
                .ToList();

            firstRow.AddRange(remainder);
        }

        return firstRow;
    }

    private IReadOnlyCollection<FactionModel?> GetCompactSecondRowWithPlaceholders()
    {
        var row = new List<FactionModel?>();
        var factions = GetBaseGameFactions();
        var firstRow = GetCompactFirstRowFactions().ToHashSet();

        var expansionFactions = factions
            .Where(x => x.GameVersion == GameVersion.ProphecyOfKings && !firstRow.Contains(x))
            .Take(7)
            .Cast<FactionModel?>()
            .ToList();

        var codexFaction = factions
            .Where(x => IsCodexVersion(x.GameVersion) && !firstRow.Contains(x))
            .Take(1)
            .Cast<FactionModel?>()
            .ToList();

        var lastExpansionFactions = factions
            .Where(x => x.GameVersion == GameVersion.ThundersEdge && !firstRow.Contains(x))
            .Take(5)
            .Cast<FactionModel?>()
            .ToList();

        var picked = expansionFactions
            .Concat(codexFaction)
            .Concat(lastExpansionFactions)
            .Where(x => x is not null)
            .Select(x => x!)
            .ToHashSet();

        var fallback = factions
            .Where(x => !firstRow.Contains(x) && !picked.Contains(x))
            .Cast<FactionModel?>()
            .ToList();

        while (expansionFactions.Count < 7 && fallback.Count > 0)
        {
            expansionFactions.Add(fallback[0]);
            fallback.RemoveAt(0);
        }

        while (codexFaction.Count < 1 && fallback.Count > 0)
        {
            codexFaction.Add(fallback[0]);
            fallback.RemoveAt(0);
        }

        while (lastExpansionFactions.Count < 5 && fallback.Count > 0)
        {
            lastExpansionFactions.Add(fallback[0]);
            fallback.RemoveAt(0);
        }

        if (expansionFactions.Count < 7 || codexFaction.Count < 1 || lastExpansionFactions.Count < 5)
        {
            return factions
                .Where(x => !firstRow.Contains(x))
                .Take(17)
                .Cast<FactionModel?>()
                .ToList();
        }

        row.Add(null);
        row.AddRange(expansionFactions);
        row.Add(null);
        row.AddRange(codexFaction);
        row.Add(null);
        row.AddRange(lastExpansionFactions);
        row.Add(null);

        return row;
    }

    private static bool IsCodexVersion(GameVersion gameVersion)
    {
        return gameVersion == GameVersion.CodexRecolo
            || gameVersion == GameVersion.CodexOrdinian
            || gameVersion == GameVersion.CodexAffinity
            || gameVersion == GameVersion.CodexVigil
            || gameVersion == GameVersion.CodexLiberation;
    }

    private IReadOnlyCollection<FactionModel> GetMobileRow1Factions()
    {
        return GetBaseGameFactions().Take(9).ToList();
    }

    private IReadOnlyCollection<FactionModel> GetMobileRow2Factions()
    {
        return GetBaseGameFactions().Skip(9).Take(8).ToList();
    }

    private IReadOnlyCollection<FactionModel?> GetMobileRow3FactionsWithPlaceholders()
    {
        var row = new List<FactionModel?>();
        var rowFactions = GetBaseGameFactions().Skip(17).Take(7).Cast<FactionModel?>().ToList();

        row.Add(null);
        row.AddRange(rowFactions);
        row.Add(null);

        return row;
    }

    private IReadOnlyCollection<FactionModel?> GetMobileRow4FactionsWithPlaceholder()
    {
        var row = new List<FactionModel?>();
        var rowFactions = GetBaseGameFactions().Skip(24).Take(6).Cast<FactionModel?>().ToList();

        if (rowFactions.Count == 0)
        {
            return row;
        }

        row.Add(null);
        row.Add(rowFactions[0]);
        row.Add(null);
        row.AddRange(rowFactions.Skip(1));
        row.Add(null);

        return row;
    }
}

