namespace TwilightImperiumUltimate.Web.Components.Factions;

public partial class FactionMenuPicker : TwilightImperiumBaseComponent
{
    private List<FactionModel> _factions = new();

    [Parameter]
    public EventCallback<FactionModel> OnFactionChanged { get; set; }

    [Parameter]
    public EventCallback<IReadOnlyCollection<FactionModel>> OnFactionsInitialized { get; set; }

    [Parameter]
    public bool BanAllFactions { get; set; } = true;

    /// <summary>
    /// Gets or sets the factions rendered by the picker instead of loading the default faction list.
    /// </summary>
    [Parameter]
    public IReadOnlyCollection<FactionModel>? ProvidedFactions { get; set; }

    public IReadOnlyCollection<FactionModel> Factions => _factions;

    [Inject]
    private IFactionProvider FactionProvider { get; set; } = default!;

    public void SetAllFactionsBanStatus(bool banStatus)
    {
        foreach (var faction in _factions)
        {
            faction.Banned = banStatus;
        }

        StateHasChanged();
    }

    protected override async Task OnInitializedAsync()
    {
        if (ProvidedFactions is not null)
        {
            _factions = ProvidedFactions.ToList();
            return;
        }

        var factions = await FactionProvider.GetAllFactions();
        _factions = Mapper.Map<List<FactionModel>>(factions);

        if (BanAllFactions)
            SetAllFactionsBanStatus(true);

        await OnFactionsInitialized.InvokeAsync(Factions);
    }

    private static string GetGameVersionLabel(GameVersion gameVersion) => gameVersion switch
    {
        GameVersion.BaseGame => Strings.GameVersion_BaseGame,
        GameVersion.ProphecyOfKings => Strings.GameVersion_ProphecyOfKings,
        GameVersion.CodexVigil => Strings.GameVersion_CodexVigil,
        GameVersion.DiscordantStars => Strings.GameVersion_DiscordantStars,
        GameVersion.ThundersEdge => Strings.GameVersion_ThundersEdge,
        _ => gameVersion.ToString(),
    };

    private IEnumerable<GameVersion> GetGameVersionGroups() =>
        Enum.GetValues<GameVersion>().Where(version => _factions.Any(faction => faction.GameVersion == version));

    private List<FactionModel> GetFactionsForGameVersion(GameVersion gameVersion) =>
        _factions.Where(faction => faction.GameVersion == gameVersion).ToList();

    private bool IsGameVersionEnabled(GameVersion gameVersion) => GetFactionsForGameVersion(gameVersion).Any(faction => !faction.Banned);

    private void SetFactionSelected(FactionModel faction, bool isSelected)
    {
        faction.Banned = !isSelected;
        OnFactionChanged.InvokeAsync(faction);
    }

    private async Task SetGameVersionEnabledAsync(GameVersion gameVersion, bool isEnabled)
    {
        foreach (var faction in GetFactionsForGameVersion(gameVersion))
        {
            faction.Banned = !isEnabled;
            await OnFactionChanged.InvokeAsync(faction);
        }
    }

    private void FactionIconClicked(FactionModel faction) => SetFactionSelected(faction, faction.Banned);

    private void FactionCheckboxChanged(FactionModel faction, ChangeEventArgs e)
    {
        var isChecked = e.Value is bool value && value;
        SetFactionSelected(faction, isChecked);
    }
}