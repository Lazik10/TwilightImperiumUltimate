using TwilightImperiumUltimate.Core.Entities.Cards;
using TwilightImperiumUltimate.Core.Enums.Cards;
using TwilightImperiumUltimate.DataAccess.Services;

namespace TwilightImperiumUltimate.DataAccess.DbContexts.TwilightImperium.Data;

internal static class FrontierCardsData
{
    internal static List<FrontierCard> FrontierCards => GetFrontierCards();

    private static List<FrontierCard> GetFrontierCards()
    {
        CreateCardImagePathService createCardImagePathService = new();

        var frontierCards = new List<FrontierCard>
        {
            new() { FrontierCardName = FrontierCardName.DeadWorld, GameVersion = GameVersion.CodexVigil },
            new() { FrontierCardName = FrontierCardName.DerelictVessel, GameVersion = GameVersion.ProphecyOfKing },
            new() { FrontierCardName = FrontierCardName.EnigmaticDevice, GameVersion = GameVersion.ProphecyOfKing },
            new() { FrontierCardName = FrontierCardName.EntropicField, GameVersion = GameVersion.CodexVigil },
            new() { FrontierCardName = FrontierCardName.GammaRelay, GameVersion = GameVersion.ProphecyOfKing },
            new() { FrontierCardName = FrontierCardName.IonStorm, GameVersion = GameVersion.ProphecyOfKing },
            new() { FrontierCardName = FrontierCardName.KeleresShip, GameVersion = GameVersion.CodexVigil },
            new() { FrontierCardName = FrontierCardName.LostCrew, GameVersion = GameVersion.ProphecyOfKing },
            new() { FrontierCardName = FrontierCardName.MajorEntropicField, GameVersion = GameVersion.CodexVigil },
            new() { FrontierCardName = FrontierCardName.MerchantStation, GameVersion = GameVersion.ProphecyOfKing },
            new() { FrontierCardName = FrontierCardName.MinorEntropicField, GameVersion = GameVersion.CodexVigil },
            new() { FrontierCardName = FrontierCardName.Mirage, GameVersion = GameVersion.ProphecyOfKing },
            new() { FrontierCardName = FrontierCardName.UnknownRelicFragment, GameVersion = GameVersion.ProphecyOfKing },
        };

        var updatedFrontierCards = frontierCards.Select((frontierCard, i) =>
        {
            frontierCard.Id = i + 1;
            frontierCard.Name = $"{frontierCard.FrontierCardName}_{nameof(ObjectiveCard.Name)}";
            frontierCard.Text = $"{frontierCard.FrontierCardName}_{nameof(ObjectiveCard.Text)}";
            frontierCard.Type = CardType.Frontier;
            frontierCard.ImagePath = createCardImagePathService.GetCardImagePath(frontierCard.FrontierCardName, frontierCard.Type);
            return frontierCard;
        }).ToList();

        return updatedFrontierCards;
    }
}
