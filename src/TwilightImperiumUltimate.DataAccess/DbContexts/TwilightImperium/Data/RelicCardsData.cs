using TwilightImperiumUltimate.Core.Entities.Cards;
using TwilightImperiumUltimate.Core.Enums.Cards;
using TwilightImperiumUltimate.DataAccess.Services;

namespace TwilightImperiumUltimate.DataAccess.DbContexts.TwilightImperium.Data;

internal static class RelicCardsData
{
    internal static List<RelicCard> RelicCards => GetRelicCards();

    private static List<RelicCard> GetRelicCards()
    {
        CreateCardImagePathService createCardImagePathService = new();

        var relicCards = new List<RelicCard>()
        {
            new() { RelicCardName = RelicCardName.DynamisCore, GameVersion = GameVersion.CodexAffinity },
            new() { RelicCardName = RelicCardName.DominusOrb, GameVersion = GameVersion.ProphecyOfKing },
            new() { RelicCardName = RelicCardName.JRXS455OLostTitanPrototypeAgent, GameVersion = GameVersion.CodexAffinity },
            new() { RelicCardName = RelicCardName.MawOfWorlds, GameVersion = GameVersion.ProphecyOfKing },
            new() { RelicCardName = RelicCardName.NanoForge, GameVersion = GameVersion.CodexAffinity },
            new() { RelicCardName = RelicCardName.ScepterOfEmelpar, GameVersion = GameVersion.ProphecyOfKing },
            new() { RelicCardName = RelicCardName.ShardOfTheThrone, GameVersion = GameVersion.ProphecyOfKing },
            new() { RelicCardName = RelicCardName.StellarConverter, GameVersion = GameVersion.ProphecyOfKing },
            new() { RelicCardName = RelicCardName.TheCodex, GameVersion = GameVersion.ProphecyOfKing },
            new() { RelicCardName = RelicCardName.TheCrownOfEmphidia, GameVersion = GameVersion.ProphecyOfKing },
            new() { RelicCardName = RelicCardName.TheCrownOfThalnos, GameVersion = GameVersion.ProphecyOfKing },
            new() { RelicCardName = RelicCardName.TheObsidian, GameVersion = GameVersion.ProphecyOfKing },
            new() { RelicCardName = RelicCardName.TheProphetsTears, GameVersion = GameVersion.ProphecyOfKing },
        };

        var updatedRelicCards = relicCards.Select((relicCard, i) =>
        {
            relicCard.Id = i + 1;
            relicCard.Name = $"{relicCard.RelicCardName}_{nameof(ObjectiveCard.Name)}";
            relicCard.Text = $"{relicCard.RelicCardName}_{nameof(ObjectiveCard.Text)}";
            relicCard.Type = CardType.Relic;
            relicCard.ImagePath = createCardImagePathService.GetCardImagePath(relicCard.RelicCardName, relicCard.Type);
            return relicCard;
        }).ToList();

        return updatedRelicCards;
    }
}
