using TwilightImperiumUltimate.Contracts.Enums;

namespace TwilightImperiumUltimate.Web.Models.Drafts;

public class ColorDraftRequest
{
    public IReadOnlyCollection<FactionName> Factions { get; set; } = new List<FactionName>();

    public IReadOnlyList<PlayerColor> Colors { get; set; } = new List<PlayerColor>();
}