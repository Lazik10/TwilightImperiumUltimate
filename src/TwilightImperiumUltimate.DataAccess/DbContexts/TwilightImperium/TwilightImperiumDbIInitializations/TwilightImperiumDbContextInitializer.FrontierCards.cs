using TwilightImperiumUltimate.Core.Entities.Cards;
using TwilightImperiumUltimate.Core.Enums.Cards;

namespace TwilightImperiumUltimate.DataAccess.DbContexts.TwilightImperium;

public partial class TwilightImperiumDbContextInitializer
{
    private void InitializeFrontierCards()
    {
        using var context = _dbContextFactory.CreateDbContext();

        var frontierCards = new List<FrontierCard>()
        {
            new FrontierCard()
            {
                FrontierCardName = FrontierCardName.DeadWorld,
                GameVersion = GameVersion.CodexVigil,
            },
            new FrontierCard()
            {
                FrontierCardName = FrontierCardName.DerelictVessel,
                GameVersion = GameVersion.ProphecyOfKing,
            },
            new FrontierCard()
            {
                FrontierCardName = FrontierCardName.EnigmaticDevice,
                GameVersion = GameVersion.ProphecyOfKing,
            },
            new FrontierCard()
            {
                FrontierCardName = FrontierCardName.EntropicField,
                GameVersion = GameVersion.CodexVigil,
            },
            new FrontierCard()
            {
                FrontierCardName = FrontierCardName.GammaRelay,
                GameVersion = GameVersion.ProphecyOfKing,
            },
            new FrontierCard()
            {
                FrontierCardName = FrontierCardName.IonStorm,
                GameVersion = GameVersion.ProphecyOfKing,
            },
            new FrontierCard()
            {
                FrontierCardName = FrontierCardName.KeleresShip,
                GameVersion = GameVersion.CodexVigil,
            },
            new FrontierCard()
            {
                FrontierCardName = FrontierCardName.LostCrew,
                GameVersion = GameVersion.ProphecyOfKing,
            },
            new FrontierCard()
            {
                FrontierCardName = FrontierCardName.MajorEntropicField,
                GameVersion = GameVersion.CodexVigil,
            },
            new FrontierCard()
            {
                FrontierCardName = FrontierCardName.MerchantStation,
                GameVersion = GameVersion.ProphecyOfKing,
            },
            new FrontierCard()
            {
                FrontierCardName = FrontierCardName.MinorEntropicField,
                GameVersion = GameVersion.CodexVigil,
            },
            new FrontierCard()
            {
                FrontierCardName = FrontierCardName.Mirage,
                GameVersion = GameVersion.ProphecyOfKing,
            },
            new FrontierCard()
            {
                FrontierCardName = FrontierCardName.UnknownRelicFragment,
                GameVersion = GameVersion.ProphecyOfKing,
            },
        };

        var updatedFrontierCards = frontierCards.Select(frontierCard =>
        {
            frontierCard.Name = $"{frontierCard.FrontierCardName}_{nameof(ObjectiveCard.Name)}";
            frontierCard.Text = $"{frontierCard.FrontierCardName}_{nameof(ObjectiveCard.Text)}";
            frontierCard.Type = CardType.Frontier;
            frontierCard.ImagePath = _cardImagePathService.GetCardImagePath(frontierCard.FrontierCardName, frontierCard.Type);
            return frontierCard;
        });

        context.AddRange(updatedFrontierCards);
        context.SaveChanges();
    }
}
