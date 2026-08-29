namespace TwilightImperiumUltimate.Web.Components.Factions.MainSection;

public partial class FactionLeaderCard
{
    [Parameter]
    [EditorRequired]
    public string Title { get; set; } = string.Empty;

    [Parameter]
    [EditorRequired]
    public string UnlockRequirement { get; set; } = string.Empty;

    [Parameter]
    [EditorRequired]
    public string ImagePath { get; set; } = string.Empty;
}
