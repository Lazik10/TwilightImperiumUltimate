using TwilightImperiumUltimate.Web.Helpers.Enums;

namespace TwilightImperiumUltimate.Web.Services.Factions;

/// <inheritdoc cref="IFactionProvider" />
public class FactionProvider(
    ITwilightImperiumApiHttpClient httpClient,
    NavigationManager navigationManager) : IFactionProvider
{
    private static readonly TimeSpan CacheDuration = TimeSpan.FromHours(24);
    private readonly SemaphoreSlim _refreshLock = new(1, 1);
    private readonly ITwilightImperiumApiHttpClient _httpClient = httpClient;
    private readonly NavigationManager _navigationManager = navigationManager;

    private IReadOnlyCollection<FactionDto> _factions = Array.Empty<FactionDto>();
    private FactionDto? _currentFaction;
    private FactionName _currentFactionName;
    private FactionSource _currentSource;
    private DateTimeOffset _lastFetched = DateTimeOffset.MinValue;

    public FactionName CurrentFactionName => _currentFactionName;

    public FactionSource CurrentSource => _currentSource;

    public FactionDto? CurrentFaction => _currentFaction;

    private bool IsExpired => _factions.Count == 0 || DateTimeOffset.UtcNow - _lastFetched > CacheDuration;

    public void ClearSource() => _currentSource = FactionSource.Official;

    public void SetSource(FactionSource source) => _currentSource = source;

    public void SetCurrentFactionName(FactionName factionName)
    {
        _currentFactionName = factionName;
        _currentFaction = GetFactionByName(factionName);
        _currentSource = factionName.GetFactionSource();

        _navigationManager.NavigateTo($"{Pages.Pages.Factions}/{_currentFactionName}");
    }
    
    public FactionDto? GetFactionByName(FactionName factionName) => _factions?.SingleOrDefault(x => x.FactionName == factionName);

    public void UpdateSourceAndFaction(FactionSource source, FactionName factionName)
    {
        _currentSource = source;
        _currentFactionName = factionName;
        _currentFaction = GetFactionByName(factionName);
    }

    public async Task<IReadOnlyCollection<FactionDto>> InitializeFactions(CancellationToken cancellationToken = default)
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

        return _factions;
    }

    public async Task<IReadOnlyCollection<FactionDto>> GetAllFactions(CancellationToken cancellationToken = default)
    {
        if (IsExpired)
            await InitializeFactions(cancellationToken);

        return _factions;
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
