using TwilightImperiumUltimate.Core.Entities.Cards;
using TwilightImperiumUltimate.Core.Enums.Cards;

namespace TwilightImperiumUltimate.DataAccess.DbContexts.TwilightImperium;

public partial class TwilightImperiumDbContextInitializer
{
    private void InitializeActionCards()
    {
        using var context = _dbContextFactory.CreateDbContext();

        var actionCards = new List<ActionCard>()
        {
            new ActionCard()
            {
                ActionCardName = ActionCardName.AncientBurialSites,
                ActionCardWindow = ActionCardWindow.AgendaPhase,
                GameVersion = GameVersion.BaseGame,
            },
            new ActionCard()
            {
                ActionCardName = ActionCardName.ArchaeologicalExpedition,
                ActionCardWindow = ActionCardWindow.Action,
                GameVersion = GameVersion.ProphecyOfKing,
            },
            new ActionCard()
            {
                ActionCardName = ActionCardName.AssassinateRepresentative,
                ActionCardWindow = ActionCardWindow.AgendaPhase,
                GameVersion = GameVersion.BaseGame,
            },
            new ActionCard()
            {
                ActionCardName = ActionCardName.Blitz,
                ActionCardWindow = ActionCardWindow.Combat,
                GameVersion = GameVersion.BaseGame,
            },
            new ActionCard()
            {
                ActionCardName = ActionCardName.Bribery,
                ActionCardWindow = ActionCardWindow.AgendaPhase,
                GameVersion = GameVersion.BaseGame,
            },
            new ActionCard()
            {
                ActionCardName = ActionCardName.Bunker,
                ActionCardWindow = ActionCardWindow.Combat,
                GameVersion = GameVersion.BaseGame,
            },
            new ActionCard()
            {
                ActionCardName = ActionCardName.ConfoundingLegalText,
                ActionCardWindow = ActionCardWindow.AgendaPhase,
                GameVersion = GameVersion.ProphecyOfKing,
            },
            new ActionCard()
            {
                ActionCardName = ActionCardName.ConfusingLegalText,
                ActionCardWindow = ActionCardWindow.AgendaPhase,
                GameVersion = GameVersion.BaseGame,
            },
            new ActionCard()
            {
                ActionCardName = ActionCardName.ConstructionRider,
                ActionCardWindow = ActionCardWindow.AgendaPhase,
                GameVersion = GameVersion.BaseGame,
            },
            new ActionCard()
            {
                ActionCardName = ActionCardName.Counterstroke,
                ActionCardWindow = ActionCardWindow.Reaction,
                GameVersion = GameVersion.BaseGame,
            },
            new ActionCard()
            {
                ActionCardName = ActionCardName.CoupDetat,
                ActionCardWindow = ActionCardWindow.Reaction,
                GameVersion = GameVersion.ProphecyOfKing,
            },
            new ActionCard()
            {
                ActionCardName = ActionCardName.CourageousToTheEnd,
                ActionCardWindow = ActionCardWindow.Combat,
                GameVersion = GameVersion.BaseGame,
            },
            new ActionCard()
            {
                ActionCardName = ActionCardName.CrippleDefenses,
                ActionCardWindow = ActionCardWindow.Action,
                GameVersion = GameVersion.BaseGame,
            },
            new ActionCard()
            {
                ActionCardName = ActionCardName.DeadlyPlot,
                ActionCardWindow = ActionCardWindow.AgendaPhase,
                GameVersion = GameVersion.ProphecyOfKing,
            },
            new ActionCard()
            {
                ActionCardName = ActionCardName.DecoyOperation,
                ActionCardWindow = ActionCardWindow.TacticalAction,
                GameVersion = GameVersion.ProphecyOfKing,
            },
            new ActionCard()
            {
                ActionCardName = ActionCardName.DiplomacyRider,
                ActionCardWindow = ActionCardWindow.AgendaPhase,
                GameVersion = GameVersion.BaseGame,
            },
            new ActionCard()
            {
                ActionCardName = ActionCardName.DiplomaticPressure,
                ActionCardWindow = ActionCardWindow.AgendaPhase,
                GameVersion = GameVersion.ProphecyOfKing,
            },
            new ActionCard()
            {
                ActionCardName = ActionCardName.DirectHit,
                ActionCardWindow = ActionCardWindow.Combat,
                GameVersion = GameVersion.BaseGame,
            },
            new ActionCard()
            {
                ActionCardName = ActionCardName.Disable,
                ActionCardWindow = ActionCardWindow.Combat,
                GameVersion = GameVersion.BaseGame,
            },
            new ActionCard()
            {
                ActionCardName = ActionCardName.DistinguishedCouncilor,
                ActionCardWindow = ActionCardWindow.AgendaPhase,
                GameVersion = GameVersion.BaseGame,
            },
            new ActionCard()
            {
                ActionCardName = ActionCardName.DivertFunding,
                ActionCardWindow = ActionCardWindow.Action,
                GameVersion = GameVersion.ProphecyOfKing,
            },
            new ActionCard()
            {
                ActionCardName = ActionCardName.EconomicInitiative,
                ActionCardWindow = ActionCardWindow.Action,
                GameVersion = GameVersion.BaseGame,
            },
            new ActionCard()
            {
                ActionCardName = ActionCardName.EmergencyRepairs,
                ActionCardWindow = ActionCardWindow.Combat,
                GameVersion = GameVersion.BaseGame,
            },
            new ActionCard()
            {
                ActionCardName = ActionCardName.ExperimentalBattlestation,
                ActionCardWindow = ActionCardWindow.TacticalAction,
                GameVersion = GameVersion.BaseGame,
            },
            new ActionCard()
            {
                ActionCardName = ActionCardName.ExplorationProbe,
                ActionCardWindow = ActionCardWindow.Action,
                GameVersion = GameVersion.ProphecyOfKing,
            },
            new ActionCard()
            {
                ActionCardName = ActionCardName.FighterConscription,
                ActionCardWindow = ActionCardWindow.Action,
                GameVersion = GameVersion.BaseGame,
            },
            new ActionCard()
            {
                ActionCardName = ActionCardName.FighterPrototype,
                ActionCardWindow = ActionCardWindow.Combat,
                GameVersion = GameVersion.BaseGame,
            },
            new ActionCard()
            {
                ActionCardName = ActionCardName.FireTeam,
                ActionCardWindow = ActionCardWindow.Combat,
                GameVersion = GameVersion.BaseGame,
            },
            new ActionCard()
            {
                ActionCardName = ActionCardName.FlankSpeed,
                ActionCardWindow = ActionCardWindow.TacticalAction,
                GameVersion = GameVersion.BaseGame,
            },
            new ActionCard()
            {
                ActionCardName = ActionCardName.FocusedResearch,
                ActionCardWindow = ActionCardWindow.Action,
                GameVersion = GameVersion.BaseGame,
            },
            new ActionCard()
            {
                ActionCardName = ActionCardName.ForwardSupplyBase,
                ActionCardWindow = ActionCardWindow.Reaction,
                GameVersion = GameVersion.BaseGame,
            },
            new ActionCard()
            {
                ActionCardName = ActionCardName.FrontlineDeployment,
                ActionCardWindow = ActionCardWindow.Action,
                GameVersion = GameVersion.BaseGame,
            },
            new ActionCard()
            {
                ActionCardName = ActionCardName.GhostShip,
                ActionCardWindow = ActionCardWindow.Action,
                GameVersion = GameVersion.BaseGame,
            },
            new ActionCard()
            {
                ActionCardName = ActionCardName.GhostSquad,
                ActionCardWindow = ActionCardWindow.Combat,
                GameVersion = GameVersion.BaseGame,
            },
            new ActionCard()
            {
                ActionCardName = ActionCardName.HackElection,
                ActionCardWindow = ActionCardWindow.AgendaPhase,
                GameVersion = GameVersion.BaseGame,
            },
            new ActionCard()
            {
                ActionCardName = ActionCardName.HarnessEnergy,
                ActionCardWindow = ActionCardWindow.TacticalAction,
                GameVersion = GameVersion.BaseGame,
            },
            new ActionCard()
            {
                ActionCardName = ActionCardName.ImperialRider,
                ActionCardWindow = ActionCardWindow.AgendaPhase,
                GameVersion = GameVersion.BaseGame,
            },
            new ActionCard()
            {
                ActionCardName = ActionCardName.Impersonation,
                ActionCardWindow = ActionCardWindow.Action,
                GameVersion = GameVersion.BaseGame,
            },
            new ActionCard()
            {
                ActionCardName = ActionCardName.InTheSilenceOfSpace,
                ActionCardWindow = ActionCardWindow.TacticalAction,
                GameVersion = GameVersion.BaseGame,
            },
            new ActionCard()
            {
                ActionCardName = ActionCardName.IndustrialInitiative,
                ActionCardWindow = ActionCardWindow.Action,
                GameVersion = GameVersion.BaseGame,
            },
            new ActionCard()
            {
                ActionCardName = ActionCardName.Infiltrate,
                ActionCardWindow = ActionCardWindow.Combat,
                GameVersion = GameVersion.BaseGame,
            },
            new ActionCard()
            {
                ActionCardName = ActionCardName.InsiderInformation,
                ActionCardWindow = ActionCardWindow.AgendaPhase,
                GameVersion = GameVersion.BaseGame,
            },
            new ActionCard()
            {
                ActionCardName = ActionCardName.Insubordination,
                ActionCardWindow = ActionCardWindow.Action,
                GameVersion = GameVersion.BaseGame,
            },
            new ActionCard()
            {
                ActionCardName = ActionCardName.Intercept,
                ActionCardWindow = ActionCardWindow.Combat,
                GameVersion = GameVersion.BaseGame,
            },
            new ActionCard()
            {
                ActionCardName = ActionCardName.LeadershipRider,
                ActionCardWindow = ActionCardWindow.AgendaPhase,
                GameVersion = GameVersion.BaseGame,
            },
            new ActionCard()
            {
                ActionCardName = ActionCardName.LostStarChart,
                ActionCardWindow = ActionCardWindow.TacticalAction,
                GameVersion = GameVersion.BaseGame,
            },
            new ActionCard()
            {
                ActionCardName = ActionCardName.LuckyShot,
                ActionCardWindow = ActionCardWindow.Action,
                GameVersion = GameVersion.BaseGame,
            },
            new ActionCard()
            {
                ActionCardName = ActionCardName.ManeuveringJets,
                ActionCardWindow = ActionCardWindow.Combat,
                GameVersion = GameVersion.BaseGame,
            },
            new ActionCard()
            {
                ActionCardName = ActionCardName.ManipulateInvestments,
                ActionCardWindow = ActionCardWindow.StrategyPhase,
                GameVersion = GameVersion.ProphecyOfKing,
            },
            new ActionCard()
            {
                ActionCardName = ActionCardName.MasterPlan,
                ActionCardWindow = ActionCardWindow.TacticalAction,
                GameVersion = GameVersion.BaseGame,
            },
            new ActionCard()
            {
                ActionCardName = ActionCardName.MiningInitiative,
                ActionCardWindow = ActionCardWindow.Action,
                GameVersion = GameVersion.BaseGame,
            },
            new ActionCard()
            {
                ActionCardName = ActionCardName.MoraleBoost,
                ActionCardWindow = ActionCardWindow.Combat,
                GameVersion = GameVersion.BaseGame,
            },
            new ActionCard()
            {
                ActionCardName = ActionCardName.NavSuite,
                ActionCardWindow = ActionCardWindow.TacticalAction,
                GameVersion = GameVersion.ProphecyOfKing,
            },
            new ActionCard()
            {
                ActionCardName = ActionCardName.Parley,
                ActionCardWindow = ActionCardWindow.Combat,
                GameVersion = GameVersion.BaseGame,
            },
            new ActionCard()
            {
                ActionCardName = ActionCardName.Plagiarize,
                ActionCardWindow = ActionCardWindow.Action,
                GameVersion = GameVersion.BaseGame,
            },
            new ActionCard()
            {
                ActionCardName = ActionCardName.Plague,
                ActionCardWindow = ActionCardWindow.Action,
                GameVersion = GameVersion.BaseGame,
            },
            new ActionCard()
            {
                ActionCardName = ActionCardName.PoliticalStability,
                ActionCardWindow = ActionCardWindow.StatusPhase,
                GameVersion = GameVersion.BaseGame,
            },
            new ActionCard()
            {
                ActionCardName = ActionCardName.PoliticsRider,
                ActionCardWindow = ActionCardWindow.AgendaPhase,
                GameVersion = GameVersion.BaseGame,
            },
            new ActionCard()
            {
                ActionCardName = ActionCardName.PublicDisgrace,
                ActionCardWindow = ActionCardWindow.StrategyPhase,
                GameVersion = GameVersion.BaseGame,
            },
            new ActionCard()
            {
                ActionCardName = ActionCardName.Rally,
                ActionCardWindow = ActionCardWindow.TacticalAction,
                GameVersion = GameVersion.BaseGame,
            },
            new ActionCard()
            {
                ActionCardName = ActionCardName.ReactorMeltdown,
                ActionCardWindow = ActionCardWindow.Action,
                GameVersion = GameVersion.BaseGame,
            },
            new ActionCard()
            {
                ActionCardName = ActionCardName.RefitTroops,
                ActionCardWindow = ActionCardWindow.Action,
                GameVersion = GameVersion.ProphecyOfKing,
            },
            new ActionCard()
            {
                ActionCardName = ActionCardName.ReflectiveShielding,
                ActionCardWindow = ActionCardWindow.Combat,
                GameVersion = GameVersion.BaseGame,
            },
            new ActionCard()
            {
                ActionCardName = ActionCardName.Reparations,
                ActionCardWindow = ActionCardWindow.TacticalAction,
                GameVersion = GameVersion.BaseGame,
            },
            new ActionCard()
            {
                ActionCardName = ActionCardName.RepealLaw,
                ActionCardWindow = ActionCardWindow.Action,
                GameVersion = GameVersion.BaseGame,
            },
            new ActionCard()
            {
                ActionCardName = ActionCardName.RevealPrototype,
                ActionCardWindow = ActionCardWindow.Combat,
                GameVersion = GameVersion.ProphecyOfKing,
            },
            new ActionCard()
            {
                ActionCardName = ActionCardName.ReverseEngineer,
                ActionCardWindow = ActionCardWindow.Reaction,
                GameVersion = GameVersion.ProphecyOfKing,
            },
            new ActionCard()
            {
                ActionCardName = ActionCardName.RiseOfAMessiah,
                ActionCardWindow = ActionCardWindow.Action,
                GameVersion = GameVersion.BaseGame,
            },
            new ActionCard()
            {
                ActionCardName = ActionCardName.Rout,
                ActionCardWindow = ActionCardWindow.Combat,
                GameVersion = GameVersion.ProphecyOfKing,
            },
            new ActionCard()
            {
                ActionCardName = ActionCardName.Sabotage,
                ActionCardWindow = ActionCardWindow.Reaction,
                GameVersion = GameVersion.BaseGame,
            },
            new ActionCard()
            {
                ActionCardName = ActionCardName.Salvage,
                ActionCardWindow = ActionCardWindow.Combat,
                GameVersion = GameVersion.BaseGame,
            },
            new ActionCard()
            {
                ActionCardName = ActionCardName.Sanction,
                ActionCardWindow = ActionCardWindow.AgendaPhase,
                GameVersion = GameVersion.BaseGame,
            },
            new ActionCard()
            {
                ActionCardName = ActionCardName.ScrambleFrequency,
                ActionCardWindow = ActionCardWindow.Combat,
                GameVersion = GameVersion.BaseGame,
            },
            new ActionCard()
            {
                ActionCardName = ActionCardName.Scuttle,
                ActionCardWindow = ActionCardWindow.Action,
                GameVersion = GameVersion.ProphecyOfKing,
            },
            new ActionCard()
            {
                ActionCardName = ActionCardName.SeizeArtifact,
                ActionCardWindow = ActionCardWindow.Action,
                GameVersion = GameVersion.ProphecyOfKing,
            },
            new ActionCard()
            {
                ActionCardName = ActionCardName.ShieldsHolding,
                ActionCardWindow = ActionCardWindow.Combat,
                GameVersion = GameVersion.BaseGame,
            },
            new ActionCard()
            {
                ActionCardName = ActionCardName.SignalJamming,
                ActionCardWindow = ActionCardWindow.Action,
                GameVersion = GameVersion.BaseGame,
            },
            new ActionCard()
            {
                ActionCardName = ActionCardName.SkilledRetreat,
                ActionCardWindow = ActionCardWindow.Combat,
                GameVersion = GameVersion.BaseGame,
            },
            new ActionCard()
            {
                ActionCardName = ActionCardName.SolarFlare,
                ActionCardWindow = ActionCardWindow.TacticalAction,
                GameVersion = GameVersion.BaseGame,
            },
            new ActionCard()
            {
                ActionCardName = ActionCardName.Spy,
                ActionCardWindow = ActionCardWindow.Action,
                GameVersion = GameVersion.BaseGame,
            },
            new ActionCard()
            {
                ActionCardName = ActionCardName.Summit,
                ActionCardWindow = ActionCardWindow.StrategyPhase,
                GameVersion = GameVersion.BaseGame,
            },
            new ActionCard()
            {
                ActionCardName = ActionCardName.TacticalBombardment,
                ActionCardWindow = ActionCardWindow.Action,
                GameVersion = GameVersion.BaseGame,
            },
            new ActionCard()
            {
                ActionCardName = ActionCardName.TechnologyRider,
                ActionCardWindow = ActionCardWindow.AgendaPhase,
                GameVersion = GameVersion.BaseGame,
            },
            new ActionCard()
            {
                ActionCardName = ActionCardName.TradeRider,
                ActionCardWindow = ActionCardWindow.AgendaPhase,
                GameVersion = GameVersion.BaseGame,
            },
            new ActionCard()
            {
                ActionCardName = ActionCardName.UnexpectedAction,
                ActionCardWindow = ActionCardWindow.Action,
                GameVersion = GameVersion.BaseGame,
            },
            new ActionCard()
            {
                ActionCardName = ActionCardName.UnstablePlanet,
                ActionCardWindow = ActionCardWindow.Action,
                GameVersion = GameVersion.BaseGame,
            },
            new ActionCard()
            {
                ActionCardName = ActionCardName.Upgrade,
                ActionCardWindow = ActionCardWindow.TacticalAction,
                GameVersion = GameVersion.BaseGame,
            },
            new ActionCard()
            {
                ActionCardName = ActionCardName.Uprising,
                ActionCardWindow = ActionCardWindow.Action,
                GameVersion = GameVersion.BaseGame,
            },
            new ActionCard()
            {
                ActionCardName = ActionCardName.Veto,
                ActionCardWindow = ActionCardWindow.AgendaPhase,
                GameVersion = GameVersion.BaseGame,
            },
            new ActionCard()
            {
                ActionCardName = ActionCardName.WarMachine,
                ActionCardWindow = ActionCardWindow.TacticalAction,
                GameVersion = GameVersion.BaseGame,
            },
            new ActionCard()
            {
                ActionCardName = ActionCardName.WarfareRider,
                ActionCardWindow = ActionCardWindow.AgendaPhase,
                GameVersion = GameVersion.BaseGame,
            },
            new ActionCard()
            {
                ActionCardName = ActionCardName.Waylay,
                ActionCardWindow = ActionCardWindow.Combat,
                GameVersion = GameVersion.ProphecyOfKing,
            },
        };

        var updatedActionCards = actionCards.Select(actionCard =>
        {
            actionCard.Name = $"{actionCard.ActionCardName}_{nameof(ActionCard.Name)}";
            actionCard.Text = $"{actionCard.ActionCardName}_{nameof(ActionCard.Text)}";
            actionCard.Type = CardType.Action;
            actionCard.ImagePath = _cardImagePathService.GetCardImagePath(actionCard.ActionCardName, actionCard.Type);
            return actionCard;
        }).ToList();

        context.AddRange(updatedActionCards);
        context.SaveChanges();
    }
}
