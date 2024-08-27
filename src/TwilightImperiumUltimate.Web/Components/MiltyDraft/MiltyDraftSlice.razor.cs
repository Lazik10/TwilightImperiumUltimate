using TwilightImperiumUltimate.Web.Services.MiltyDraft;

namespace TwilightImperiumUltimate.Web.Components.MiltyDraft;

public partial class MiltyDraftSlice
{
    [Parameter]
    public SliceModel SliceModel { get; set; } = new SliceModel();

    [Parameter]
    public DraftColor Color { get; set; } = DraftColor.None;

    [CascadingParameter(Name = "MiltyDraftMainDraft")]
    public MiltyDraftMainDraft MiltyDraftMainDraft { get; set; } = default!;

    [Inject]
    private IMiltyDraftService MiltyDraftService { get; set; } = default!;

    private async Task PickSlice()
    {
        if (IsDisabled())
            return;

        await MiltyDraftService.SlicePicked(SliceModel);
        await MiltyDraftMainDraft.Refresh();
    }

    private bool IsDisabled()
    {
        return MiltyDraftService.State == MiltyDraftState.Finished
            || (MiltyDraftService.State == MiltyDraftState.Started
            && ((MiltyDraftService.ActivePlayer.Slice is not null && MiltyDraftService.ActivePlayer.Slice.IsPicked)
            || SliceModel.IsPicked
            || MiltyDraftService.Slices.Count(x => x.IsPicked) >= MiltyDraftService.Players.Count));
    }

    private string IsColorless()
    {
        return !SliceModel.IsPicked && MiltyDraftService.Slices.Count(x => x.IsPicked) >= MiltyDraftService.Players.Count
            ? "filter: grayscale(100%); -webkit-filter: grayscale(100%);" : string.Empty;
    }
}