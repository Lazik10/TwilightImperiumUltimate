namespace TwilightImperiumUltimate.DataAccess.DbContexts.TwilightImperium;

public partial class TwilightImperiumDbContextInitializer
{
    private void InitializeAllCards()
    {
        InitializeActionCards();
        InitializeAgendaCards();
        InitializeFrontierCards();
        InitializeRelicCards();
        InitializeStrategyCards();
        InitializeExplorationCard();
        InitializeObjectiveCards();
    }
}
