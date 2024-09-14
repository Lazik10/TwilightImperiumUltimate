namespace TwilightImperiumUltimate.Web.Models.GameTracker;

public class GameTrackerPlayerModel
{
    public int Id { get; set; }

    public string DefaultName { get; set; } = string.Empty;

    public string Name { get; set; } = string.Empty;

    public string ColoTextName { get; set; } = string.Empty;

    public PlayerColor PlayerColor { get; set; }

    public bool IsColorPicked { get; set; }

    public FactionName FactionName { get; set; }

    public InitiativeOrder Initiative { get; set; } = InitiativeOrder.First;

    public StrategyCardName StrategyCard { get; set; }

    public int Score { get; set; }

    public int Influence { get; set; }

    public TimeSpan ElapsedTime { get; set; }

    public bool StrategyCardUsed { get; set; }

    public bool Passed { get; set; }

    public PlayerStatus PlayerStatus { get; set; }

    public IReadOnlyCollection<ObjectiveCardModel> ScoredObjectives { get; set; } = new List<ObjectiveCardModel>();

    public BonusObjectives BonusObjectives { get; set; } = new BonusObjectives();
}
