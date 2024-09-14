namespace TwilightImperiumUltimate.Web.Models.GameTracker;

public class GameTrackerObjectiveCardModel
{
    public bool Revealed { get; set; }

    public bool IsLeakedSecret { get; set; }

    public ObjectiveCardModel ObjectiveCard { get; set; } = new();

    public IReadOnlyCollection<FactionName> ScoredFactions { get; set; } = new List<FactionName>();

    public int ScorePoints => GetPoints();

    public void AddScoredFaction(FactionName factionName)
    {
        var scoredFactions = ScoredFactions.ToList();
        scoredFactions.Add(factionName);
        ScoredFactions = scoredFactions;
    }

    public void RemoveScoredFaction(FactionName factionName)
    {
        var scoredFactions = ScoredFactions.ToList();
        scoredFactions.Remove(factionName);
        ScoredFactions = scoredFactions;
    }

    private int GetPoints()
    {
        if (ObjectiveCard is null)
            return 0;

        if (ObjectiveCard.ObjectiveCardType == ObjectiveCardType.Secret
            || ObjectiveCard.ObjectiveCardType == ObjectiveCardType.StageOne)
            return 1;

        if (ObjectiveCard.ObjectiveCardType == ObjectiveCardType.StageTwo)
            return 2;

        return 0;
    }
}
