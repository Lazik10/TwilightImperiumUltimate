using Microsoft.AspNetCore.Components;
using TwilightImperiumUltimate.Web.Enums;
using TwilightImperiumUltimate.Web.Models.Drafts;

namespace TwilightImperiumUltimate.Web.Components.Drafts.Faction;

public partial class FactionDraftPlayerRow
{
    [Parameter]
    public FactionDraftPlayerModel Player { get; set; } = new FactionDraftPlayerModel();

    [Parameter]
    public DraftStage DraftStage { get; set; } = DraftStage.Draft;

    [Parameter]
    public int Index { get; set; } = 0;
}