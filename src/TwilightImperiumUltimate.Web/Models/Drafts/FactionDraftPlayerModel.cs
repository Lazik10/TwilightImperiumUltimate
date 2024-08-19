using TwilightImperiumUltimate.Contracts.Enums;

namespace TwilightImperiumUltimate.Web.Models.Drafts;

public class FactionDraftPlayerModel
{
    public int Id { get; set; }

    public string Name { get; set; } = string.Empty;

    public IReadOnlyCollection<FactionName> DraftedFactions { get; set; } = [];
}
