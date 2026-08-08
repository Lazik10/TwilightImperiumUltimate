using TwilightImperiumUltimate.Web.Helpers.Enums;

namespace TwilightImperiumUltimate.Web.Services.Factions;

/// <inheritdoc cref="IFactionProvider" />
public class FactionProvider(ITwilightImperiumApiHttpClient httpClient) : IFactionProvider
{
    private static readonly TimeSpan CacheDuration = TimeSpan.FromHours(24);
    private readonly SemaphoreSlim _refreshLock = new(1, 1);
    private readonly ITwilightImperiumApiHttpClient _httpClient = httpClient;

    private IReadOnlyCollection<FactionDto>? _factions;
    private FactionDto? _currentFaction;
    private FactionName _currentFactionName;
    private FactionSource? _source;
    private DateTimeOffset _lastFetched = DateTimeOffset.MinValue;

    public FactionName CurrentFactionName => _currentFactionName;

    public FactionSource? Source => _source;

    public FactionDto? CurrentFaction => _currentFaction;

    private bool IsExpired => _factions is null || DateTimeOffset.UtcNow - _lastFetched > CacheDuration;

    public void ClearSource() => _source = null;

    public void SetSource(FactionSource source) => _source = source;

    public void SetCurrentFactionName(FactionName factionName) => _currentFactionName = factionName;
    
    public FactionDto? GetFactionByName(FactionName factionName) => _factions?.SingleOrDefault(x => x.FactionName == factionName);

    public async Task<IReadOnlyCollection<FactionDto>> GetAllFactions(CancellationToken cancellationToken = default)
    {
        if (!IsExpired)
            return _factions!;

        await _refreshLock.WaitAsync(cancellationToken);
        try
        {
            // Re-check after acquiring the lock in case another caller already refreshed the cache.
            if (IsExpired)
                await FetchAllFactions(cancellationToken);
        }
        finally
        {
            _refreshLock.Release();
        }

        return _factions ?? [];
    }

    public async Task<IReadOnlyCollection<FactionDto>> GetFactionsByGameVersions(IReadOnlyCollection<GameVersion> gameVersions, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(gameVersions);

        var factions = await GetAllFactions(cancellationToken);

        return factions.Where(x => gameVersions.Contains(x.GameVersion)).ToList();
    }

    public async Task<IReadOnlyCollection<FactionDto>> GetFactionsBySource(FactionSource source, CancellationToken cancellationToken = default)
    {
        var factions = await GetAllFactions(cancellationToken);

        return factions.Where(x => source.GetGameVersionsFromFactionSource().Contains(x.GameVersion)).ToList();
    }

    private async Task FetchAllFactions(CancellationToken cancellationToken)
    {
        var (response, statusCode) = await _httpClient.GetAsync<ApiResponse<ItemListDto<FactionDto>>>(Paths.ApiPath_Factions, cancellationToken: cancellationToken);

        if (statusCode != HttpStatusCode.OK || response?.Data is null)
            return;

        _factions = response.Data.Items!;
        _lastFetched = DateTimeOffset.UtcNow;
    }
}
