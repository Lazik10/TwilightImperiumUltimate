namespace TwilightImperiumUltimate.Web.Models.Factions;

public class FactionModel
{
    public FactionName FactionName { get; set; }

    public int Commodities { get; set; }

    public ComplexityRating ComplexityRating { get; set; }

    public GameVersion GameVersion { get; set; }

    public bool Banned { get; set; }

    public IReadOnlyCollection<UnitModel> StartingUnits { get; set; } = new List<UnitModel>();

    public IReadOnlyCollection<TechnologyModel> StartingTechnologies { get; set; } = new List<TechnologyModel>();

    public IReadOnlyCollection<PromissoryNoteCardName> PromissaryNotes { get; set; } = new List<PromissoryNoteCardName>();
}
