using TwilightImperiumUltimate.DataAccess.Repositories;
using TwilightImperiumUltimate.Draft.Drafts.MapDraft.Interfaces;
using TwilightImperiumUltimate.Draft.ValueObjects;

namespace TwilightImperiumUltimate.Draft.Drafts.MapDraft.Implementations;

public class SystemTilesForMapSetupProvider(
    IGalaxyRepository galaxyRepository)
    : ISystemTilesForMapSetupProvider
{
    private readonly IGalaxyRepository _galaxyRepository = galaxyRepository;
    private readonly Random _random = new();

    public async Task<SystemTilesForMapSetup> GetSystemTilesForMapSetup(CancellationToken cancellationToken)
    {
        var mecatolRex = await _galaxyRepository.GetMecatolRex(cancellationToken);
        var emptyTilePlaceholder = await _galaxyRepository.GetEmptyPlaceholderSystemTile(cancellationToken);
        var homeTilePlaceholder = await _galaxyRepository.GetHomePlaceholderSystemTile(cancellationToken);
        var homeTiles = await _galaxyRepository.GetHomeSystemTiles(cancellationToken);
        var shuffledHomeTiles = homeTiles.OrderBy(x => _random.Next()).ToList();
        var tiles = await _galaxyRepository.GetSystemTilesForBuildingGalaxy(cancellationToken);
        var shuffledTiles = tiles.OrderBy(x => _random.Next()).ToList();

        return new SystemTilesForMapSetup(mecatolRex, emptyTilePlaceholder, homeTilePlaceholder, shuffledHomeTiles, shuffledTiles);
    }
}
