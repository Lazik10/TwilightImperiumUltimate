using TwilightImperiumUltimate.Web.Services.MiltyDraft;

namespace TwilightImperiumUltimate.Web.Components.MiltyDraft;

public partial class MiltyDraftFaction
{
    [Parameter]
    public FactionName Faction { get; set; } = default!;

    [Parameter]
    public bool IsPicked { get; set; }

    [Parameter]
    public DraftColor Color { get; set; } = DraftColor.None;

    [Parameter]
    public bool IsColorless { get; set; }

    [Inject]
    private IPathProvider PathProvider { get; set; } = default!;

    [Inject]
    private IMiltyDraftService MiltyDraftService { get; set; } = default!;

    private string GetFactionImage()
    {
        return PathProvider.GetFactionIconPath(Faction);
    }

    private string ApplyGreyscale()
    {
        return IsColorless ? "grayscale(100%)" : string.Empty;
    }
}