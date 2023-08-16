using TwilightImperiumUltimate.Core.Entities.Cards;
using TwilightImperiumUltimate.Core.Enums.Cards;

namespace TwilightImperiumUltimate.DataAccess.DbContexts.TwilightImperium;

public partial class TwilightImperiumDbContextInitializer
{
    private void InitializeRelicCards()
    {
        using var context = _dbContextFactory.CreateDbContext();

        var relicCards = new List<RelicCard>()
        {
            new RelicCard
            {
                RelicCardName = RelicCardName.DynamisCore,
                GameVersion = GameVersion.CodexAffinity,
            },
            new RelicCard
            {
                RelicCardName = RelicCardName.DominusOrb,
                GameVersion = GameVersion.ProphecyOfKing,
            },
            new RelicCard
            {
                RelicCardName = RelicCardName.JRXS455OLostTitanPrototype,
                GameVersion = GameVersion.CodexAffinity,
            },
            new RelicCard
            {
                RelicCardName = RelicCardName.MawOfWorlds,
                GameVersion = GameVersion.ProphecyOfKing,
            },
            new RelicCard
            {
                RelicCardName = RelicCardName.NanoForge,
                GameVersion = GameVersion.CodexAffinity,
            },
            new RelicCard
            {
                RelicCardName = RelicCardName.ScepterOfEmelpar,
                GameVersion = GameVersion.ProphecyOfKing,
            },
            new RelicCard
            {
                RelicCardName = RelicCardName.ShardOfTheThrone,
                GameVersion = GameVersion.ProphecyOfKing,
            },
            new RelicCard
            {
                RelicCardName = RelicCardName.StellarConverter,
                GameVersion = GameVersion.ProphecyOfKing,
            },
            new RelicCard
            {
                RelicCardName = RelicCardName.TheCodex,
                GameVersion = GameVersion.ProphecyOfKing,
            },
            new RelicCard
            {
                RelicCardName = RelicCardName.TheCrownOfEmphidia,
                GameVersion = GameVersion.ProphecyOfKing,
            },
            new RelicCard
            {
                RelicCardName = RelicCardName.TheCrownOfThalnos,
                GameVersion = GameVersion.ProphecyOfKing,
            },
            new RelicCard
            {
                RelicCardName = RelicCardName.TheObsidian,
                GameVersion = GameVersion.ProphecyOfKing,
            },
            new RelicCard
            {
                RelicCardName = RelicCardName.TheProphetsTears,
                GameVersion = GameVersion.ProphecyOfKing,
            },
        };

        var updatedRelicCards = relicCards.Select(relicCard =>
        {
            relicCard.Name = $"{relicCard.RelicCardName}_{nameof(ObjectiveCard.Name)}";
            relicCard.Text = $"{relicCard.RelicCardName}_{nameof(ObjectiveCard.Text)}";
            relicCard.Type = CardType.Relic;
            relicCard.ImagePath = _cardImagePathService.GetCardImagePath(relicCard.RelicCardName, relicCard.Type);
            return relicCard;
        });

        context.AddRange(updatedRelicCards);
        context.SaveChanges();
    }
}
