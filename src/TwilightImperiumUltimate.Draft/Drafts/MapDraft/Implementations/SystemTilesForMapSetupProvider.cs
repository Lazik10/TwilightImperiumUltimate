using TwilightImperiumUltimate.DataAccess.Repositories;
using TwilightImperiumUltimate.Draft.Drafts.MapDraft.Interfaces;
using TwilightImperiumUltimate.Draft.Drafts.MapDraft.MapSettings;
using TwilightImperiumUltimate.Draft.ValueObjects;

namespace TwilightImperiumUltimate.Draft.Drafts.MapDraft.Implementations;

public class SystemTilesForMapSetupProvider(
    IGalaxyRepository galaxyRepository)
    : ISystemTilesForMapSetupProvider
{
    private readonly IGalaxyRepository _galaxyRepository = galaxyRepository;
    private readonly Random _random = new();

    public async Task<SystemTilesForMapSetup> GetSystemTilesForMapSetup(IMapSettings mapSettings, GenerateMapRequest request, CancellationToken cancellationToken)
    {
        var mecatolRex = await _galaxyRepository.GetMecatolRex(cancellationToken);
        var emptyTilePlaceholder = await _galaxyRepository.GetEmptyPlaceholderSystemTile(cancellationToken);
        var homeTilePlaceholder = await _galaxyRepository.GetHomePlaceholderSystemTile(cancellationToken);
        var homeTiles = await _galaxyRepository.GetHomeSystemTiles(cancellationToken);
        var shuffledHomeTiles = homeTiles.OrderBy(x => _random.Next()).ToList();

        var blueTiles = await _galaxyRepository.GetBlueSystemTiles(cancellationToken);
        var shuffledBlueTiles = blueTiles
            .Where(x => request.GameVersions.Contains(x.GameVersion))
            .OrderBy(x => _random.Next())
            .ToList();

        var redTiles = await _galaxyRepository.GetRedSystemTiles(cancellationToken);
        var shuffledRedTiles = redTiles
            .Where(x => request.GameVersions.Contains(x.GameVersion))
            .OrderBy(x => _random.Next())
            .ToList();

        var hyperLines = await _galaxyRepository.GetAllHyperlines(cancellationToken);

        var frameSystemPlaceholder = await _galaxyRepository.GetFrameSystemTile(cancellationToken);

        return new SystemTilesForMapSetup(
            mecatolRex,
            emptyTilePlaceholder,
            homeTilePlaceholder,
            frameSystemPlaceholder,
            shuffledHomeTiles,
            shuffledBlueTiles,
            shuffledRedTiles,
            hyperLines);
    }
}
