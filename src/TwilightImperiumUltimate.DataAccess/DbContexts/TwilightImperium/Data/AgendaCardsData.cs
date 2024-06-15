using TwilightImperiumUltimate.Core.Entities.Cards;
using TwilightImperiumUltimate.Core.Enums.Cards;
using TwilightImperiumUltimate.DataAccess.Services;

namespace TwilightImperiumUltimate.DataAccess.DbContexts.TwilightImperium.Data;

internal static class AgendaCardsData
{
    internal static List<AgendaCard> AgendaCards => GetAgendaCards();

    private static List<AgendaCard> GetAgendaCards()
    {
        CreateCardImagePathService createCardImagePathService = new();

        var agendaCards = new List<AgendaCard>()
        {
            new() { AgendaCardName = AgendaCardName.AntiIntellectualRevolution, AgendaCardType = AgendaCardType.Law, GameVersion = GameVersion.BaseGame },
            new() { AgendaCardName = AgendaCardName.ArchivedSecret, AgendaCardType = AgendaCardType.Directive, GameVersion = GameVersion.BaseGame },
            new() { AgendaCardName = AgendaCardName.ArmsReduction, AgendaCardType = AgendaCardType.Directive, GameVersion = GameVersion.BaseGame },
            new() { AgendaCardName = AgendaCardName.ClassifiedDocumentLeaks, AgendaCardType = AgendaCardType.Law, GameVersion = GameVersion.BaseGame },
            new() { AgendaCardName = AgendaCardName.ColonialRedistribution, AgendaCardType = AgendaCardType.Directive, GameVersion = GameVersion.BaseGame },
            new() { AgendaCardName = AgendaCardName.CommitteeFormation, AgendaCardType = AgendaCardType.Law, GameVersion = GameVersion.BaseGame },
            new() { AgendaCardName = AgendaCardName.CompensatedDisarmament, AgendaCardType = AgendaCardType.Directive, GameVersion = GameVersion.BaseGame },
            new() { AgendaCardName = AgendaCardName.ConventionsOfWar, AgendaCardType = AgendaCardType.Law, GameVersion = GameVersion.BaseGame },
            new() { AgendaCardName = AgendaCardName.CoreMining, AgendaCardType = AgendaCardType.Law, GameVersion = GameVersion.BaseGame },
            new() { AgendaCardName = AgendaCardName.DemilitarizedZone, AgendaCardType = AgendaCardType.Law, GameVersion = GameVersion.BaseGame },
            new() { AgendaCardName = AgendaCardName.EconomicEquality, AgendaCardType = AgendaCardType.Directive, GameVersion = GameVersion.BaseGame },
            new() { AgendaCardName = AgendaCardName.EnforcedTravelBan, AgendaCardType = AgendaCardType.Law, GameVersion = GameVersion.BaseGame },
            new() { AgendaCardName = AgendaCardName.ExecutiveSanctions, AgendaCardType = AgendaCardType.Law, GameVersion = GameVersion.BaseGame },
            new() { AgendaCardName = AgendaCardName.FleetRegulations, AgendaCardType = AgendaCardType.Law, GameVersion = GameVersion.BaseGame },
            new() { AgendaCardName = AgendaCardName.HolyPlanetOfIxth, AgendaCardType = AgendaCardType.Law, GameVersion = GameVersion.BaseGame },
            new() { AgendaCardName = AgendaCardName.HomelandDefenseAct, AgendaCardType = AgendaCardType.Law, GameVersion = GameVersion.BaseGame },
            new() { AgendaCardName = AgendaCardName.ImperialArbiter, AgendaCardType = AgendaCardType.Law, GameVersion = GameVersion.BaseGame },
            new() { AgendaCardName = AgendaCardName.IncentiveProgram, AgendaCardType = AgendaCardType.Directive, GameVersion = GameVersion.BaseGame },
            new() { AgendaCardName = AgendaCardName.IxthianArtifact, AgendaCardType = AgendaCardType.Directive, GameVersion = GameVersion.BaseGame },
            new() { AgendaCardName = AgendaCardName.JudicialAbolishment, AgendaCardType = AgendaCardType.Directive, GameVersion = GameVersion.BaseGame },
            new() { AgendaCardName = AgendaCardName.MinisterOfCommerce, AgendaCardType = AgendaCardType.Law, GameVersion = GameVersion.BaseGame },
            new() { AgendaCardName = AgendaCardName.MinisterOfExploration, AgendaCardType = AgendaCardType.Law, GameVersion = GameVersion.BaseGame },
            new() { AgendaCardName = AgendaCardName.MinisterOfIndustry, AgendaCardType = AgendaCardType.Law, GameVersion = GameVersion.BaseGame },
            new() { AgendaCardName = AgendaCardName.MinisterOfPeace, AgendaCardType = AgendaCardType.Law, GameVersion = GameVersion.BaseGame },
            new() { AgendaCardName = AgendaCardName.MinisterOfPolicy, AgendaCardType = AgendaCardType.Law, GameVersion = GameVersion.BaseGame },
            new() { AgendaCardName = AgendaCardName.MinisterOfSciences, AgendaCardType = AgendaCardType.Law, GameVersion = GameVersion.BaseGame },
            new() { AgendaCardName = AgendaCardName.MinisterOfWar, AgendaCardType = AgendaCardType.Law, GameVersion = GameVersion.BaseGame },
            new() { AgendaCardName = AgendaCardName.MiscountDisclosed, AgendaCardType = AgendaCardType.Directive, GameVersion = GameVersion.BaseGame },
            new() { AgendaCardName = AgendaCardName.Mutiny, AgendaCardType = AgendaCardType.Directive, GameVersion = GameVersion.BaseGame },
            new() { AgendaCardName = AgendaCardName.NewConstitution, AgendaCardType = AgendaCardType.Directive, GameVersion = GameVersion.BaseGame },
            new() { AgendaCardName = AgendaCardName.ProphecyOfIxth, AgendaCardType = AgendaCardType.Law, GameVersion = GameVersion.BaseGame },
            new() { AgendaCardName = AgendaCardName.PublicExecution, AgendaCardType = AgendaCardType.Directive, GameVersion = GameVersion.BaseGame },
            new() { AgendaCardName = AgendaCardName.PublicizeWeaponSchematics, AgendaCardType = AgendaCardType.Law, GameVersion = GameVersion.BaseGame },
            new() { AgendaCardName = AgendaCardName.RegulatedConscription, AgendaCardType = AgendaCardType.Law, GameVersion = GameVersion.BaseGame },
            new() { AgendaCardName = AgendaCardName.RepresentativeGovernment, AgendaCardType = AgendaCardType.Law, GameVersion = GameVersion.BaseGame },
            new() { AgendaCardName = AgendaCardName.ResearchTeamBiotic, AgendaCardType = AgendaCardType.Law, GameVersion = GameVersion.BaseGame },
            new() { AgendaCardName = AgendaCardName.ResearchTeamCybernetic, AgendaCardType = AgendaCardType.Law, GameVersion = GameVersion.BaseGame },
            new() { AgendaCardName = AgendaCardName.ResearchTeamPropulsion, AgendaCardType = AgendaCardType.Law, GameVersion = GameVersion.BaseGame },
            new() { AgendaCardName = AgendaCardName.ResearchTeamWarfare, AgendaCardType = AgendaCardType.Law, GameVersion = GameVersion.BaseGame },
            new() { AgendaCardName = AgendaCardName.SeedOfAnEmpire, AgendaCardType = AgendaCardType.Directive, GameVersion = GameVersion.BaseGame },
            new() { AgendaCardName = AgendaCardName.SenateSanctuary, AgendaCardType = AgendaCardType.Law, GameVersion = GameVersion.BaseGame },
            new() { AgendaCardName = AgendaCardName.ShardOfTheThrone, AgendaCardType = AgendaCardType.Law, GameVersion = GameVersion.BaseGame },
            new() { AgendaCardName = AgendaCardName.SharedResearch, AgendaCardType = AgendaCardType.Law, GameVersion = GameVersion.BaseGame },
            new() { AgendaCardName = AgendaCardName.SwordsToPlowshares, AgendaCardType = AgendaCardType.Directive, GameVersion = GameVersion.BaseGame },
            new() { AgendaCardName = AgendaCardName.TerraformingInitiative, AgendaCardType = AgendaCardType.Law, GameVersion = GameVersion.BaseGame },
            new() { AgendaCardName = AgendaCardName.TheCrownOfEmphidia, AgendaCardType = AgendaCardType.Law, GameVersion = GameVersion.BaseGame },
            new() { AgendaCardName = AgendaCardName.TheCrownOfThalnos, AgendaCardType = AgendaCardType.Law, GameVersion = GameVersion.BaseGame },
            new() { AgendaCardName = AgendaCardName.UnconventionalMeasures, AgendaCardType = AgendaCardType.Directive, GameVersion = GameVersion.BaseGame },
            new() { AgendaCardName = AgendaCardName.WormholeReconstruction, AgendaCardType = AgendaCardType.Law, GameVersion = GameVersion.BaseGame },
            new() { AgendaCardName = AgendaCardName.WormholeResearch, AgendaCardType = AgendaCardType.Directive, GameVersion = GameVersion.BaseGame },

            // Propecy of Kings
            new() { AgendaCardName = AgendaCardName.ArmedForcesStandardization, AgendaCardType = AgendaCardType.Directive, GameVersion = GameVersion.ProphecyOfKing },
            new() { AgendaCardName = AgendaCardName.ArticlesOfWar, AgendaCardType = AgendaCardType.Law, GameVersion = GameVersion.ProphecyOfKing },
            new() { AgendaCardName = AgendaCardName.ChecksAndBalances, AgendaCardType = AgendaCardType.Law, GameVersion = GameVersion.ProphecyOfKing },
            new() { AgendaCardName = AgendaCardName.ClandestineOperations, AgendaCardType = AgendaCardType.Directive, GameVersion = GameVersion.ProphecyOfKing },
            new() { AgendaCardName = AgendaCardName.CovertLegislation, AgendaCardType = AgendaCardType.Directive, GameVersion = GameVersion.ProphecyOfKing },
            new() { AgendaCardName = AgendaCardName.GalacticCrisisPact, AgendaCardType = AgendaCardType.Directive, GameVersion = GameVersion.ProphecyOfKing },
            new() { AgendaCardName = AgendaCardName.MinisterOfAntiquities, AgendaCardType = AgendaCardType.Directive, GameVersion = GameVersion.ProphecyOfKing },
            new() { AgendaCardName = AgendaCardName.NexusSovereignty, AgendaCardType = AgendaCardType.Law, GameVersion = GameVersion.ProphecyOfKing },
            new() { AgendaCardName = AgendaCardName.PoliticalCensure, AgendaCardType = AgendaCardType.Law, GameVersion = GameVersion.ProphecyOfKing },
            new() { AgendaCardName = AgendaCardName.RearmamentAgreement, AgendaCardType = AgendaCardType.Directive, GameVersion = GameVersion.ProphecyOfKing },
            new() { AgendaCardName = AgendaCardName.RepresentativeGovernmentProphecyOfKings, AgendaCardType = AgendaCardType.Law, GameVersion = GameVersion.ProphecyOfKing },
            new() { AgendaCardName = AgendaCardName.ResearchGrantReallocation, AgendaCardType = AgendaCardType.Directive, GameVersion = GameVersion.ProphecyOfKing },
            new() { AgendaCardName = AgendaCardName.SearchWarrant, AgendaCardType = AgendaCardType.Law, GameVersion = GameVersion.ProphecyOfKing },
        };

        var updatedAgendaCards = agendaCards.Select((agendaCard, i) =>
        {
            agendaCard.Id = i + 1;
            agendaCard.Name = $"{agendaCard.AgendaCardName}_{nameof(ActionCard.Name)}";
            agendaCard.Text = $"{agendaCard.AgendaCardName}_{nameof(ActionCard.Text)}";
            agendaCard.Type = CardType.Agenda;
            agendaCard.ImagePath = createCardImagePathService.GetCardImagePath(agendaCard.AgendaCardName, agendaCard.Type);
            return agendaCard;
        }).ToList();

        return updatedAgendaCards;
    }
}
