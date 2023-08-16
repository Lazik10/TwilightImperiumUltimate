using TwilightImperiumUltimate.Core.Entities.Cards;
using TwilightImperiumUltimate.Core.Enums.Cards;

namespace TwilightImperiumUltimate.DataAccess.DbContexts.TwilightImperium;

public partial class TwilightImperiumDbContextInitializer
{
    private void InitializeAgendaCards()
    {
        using var context = _dbContextFactory.CreateDbContext();

        var agendaCards = new List<AgendaCard>()
        {
            // Base Game
            new AgendaCard
            {
                AgendaCardName = AgendaCardName.AntiIntellectualRevolution,
                AgendaCardType = AgendaCardType.Law,
                GameVersion = GameVersion.BaseGame,
            },
            new AgendaCard
            {
                AgendaCardName = AgendaCardName.ArchivedSecret,
                AgendaCardType = AgendaCardType.Directive,
                GameVersion = GameVersion.BaseGame,
            },
            new AgendaCard
            {
                AgendaCardName = AgendaCardName.ArmsReduction,
                AgendaCardType = AgendaCardType.Directive,
                GameVersion = GameVersion.BaseGame,
            },
            new AgendaCard
            {
                AgendaCardName = AgendaCardName.ClassifiedDocumentLeaks,
                AgendaCardType = AgendaCardType.Law,
                GameVersion = GameVersion.BaseGame,
            },
            new AgendaCard
            {
                AgendaCardName = AgendaCardName.ColonialRedistribution,
                AgendaCardType = AgendaCardType.Directive,
                GameVersion = GameVersion.BaseGame,
            },
            new AgendaCard
            {
                AgendaCardName = AgendaCardName.CommitteeFormation,
                AgendaCardType = AgendaCardType.Law,
                GameVersion = GameVersion.BaseGame,
            },
            new AgendaCard
            {
                AgendaCardName = AgendaCardName.CompensatedDisarmament,
                AgendaCardType = AgendaCardType.Directive,
                GameVersion = GameVersion.BaseGame,
            },
            new AgendaCard
            {
                AgendaCardName = AgendaCardName.ConventionsOfWar,
                AgendaCardType = AgendaCardType.Law,
                GameVersion = GameVersion.BaseGame,
            },
            new AgendaCard
            {
                AgendaCardName = AgendaCardName.CoreMining,
                AgendaCardType = AgendaCardType.Law,
                GameVersion = GameVersion.BaseGame,
            },
            new AgendaCard
            {
                AgendaCardName = AgendaCardName.DemilitarizedZone,
                AgendaCardType = AgendaCardType.Law,
                GameVersion = GameVersion.BaseGame,
            },
            new AgendaCard
            {
                AgendaCardName = AgendaCardName.EconomicEquality,
                AgendaCardType = AgendaCardType.Directive,
                GameVersion = GameVersion.BaseGame,
            },
            new AgendaCard
            {
                AgendaCardName = AgendaCardName.EnforcedTravelBan,
                AgendaCardType = AgendaCardType.Law,
                GameVersion = GameVersion.BaseGame,
            },
            new AgendaCard
            {
                AgendaCardName = AgendaCardName.ExecutiveSanctions,
                AgendaCardType = AgendaCardType.Law,
                GameVersion = GameVersion.BaseGame,
            },
            new AgendaCard
            {
                AgendaCardName = AgendaCardName.FleetRegulations,
                AgendaCardType = AgendaCardType.Law,
                GameVersion = GameVersion.BaseGame,
            },
            new AgendaCard
            {
                AgendaCardName = AgendaCardName.HolyPlanetOfIxth,
                AgendaCardType = AgendaCardType.Law,
                GameVersion = GameVersion.BaseGame,
            },
            new AgendaCard
            {
                AgendaCardName = AgendaCardName.HomelandDefenseAct,
                AgendaCardType = AgendaCardType.Law,
                GameVersion = GameVersion.BaseGame,
            },
            new AgendaCard
            {
                AgendaCardName = AgendaCardName.ImperialArbiter,
                AgendaCardType = AgendaCardType.Law,
                GameVersion = GameVersion.BaseGame,
            },
            new AgendaCard
            {
                AgendaCardName = AgendaCardName.IncentiveProgram,
                AgendaCardType = AgendaCardType.Directive,
                GameVersion = GameVersion.BaseGame,
            },
            new AgendaCard
            {
                AgendaCardName = AgendaCardName.IxthianArtifact,
                AgendaCardType = AgendaCardType.Directive,
                GameVersion = GameVersion.BaseGame,
            },
            new AgendaCard
            {
                AgendaCardName = AgendaCardName.JudicialAbolishment,
                AgendaCardType = AgendaCardType.Directive,
                GameVersion = GameVersion.BaseGame,
            },
            new AgendaCard
            {
                AgendaCardName = AgendaCardName.MinisterOfCommerce,
                AgendaCardType = AgendaCardType.Law,
                GameVersion = GameVersion.BaseGame,
            },
            new AgendaCard
            {
                AgendaCardName = AgendaCardName.MinisterOfExploration,
                AgendaCardType = AgendaCardType.Law,
                GameVersion = GameVersion.BaseGame,
            },
            new AgendaCard
            {
                AgendaCardName = AgendaCardName.MinisterOfIndustry,
                AgendaCardType = AgendaCardType.Law,
                GameVersion = GameVersion.BaseGame,
            },
            new AgendaCard
            {
                AgendaCardName = AgendaCardName.MinisterOfPeace,
                AgendaCardType = AgendaCardType.Law,
                GameVersion = GameVersion.BaseGame,
            },
            new AgendaCard
            {
                AgendaCardName = AgendaCardName.MinisterOfPolicy,
                AgendaCardType = AgendaCardType.Law,
                GameVersion = GameVersion.BaseGame,
            },
            new AgendaCard
            {
                AgendaCardName = AgendaCardName.MinisterOfSciences,
                AgendaCardType = AgendaCardType.Law,
                GameVersion = GameVersion.BaseGame,
            },
            new AgendaCard
            {
                AgendaCardName = AgendaCardName.MinisterOfWar,
                AgendaCardType = AgendaCardType.Law,
                GameVersion = GameVersion.BaseGame,
            },
            new AgendaCard
            {
                AgendaCardName = AgendaCardName.MiscountDisclosed,
                AgendaCardType = AgendaCardType.Directive,
                GameVersion = GameVersion.BaseGame,
            },
            new AgendaCard
            {
                AgendaCardName = AgendaCardName.Mutiny,
                AgendaCardType = AgendaCardType.Directive,
                GameVersion = GameVersion.BaseGame,
            },
            new AgendaCard
            {
                AgendaCardName = AgendaCardName.NewConstitution,
                AgendaCardType = AgendaCardType.Directive,
                GameVersion = GameVersion.BaseGame,
            },
            new AgendaCard
            {
                AgendaCardName = AgendaCardName.ProphecyOfIxth,
                AgendaCardType = AgendaCardType.Law,
                GameVersion = GameVersion.BaseGame,
            },
            new AgendaCard
            {
                AgendaCardName = AgendaCardName.PublicExecution,
                AgendaCardType = AgendaCardType.Directive,
                GameVersion = GameVersion.BaseGame,
            },
            new AgendaCard
            {
                AgendaCardName = AgendaCardName.PublicizeWeaponSchematics,
                AgendaCardType = AgendaCardType.Law,
                GameVersion = GameVersion.BaseGame,
            },
            new AgendaCard
            {
                AgendaCardName = AgendaCardName.RegulatedConscription,
                AgendaCardType = AgendaCardType.Law,
                GameVersion = GameVersion.BaseGame,
            },
            new AgendaCard
            {
                AgendaCardName = AgendaCardName.RepresentativeGovernment,
                AgendaCardType = AgendaCardType.Law,
                GameVersion = GameVersion.BaseGame,
            },
            new AgendaCard
            {
                AgendaCardName = AgendaCardName.ResearchTeamBiotic,
                AgendaCardType = AgendaCardType.Law,
                GameVersion = GameVersion.BaseGame,
            },
            new AgendaCard
            {
                AgendaCardName = AgendaCardName.ResearchTeamCybernetic,
                AgendaCardType = AgendaCardType.Law,
                GameVersion = GameVersion.BaseGame,
            },
            new AgendaCard
            {
                AgendaCardName = AgendaCardName.ResearchTeamPropulsion,
                AgendaCardType = AgendaCardType.Law,
                GameVersion = GameVersion.BaseGame,
            },
            new AgendaCard
            {
                AgendaCardName = AgendaCardName.ResearchTeamWarfare,
                AgendaCardType = AgendaCardType.Law,
                GameVersion = GameVersion.BaseGame,
            },
            new AgendaCard
            {
                AgendaCardName = AgendaCardName.SeedOfAnEmpire,
                AgendaCardType = AgendaCardType.Directive,
                GameVersion = GameVersion.BaseGame,
            },
            new AgendaCard
            {
                AgendaCardName = AgendaCardName.SenateSanctuary,
                AgendaCardType = AgendaCardType.Law,
                GameVersion = GameVersion.BaseGame,
            },
            new AgendaCard
            {
                AgendaCardName = AgendaCardName.ShardOfTheThrone,
                AgendaCardType = AgendaCardType.Law,
                GameVersion = GameVersion.BaseGame,
            },
            new AgendaCard
            {
                AgendaCardName = AgendaCardName.SharedResearch,
                AgendaCardType = AgendaCardType.Law,
                GameVersion = GameVersion.BaseGame,
            },
            new AgendaCard
            {
                AgendaCardName = AgendaCardName.SwordsToPlowshares,
                AgendaCardType = AgendaCardType.Directive,
                GameVersion = GameVersion.BaseGame,
            },
            new AgendaCard
            {
                AgendaCardName = AgendaCardName.TerraformingInitiative,
                AgendaCardType = AgendaCardType.Law,
                GameVersion = GameVersion.BaseGame,
            },
            new AgendaCard
            {
                AgendaCardName = AgendaCardName.TheCrownOfEmphidia,
                AgendaCardType = AgendaCardType.Law,
                GameVersion = GameVersion.BaseGame,
            },
            new AgendaCard
            {
                AgendaCardName = AgendaCardName.TheCrownOfThalnos,
                AgendaCardType = AgendaCardType.Law,
                GameVersion = GameVersion.BaseGame,
            },
            new AgendaCard
            {
                AgendaCardName = AgendaCardName.UnconventionalMeasures,
                AgendaCardType = AgendaCardType.Directive,
                GameVersion = GameVersion.BaseGame,
            },
            new AgendaCard
            {
                AgendaCardName = AgendaCardName.WormholeReconstruction,
                AgendaCardType = AgendaCardType.Law,
                GameVersion = GameVersion.BaseGame,
            },
            new AgendaCard
            {
                AgendaCardName = AgendaCardName.WormholeResearch,
                AgendaCardType = AgendaCardType.Directive,
                GameVersion = GameVersion.BaseGame,
            },

            // Propecy of Kings
            new AgendaCard
            {
                AgendaCardName = AgendaCardName.ArmedForcesStandardization,
                AgendaCardType = AgendaCardType.Directive,
                GameVersion = GameVersion.ProphecyOfKing,
            },
            new AgendaCard
            {
                AgendaCardName = AgendaCardName.ArticlesOfWar,
                AgendaCardType = AgendaCardType.Law,
                GameVersion = GameVersion.ProphecyOfKing,
            },
            new AgendaCard
            {
                AgendaCardName = AgendaCardName.ChecksAndBalances,
                AgendaCardType = AgendaCardType.Law,
                GameVersion = GameVersion.ProphecyOfKing,
            },
            new AgendaCard
            {
                AgendaCardName = AgendaCardName.ClandestineOperations,
                AgendaCardType = AgendaCardType.Directive,
                GameVersion = GameVersion.ProphecyOfKing,
            },
            new AgendaCard
            {
                AgendaCardName = AgendaCardName.CovertLegislation,
                AgendaCardType = AgendaCardType.Directive,
                GameVersion = GameVersion.ProphecyOfKing,
            },
            new AgendaCard
            {
                AgendaCardName = AgendaCardName.GalacticCrisisPact,
                AgendaCardType = AgendaCardType.Directive,
                GameVersion = GameVersion.ProphecyOfKing,
            },
            new AgendaCard
            {
                AgendaCardName = AgendaCardName.MinisterOfAntiquities,
                AgendaCardType = AgendaCardType.Directive,
                GameVersion = GameVersion.ProphecyOfKing,
            },
            new AgendaCard
            {
                AgendaCardName = AgendaCardName.NexusSovereignty,
                AgendaCardType = AgendaCardType.Law,
                GameVersion = GameVersion.ProphecyOfKing,
            },
            new AgendaCard
            {
                AgendaCardName = AgendaCardName.PoliticalCensure,
                AgendaCardType = AgendaCardType.Law,
                GameVersion = GameVersion.ProphecyOfKing,
            },
            new AgendaCard
            {
                AgendaCardName = AgendaCardName.RearmamentAgreement,
                AgendaCardType = AgendaCardType.Directive,
                GameVersion = GameVersion.ProphecyOfKing,
            },
            new AgendaCard
            {
                AgendaCardName = AgendaCardName.RepresentativeGovernmentProphecyOfKings,
                AgendaCardType = AgendaCardType.Law,
                GameVersion = GameVersion.ProphecyOfKing,
            },
            new AgendaCard
            {
                AgendaCardName = AgendaCardName.ResearchGrantReallocation,
                AgendaCardType = AgendaCardType.Directive,
                GameVersion = GameVersion.ProphecyOfKing,
            },
            new AgendaCard
            {
                AgendaCardName = AgendaCardName.SearchWarrant,
                AgendaCardType = AgendaCardType.Law,
                GameVersion = GameVersion.ProphecyOfKing,
            },
        };

        var updatedAgendaCards = agendaCards.Select(agendaCard =>
        {
            agendaCard.Name = $"{agendaCard.AgendaCardName}_{nameof(ActionCard.Name)}";
            agendaCard.Text = $"{agendaCard.AgendaCardName}_{nameof(ActionCard.Text)}";
            agendaCard.Type = CardType.Agenda;
            agendaCard.ImagePath = _cardImagePathService.GetCardImagePath(agendaCard.AgendaCardName, agendaCard.Type);
            return agendaCard;
        });

        context.AddRange(updatedAgendaCards);
        context.SaveChanges();
    }
}
