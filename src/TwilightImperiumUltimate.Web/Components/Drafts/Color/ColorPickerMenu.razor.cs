namespace TwilightImperiumUltimate.Web.Components.Drafts.Color;

public partial class ColorPickerMenu : TwilightImperiumBaseComponent
{
    private List<FactionModel> _factions = new();

    [Parameter]
    public EventCallback<FactionModel> OnFactionClickGetFaction { get; set; }

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
        var factions = await FactionProvider.GetAllFactions();
        _factions = Mapper.Map<List<FactionModel>>(factions);
        SetAllFactionsBanStatus(true);
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

    private IEnumerable<GameVersion> GetGameVersionGroups()
    {
        return Enum.GetValues<GameVersion>().Where(version => _factions.Any(faction => faction.GameVersion == version));
    }

    private List<FactionModel> GetFactionsForGameVersion(GameVersion gameVersion)
    {
        return _factions.Where(faction => faction.GameVersion == gameVersion).ToList();
    }

    private void SetFactionSelected(FactionModel faction, bool isSelected)
    {
        faction.Banned = !isSelected;
        OnFactionClickGetFaction.InvokeAsync(faction);
    }

    private void FactionIconClicked(FactionModel faction)
    {
        // Faction.Banned currently means "not selected", so toggling passes its current value as the new selected state.
        SetFactionSelected(faction, faction.Banned);
    }

    private void FactionCheckboxChanged(FactionModel faction, ChangeEventArgs e)
    {
        var isChecked = e.Value is bool value && value;
        SetFactionSelected(faction, isChecked);
    }
}
