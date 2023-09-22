using TwilightImperiumUltimate.Core.Entities.Cards;
using TwilightImperiumUltimate.Core.Enums.Cards;

namespace TwilightImperiumUltimate.DataAccess.DbContexts.TwilightImperium;

public partial class TwilightImperiumDbContextInitializer
{
    private void InitializeStrategyCards()
    {
        using var context = _dbContextFactory.CreateDbContext();

        var strategyCards = new List<StrategyCard>()
        {
            new StrategyCard()
            {
                StrategyCardName = StrategyCardName.Leadership,
                InitiativeOrder = InitiativeOrder.One,
                GameVersion = GameVersion.BaseGame,
            },
            new StrategyCard()
            {
                StrategyCardName = StrategyCardName.Diplomacy,
                InitiativeOrder = InitiativeOrder.Two,
                GameVersion = GameVersion.BaseGame,
            },
            new StrategyCard()
            {
                StrategyCardName = StrategyCardName.Politics,
                InitiativeOrder = InitiativeOrder.Three,
                GameVersion = GameVersion.BaseGame,
            },
            new StrategyCard()
            {
                StrategyCardName = StrategyCardName.Construction,
                InitiativeOrder = InitiativeOrder.Four,
                GameVersion = GameVersion.BaseGame,
            },
            new StrategyCard()
            {
                StrategyCardName = StrategyCardName.Trade,
                InitiativeOrder = InitiativeOrder.Five,
                GameVersion = GameVersion.BaseGame,
            },
            new StrategyCard()
            {
                StrategyCardName = StrategyCardName.Warfare,
                InitiativeOrder = InitiativeOrder.Six,
                GameVersion = GameVersion.BaseGame,
            },
            new StrategyCard()
            {
                StrategyCardName = StrategyCardName.Technology,
                InitiativeOrder = InitiativeOrder.Seven,
                GameVersion = GameVersion.BaseGame,
            },
            new StrategyCard()
            {
                StrategyCardName = StrategyCardName.Imperial,
                InitiativeOrder = InitiativeOrder.Eight,
                GameVersion = GameVersion.BaseGame,
            },

            // Updated in Prophecy of Kings
            new StrategyCard()
            {
                StrategyCardName = StrategyCardName.DiplomacyCodexOrdinian,
                InitiativeOrder = InitiativeOrder.Two,
                GameVersion = GameVersion.CodexOrdinian,
            },
            new StrategyCard()
            {
                StrategyCardName = StrategyCardName.ConstructionProphecyOfKings,
                InitiativeOrder = InitiativeOrder.Four,
                GameVersion = GameVersion.ProphecyOfKing,
            },
        };

        var updatedStrategyCards = strategyCards.Select(strategyCard =>
        {
            strategyCard.Name = $"{strategyCard.StrategyCardName}_{nameof(ObjectiveCard.Name)}";
            strategyCard.Text = $"{strategyCard.StrategyCardName}_{nameof(ObjectiveCard.Text)}";
            strategyCard.Type = CardType.Strategy;
            strategyCard.ImagePath = _cardImagePathService.GetCardImagePath(strategyCard.StrategyCardName, strategyCard.Type);
            return strategyCard;
        });

        context.AddRange(updatedStrategyCards);
        context.SaveChanges();
    }
}
