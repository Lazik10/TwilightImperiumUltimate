namespace TwilightImperiumUltimate.DataAccess.Repositories;

public interface IMapArchiveRepository
{
    Task<IEnumerable<Map>> GetAllMaps(CancellationToken cancellationToken);

    Task<Map?> GetMapById(int id, CancellationToken cancellationToken);

    Task<bool> AddNewMap(Map map, CancellationToken cancellationToken);

    Task<bool> DeleteMap(int id, CancellationToken cancellationToken);

    Task<MapRating?> GetMapRatingFromUser(string userId, int mapId, CancellationToken cancellationToken);

    Task<MapRating> AddOrUpdateUserMapRating(string userId, int mapId, float rating, CancellationToken cancellationToken);
}
