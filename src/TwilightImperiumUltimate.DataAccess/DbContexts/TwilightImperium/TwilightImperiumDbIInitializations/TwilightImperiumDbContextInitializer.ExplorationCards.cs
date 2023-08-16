using TwilightImperiumUltimate.Core.Entities.Cards;
using TwilightImperiumUltimate.Core.Enums.Cards;

namespace TwilightImperiumUltimate.DataAccess.DbContexts.TwilightImperium;

public partial class TwilightImperiumDbContextInitializer
{
    private void InitializeExplorationCard()
    {
        using var context = _dbContextFactory.CreateDbContext();

        var explorationCards = new List<ExplorationCard>()
        {
            // Cultural
            new ExplorationCard()
            {
                ExplorationCardName = ExplorationCardName.CulturalRelicFragment,
                ExplorationPlanetTrait = PlanetTrait.Cultural,
            },
            new ExplorationCard()
            {
                ExplorationCardName = ExplorationCardName.DemilitarizedZone,
                ExplorationPlanetTrait = PlanetTrait.Cultural,
            },
            new ExplorationCard()
            {
                ExplorationCardName = ExplorationCardName.DysonSphere,
                ExplorationPlanetTrait = PlanetTrait.Cultural,
            },
            new ExplorationCard()
            {
                ExplorationCardName = ExplorationCardName.Freelancers,
                ExplorationPlanetTrait = PlanetTrait.Cultural,
            },
            new ExplorationCard()
            {
                ExplorationCardName = ExplorationCardName.GammaWormhole,
                ExplorationPlanetTrait = PlanetTrait.Cultural,
            },
            new ExplorationCard()
            {
                ExplorationCardName = ExplorationCardName.MercenaryOutfit,
                ExplorationPlanetTrait = PlanetTrait.Cultural,
            },
            new ExplorationCard()
            {
                ExplorationCardName = ExplorationCardName.ParadiseWorld,
                ExplorationPlanetTrait = PlanetTrait.Cultural,
            },
            new ExplorationCard()
            {
                ExplorationCardName = ExplorationCardName.TombOfEmphidia,
                ExplorationPlanetTrait = PlanetTrait.Cultural,
            },

            // Hazardous
            new ExplorationCard()
            {
                ExplorationCardName = ExplorationCardName.CoreMine,
                ExplorationPlanetTrait = PlanetTrait.Hazardous,
            },
            new ExplorationCard()
            {
                ExplorationCardName = ExplorationCardName.Expedition,
                ExplorationPlanetTrait = PlanetTrait.Hazardous,
            },
            new ExplorationCard()
            {
                ExplorationCardName = ExplorationCardName.HazardousRelicFragment,
                ExplorationPlanetTrait = PlanetTrait.Hazardous,
            },
            new ExplorationCard()
            {
                ExplorationCardName = ExplorationCardName.LazaxSurvivors,
                ExplorationPlanetTrait = PlanetTrait.Hazardous,
            },
            new ExplorationCard()
            {
                ExplorationCardName = ExplorationCardName.MiningWorld,
                ExplorationPlanetTrait = PlanetTrait.Hazardous,
            },
            new ExplorationCard()
            {
                ExplorationCardName = ExplorationCardName.RichWorld,
                ExplorationPlanetTrait = PlanetTrait.Hazardous,
            },
            new ExplorationCard()
            {
                ExplorationCardName = ExplorationCardName.VolatileFuelSource,
                ExplorationPlanetTrait = PlanetTrait.Hazardous,
            },
            new ExplorationCard()
            {
                ExplorationCardName = ExplorationCardName.WarfareResearchFacility,
                ExplorationPlanetTrait = PlanetTrait.Hazardous,
            },

            // Industrial
            new ExplorationCard()
            {
                ExplorationCardName = ExplorationCardName.AbandonedWarehouses,
                ExplorationPlanetTrait = PlanetTrait.Industrial,
            },
            new ExplorationCard()
            {
                ExplorationCardName = ExplorationCardName.BioticResearchFacility,
                ExplorationPlanetTrait = PlanetTrait.Industrial,
            },
            new ExplorationCard()
            {
                ExplorationCardName = ExplorationCardName.CyberneticResearchFacility,
                ExplorationPlanetTrait = PlanetTrait.Industrial,
            },
            new ExplorationCard()
            {
                ExplorationCardName = ExplorationCardName.FunctioningBase,
                ExplorationPlanetTrait = PlanetTrait.Industrial,
            },
            new ExplorationCard()
            {
                ExplorationCardName = ExplorationCardName.IndustrialRelicFragment,
                ExplorationPlanetTrait = PlanetTrait.Industrial,
            },
            new ExplorationCard()
            {
                ExplorationCardName = ExplorationCardName.LocalFabricators,
                ExplorationPlanetTrait = PlanetTrait.Industrial,
            },
            new ExplorationCard()
            {
                ExplorationCardName = ExplorationCardName.PropulsionResearchFacility,
                ExplorationPlanetTrait = PlanetTrait.Industrial,
            },
        };

        var updatedExplorationCards = explorationCards.Select(explorationCard =>
        {
            explorationCard.Name = $"{explorationCard.ExplorationCardName}_{nameof(ObjectiveCard.Name)}";
            explorationCard.Text = $"{explorationCard.ExplorationCardName}_{nameof(ObjectiveCard.Text)}";
            explorationCard.Type = CardType.Exploration;
            explorationCard.ImagePath = _cardImagePathService.GetCardImagePath(explorationCard.ExplorationCardName, explorationCard.Type);
            explorationCard.GameVersion = GameVersion.ProphecyOfKing;
            return explorationCard;
        });

        context.AddRange(updatedExplorationCards);
        context.SaveChanges();
    }
}
