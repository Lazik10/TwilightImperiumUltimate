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
        await SeedIfEmpty(context.PromissoryNoteCards, PromissaryNoteCardsData.PromissaryNoteCards, context);

        // Factions
        await SeedIfEmpty(context.Factions, FactionsData.Factions, context);
        await SeedIfEmpty(context.Units, UnitsData.Units, context);
        await SeedIfEmpty(context.FactionUnit, FactionUnitsData.FactionUnits, context);
        await SeedIfEmpty(context.FactionColorImportances, FactionColorImportancesData.FactionColorImportances, context);

        // Galaxy
        await SeedIfEmpty(context.SystemTiles, GalaxyData.SystemTiles, context);
        await SeedIfEmpty(context.MapRedPositions, MapRedPositionsData.MapRedPositions, context);

        // Users
        await SeedIfEmpty(context.Users, UserData.Users, context);
        await SeedIfEmpty(context.Roles, RolesData.Roles, context);

        // Articles
        await SeedIfEmptyArticles(context.NewsArticles, ArticlesData.Articles, context);

        // Players
        await SeedIfEmpty(context.Players, PlayersData.Players, context);

        // Rules
        await SeedIfEmpty(context.Rules, RulesData.Rules, context);

        // Technologies
        await SeedIfEmpty(context.Technologies, TechnologiesData.Technologies, context);

        // Websited
        await SeedIfEmpty(context.Websites, WebsitesData.Websites, context);

        await SeedIfEmpty(context.FactionTechnology, FactionTechnologiesData.FactionTechnologies, context);
    }

    private static async Task SeedIfEmpty<TEntity>(DbSet<TEntity> dbSet, List<TEntity> data, TwilightImperiumDbContext context)
        where TEntity : class
    {
        if (!await dbSet.AnyAsync())
        {
            try
            {
                await dbSet.AddRangeAsync(data);
                await context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Unable to seed data for DbSet: {dbSet.EntityType.Name} and data {data.Count} with exception: {ex.Message}");
            }
        }
    }

    private static async Task SeedIfEmptyArticles(DbSet<NewsArticle> dbSet, IEnumerable<NewsArticle> data, TwilightImperiumDbContext context)
    {
        if (!await dbSet.AnyAsync())
        {
            var user = await context.Users.FirstOrDefaultAsync();
            if (user is not null)
            {
                foreach (var article in data)
                {
                    article.UserId = user.Id;
                }
            }

            await dbSet.AddRangeAsync(data);
            await context.SaveChangesAsync();
        }
    }
}
