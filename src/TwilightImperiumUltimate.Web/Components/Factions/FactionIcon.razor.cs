namespace TwilightImperiumUltimate.Web.Components.Factions;

public partial class FactionIcon : TwilightImperiumBaseComponent
{
    [Parameter]
    public required FactionModel Faction { get; set; }

    [Parameter]
    public bool EnableBanMode { get; set; }

    [Parameter]
    public EventCallback<FactionModel> OnClick { get; set; }

    [Parameter]
    public bool NoPadding { get; set; }

    public string Name => Faction.FactionName.ToString();

    private string GetBanIconState() => Faction.Banned && EnableBanMode ? "colorless" : string.Empty;

    private string GetFactionIconPath() => PathProvider.GetFactionIconPath(Faction.FactionName);
}
