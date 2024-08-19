namespace TwilightImperiumUltimate.DataAccess.Repositories;

public interface ICardRepository
{
    Task<List<ActionCard>> GetAllActionCards(CancellationToken ct);

    Task<List<AgendaCard>> GetAllAgendaCards(CancellationToken ct);

    Task<List<ExplorationCard>> GetAllExplorationCards(CancellationToken ct);

    Task<List<FrontierCard>> GetAllFrontierCards(CancellationToken ct);

    Task<List<ObjectiveCard>> GetAllObjectiveCards(CancellationToken ct);

    Task<List<ObjectiveCard>> GetObjectiveCardsWithSpecificType(ObjectiveCardType objectiveCardType, CancellationToken ct);

    Task<List<PromissoryNoteCard>> GetAllPromissoryNoteCards(CancellationToken ct);

    Task<List<RelicCard>> GetAllRelicCards(CancellationToken ct);

    Task<List<StrategyCard>> GetAllStrategyCards(CancellationToken ct);
}
