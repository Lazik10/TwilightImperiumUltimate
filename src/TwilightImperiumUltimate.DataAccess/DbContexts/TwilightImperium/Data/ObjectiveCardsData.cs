using TwilightImperiumUltimate.Core.Entities.Cards;
using TwilightImperiumUltimate.Core.Enums.Cards;
using TwilightImperiumUltimate.DataAccess.Services;

namespace TwilightImperiumUltimate.DataAccess.DbContexts.TwilightImperium.Data;

internal static class ObjectiveCardsData
{
    internal static List<ObjectiveCard> ObjectiveCards => GetObjectiveCards();

    private static List<ObjectiveCard> GetObjectiveCards()
    {
        CreateCardImagePathService createCardImagePathService = new();

        var objectiveCards = new List<ObjectiveCard>()
        {
            // First Stage
            new() { ObjectiveCardName = ObjectiveCardName.AMassWealth, GameVersion = GameVersion.ProphecyOfKing, ObjectiveCardType = ObjectiveCardType.StageOne, Type = CardType.Objective },
            new() { ObjectiveCardName = ObjectiveCardName.BuildDefenses, GameVersion = GameVersion.ProphecyOfKing, ObjectiveCardType = ObjectiveCardType.StageOne, Type = CardType.Objective },
            new() { ObjectiveCardName = ObjectiveCardName.CornerTheMarket, GameVersion = GameVersion.BaseGame, ObjectiveCardType = ObjectiveCardType.StageOne, Type = CardType.Objective },
            new() { ObjectiveCardName = ObjectiveCardName.DevelopWeaponry, GameVersion = GameVersion.BaseGame, ObjectiveCardType = ObjectiveCardType.StageOne, Type = CardType.Objective },
            new() { ObjectiveCardName = ObjectiveCardName.DiscoverLostOutposts, GameVersion = GameVersion.ProphecyOfKing, ObjectiveCardType = ObjectiveCardType.StageOne, Type = CardType.Objective },
            new() { ObjectiveCardName = ObjectiveCardName.DiversifyResearch, GameVersion = GameVersion.BaseGame, ObjectiveCardType = ObjectiveCardType.StageOne, Type = CardType.Objective },
            new() { ObjectiveCardName = ObjectiveCardName.EngineerAMarvel, GameVersion = GameVersion.ProphecyOfKing, ObjectiveCardType = ObjectiveCardType.StageOne, Type = CardType.Objective },
            new() { ObjectiveCardName = ObjectiveCardName.ErectAMonument, GameVersion = GameVersion.BaseGame, ObjectiveCardType = ObjectiveCardType.StageOne, Type = CardType.Objective },
            new() { ObjectiveCardName = ObjectiveCardName.ExpandBorders, GameVersion = GameVersion.BaseGame, ObjectiveCardType = ObjectiveCardType.StageOne, Type = CardType.Objective },
            new() { ObjectiveCardName = ObjectiveCardName.ExploreDeepSpace, GameVersion = GameVersion.ProphecyOfKing, ObjectiveCardType = ObjectiveCardType.StageOne, Type = CardType.Objective },
            new() { ObjectiveCardName = ObjectiveCardName.FoundResearchOutposts, GameVersion = GameVersion.BaseGame, ObjectiveCardType = ObjectiveCardType.StageOne, Type = CardType.Objective },
            new() { ObjectiveCardName = ObjectiveCardName.ImproveInfrastructure, GameVersion = GameVersion.ProphecyOfKing, ObjectiveCardType = ObjectiveCardType.StageOne, Type = CardType.Objective },
            new() { ObjectiveCardName = ObjectiveCardName.IntimidateCouncil, GameVersion = GameVersion.BaseGame, ObjectiveCardType = ObjectiveCardType.StageOne, Type = CardType.Objective },
            new() { ObjectiveCardName = ObjectiveCardName.LeadFromTheFront, GameVersion = GameVersion.BaseGame, ObjectiveCardType = ObjectiveCardType.StageOne, Type = CardType.Objective },
            new() { ObjectiveCardName = ObjectiveCardName.MakeHistory, GameVersion = GameVersion.ProphecyOfKing, ObjectiveCardType = ObjectiveCardType.StageOne, Type = CardType.Objective },
            new() { ObjectiveCardName = ObjectiveCardName.NegotiateTradeRoutes, GameVersion = GameVersion.BaseGame, ObjectiveCardType = ObjectiveCardType.StageOne, Type = CardType.Objective },
            new() { ObjectiveCardName = ObjectiveCardName.PopulateTheOuterRim, GameVersion = GameVersion.ProphecyOfKing, ObjectiveCardType = ObjectiveCardType.StageOne, Type = CardType.Objective },
            new() { ObjectiveCardName = ObjectiveCardName.PushBoundaries, GameVersion = GameVersion.ProphecyOfKing, ObjectiveCardType = ObjectiveCardType.StageOne, Type = CardType.Objective },
            new() { ObjectiveCardName = ObjectiveCardName.RaiseAFleet, GameVersion = GameVersion.ProphecyOfKing, ObjectiveCardType = ObjectiveCardType.StageOne, Type = CardType.Objective },
            new() { ObjectiveCardName = ObjectiveCardName.SwayTheCouncil, GameVersion = GameVersion.BaseGame, ObjectiveCardType = ObjectiveCardType.StageOne, Type = CardType.Objective },

            // Second Stage
            new() { ObjectiveCardName = ObjectiveCardName.AchieveSupremacy, GameVersion = GameVersion.ProphecyOfKing, ObjectiveCardType = ObjectiveCardType.StageTwo, Type = CardType.Objective },
            new() { ObjectiveCardName = ObjectiveCardName.BecomeALegend, GameVersion = GameVersion.ProphecyOfKing, ObjectiveCardType = ObjectiveCardType.StageTwo, Type = CardType.Objective },
            new() { ObjectiveCardName = ObjectiveCardName.CentralizeGalacticTrade, GameVersion = GameVersion.BaseGame, ObjectiveCardType = ObjectiveCardType.StageTwo, Type = CardType.Objective },
            new() { ObjectiveCardName = ObjectiveCardName.CommandAnArmada, GameVersion = GameVersion.ProphecyOfKing, ObjectiveCardType = ObjectiveCardType.StageTwo, Type = CardType.Objective },
            new() { ObjectiveCardName = ObjectiveCardName.ConquerTheWeak, GameVersion = GameVersion.BaseGame, ObjectiveCardType = ObjectiveCardType.StageTwo, Type = CardType.Objective },
            new() { ObjectiveCardName = ObjectiveCardName.ConstructMassiveCities, GameVersion = GameVersion.ProphecyOfKing, ObjectiveCardType = ObjectiveCardType.StageTwo, Type = CardType.Objective },
            new() { ObjectiveCardName = ObjectiveCardName.ControlTheBorderlands, GameVersion = GameVersion.ProphecyOfKing, ObjectiveCardType = ObjectiveCardType.StageTwo, Type = CardType.Objective },
            new() { ObjectiveCardName = ObjectiveCardName.FormGalacticBrainTrust, GameVersion = GameVersion.BaseGame, ObjectiveCardType = ObjectiveCardType.StageTwo, Type = CardType.Objective },
            new() { ObjectiveCardName = ObjectiveCardName.FoundAGoldenAge, GameVersion = GameVersion.BaseGame, ObjectiveCardType = ObjectiveCardType.StageTwo, Type = CardType.Objective },
            new() { ObjectiveCardName = ObjectiveCardName.GalvanizeThePeople, GameVersion = GameVersion.BaseGame, ObjectiveCardType = ObjectiveCardType.StageTwo, Type = CardType.Objective },
            new() { ObjectiveCardName = ObjectiveCardName.HoldVastReserves, GameVersion = GameVersion.ProphecyOfKing, ObjectiveCardType = ObjectiveCardType.StageTwo, Type = CardType.Objective },
            new() { ObjectiveCardName = ObjectiveCardName.ManipulateGalacticLaw, GameVersion = GameVersion.BaseGame, ObjectiveCardType = ObjectiveCardType.StageTwo, Type = CardType.Objective },
            new() { ObjectiveCardName = ObjectiveCardName.MasterTheSciences, GameVersion = GameVersion.BaseGame, ObjectiveCardType = ObjectiveCardType.StageTwo, Type = CardType.Objective },
            new() { ObjectiveCardName = ObjectiveCardName.PatrolVastTerritories, GameVersion = GameVersion.ProphecyOfKing, ObjectiveCardType = ObjectiveCardType.StageTwo, Type = CardType.Objective },
            new() { ObjectiveCardName = ObjectiveCardName.ProtectTheBorder, GameVersion = GameVersion.ProphecyOfKing, ObjectiveCardType = ObjectiveCardType.StageTwo, Type = CardType.Objective },
            new() { ObjectiveCardName = ObjectiveCardName.ReclaimAncientMonuments, GameVersion = GameVersion.ProphecyOfKing, ObjectiveCardType = ObjectiveCardType.StageTwo, Type = CardType.Objective },
            new() { ObjectiveCardName = ObjectiveCardName.RevolutionizeWarfare, GameVersion = GameVersion.BaseGame, ObjectiveCardType = ObjectiveCardType.StageTwo, Type = CardType.Objective },
            new() { ObjectiveCardName = ObjectiveCardName.RuleDistantLands, GameVersion = GameVersion.ProphecyOfKing, ObjectiveCardType = ObjectiveCardType.StageTwo, Type = CardType.Objective },
            new() { ObjectiveCardName = ObjectiveCardName.SubdueTheGalaxy, GameVersion = GameVersion.BaseGame, ObjectiveCardType = ObjectiveCardType.StageTwo, Type = CardType.Objective },
            new() { ObjectiveCardName = ObjectiveCardName.UnifyTheColonies, GameVersion = GameVersion.BaseGame, ObjectiveCardType = ObjectiveCardType.StageTwo, Type = CardType.Objective },

            // Secrets
            new() { ObjectiveCardName = ObjectiveCardName.AdaptNewStrategies, GameVersion = GameVersion.BaseGame, ObjectiveCardType = ObjectiveCardType.Secret, Type = CardType.Objective },
            new() { ObjectiveCardName = ObjectiveCardName.BecomeAMartyr, GameVersion = GameVersion.ProphecyOfKing, ObjectiveCardType = ObjectiveCardType.Secret, Type = CardType.Objective },
            new() { ObjectiveCardName = ObjectiveCardName.BecomeTheGatekeeper, GameVersion = GameVersion.BaseGame, ObjectiveCardType = ObjectiveCardType.Secret, Type = CardType.Objective },
            new() { ObjectiveCardName = ObjectiveCardName.BetrayAFriend, GameVersion = GameVersion.ProphecyOfKing, ObjectiveCardType = ObjectiveCardType.Secret, Type = CardType.Objective },
            new() { ObjectiveCardName = ObjectiveCardName.BraveTheVoid, GameVersion = GameVersion.ProphecyOfKing, ObjectiveCardType = ObjectiveCardType.Secret, Type = CardType.Objective },
            new() { ObjectiveCardName = ObjectiveCardName.ControlTheRegion, GameVersion = GameVersion.BaseGame, ObjectiveCardType = ObjectiveCardType.Secret, Type = CardType.Objective },
            new() { ObjectiveCardName = ObjectiveCardName.CutSupplyLines, GameVersion = GameVersion.BaseGame, ObjectiveCardType = ObjectiveCardType.Secret, Type = CardType.Objective },
            new() { ObjectiveCardName = ObjectiveCardName.DarkenTheSkies, GameVersion = GameVersion.ProphecyOfKing, ObjectiveCardType = ObjectiveCardType.Secret, Type = CardType.Objective },
            new() { ObjectiveCardName = ObjectiveCardName.DefySpaceAndTime, GameVersion = GameVersion.ProphecyOfKing, ObjectiveCardType = ObjectiveCardType.Secret, Type = CardType.Objective },
            new() { ObjectiveCardName = ObjectiveCardName.DemonstrateYourPower, GameVersion = GameVersion.ProphecyOfKing, ObjectiveCardType = ObjectiveCardType.Secret, Type = CardType.Objective },
            new() { ObjectiveCardName = ObjectiveCardName.DestroyHereticalWorks, GameVersion = GameVersion.ProphecyOfKing, ObjectiveCardType = ObjectiveCardType.Secret, Type = CardType.Objective },
            new() { ObjectiveCardName = ObjectiveCardName.DestroyTheirGreatestShip, GameVersion = GameVersion.BaseGame, ObjectiveCardType = ObjectiveCardType.Secret, Type = CardType.Objective },
            new() { ObjectiveCardName = ObjectiveCardName.DictatePolicy, GameVersion = GameVersion.ProphecyOfKing, ObjectiveCardType = ObjectiveCardType.Secret, Type = CardType.Objective },
            new() { ObjectiveCardName = ObjectiveCardName.DriveTheDebate, GameVersion = GameVersion.ProphecyOfKing, ObjectiveCardType = ObjectiveCardType.Secret, Type = CardType.Objective },
            new() { ObjectiveCardName = ObjectiveCardName.EstablishAPerimeter, GameVersion = GameVersion.BaseGame, ObjectiveCardType = ObjectiveCardType.Secret, Type = CardType.Objective },
            new() { ObjectiveCardName = ObjectiveCardName.EstablishHegemony, GameVersion = GameVersion.ProphecyOfKing, ObjectiveCardType = ObjectiveCardType.Secret, Type = CardType.Objective },
            new() { ObjectiveCardName = ObjectiveCardName.FightWithPrecision, GameVersion = GameVersion.ProphecyOfKing, ObjectiveCardType = ObjectiveCardType.Secret, Type = CardType.Objective },
            new() { ObjectiveCardName = ObjectiveCardName.FightWithPrecisionOmega, GameVersion = GameVersion.CodexVigil, ObjectiveCardType = ObjectiveCardType.Secret, Type = CardType.Objective },
            new() { ObjectiveCardName = ObjectiveCardName.ForgeAnAlliance, GameVersion = GameVersion.BaseGame, ObjectiveCardType = ObjectiveCardType.Secret, Type = CardType.Objective },
            new() { ObjectiveCardName = ObjectiveCardName.FormASpyNetwork, GameVersion = GameVersion.BaseGame, ObjectiveCardType = ObjectiveCardType.Secret, Type = CardType.Objective },
            new() { ObjectiveCardName = ObjectiveCardName.FosterCohesion, GameVersion = GameVersion.ProphecyOfKing, ObjectiveCardType = ObjectiveCardType.Secret, Type = CardType.Objective },
            new() { ObjectiveCardName = ObjectiveCardName.FuelTheWarMachine, GameVersion = GameVersion.BaseGame, ObjectiveCardType = ObjectiveCardType.Secret, Type = CardType.Objective },
            new() { ObjectiveCardName = ObjectiveCardName.GatherAMightyFleet, GameVersion = GameVersion.BaseGame, ObjectiveCardType = ObjectiveCardType.Secret, Type = CardType.Objective },
            new() { ObjectiveCardName = ObjectiveCardName.HoardRawMaterials, GameVersion = GameVersion.ProphecyOfKing, ObjectiveCardType = ObjectiveCardType.Secret, Type = CardType.Objective },
            new() { ObjectiveCardName = ObjectiveCardName.LearnTheSecretsOfTheCosmos, GameVersion = GameVersion.BaseGame, ObjectiveCardType = ObjectiveCardType.Secret, Type = CardType.Objective },
            new() { ObjectiveCardName = ObjectiveCardName.MakeAnExampleOfTheirWorld, GameVersion = GameVersion.BaseGame, ObjectiveCardType = ObjectiveCardType.Secret, Type = CardType.Objective },
            new() { ObjectiveCardName = ObjectiveCardName.MakeAnExampleOfTheirWorldOmega, GameVersion = GameVersion.CodexVigil, ObjectiveCardType = ObjectiveCardType.Secret, Type = CardType.Objective },
            new() { ObjectiveCardName = ObjectiveCardName.MasterTheLawsOfPhysics, GameVersion = GameVersion.BaseGame, ObjectiveCardType = ObjectiveCardType.Secret, Type = CardType.Objective },
            new() { ObjectiveCardName = ObjectiveCardName.MechanizeTheMilitary, GameVersion = GameVersion.ProphecyOfKing, ObjectiveCardType = ObjectiveCardType.Secret, Type = CardType.Objective },
            new() { ObjectiveCardName = ObjectiveCardName.MineRareMinerals, GameVersion = GameVersion.BaseGame, ObjectiveCardType = ObjectiveCardType.Secret, Type = CardType.Objective },
            new() { ObjectiveCardName = ObjectiveCardName.MonopolizeProduction, GameVersion = GameVersion.BaseGame, ObjectiveCardType = ObjectiveCardType.Secret, Type = CardType.Objective },
            new() { ObjectiveCardName = ObjectiveCardName.OccupyTheFringe, GameVersion = GameVersion.ProphecyOfKing, ObjectiveCardType = ObjectiveCardType.Secret, Type = CardType.Objective },
            new() { ObjectiveCardName = ObjectiveCardName.OccupyTheSeatOfTheEmpire, GameVersion = GameVersion.BaseGame, ObjectiveCardType = ObjectiveCardType.Secret, Type = CardType.Objective },
            new() { ObjectiveCardName = ObjectiveCardName.ProduceEnMasse, GameVersion = GameVersion.ProphecyOfKing, ObjectiveCardType = ObjectiveCardType.Secret, Type = CardType.Objective },
            new() { ObjectiveCardName = ObjectiveCardName.ProveEndurance, GameVersion = GameVersion.ProphecyOfKing, ObjectiveCardType = ObjectiveCardType.Secret, Type = CardType.Objective },
            new() { ObjectiveCardName = ObjectiveCardName.SeizeAnIcon, GameVersion = GameVersion.ProphecyOfKing, ObjectiveCardType = ObjectiveCardType.Secret, Type = CardType.Objective },
            new() { ObjectiveCardName = ObjectiveCardName.SparkARebellion, GameVersion = GameVersion.BaseGame, ObjectiveCardType = ObjectiveCardType.Secret, Type = CardType.Objective },
            new() { ObjectiveCardName = ObjectiveCardName.StakeYourClaim, GameVersion = GameVersion.ProphecyOfKing, ObjectiveCardType = ObjectiveCardType.Secret, Type = CardType.Objective },
            new() { ObjectiveCardName = ObjectiveCardName.StrengthenBonds, GameVersion = GameVersion.ProphecyOfKing, ObjectiveCardType = ObjectiveCardType.Secret, Type = CardType.Objective },
            new() { ObjectiveCardName = ObjectiveCardName.ThreatenEnemies, GameVersion = GameVersion.BaseGame, ObjectiveCardType = ObjectiveCardType.Secret, Type = CardType.Objective },
            new() { ObjectiveCardName = ObjectiveCardName.TurnTheirFleetsToDust, GameVersion = GameVersion.BaseGame, ObjectiveCardType = ObjectiveCardType.Secret, Type = CardType.Objective },
            new() { ObjectiveCardName = ObjectiveCardName.TurnTheirFleetsToDustOmega, GameVersion = GameVersion.CodexVigil, ObjectiveCardType = ObjectiveCardType.Secret, Type = CardType.Objective },
            new() { ObjectiveCardName = ObjectiveCardName.UnveilFlagship, GameVersion = GameVersion.BaseGame, ObjectiveCardType = ObjectiveCardType.Secret, Type = CardType.Objective },
        };

        var updatedObjectiveCards = objectiveCards.Select((objectiveCard, i) =>
        {
            objectiveCard.Id = i + 1;
            objectiveCard.Name = $"{objectiveCard.ObjectiveCardName}_{nameof(ObjectiveCard.Name)}";
            objectiveCard.Text = $"{objectiveCard.ObjectiveCardName}_{nameof(ObjectiveCard.Text)}";
            objectiveCard.Type = CardType.Objective;
            objectiveCard.ImagePath = createCardImagePathService.GetCardImagePath(objectiveCard.ObjectiveCardName, objectiveCard.Type);
            return objectiveCard;
        }).ToList();

        return updatedObjectiveCards;
    }
}
