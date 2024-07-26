using TwilightImperiumUltimate.Core.Entities.Website;

namespace TwilightImperiumUltimate.DataAccess.Repositories;

public interface IWebsiteRepository
{
    Task<List<Website>> GetAllWebsites(CancellationToken cancellationToken);
}
