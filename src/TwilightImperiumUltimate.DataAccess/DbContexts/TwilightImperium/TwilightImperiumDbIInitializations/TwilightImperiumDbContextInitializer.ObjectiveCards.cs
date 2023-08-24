using TwilightImperiumUltimate.Core.Entities.Cards;
using TwilightImperiumUltimate.Core.Enums.Cards;

namespace TwilightImperiumUltimate.DataAccess.DbContexts.TwilightImperium;

public partial class TwilightImperiumDbContextInitializer
{
    private void InitializeObjectiveCards()
    {
        using var context = _dbContextFactory.CreateDbContext();

        var objectiveCards = new List<ObjectiveCard>()
        {
            // First Stage
            new ObjectiveCard()
            {
                ObjectiveCardName = ObjectiveCardName.AmassWealth,
                GameVersion = GameVersion.ProphecyOfKing,
                ObjectiveCardType = ObjectiveCardType.StageOne,
                Type = CardType.Objective,
            },
            new ObjectiveCard()
            {
                ObjectiveCardName = ObjectiveCardName.BuildDefenses,
                GameVersion = GameVersion.ProphecyOfKing,
                ObjectiveCardType = ObjectiveCardType.StageOne,
                Type = CardType.Objective,
            },
            new ObjectiveCard()
            {
                ObjectiveCardName = ObjectiveCardName.CornerTheMarket,
                GameVersion = GameVersion.BaseGame,
                ObjectiveCardType = ObjectiveCardType.StageOne,
                Type = CardType.Objective,
            },
            new ObjectiveCard()
            {
                ObjectiveCardName = ObjectiveCardName.DevelopWeaponry,
                GameVersion = GameVersion.BaseGame,
                ObjectiveCardType = ObjectiveCardType.StageOne,
                Type = CardType.Objective,
            },
            new ObjectiveCard()
            {
                ObjectiveCardName = ObjectiveCardName.DiscoverLostOutposts,
                GameVersion = GameVersion.ProphecyOfKing,
                ObjectiveCardType = ObjectiveCardType.StageOne,
                Type = CardType.Objective,
            },
            new ObjectiveCard()
            {
                ObjectiveCardName = ObjectiveCardName.DiversifyRearch,
                GameVersion = GameVersion.BaseGame,
                ObjectiveCardType = ObjectiveCardType.StageOne,
                Type = CardType.Objective,
            },
            new ObjectiveCard()
            {
                ObjectiveCardName = ObjectiveCardName.EngineerAMarvel,
                GameVersion = GameVersion.ProphecyOfKing,
                ObjectiveCardType = ObjectiveCardType.StageOne,
                Type = CardType.Objective,
            },
            new ObjectiveCard()
            {
                ObjectiveCardName = ObjectiveCardName.ErectAMonument,
                GameVersion = GameVersion.BaseGame,
                ObjectiveCardType = ObjectiveCardType.StageOne,
                Type = CardType.Objective,
            },
            new ObjectiveCard()
            {
                ObjectiveCardName = ObjectiveCardName.ExpandBorders,
                GameVersion = GameVersion.BaseGame,
                ObjectiveCardType = ObjectiveCardType.StageOne,
                Type = CardType.Objective,
            },
            new ObjectiveCard()
            {
                ObjectiveCardName = ObjectiveCardName.ExploreDeepSpace,
                GameVersion = GameVersion.ProphecyOfKing,
                ObjectiveCardType = ObjectiveCardType.StageOne,
                Type = CardType.Objective,
            },
            new ObjectiveCard()
            {
                ObjectiveCardName = ObjectiveCardName.FoundResearchOutposts,
                GameVersion = GameVersion.BaseGame,
                ObjectiveCardType = ObjectiveCardType.StageOne,
                Type = CardType.Objective,
            },
            new ObjectiveCard()
            {
                ObjectiveCardName = ObjectiveCardName.ImproveInfrastructure,
                GameVersion = GameVersion.ProphecyOfKing,
                ObjectiveCardType = ObjectiveCardType.StageOne,
                Type = CardType.Objective,
            },
            new ObjectiveCard()
            {
                ObjectiveCardName = ObjectiveCardName.IntimidateTheCouncil,
                GameVersion = GameVersion.BaseGame,
                ObjectiveCardType = ObjectiveCardType.StageOne,
                Type = CardType.Objective,
            },
            new ObjectiveCard()
            {
                ObjectiveCardName = ObjectiveCardName.LeadFromTheFront,
                GameVersion = GameVersion.BaseGame,
                ObjectiveCardType = ObjectiveCardType.StageOne,
                Type = CardType.Objective,
            },
            new ObjectiveCard()
            {
                ObjectiveCardName = ObjectiveCardName.MakeHistory,
                GameVersion = GameVersion.ProphecyOfKing,
                ObjectiveCardType = ObjectiveCardType.StageOne,
                Type = CardType.Objective,
            },
            new ObjectiveCard()
            {
                ObjectiveCardName = ObjectiveCardName.NegotiateTradeRoutes,
                GameVersion = GameVersion.BaseGame,
                ObjectiveCardType = ObjectiveCardType.StageOne,
                Type = CardType.Objective,
            },
            new ObjectiveCard()
            {
                ObjectiveCardName = ObjectiveCardName.PopulateTheOuterRim,
                GameVersion = GameVersion.ProphecyOfKing,
                ObjectiveCardType = ObjectiveCardType.StageOne,
                Type = CardType.Objective,
            },
            new ObjectiveCard()
            {
                ObjectiveCardName = ObjectiveCardName.PushBoundaries,
                GameVersion = GameVersion.ProphecyOfKing,
                ObjectiveCardType = ObjectiveCardType.StageOne,
                Type = CardType.Objective,
            },
            new ObjectiveCard()
            {
                ObjectiveCardName = ObjectiveCardName.RaiseAFleet,
                GameVersion = GameVersion.ProphecyOfKing,
                ObjectiveCardType = ObjectiveCardType.StageOne,
                Type = CardType.Objective,
            },
            new ObjectiveCard()
            {
                ObjectiveCardName = ObjectiveCardName.SwayTheCouncil,
                GameVersion = GameVersion.BaseGame,
                ObjectiveCardType = ObjectiveCardType.StageOne,
                Type = CardType.Objective,
            },

            // Second Stage
            new ObjectiveCard()
            {
                ObjectiveCardName = ObjectiveCardName.AchieveSupremacy,
                GameVersion = GameVersion.ProphecyOfKing,
                ObjectiveCardType = ObjectiveCardType.StageTwo,
                Type = CardType.Objective,
            },
            new ObjectiveCard()
            {
                ObjectiveCardName = ObjectiveCardName.BecomeALegend,
                GameVersion = GameVersion.ProphecyOfKing,
                ObjectiveCardType = ObjectiveCardType.StageTwo,
                Type = CardType.Objective,
            },
            new ObjectiveCard()
            {
                ObjectiveCardName = ObjectiveCardName.CentralizeGalacticTrade,
                GameVersion = GameVersion.BaseGame,
                ObjectiveCardType = ObjectiveCardType.StageTwo,
                Type = CardType.Objective,
            },
            new ObjectiveCard()
            {
                ObjectiveCardName = ObjectiveCardName.CommandAnArmada,
                GameVersion = GameVersion.ProphecyOfKing,
                ObjectiveCardType = ObjectiveCardType.StageTwo,
                Type = CardType.Objective,
            },
            new ObjectiveCard()
            {
                ObjectiveCardName = ObjectiveCardName.ConquerTheWeak,
                GameVersion = GameVersion.BaseGame,
                ObjectiveCardType = ObjectiveCardType.StageTwo,
                Type = CardType.Objective,
            },
            new ObjectiveCard()
            {
                ObjectiveCardName = ObjectiveCardName.ConstructMassiveCities,
                GameVersion = GameVersion.ProphecyOfKing,
                ObjectiveCardType = ObjectiveCardType.StageTwo,
                Type = CardType.Objective,
            },
            new ObjectiveCard()
            {
                ObjectiveCardName = ObjectiveCardName.ControlTheBorderlands,
                GameVersion = GameVersion.ProphecyOfKing,
                ObjectiveCardType = ObjectiveCardType.StageTwo,
                Type = CardType.Objective,
            },
            new ObjectiveCard()
            {
                ObjectiveCardName = ObjectiveCardName.FormGalacticBrainTrust,
                GameVersion = GameVersion.BaseGame,
                ObjectiveCardType = ObjectiveCardType.StageTwo,
                Type = CardType.Objective,
            },
            new ObjectiveCard()
            {
                ObjectiveCardName = ObjectiveCardName.FoundAGoldenAge,
                GameVersion = GameVersion.BaseGame,
                ObjectiveCardType = ObjectiveCardType.StageTwo,
                Type = CardType.Objective,
            },
            new ObjectiveCard()
            {
                ObjectiveCardName = ObjectiveCardName.GalvanizeThePeople,
                GameVersion = GameVersion.BaseGame,
                ObjectiveCardType = ObjectiveCardType.StageTwo,
                Type = CardType.Objective,
            },
            new ObjectiveCard()
            {
                ObjectiveCardName = ObjectiveCardName.HoldVastReserves,
                GameVersion = GameVersion.ProphecyOfKing,
                ObjectiveCardType = ObjectiveCardType.StageTwo,
                Type = CardType.Objective,
            },
            new ObjectiveCard()
            {
                ObjectiveCardName = ObjectiveCardName.ManipulateGalacticLaw,
                GameVersion = GameVersion.BaseGame,
                ObjectiveCardType = ObjectiveCardType.StageTwo,
                Type = CardType.Objective,
            },
            new ObjectiveCard()
            {
                ObjectiveCardName = ObjectiveCardName.MasterOfSciences,
                GameVersion = GameVersion.BaseGame,
                ObjectiveCardType = ObjectiveCardType.StageTwo,
                Type = CardType.Objective,
            },
            new ObjectiveCard()
            {
                ObjectiveCardName = ObjectiveCardName.PatrolVastTerritories,
                GameVersion = GameVersion.ProphecyOfKing,
                ObjectiveCardType = ObjectiveCardType.StageTwo,
                Type = CardType.Objective,
            },
            new ObjectiveCard()
            {
                ObjectiveCardName = ObjectiveCardName.ProtectTheBorder,
                GameVersion = GameVersion.ProphecyOfKing,
                ObjectiveCardType = ObjectiveCardType.StageTwo,
                Type = CardType.Objective,
            },
            new ObjectiveCard()
            {
                ObjectiveCardName = ObjectiveCardName.ReclaimAncientMonuments,
                GameVersion = GameVersion.ProphecyOfKing,
                ObjectiveCardType = ObjectiveCardType.StageTwo,
                Type = CardType.Objective,
            },
            new ObjectiveCard()
            {
                ObjectiveCardName = ObjectiveCardName.RevolutionizeWarfare,
                GameVersion = GameVersion.BaseGame,
                ObjectiveCardType = ObjectiveCardType.StageTwo,
                Type = CardType.Objective,
            },
            new ObjectiveCard()
            {
                ObjectiveCardName = ObjectiveCardName.RuleDistantLands,
                GameVersion = GameVersion.ProphecyOfKing,
                ObjectiveCardType = ObjectiveCardType.StageTwo,
                Type = CardType.Objective,
            },
            new ObjectiveCard()
            {
                ObjectiveCardName = ObjectiveCardName.SubdueTheGalaxy,
                GameVersion = GameVersion.BaseGame,
                ObjectiveCardType = ObjectiveCardType.StageTwo,
                Type = CardType.Objective,
            },
            new ObjectiveCard()
            {
                ObjectiveCardName = ObjectiveCardName.UnifyTheColonies,
                GameVersion = GameVersion.BaseGame,
                ObjectiveCardType = ObjectiveCardType.StageTwo,
                Type = CardType.Objective,
            },

            // Secrets
            new ObjectiveCard()
            {
                ObjectiveCardName = ObjectiveCardName.AdaptNewStrategies,
                GameVersion = GameVersion.BaseGame,
                ObjectiveCardType = ObjectiveCardType.Secret,
                Type = CardType.Objective,
            },
            new ObjectiveCard()
            {
                ObjectiveCardName = ObjectiveCardName.BecomeAMartyr,
                GameVersion = GameVersion.ProphecyOfKing,
                ObjectiveCardType = ObjectiveCardType.Secret,
                Type = CardType.Objective,
            },
            new ObjectiveCard()
            {
                ObjectiveCardName = ObjectiveCardName.BecomeTheGatekeeper,
                GameVersion = GameVersion.BaseGame,
                ObjectiveCardType = ObjectiveCardType.Secret,
                Type = CardType.Objective,
            },
            new ObjectiveCard()
            {
                ObjectiveCardName = ObjectiveCardName.BetrayAFriend,
                GameVersion = GameVersion.ProphecyOfKing,
                ObjectiveCardType = ObjectiveCardType.Secret,
                Type = CardType.Objective,
            },
            new ObjectiveCard()
            {
                ObjectiveCardName = ObjectiveCardName.BraveTheVoid,
                GameVersion = GameVersion.ProphecyOfKing,
                ObjectiveCardType = ObjectiveCardType.Secret,
                Type = CardType.Objective,
            },
            new ObjectiveCard()
            {
                ObjectiveCardName = ObjectiveCardName.ControlTheRegion,
                GameVersion = GameVersion.BaseGame,
                ObjectiveCardType = ObjectiveCardType.Secret,
                Type = CardType.Objective,
            },
            new ObjectiveCard()
            {
                ObjectiveCardName = ObjectiveCardName.CutSupplyLines,
                GameVersion = GameVersion.BaseGame,
                ObjectiveCardType = ObjectiveCardType.Secret,
                Type = CardType.Objective,
            },
            new ObjectiveCard()
            {
                ObjectiveCardName = ObjectiveCardName.DarkenTheSkies,
                GameVersion = GameVersion.ProphecyOfKing,
                ObjectiveCardType = ObjectiveCardType.Secret,
                Type = CardType.Objective,
            },
            new ObjectiveCard()
            {
                ObjectiveCardName = ObjectiveCardName.DefySpaceAndTime,
                GameVersion = GameVersion.ProphecyOfKing,
                ObjectiveCardType = ObjectiveCardType.Secret,
                Type = CardType.Objective,
            },
            new ObjectiveCard()
            {
                ObjectiveCardName = ObjectiveCardName.DemonstrateYourPower,
                GameVersion = GameVersion.ProphecyOfKing,
                ObjectiveCardType = ObjectiveCardType.Secret,
                Type = CardType.Objective,
            },
            new ObjectiveCard()
            {
                ObjectiveCardName = ObjectiveCardName.DestroyHereticalWorks,
                GameVersion = GameVersion.ProphecyOfKing,
                ObjectiveCardType = ObjectiveCardType.Secret,
                Type = CardType.Objective,
            },
            new ObjectiveCard()
            {
                ObjectiveCardName = ObjectiveCardName.DestroyTheirGreatestShip,
                GameVersion = GameVersion.BaseGame,
                ObjectiveCardType = ObjectiveCardType.Secret,
                Type = CardType.Objective,
            },
            new ObjectiveCard()
            {
                ObjectiveCardName = ObjectiveCardName.DictatePolicy,
                GameVersion = GameVersion.ProphecyOfKing,
                ObjectiveCardType = ObjectiveCardType.Secret,
                Type = CardType.Objective,
            },
            new ObjectiveCard()
            {
                ObjectiveCardName = ObjectiveCardName.DriveTheDebate,
                GameVersion = GameVersion.ProphecyOfKing,
                ObjectiveCardType = ObjectiveCardType.Secret,
                Type = CardType.Objective,
            },
            new ObjectiveCard()
            {
                ObjectiveCardName = ObjectiveCardName.EstablishAPerimeter,
                GameVersion = GameVersion.BaseGame,
                ObjectiveCardType = ObjectiveCardType.Secret,
                Type = CardType.Objective,
            },
            new ObjectiveCard()
            {
                ObjectiveCardName = ObjectiveCardName.EstablishHegemony,
                GameVersion = GameVersion.ProphecyOfKing,
                ObjectiveCardType = ObjectiveCardType.Secret,
                Type = CardType.Objective,
            },
            new ObjectiveCard()
            {
                ObjectiveCardName = ObjectiveCardName.FightWithPrecision,
                GameVersion = GameVersion.ProphecyOfKing,
                ObjectiveCardType = ObjectiveCardType.Secret,
                Type = CardType.Objective,
            },
            new ObjectiveCard()
            {
                ObjectiveCardName = ObjectiveCardName.FightWithPrecisionOmega,
                GameVersion = GameVersion.CodexVigil,
                ObjectiveCardType = ObjectiveCardType.Secret,
                Type = CardType.Objective,
            },
            new ObjectiveCard()
            {
                ObjectiveCardName = ObjectiveCardName.ForgeAnAlliance,
                GameVersion = GameVersion.BaseGame,
                ObjectiveCardType = ObjectiveCardType.Secret,
                Type = CardType.Objective,
            },
            new ObjectiveCard()
            {
                ObjectiveCardName = ObjectiveCardName.FormASpyNetwork,
                GameVersion = GameVersion.BaseGame,
                ObjectiveCardType = ObjectiveCardType.Secret,
                Type = CardType.Objective,
            },
            new ObjectiveCard()
            {
                ObjectiveCardName = ObjectiveCardName.FosterCohesion,
                GameVersion = GameVersion.ProphecyOfKing,
                ObjectiveCardType = ObjectiveCardType.Secret,
                Type = CardType.Objective,
            },
            new ObjectiveCard()
            {
                ObjectiveCardName = ObjectiveCardName.FuelTheWarMachine,
                GameVersion = GameVersion.BaseGame,
                ObjectiveCardType = ObjectiveCardType.Secret,
                Type = CardType.Objective,
            },
            new ObjectiveCard()
            {
                ObjectiveCardName = ObjectiveCardName.GatherAMightyFleet,
                GameVersion = GameVersion.BaseGame,
                ObjectiveCardType = ObjectiveCardType.Secret,
                Type = CardType.Objective,
            },
            new ObjectiveCard()
            {
                ObjectiveCardName = ObjectiveCardName.HoardRawMaterials,
                GameVersion = GameVersion.ProphecyOfKing,
                ObjectiveCardType = ObjectiveCardType.Secret,
                Type = CardType.Objective,
            },
            new ObjectiveCard()
            {
                ObjectiveCardName = ObjectiveCardName.LearnTheSecretsOfTheCosmos,
                GameVersion = GameVersion.BaseGame,
                ObjectiveCardType = ObjectiveCardType.Secret,
                Type = CardType.Objective,
            },
            new ObjectiveCard()
            {
                ObjectiveCardName = ObjectiveCardName.MakeAnExampleOfTheirWorld,
                GameVersion = GameVersion.BaseGame,
                ObjectiveCardType = ObjectiveCardType.Secret,
                Type = CardType.Objective,
            },
            new ObjectiveCard()
            {
                ObjectiveCardName = ObjectiveCardName.MakeAnExampleOfTheirWorldOmega,
                GameVersion = GameVersion.CodexVigil,
                ObjectiveCardType = ObjectiveCardType.Secret,
                Type = CardType.Objective,
            },
            new ObjectiveCard()
            {
                ObjectiveCardName = ObjectiveCardName.MasterTheLawsOfPhysics,
                GameVersion = GameVersion.BaseGame,
                ObjectiveCardType = ObjectiveCardType.Secret,
                Type = CardType.Objective,
            },
            new ObjectiveCard()
            {
                ObjectiveCardName = ObjectiveCardName.MechanizeTheMilitary,
                GameVersion = GameVersion.ProphecyOfKing,
                ObjectiveCardType = ObjectiveCardType.Secret,
                Type = CardType.Objective,
            },
            new ObjectiveCard()
            {
                ObjectiveCardName = ObjectiveCardName.MineRareMinerals,
                GameVersion = GameVersion.BaseGame,
                ObjectiveCardType = ObjectiveCardType.Secret,
                Type = CardType.Objective,
            },
            new ObjectiveCard()
            {
                ObjectiveCardName = ObjectiveCardName.MonopolizeProduction,
                GameVersion = GameVersion.BaseGame,
                ObjectiveCardType = ObjectiveCardType.Secret,
                Type = CardType.Objective,
            },
            new ObjectiveCard()
            {
                ObjectiveCardName = ObjectiveCardName.OccupyTheFringe,
                GameVersion = GameVersion.ProphecyOfKing,
                ObjectiveCardType = ObjectiveCardType.Secret,
                Type = CardType.Objective,
            },
            new ObjectiveCard()
            {
                ObjectiveCardName = ObjectiveCardName.OccupyTheSeatOfTheEmpire,
                GameVersion = GameVersion.BaseGame,
                ObjectiveCardType = ObjectiveCardType.Secret,
                Type = CardType.Objective,
            },
            new ObjectiveCard()
            {
                ObjectiveCardName = ObjectiveCardName.ProduceEnMasse,
                GameVersion = GameVersion.ProphecyOfKing,
                ObjectiveCardType = ObjectiveCardType.Secret,
                Type = CardType.Objective,
            },
            new ObjectiveCard()
            {
                ObjectiveCardName = ObjectiveCardName.ProveEndurance,
                GameVersion = GameVersion.ProphecyOfKing,
                ObjectiveCardType = ObjectiveCardType.Secret,
                Type = CardType.Objective,
            },
            new ObjectiveCard()
            {
                ObjectiveCardName = ObjectiveCardName.SeizeAnIcon,
                GameVersion = GameVersion.ProphecyOfKing,
                ObjectiveCardType = ObjectiveCardType.Secret,
                Type = CardType.Objective,
            },
            new ObjectiveCard()
            {
                ObjectiveCardName = ObjectiveCardName.SparkARebellion,
                GameVersion = GameVersion.BaseGame,
                ObjectiveCardType = ObjectiveCardType.Secret,
                Type = CardType.Objective,
            },
            new ObjectiveCard()
            {
                ObjectiveCardName = ObjectiveCardName.StakeYourClaim,
                GameVersion = GameVersion.ProphecyOfKing,
                ObjectiveCardType = ObjectiveCardType.Secret,
                Type = CardType.Objective,
            },
            new ObjectiveCard()
            {
                ObjectiveCardName = ObjectiveCardName.StrengthenBonds,
                GameVersion = GameVersion.ProphecyOfKing,
                ObjectiveCardType = ObjectiveCardType.Secret,
                Type = CardType.Objective,
            },
            new ObjectiveCard()
            {
                ObjectiveCardName = ObjectiveCardName.ThreatenEnemies,
                GameVersion = GameVersion.BaseGame,
                ObjectiveCardType = ObjectiveCardType.Secret,
                Type = CardType.Objective,
            },
            new ObjectiveCard()
            {
                ObjectiveCardName = ObjectiveCardName.TurnTheirFleetsToDust,
                GameVersion = GameVersion.BaseGame,
                ObjectiveCardType = ObjectiveCardType.Secret,
                Type = CardType.Objective,
            },
            new ObjectiveCard()
            {
                ObjectiveCardName = ObjectiveCardName.TurnTheirFleetsToDustOmega,
                GameVersion = GameVersion.CodexVigil,
                ObjectiveCardType = ObjectiveCardType.Secret,
                Type = CardType.Objective,
            },
            new ObjectiveCard()
            {
                ObjectiveCardName = ObjectiveCardName.UnveilFlagship,
                GameVersion = GameVersion.BaseGame,
                ObjectiveCardType = ObjectiveCardType.Secret,
                Type = CardType.Objective,
            },
        };

        var updatedObjectiveCards = objectiveCards.Select(objectiveCard =>
        {
            objectiveCard.Name = $"{objectiveCard.ObjectiveCardName}_{nameof(ObjectiveCard.Name)}";
            objectiveCard.Text = $"{objectiveCard.ObjectiveCardName}_{nameof(ObjectiveCard.Text)}";
            objectiveCard.Type = CardType.Objective;
            objectiveCard.ImagePath = _cardImagePathService.GetCardImagePath(objectiveCard.ObjectiveCardName, objectiveCard.Type);
            return objectiveCard;
        });

        context.AddRange(updatedObjectiveCards);
        context.SaveChanges();
    }
}
