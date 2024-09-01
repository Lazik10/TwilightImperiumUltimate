using Microsoft.Extensions.Logging;

namespace TwilightImperiumUltimate.DataAccess.Repositories;

public class MapArchiveRepository(
    IDbContextFactory<TwilightImperiumDbContext> context,
    ILogger<MapArchiveRepository> logger)
    : IMapArchiveRepository
{
    private readonly IDbContextFactory<TwilightImperiumDbContext> _context = context;
    private readonly ILogger<MapArchiveRepository> _logger = logger;

    public async Task<bool> AddNewMap(Map map, CancellationToken cancellationToken)
    {
        await using var dbContext = await _context.CreateDbContextAsync(cancellationToken);

        try
        {
            dbContext.Maps.Add(map);
            await dbContext.SaveChangesAsync(cancellationToken);

            map.MapArchiveLink = string.Concat(map.MapArchiveLink, map.Id);

            dbContext.Maps.Update(map);
            await dbContext.SaveChangesAsync(cancellationToken);

            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to add new map");
            return false;
        }
    }

    public async Task<MapRating> AddOrUpdateUserMapRating(string userId, int mapId, float rating, CancellationToken cancellationToken)
    {
        await using var dbContext = await _context.CreateDbContextAsync(cancellationToken);

        var mapRating = await dbContext.MapRatings
            .Where(x => x.UserId == userId && x.MapId == mapId)
            .FirstOrDefaultAsync(cancellationToken);

        if (mapRating is null)
        {
            mapRating = new MapRating
            {
                UserId = userId,
                MapId = mapId,
                Rating = rating,
            };

            dbContext.MapRatings.Add(mapRating);
        }
        else
        {
            mapRating.Rating = rating;
            dbContext.MapRatings.Update(mapRating);
        }

        await dbContext.SaveChangesAsync(cancellationToken);

        return mapRating;
    }

    public async Task<bool> DeleteMap(int id, CancellationToken cancellationToken)
    {
        await using var dbContext = await _context.CreateDbContextAsync(cancellationToken);
        var map = await dbContext.Maps.FindAsync(new object?[] { id, cancellationToken }, cancellationToken: cancellationToken);

        if (map is null)
        {
            return false;
        }

        dbContext.Maps.Remove(map);
        await dbContext.SaveChangesAsync(cancellationToken);
        return true;
    }

    public async Task<IEnumerable<Map>> GetAllMaps(CancellationToken cancellationToken)
    {
        await using var dbContext = await _context.CreateDbContextAsync(cancellationToken);
        return await dbContext.Maps
            .Include(x => x.MapRatings)
            .ToListAsync(cancellationToken);
    }

    public async Task<Map?> GetMapById(int id, CancellationToken cancellationToken)
    {
        await using var dbContext = await _context.CreateDbContextAsync(cancellationToken);
        return await dbContext.Maps
            .Include(x => x.MapRatings)
            .Where(x => x.Id == id)
            .FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<MapRating?> GetMapRatingFromUser(string userId, int mapId, CancellationToken cancellationToken)
    {
        await using var dbContext = await _context.CreateDbContextAsync(cancellationToken);

        return await dbContext.MapRatings
            .Where(x => x.UserId == userId && x.MapId == mapId)
            .FirstOrDefaultAsync(cancellationToken);
    }
}
