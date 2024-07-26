namespace TwilightImperiumUltimate.DataAccess.Repositories;

public interface IFactionRepository
{
    Task<List<Faction>> GetAllFactions(CancellationToken cancellationToken);

    Task<Faction> GetFactionById(int id, CancellationToken cancellationToken);
}
