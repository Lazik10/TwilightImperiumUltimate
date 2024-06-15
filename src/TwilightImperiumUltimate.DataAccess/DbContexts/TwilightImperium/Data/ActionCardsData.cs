using TwilightImperiumUltimate.Core.Entities.Cards;
using TwilightImperiumUltimate.Core.Enums.Cards;
using TwilightImperiumUltimate.DataAccess.Services;

namespace TwilightImperiumUltimate.DataAccess.DbContexts.TwilightImperium.Data;

internal static class ActionCardsData
{
    internal static List<ActionCard> ActionCards => GetActionCards();

    private static List<ActionCard> GetActionCards()
    {
        CreateCardImagePathService createCardImagePathService = new();

        var actionCards = new List<ActionCard>()
        {
            new() { Id = 1, ActionCardName = ActionCardName.AncientBurialSites, ActionCardWindow = ActionCardWindow.AgendaPhase, GameVersion = GameVersion.BaseGame },
            new() { ActionCardName = ActionCardName.ArchaeologicalExpedition, ActionCardWindow = ActionCardWindow.Action, GameVersion = GameVersion.ProphecyOfKing },
            new() { ActionCardName = ActionCardName.AssassinateRepresentative, ActionCardWindow = ActionCardWindow.AgendaPhase, GameVersion = GameVersion.BaseGame },
            new() { ActionCardName = ActionCardName.Blitz, ActionCardWindow = ActionCardWindow.Combat, GameVersion = GameVersion.BaseGame },
            new() { ActionCardName = ActionCardName.Bribery, ActionCardWindow = ActionCardWindow.AgendaPhase, GameVersion = GameVersion.BaseGame },
            new() { ActionCardName = ActionCardName.Bunker, ActionCardWindow = ActionCardWindow.Combat, GameVersion = GameVersion.BaseGame },
            new() { ActionCardName = ActionCardName.ConfoundingLegalText, ActionCardWindow = ActionCardWindow.AgendaPhase, GameVersion = GameVersion.ProphecyOfKing },
            new() { ActionCardName = ActionCardName.ConfusingLegalText, ActionCardWindow = ActionCardWindow.AgendaPhase, GameVersion = GameVersion.BaseGame },
            new() { ActionCardName = ActionCardName.ConstructionRider, ActionCardWindow = ActionCardWindow.AgendaPhase, GameVersion = GameVersion.BaseGame },
            new() { ActionCardName = ActionCardName.Counterstroke, ActionCardWindow = ActionCardWindow.Reaction, GameVersion = GameVersion.BaseGame },
            new() { ActionCardName = ActionCardName.CoupDetat, ActionCardWindow = ActionCardWindow.Reaction, GameVersion = GameVersion.ProphecyOfKing },
            new() { ActionCardName = ActionCardName.CourageousToTheEnd, ActionCardWindow = ActionCardWindow.Combat, GameVersion = GameVersion.BaseGame },
            new() { ActionCardName = ActionCardName.CrippleDefenses, ActionCardWindow = ActionCardWindow.Action, GameVersion = GameVersion.BaseGame },
            new() { ActionCardName = ActionCardName.DeadlyPlot, ActionCardWindow = ActionCardWindow.AgendaPhase, GameVersion = GameVersion.ProphecyOfKing },
            new() { ActionCardName = ActionCardName.DecoyOperation, ActionCardWindow = ActionCardWindow.TacticalAction, GameVersion = GameVersion.ProphecyOfKing },
            new() { ActionCardName = ActionCardName.DiplomacyRider, ActionCardWindow = ActionCardWindow.AgendaPhase, GameVersion = GameVersion.BaseGame },
            new() { ActionCardName = ActionCardName.DiplomaticPressure, ActionCardWindow = ActionCardWindow.AgendaPhase, GameVersion = GameVersion.ProphecyOfKing },
            new() { ActionCardName = ActionCardName.DirectHit, ActionCardWindow = ActionCardWindow.Combat, GameVersion = GameVersion.BaseGame },
            new() { ActionCardName = ActionCardName.Disable, ActionCardWindow = ActionCardWindow.Combat, GameVersion = GameVersion.BaseGame },
            new() { ActionCardName = ActionCardName.DistinguishedCouncilor, ActionCardWindow = ActionCardWindow.AgendaPhase, GameVersion = GameVersion.BaseGame },
            new() { ActionCardName = ActionCardName.DivertFunding, ActionCardWindow = ActionCardWindow.Action, GameVersion = GameVersion.ProphecyOfKing },
            new() { ActionCardName = ActionCardName.EconomicInitiative, ActionCardWindow = ActionCardWindow.Action, GameVersion = GameVersion.BaseGame },
            new() { ActionCardName = ActionCardName.EmergencyRepairs, ActionCardWindow = ActionCardWindow.Combat, GameVersion = GameVersion.BaseGame },
            new() { ActionCardName = ActionCardName.ExperimentalBattlestation, ActionCardWindow = ActionCardWindow.TacticalAction, GameVersion = GameVersion.BaseGame },
            new() { ActionCardName = ActionCardName.ExplorationProbe, ActionCardWindow = ActionCardWindow.Action, GameVersion = GameVersion.ProphecyOfKing },
            new() { ActionCardName = ActionCardName.FighterConscription, ActionCardWindow = ActionCardWindow.Action, GameVersion = GameVersion.BaseGame },
            new() { ActionCardName = ActionCardName.FighterPrototype, ActionCardWindow = ActionCardWindow.Combat, GameVersion = GameVersion.BaseGame },
            new() { ActionCardName = ActionCardName.FireTeam, ActionCardWindow = ActionCardWindow.Combat, GameVersion = GameVersion.BaseGame },
            new() { ActionCardName = ActionCardName.FlankSpeed, ActionCardWindow = ActionCardWindow.TacticalAction, GameVersion = GameVersion.BaseGame },
            new() { ActionCardName = ActionCardName.FocusedResearch, ActionCardWindow = ActionCardWindow.Action, GameVersion = GameVersion.BaseGame },
            new() { ActionCardName = ActionCardName.ForwardSupplyBase, ActionCardWindow = ActionCardWindow.Reaction, GameVersion = GameVersion.BaseGame },
            new() { ActionCardName = ActionCardName.FrontlineDeployment, ActionCardWindow = ActionCardWindow.Action, GameVersion = GameVersion.BaseGame },
            new() { ActionCardName = ActionCardName.GhostShip, ActionCardWindow = ActionCardWindow.Action, GameVersion = GameVersion.BaseGame },
            new() { ActionCardName = ActionCardName.GhostSquad, ActionCardWindow = ActionCardWindow.Combat, GameVersion = GameVersion.BaseGame },
            new() { ActionCardName = ActionCardName.HackElection, ActionCardWindow = ActionCardWindow.AgendaPhase, GameVersion = GameVersion.BaseGame },
            new() { ActionCardName = ActionCardName.HarnessEnergy, ActionCardWindow = ActionCardWindow.TacticalAction, GameVersion = GameVersion.BaseGame },
            new() { ActionCardName = ActionCardName.ImperialRider, ActionCardWindow = ActionCardWindow.AgendaPhase, GameVersion = GameVersion.BaseGame },
            new() { ActionCardName = ActionCardName.Impersonation, ActionCardWindow = ActionCardWindow.Action, GameVersion = GameVersion.BaseGame },
            new() { ActionCardName = ActionCardName.InTheSilenceOfSpace, ActionCardWindow = ActionCardWindow.TacticalAction, GameVersion = GameVersion.BaseGame },
            new() { ActionCardName = ActionCardName.IndustrialInitiative, ActionCardWindow = ActionCardWindow.Action, GameVersion = GameVersion.BaseGame },
            new() { ActionCardName = ActionCardName.Infiltrate, ActionCardWindow = ActionCardWindow.Combat, GameVersion = GameVersion.BaseGame },
            new() { ActionCardName = ActionCardName.InsiderInformation, ActionCardWindow = ActionCardWindow.AgendaPhase, GameVersion = GameVersion.BaseGame },
            new() { ActionCardName = ActionCardName.Insubordination, ActionCardWindow = ActionCardWindow.Action, GameVersion = GameVersion.BaseGame },
            new() { ActionCardName = ActionCardName.Intercept, ActionCardWindow = ActionCardWindow.Combat, GameVersion = GameVersion.BaseGame },
            new() { ActionCardName = ActionCardName.LeadershipRider, ActionCardWindow = ActionCardWindow.AgendaPhase, GameVersion = GameVersion.BaseGame },
            new() { ActionCardName = ActionCardName.LostStarChart, ActionCardWindow = ActionCardWindow.TacticalAction, GameVersion = GameVersion.BaseGame },
            new() { ActionCardName = ActionCardName.LuckyShot, ActionCardWindow = ActionCardWindow.Action, GameVersion = GameVersion.BaseGame },
            new() { ActionCardName = ActionCardName.ManeuveringJets, ActionCardWindow = ActionCardWindow.Combat, GameVersion = GameVersion.BaseGame },
            new() { ActionCardName = ActionCardName.ManipulateInvestments, ActionCardWindow = ActionCardWindow.StrategyPhase, GameVersion = GameVersion.ProphecyOfKing },
            new() { ActionCardName = ActionCardName.MasterPlan, ActionCardWindow = ActionCardWindow.TacticalAction, GameVersion = GameVersion.BaseGame },
            new() { ActionCardName = ActionCardName.MiningInitiative, ActionCardWindow = ActionCardWindow.Action, GameVersion = GameVersion.BaseGame },
            new() { ActionCardName = ActionCardName.MoraleBoost, ActionCardWindow = ActionCardWindow.Combat, GameVersion = GameVersion.BaseGame },
            new() { ActionCardName = ActionCardName.NavSuite, ActionCardWindow = ActionCardWindow.TacticalAction, GameVersion = GameVersion.ProphecyOfKing },
            new() { ActionCardName = ActionCardName.Parley, ActionCardWindow = ActionCardWindow.Combat, GameVersion = GameVersion.BaseGame },
            new() { ActionCardName = ActionCardName.Plagiarize, ActionCardWindow = ActionCardWindow.Action, GameVersion = GameVersion.BaseGame },
            new() { ActionCardName = ActionCardName.Plague, ActionCardWindow = ActionCardWindow.Action, GameVersion = GameVersion.BaseGame },
            new() { ActionCardName = ActionCardName.PoliticalStability, ActionCardWindow = ActionCardWindow.StatusPhase, GameVersion = GameVersion.BaseGame },
            new() { ActionCardName = ActionCardName.PoliticsRider, ActionCardWindow = ActionCardWindow.AgendaPhase, GameVersion = GameVersion.BaseGame },
            new() { ActionCardName = ActionCardName.PublicDisgrace, ActionCardWindow = ActionCardWindow.StrategyPhase, GameVersion = GameVersion.BaseGame },
            new() { ActionCardName = ActionCardName.Rally, ActionCardWindow = ActionCardWindow.TacticalAction, GameVersion = GameVersion.BaseGame },
            new() { ActionCardName = ActionCardName.ReactorMeltdown, ActionCardWindow = ActionCardWindow.Action, GameVersion = GameVersion.BaseGame },
            new() { ActionCardName = ActionCardName.RefitTroops, ActionCardWindow = ActionCardWindow.Action, GameVersion = GameVersion.ProphecyOfKing },
            new() { ActionCardName = ActionCardName.ReflectiveShielding, ActionCardWindow = ActionCardWindow.Combat, GameVersion = GameVersion.BaseGame },
            new() { ActionCardName = ActionCardName.Reparations, ActionCardWindow = ActionCardWindow.TacticalAction, GameVersion = GameVersion.BaseGame },
            new() { ActionCardName = ActionCardName.RepealLaw, ActionCardWindow = ActionCardWindow.Action, GameVersion = GameVersion.BaseGame },
            new() { ActionCardName = ActionCardName.RevealPrototype, ActionCardWindow = ActionCardWindow.Combat, GameVersion = GameVersion.ProphecyOfKing },
            new() { ActionCardName = ActionCardName.ReverseEngineer, ActionCardWindow = ActionCardWindow.Reaction, GameVersion = GameVersion.ProphecyOfKing },
            new() { ActionCardName = ActionCardName.RiseOfAMessiah, ActionCardWindow = ActionCardWindow.Action, GameVersion = GameVersion.BaseGame },
            new() { ActionCardName = ActionCardName.Rout, ActionCardWindow = ActionCardWindow.Combat, GameVersion = GameVersion.ProphecyOfKing },
            new() { ActionCardName = ActionCardName.Sabotage, ActionCardWindow = ActionCardWindow.Reaction, GameVersion = GameVersion.BaseGame },
            new() { ActionCardName = ActionCardName.Salvage, ActionCardWindow = ActionCardWindow.Combat, GameVersion = GameVersion.BaseGame },
            new() { ActionCardName = ActionCardName.Sanction, ActionCardWindow = ActionCardWindow.AgendaPhase, GameVersion = GameVersion.BaseGame },
            new() { ActionCardName = ActionCardName.ScrambleFrequency, ActionCardWindow = ActionCardWindow.Combat, GameVersion = GameVersion.BaseGame },
            new() { ActionCardName = ActionCardName.Scuttle, ActionCardWindow = ActionCardWindow.Action, GameVersion = GameVersion.ProphecyOfKing },
            new() { ActionCardName = ActionCardName.SeizeArtifact, ActionCardWindow = ActionCardWindow.Action, GameVersion = GameVersion.ProphecyOfKing },
            new() { ActionCardName = ActionCardName.ShieldsHolding, ActionCardWindow = ActionCardWindow.Combat, GameVersion = GameVersion.BaseGame },
            new() { ActionCardName = ActionCardName.SignalJamming, ActionCardWindow = ActionCardWindow.Action, GameVersion = GameVersion.BaseGame },
            new() { ActionCardName = ActionCardName.SkilledRetreat, ActionCardWindow = ActionCardWindow.Combat, GameVersion = GameVersion.BaseGame },
            new() { ActionCardName = ActionCardName.SolarFlare, ActionCardWindow = ActionCardWindow.TacticalAction, GameVersion = GameVersion.BaseGame },
            new() { ActionCardName = ActionCardName.Spy, ActionCardWindow = ActionCardWindow.Action, GameVersion = GameVersion.BaseGame },
            new() { ActionCardName = ActionCardName.Summit, ActionCardWindow = ActionCardWindow.StrategyPhase, GameVersion = GameVersion.BaseGame },
            new() { ActionCardName = ActionCardName.TacticalBombardment, ActionCardWindow = ActionCardWindow.Action, GameVersion = GameVersion.BaseGame },
            new() { ActionCardName = ActionCardName.TechnologyRider, ActionCardWindow = ActionCardWindow.AgendaPhase, GameVersion = GameVersion.BaseGame },
            new() { ActionCardName = ActionCardName.TradeRider, ActionCardWindow = ActionCardWindow.AgendaPhase, GameVersion = GameVersion.BaseGame },
            new() { ActionCardName = ActionCardName.UnexpectedAction, ActionCardWindow = ActionCardWindow.Action, GameVersion = GameVersion.BaseGame },
            new() { ActionCardName = ActionCardName.UnstablePlanet, ActionCardWindow = ActionCardWindow.Action, GameVersion = GameVersion.BaseGame },
            new() { ActionCardName = ActionCardName.Upgrade, ActionCardWindow = ActionCardWindow.TacticalAction, GameVersion = GameVersion.BaseGame },
            new() { ActionCardName = ActionCardName.Uprising, ActionCardWindow = ActionCardWindow.Action, GameVersion = GameVersion.BaseGame },
            new() { ActionCardName = ActionCardName.Veto, ActionCardWindow = ActionCardWindow.AgendaPhase, GameVersion = GameVersion.BaseGame },
            new() { ActionCardName = ActionCardName.WarMachine, ActionCardWindow = ActionCardWindow.TacticalAction, GameVersion = GameVersion.BaseGame },
            new() { ActionCardName = ActionCardName.WarfareRider, ActionCardWindow = ActionCardWindow.AgendaPhase, GameVersion = GameVersion.BaseGame },
            new() { ActionCardName = ActionCardName.Waylay, ActionCardWindow = ActionCardWindow.Combat, GameVersion = GameVersion.ProphecyOfKing },
        };

        var updatedActionCards = actionCards.Select((actionCard, i) =>
        {
            actionCard.Id = i + 1;
            actionCard.Name = $"{actionCard.ActionCardName}_{nameof(ActionCard.Name)}";
            actionCard.Text = $"{actionCard.ActionCardName}_{nameof(ActionCard.Text)}";
            actionCard.Type = CardType.Action;
            actionCard.ImagePath = createCardImagePathService.GetCardImagePath(actionCard.ActionCardName, actionCard.Type);
            return actionCard;
        }).ToList();

        return updatedActionCards;
    }
}
