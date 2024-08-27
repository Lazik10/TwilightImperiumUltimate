using System.Globalization;
using TwilightImperiumUltimate.Web.Helpers.Enums;
using TwilightImperiumUltimate.Web.Services.MiltyDraft;

namespace TwilightImperiumUltimate.Web.Components.MiltyDraft;

public partial class MiltyDraftPlayerInfo
{
    [Parameter]
    public MiltyDraftPlayerModel Player { get; set; } = new MiltyDraftPlayerModel();

    [Inject]
    private IMiltyDraftSettingsService MiltyDraftSettingsService { get; set; } = default!;

    private TextColor TextColor => Player.PlayerColor.GetTextColor();

    private string GetPlayerName() => MiltyDraftSettingsService.EnablePlayerNames ? Player.PlayerName : Player.PlayerDefaultName;

    private string GetFactionText() => Player.Faction == FactionName.None ? string.Empty : Player.Faction.GetFactionUIText(FactionResourceType.Title);

    private string GetInitiativeText() => Player.Initiative == MiltyDraftInitiative.None ? string.Empty : Player.Initiative.GetDisplayName();

    private string GetSliceId()
    {
        if (Player.Slice is null || !Player.Slice.IsPicked)
        {
            return string.Empty;
        }

        return (Player.Slice.Id + 1).ToString(CultureInfo.InvariantCulture);
    }
}