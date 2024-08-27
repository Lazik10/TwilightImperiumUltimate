using TwilightImperiumUltimate.Web.Services.MiltyDraft;

namespace TwilightImperiumUltimate.Web.Components.MiltyDraft;

public partial class MiltyDraftInitiativeOrder
{
    [Parameter]
    public bool IsPicked { get; set; }

    [Parameter]
    public DraftColor Color { get; set; } = DraftColor.None;

    [Parameter]
    public MiltyDraftInitiative Initiative { get; set; } = default!;

    [Parameter]
    public int FontSize { get; set; } = 16;

    [CascadingParameter(Name = "MiltyDraftMainDraft")]
    public MiltyDraftMainDraft MiltyDraftMainDraft { get; set; } = default!;

    [Inject]
    private IMiltyDraftService MiltyDraftService { get; set; } = default!;

    private async Task PickInitiative()
    {
        if (IsDisabled())
            return;

        IsPicked = true;
        Color = MiltyDraftService.ActivePlayer.PlayerColor;
        await MiltyDraftService.InitiativePicked(Initiative);
        await MiltyDraftMainDraft.Refresh();
    }

    private bool IsDisabled()
    {
        return MiltyDraftService.State == MiltyDraftState.Finished || MiltyDraftService.State == MiltyDraftState.Initialized
            || (MiltyDraftService.State == MiltyDraftState.Started
            && (MiltyDraftService.ActivePlayer.Initiative != MiltyDraftInitiative.None
            || IsPicked
            || MiltyDraftService.Initiatives.Count(x => x.Picked) >= MiltyDraftService.Players.Count));
    }

    private string IsColorless()
    {
        return !IsPicked && MiltyDraftService.Initiatives.Count(x => x.Picked) >= MiltyDraftService.Players.Count
            ? "filter: grayscale(100%); -webkit-filter: grayscale(100%);" : string.Empty;
    }
}