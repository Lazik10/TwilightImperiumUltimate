using TwilightImperiumUltimate.Core.Entities.Cards;
using TwilightImperiumUltimate.Core.Enums.Cards;
using TwilightImperiumUltimate.DataAccess.Services;

namespace TwilightImperiumUltimate.DataAccess.DbContexts.TwilightImperium.Data;

internal static class StrategyCardsData
{
    internal static List<StrategyCard> StrategyCards => GetStrategyCards();

    private static List<StrategyCard> GetStrategyCards()
    {
        CreateCardImagePathService createCardImagePathService = new();

        var strategyCards = new List<StrategyCard>
        {
            new() { StrategyCardName = StrategyCardName.Leadership, InitiativeOrder = InitiativeOrder.One, GameVersion = GameVersion.BaseGame },
            new() { StrategyCardName = StrategyCardName.Politics, InitiativeOrder = InitiativeOrder.Three, GameVersion = GameVersion.BaseGame },
            new() { StrategyCardName = StrategyCardName.Trade, InitiativeOrder = InitiativeOrder.Five, GameVersion = GameVersion.BaseGame },
            new() { StrategyCardName = StrategyCardName.Warfare, InitiativeOrder = InitiativeOrder.Six, GameVersion = GameVersion.BaseGame },
            new() { StrategyCardName = StrategyCardName.Technology, InitiativeOrder = InitiativeOrder.Seven, GameVersion = GameVersion.BaseGame },
            new() { StrategyCardName = StrategyCardName.Imperial, InitiativeOrder = InitiativeOrder.Eight, GameVersion = GameVersion.BaseGame },

            // Updated in Prophecy of Kings
            new() { StrategyCardName = StrategyCardName.DiplomacyCodexOrdinian, InitiativeOrder = InitiativeOrder.Two, GameVersion = GameVersion.CodexOrdinian },
            new() { StrategyCardName = StrategyCardName.ConstructionProphecyOfKings, InitiativeOrder = InitiativeOrder.Four, GameVersion = GameVersion.ProphecyOfKing },
        };

        var updatedStrategyCards = strategyCards.Select(strategyCard =>
        {
            strategyCard.Name = $"{strategyCard.StrategyCardName}_{nameof(ObjectiveCard.Name)}";
            strategyCard.Text = $"{strategyCard.StrategyCardName}_{nameof(ObjectiveCard.Text)}";
            strategyCard.Type = CardType.Strategy;
            strategyCard.ImagePath = createCardImagePathService.GetCardImagePath(strategyCard.StrategyCardName, strategyCard.Type);
            return strategyCard;
        }).ToList();

        return updatedStrategyCards;
    }
}
