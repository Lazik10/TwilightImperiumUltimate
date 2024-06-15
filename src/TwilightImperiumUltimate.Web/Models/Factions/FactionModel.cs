using TwilightImperiumUltimate.Web.Enums;

namespace TwilightImperiumUltimate.Web.Models.Factions;

public class FactionModel
{
    public FactionName FactionName { get; set; }

    public int Commodities { get; set; }

    public int ComplexityRating { get; set; }

    public GameVersion GameVersion { get; set; }

    public bool Banned { get; set; }

    public IReadOnlyDictionary<UnitName, int> Units { get; set; } = new Dictionary<UnitName, int>();

    public IReadOnlyCollection<PromissaryNoteName> PromissaryNotes { get; set; } = new List<PromissaryNoteName>();
}
