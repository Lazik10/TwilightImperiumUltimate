using System.Runtime.CompilerServices;
using TwilightImperiumUltimate.Web.Services.MiltyDraft;

namespace TwilightImperiumUltimate.Web.Components.MiltyDraft;

public partial class MiltyDraftFactionRow
{
    [Parameter]
    public IReadOnlyCollection<MiltyDraftFactionModel> DraftedFactions { get; set; } = new List<MiltyDraftFactionModel>();

    [CascadingParameter(Name = "MiltyDraftMainDraft")]
    public MiltyDraftMainDraft MiltyDraftMainDraft { get; set; } = default!;

    [Inject]
    private IMiltyDraftService MiltyDraftService { get; set; } = default!;

    [Inject]
    private IMiltyDraftSettingsService MiltyDraftSettingsService { get; set; } = default!;

    private MiltyDraftState GetMiltyDraftState() => MiltyDraftService.State;

    private int GetGridColumns()
    {
        return MiltyDraftSettingsService.NumberOfFactions > 10 ? 10 : MiltyDraftSettingsService.NumberOfFactions;
    }

    private async Task PickFaction(MiltyDraftFactionModel faction)
    {
        if (IsDisabled(faction))
            return;

        await MiltyDraftService.FactionPicked(faction.FactionName);
        await MiltyDraftMainDraft.Refresh();
    }

    private bool IsDisabled(MiltyDraftFactionModel faction)
    {
        return MiltyDraftService.State == MiltyDraftState.Finished || MiltyDraftService.State == MiltyDraftState.Initialized
            || (MiltyDraftService.State == MiltyDraftState.Started
            && (MiltyDraftService.ActivePlayer.Faction != FactionName.None
            || faction.IsPicked
            || MiltyDraftService.DraftedFactions.Count(x => x.IsPicked) >= MiltyDraftService.Players.Count));
    }

    private bool IsColorless(MiltyDraftFactionModel faction)
    {
        var isPicked = faction.IsPicked;
        var draftedFactionsCount = MiltyDraftService.DraftedFactions.Count(x => x.IsPicked);
        var playersCount = MiltyDraftService.Players.Count;
        var result = !isPicked && (draftedFactionsCount >= playersCount);

        return result;
    }
}