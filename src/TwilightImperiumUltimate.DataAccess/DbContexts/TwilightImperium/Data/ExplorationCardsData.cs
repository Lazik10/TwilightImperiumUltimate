using TwilightImperiumUltimate.Core.Entities.Cards;
using TwilightImperiumUltimate.Core.Enums.Cards;
using TwilightImperiumUltimate.DataAccess.Services;

namespace TwilightImperiumUltimate.DataAccess.DbContexts.TwilightImperium.Data;

internal static class ExplorationCardsData
{
    internal static List<ExplorationCard> ExplorationCards => GetExplorationCards();

    private static List<ExplorationCard> GetExplorationCards()
    {
        CreateCardImagePathService createCardImagePathService = new();

        var explorationCards = new List<ExplorationCard>
        {
            // Cultural
            new() { ExplorationCardName = ExplorationCardName.CulturalRelicFragment, ExplorationPlanetTrait = PlanetTrait.Cultural },
            new() { ExplorationCardName = ExplorationCardName.DemilitarizedZone, ExplorationPlanetTrait = PlanetTrait.Cultural },
            new() { ExplorationCardName = ExplorationCardName.DysonSphere, ExplorationPlanetTrait = PlanetTrait.Cultural },
            new() { ExplorationCardName = ExplorationCardName.Freelancers, ExplorationPlanetTrait = PlanetTrait.Cultural },
            new() { ExplorationCardName = ExplorationCardName.GammaWormhole, ExplorationPlanetTrait = PlanetTrait.Cultural },
            new() { ExplorationCardName = ExplorationCardName.MercenaryOutfit, ExplorationPlanetTrait = PlanetTrait.Cultural },
            new() { ExplorationCardName = ExplorationCardName.ParadiseWorld, ExplorationPlanetTrait = PlanetTrait.Cultural },
            new() { ExplorationCardName = ExplorationCardName.TombOfEmphidia, ExplorationPlanetTrait = PlanetTrait.Cultural },

            // Hazardous
            new() { ExplorationCardName = ExplorationCardName.CoreMine, ExplorationPlanetTrait = PlanetTrait.Hazardous },
            new() { ExplorationCardName = ExplorationCardName.Expedition, ExplorationPlanetTrait = PlanetTrait.Hazardous },
            new() { ExplorationCardName = ExplorationCardName.HazardousRelicFragment, ExplorationPlanetTrait = PlanetTrait.Hazardous },
            new() { ExplorationCardName = ExplorationCardName.LazaxSurvivors, ExplorationPlanetTrait = PlanetTrait.Hazardous },
            new() { ExplorationCardName = ExplorationCardName.MiningWorld, ExplorationPlanetTrait = PlanetTrait.Hazardous },
            new() { ExplorationCardName = ExplorationCardName.RichWorld, ExplorationPlanetTrait = PlanetTrait.Hazardous },
            new() { ExplorationCardName = ExplorationCardName.VolatileFuelSource, ExplorationPlanetTrait = PlanetTrait.Hazardous },
            new() { ExplorationCardName = ExplorationCardName.WarfareResearchFacility, ExplorationPlanetTrait = PlanetTrait.Hazardous },

            // Industrial
            new() { ExplorationCardName = ExplorationCardName.AbandonedWarehouses, ExplorationPlanetTrait = PlanetTrait.Industrial },
            new() { ExplorationCardName = ExplorationCardName.BioticResearchFacility, ExplorationPlanetTrait = PlanetTrait.Industrial },
            new() { ExplorationCardName = ExplorationCardName.CyberneticResearchFacility, ExplorationPlanetTrait = PlanetTrait.Industrial },
            new() { ExplorationCardName = ExplorationCardName.FunctioningBase, ExplorationPlanetTrait = PlanetTrait.Industrial },
            new() { ExplorationCardName = ExplorationCardName.IndustrialRelicFragment, ExplorationPlanetTrait = PlanetTrait.Industrial },
            new() { ExplorationCardName = ExplorationCardName.LocalFabricators, ExplorationPlanetTrait = PlanetTrait.Industrial },
            new() { ExplorationCardName = ExplorationCardName.PropulsionResearchFacility, ExplorationPlanetTrait = PlanetTrait.Industrial },
        };

        var updatedExplorationCards = explorationCards.Select((explorationCard, i) =>
        {
            explorationCard.Id = i + 1;
            explorationCard.Name = $"{explorationCard.ExplorationCardName}_{nameof(ObjectiveCard.Name)}";
            explorationCard.Text = $"{explorationCard.ExplorationCardName}_{nameof(ObjectiveCard.Text)}";
            explorationCard.Type = CardType.Exploration;
            explorationCard.ImagePath = createCardImagePathService.GetCardImagePath(explorationCard.ExplorationCardName, explorationCard.Type);
            explorationCard.GameVersion = GameVersion.ProphecyOfKing;
            return explorationCard;
        }).ToList();

        return updatedExplorationCards;
    }
}
