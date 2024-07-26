namespace TwilightImperiumUltimate.DataAccess.Repositories;

public interface ITechnologyRepository
{
    Task<List<Technology>> GetAllTechnologies(CancellationToken cancellationToken);
}
