namespace TwilightImperiumUltimate.Web.Components.Cards;

public partial class CardTypeNavBar
{
    private const int MobileRowBreakAfter = 5;

    [Parameter]
    public string SelectedTypeOfCard { get; set; } = string.Empty;

    [Parameter]
    public EventCallback<string> OnTypeSelected { get; set; }

    private IReadOnlyList<CardTypeNavDefinition> CardTypes =>
    [
        new(Paths.ResourcePath_StrategyCard, nameof(Strings.StrategyCard), $"icon for {Strings.CardTitle_Strategy}", Strings.CardTitle_Strategy, true),
        new(Paths.ResourcePath_ActionCard, nameof(Strings.ActionCard), $"icon for {Strings.CardTitle_Action}", Strings.CardTitle_Action),
        new(Paths.ResourcePath_AgendaCard, nameof(Strings.AgendaCard), $"icon for {Strings.CardTitle_Agenda}", Strings.CardTitle_Agenda),
        new(Paths.ResourcePath_ObjectiveSecret, nameof(Strings.ObjectiveCardSecret), $"icon for {Strings.CardTitle_Secret}", Strings.CardTitle_Secret),
        new(Paths.ResourcePath_ObjectiveStageOne, nameof(Strings.ObjectiveCardStageOne), $"icon for {Strings.CardTitle_ObjectiveOne}", Strings.CardTitle_ObjectiveOne),
        new(Paths.ResourcePath_ObjectiveStageTwo, nameof(Strings.ObjectiveCardStageTwo), $"icon for {Strings.CardTitle_ObjectiveTwo}", Strings.CardTitle_ObjectiveTwo),
        new(Paths.ResourcePath_ExplorationCard, nameof(Strings.ExplorationCard), $"icon for {Strings.CardTitle_Exploration}", Strings.CardTitle_Exploration),
        new(Paths.ResourcePath_RelicCard, nameof(Strings.RelicCard), $"icon for {Strings.CardTitle_Relic}", Strings.CardTitle_Relic, true),
        new(Paths.ResourcePath_FrontierCard, nameof(Strings.FrontierCard), $"icon for {Strings.CardTitle_Frontier}", Strings.CardTitle_Frontier),
        new(Paths.ResourcePath_PromissoryCard, nameof(Strings.PromissoryNoteCard), $"icon for {Strings.CardTitle_PromissoryNote}", Strings.CardTitle_PromissoryNote),
    ];

    private IEnumerable<CardTypeNavDefinition> FirstRowCardTypes => CardTypes.Take(MobileRowBreakAfter);

    private IEnumerable<CardTypeNavDefinition> SecondRowCardTypes => CardTypes.Skip(MobileRowBreakAfter);

    private Task HandleSelection(string typeOfCard) => OnTypeSelected.InvokeAsync(typeOfCard);

    private sealed record CardTypeNavDefinition(
        string TypePath,
        string IconName,
        string AltText,
        string Title,
        bool NoBorder = false);
}
