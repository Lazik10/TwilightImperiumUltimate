using TwilightImperiumUltimate.Core.Entities.Statistics;
using TwilightImperiumUltimate.Core.Entities.Tigl;
using TwilightImperiumUltimate.Core.Entities.Tigl.Ranks;
using TwilightImperiumUltimate.DataAccess.DbContexts.TwilightImperium.Data;

namespace TwilightImperiumUltimate.DataAccess.DbContexts.TwilightImperium;

public static class TwilightImperiumDbContextSeed
{
    public static void SeedDatabaseAsync(this TwilightImperiumDbContext context, ModelBuilder modelBuilder)
    {
        ArgumentNullException.ThrowIfNull(context);
        ArgumentNullException.ThrowIfNull(modelBuilder);

        modelBuilder.Entity<ActionCard>().HasData(ActionCardsData.ActionCards);
        modelBuilder.Entity<AgendaCard>().HasData(AgendaCardsData.AgendaCards);
        modelBuilder.Entity<ExplorationCard>().HasData(ExplorationCardsData.ExplorationCards);
        modelBuilder.Entity<FrontierCard>().HasData(FrontierCardsData.FrontierCards);
        modelBuilder.Entity<ObjectiveCard>().HasData(ObjectiveCardsData.ObjectiveCards);
        modelBuilder.Entity<RelicCard>().HasData(RelicCardsData.RelicCards);
        modelBuilder.Entity<StrategyCard>().HasData(StrategyCardsData.StrategyCards);
        modelBuilder.Entity<PromissoryNoteCard>().HasData(PromissaryNoteCardsData.PromissaryNoteCards);
        modelBuilder.Entity<BreakthroughCard>().HasData(BreakthroughCardsData.BreakthroughCards);
        modelBuilder.Entity<FlagshipCard>().HasData(FlagshipCardsData.FlagshipCards);
        modelBuilder.Entity<SpecialComponentCard>().HasData(SpecialComponentCardsData.SpecialComponentCards);

        modelBuilder.Entity<Faction>().HasData(FactionsData.Factions);
        modelBuilder.Entity<Unit>().HasData(UnitsData.Units);
        modelBuilder.Entity<FactionUnit>().HasData(FactionUnitsData.FactionUnits);
        modelBuilder.Entity<FactionColorImportance>().HasData(FactionColorImportancesData.FactionColorImportances);

        modelBuilder.Entity<Planet>().HasData(PlanetsData.Planets);
        modelBuilder.Entity<Wormhole>().HasData(WormholesData.Wormholes);
        modelBuilder.Entity<SystemTile>().HasData(SystemTilesData.SystemTiles);

        modelBuilder.Entity<TwilightImperiumUser>().HasData(UserData.Users);
        modelBuilder.Entity<IdentityRole>().HasData(RolesData.Roles);

        modelBuilder.Entity<NewsArticle>().HasData(ArticlesData.Articles);
        modelBuilder.Entity<Map>().HasData(MapData.Maps);
        modelBuilder.Entity<SliceDraft>().HasData(SlicesDraftData.SliceDrafts);

        modelBuilder.Entity<Rule>().HasData(RulesData.Rules);
        modelBuilder.Entity<Faq>().HasData(FaqData.Faqs);

        modelBuilder.Entity<Technology>().HasData(TechnologiesData.Technologies);

        modelBuilder.Entity<Website>().HasData(WebsitesData.Websites);

        modelBuilder.Entity<FactionTechnology>().HasData(FactionTechnologiesData.FactionTechnologies);

        modelBuilder.Entity<WebsiteStatistics>().HasData(WebsiteStatisticsData.WebsiteStatistics);

        modelBuilder.Entity<Season>().HasData(SeasonsData.Seasons);

        modelBuilder.Entity<TiglParameter>().HasData(TiglParametersData.TiglParameters);

        modelBuilder.Entity<Achievement>().HasData(AchievementsData.Achievements);

        modelBuilder.Entity<PrestigeRank>().HasData(PrestigeRankData.PrestigeRanks);

        modelBuilder.Entity<Leader>().HasData(LeadersData.Leaders);
    }
}
