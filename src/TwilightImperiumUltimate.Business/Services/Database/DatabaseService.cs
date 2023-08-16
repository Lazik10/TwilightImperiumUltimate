using TwilightImperiumUltimate.DataAccess.DbContexts.TwilightImperium;

namespace TwilightImperiumUltimate.Business.Services.Database;

public class DatabaseService
{
    private readonly TwilightImperiumDbContextInitializer _dbContextInitializer;

    public DatabaseService(TwilightImperiumDbContextInitializer dbContextInitializer)
    {
        _dbContextInitializer = dbContextInitializer;
    }

    public void InitializeDatabase()
    {
        _dbContextInitializer.RestoreToDefault();
    }
}
