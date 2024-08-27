namespace TwilightImperiumUltimate.Web.Models.MiltyDrafts;

public class MiltyDraftPlayerModel
{
    public int PlayerId { get; set; }

    public string PlayerName { get; set; } = string.Empty;

    public string PlayerDefaultName { get; set; } = string.Empty;

    public DraftColor PlayerColor { get; set; }

    public FactionName Faction { get; set; }

    public MiltyDraftInitiative Initiative { get; set; }

    public SliceModel Slice { get; set; }

    public int DraftOrder { get; set; }
}
