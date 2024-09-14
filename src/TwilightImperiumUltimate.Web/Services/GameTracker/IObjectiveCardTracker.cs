using TwilightImperiumUltimate.Web.Models.GameTracker;

namespace TwilightImperiumUltimate.Web.Services.GameTracker;

public interface IObjectiveCardTracker
{
    bool ManualPickEnabled { get; set; }

    ObjectiveCardType? IncentiveProgram { get; set; }

    IReadOnlyCollection<GameTrackerObjectiveCardModel> ObjectiveCards { get; }

    IReadOnlyCollection<GameTrackerObjectiveCardModel> Secrets { get; }

    IReadOnlyCollection<GameTrackerObjectiveCardModel> DraftedStageOneObjectives { get; }

    IReadOnlyCollection<GameTrackerObjectiveCardModel> DraftedStageTwoObjectives { get; }

    Task InitializeRequiredCards();

    Task DraftObjectiveCards();

    Task RevealNextObjective(bool initialReveal = false);

    Task UpdateObjectiveCards(ObjectiveCardName selectedObjectiveCard, ObjectiveCardName previousObjectiveCard);

    Task UpdateClassifiedDocumentLeaks(ObjectiveCardName? selectedSecretCard);

    Task ClearClassifiedDocumentLeaks();

    Task ResetIncentiveProgram(ObjectiveCardType objectiveCardType);

    Task RevealIncentiveProgramCard(ObjectiveCardType objectiveCardType);
}
