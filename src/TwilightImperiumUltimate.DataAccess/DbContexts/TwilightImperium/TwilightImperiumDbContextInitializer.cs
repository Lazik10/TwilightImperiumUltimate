using TwilightImperiumUltimate.DataAccess.Services;

namespace TwilightImperiumUltimate.DataAccess.DbContexts.TwilightImperium;

public partial class TwilightImperiumDbContextInitializer
{
    private readonly IDbContextFactory<TwilightImperiumDbContext> _dbContextFactory;
    private readonly ICreateCardImagePathService _cardImagePathService;

    public TwilightImperiumDbContextInitializer(
        IDbContextFactory<TwilightImperiumDbContext> dbContextFactory,
        ICreateCardImagePathService cardImagePathService)
    {
        _dbContextFactory = dbContextFactory;
        _cardImagePathService = cardImagePathService;
    }

    public void RestoreToDefault()
    {
        using var dbContext = _dbContextFactory.CreateDbContext();

        dbContext.Database.EnsureDeleted();
        dbContext.Database.EnsureCreated();

        InitializeTiles();
        InitializeTechnologies();
        InitializeRaces();
        InitializeEightTestPlayers();
        InitializeAllCards();
        InitializeFactionColorImportance();

        InitializeNewsArticles();
    }
}
