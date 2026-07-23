using TwilightImperiumUltimate.Core.Entities.Website;

namespace TwilightImperiumUltimate.DataAccess.Repositories;

public interface IWebsiteRepository
{
    Task<List<Website>> GetAllWebsites(CancellationToken cancellationToken);

    Task<Website> CreateWebsite(Website website, CancellationToken cancellationToken);

    Task<bool> DeleteWebsite(int id, CancellationToken cancellationToken);
}
