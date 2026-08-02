using TwilightImperiumUltimate.Web.Helpers.Enums;

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

    /// <summary>
    /// Gets or sets an optional SEO-friendly link to this faction's dedicated route.
    /// When set, the tile renders as a real anchor in addition to raising <see cref="OnClick"/>.
    /// </summary>
    [Parameter]
    public string? Href { get; set; }

    public string Name => Faction.FactionName.ToString();

    private string GetBanIconState() => Faction.Banned && EnableBanMode ? "colorless" : string.Empty;

    private string GetFactionIconPath() => PathProvider.GetFactionIconPath(Faction.FactionName);

    private string GetIconCssClass() =>
        (NoPadding ? "responsive-icon responsive-icon-no-padding" : "responsive-icon") + $" {GetSourceCssClass()}";

    private string GetSourceCssClass() => Faction.FactionName.GetFactionSource() switch
    {
        FactionSource.DiscordantStars => "source-discordant-stars",
        FactionSource.BlueRiverie => "source-blue-riverie",
        FactionSource.TwilightsFall => "source-twilights-fall",
        FactionSource.WhispersFromTheVoid => "source-whispers-from-the-void",
        _ => "source-official",
    };
}
