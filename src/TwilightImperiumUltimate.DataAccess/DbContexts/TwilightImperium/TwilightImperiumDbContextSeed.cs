using TwilightImperiumUltimate.DataAccess.DbContexts.TwilightImperium.Data;

namespace TwilightImperiumUltimate.DataAccess.DbContexts.TwilightImperium;

public static class TwilightImperiumDbContextSeed
{
    public static async Task SeedDatabaseAsync(this TwilightImperiumDbContext context)
    {
        ArgumentNullException.ThrowIfNull(context);

        // Cards
        await SeedIfEmpty(context.ActionCards, ActionCardsData.ActionCards, context);
        await SeedIfEmpty(context.AgendaCards, AgendaCardsData.AgendaCards, context);
        await SeedIfEmpty(context.ExplorationCards, ExplorationCardsData.ExplorationCards, context);
        await SeedIfEmpty(context.FrontierCards, FrontierCardsData.FrontierCards, context);
        await SeedIfEmpty(context.ObjectiveCards, ObjectiveCardsData.ObjectiveCards, context);
        await SeedIfEmpty(context.RelicCards, RelicCardsData.RelicCards, context);
        await SeedIfEmpty(context.StrategyCards, StrategyCardsData.StrategyCards, context);
        await SeedIfEmpty(context.PromissaryNoteCards, PromissaryNoteCardsData.PromissaryNoteCards, context);

        // Factions
        await SeedIfEmpty(context.Factions, FactionsData.Factions, context);
        await SeedIfEmpty(context.Units, UnitsData.Units, context);
        await SeedIfEmpty(context.FactionUnit, FactionUnitsData.FactionUnits, context);
        await SeedIfEmpty(context.FactionColorImportances, FactionColorImportancesData.FactionColorImportances, context);

        // Galaxy
        await SeedIfEmpty(context.SystemTiles, GalaxyData.SystemTiles, context);

        // Articles
        await SeedIfEmpty(context.Users, UsersData.Users, context);
        await SeedIfEmpty(context.NewsArticles, ArticlesData.Articles, context);

        // Players
        await SeedIfEmpty(context.Players, PlayersData.Players, context);

        // Rules
        await SeedIfEmpty(context.Rules, RulesData.Rules, context);

        // Technologies
        await SeedIfEmpty(context.Technologies, TechnologiesData.Technologies, context);
    }

    private static async Task SeedIfEmpty<TEntity>(DbSet<TEntity> dbSet, IEnumerable<TEntity> data, TwilightImperiumDbContext context)
        where TEntity : class
    {
        if (!await dbSet.AnyAsync())
        {
            await dbSet.AddRangeAsync(data);
            await context.SaveChangesAsync();
        }
    }
}
