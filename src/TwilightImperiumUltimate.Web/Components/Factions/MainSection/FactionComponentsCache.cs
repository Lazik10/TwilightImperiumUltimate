using TwilightImperiumUltimate.Contracts.DTOs.Card;

namespace TwilightImperiumUltimate.Web.Components.Factions.MainSection;

internal static class FactionComponentsCache
{
    private static readonly SemaphoreSlim CacheLock = new(1, 1);

    private static bool _isLoaded;

    public static IReadOnlyList<SystemTileModel> SystemTiles { get; private set; } = [];

    public static IReadOnlyList<TechnologyModel> Technologies { get; private set; } = [];

    public static IReadOnlyList<PromissoryNoteCardModel> PromissoryNotes { get; private set; } = [];

    public static IReadOnlyList<BreakthroughCardModel> BreakthroughCards { get; private set; } = [];

    public static IReadOnlyList<FlagshipCardModel> FlagshipCards { get; private set; } = [];

    public static IReadOnlyList<SpecialComponentCardModel> SpecialComponentCards { get; private set; } = [];

    public static async Task EnsureLoadedAsync(ITwilightImperiumApiHttpClient httpClient, IMapper mapper)
    {
        if (_isLoaded)
            return;

        await CacheLock.WaitAsync();
        try
        {
            if (_isLoaded)
                return;

            await LoadSystemTilesAsync(httpClient, mapper);
            await LoadTechnologiesAsync(httpClient, mapper);
            await LoadPromissoryNoteCardsAsync(httpClient, mapper);
            await LoadBreakthroughCardsAsync(httpClient, mapper);
            await LoadFlagshipCardsAsync(httpClient, mapper);
            await LoadSpecialComponentCardsAsync(httpClient, mapper);

            _isLoaded = SystemTiles.Count > 0;
        }
        finally
        {
            CacheLock.Release();
        }
    }

    private static async Task LoadSystemTilesAsync(ITwilightImperiumApiHttpClient httpClient, IMapper mapper)
    {
        var result = await httpClient.GetAsync<ApiResponse<ItemListDto<SystemTileDto>>>(Paths.ApiPath_SystemTiles);

        if (result.StatusCode == HttpStatusCode.OK)
            SystemTiles = mapper.Map<List<SystemTileModel>>(result.Response!.Data!.Items);
    }

    private static async Task LoadTechnologiesAsync(ITwilightImperiumApiHttpClient httpClient, IMapper mapper)
    {
        var result = await httpClient.GetAsync<ApiResponse<ItemListDto<TechnologyDto>>>(Paths.ApiPath_Technologies);

        if (result.StatusCode == HttpStatusCode.OK)
            Technologies = mapper.Map<List<TechnologyModel>>(result.Response!.Data!.Items!);
    }

    private static async Task LoadPromissoryNoteCardsAsync(ITwilightImperiumApiHttpClient httpClient, IMapper mapper)
    {
        var result = await httpClient.GetAsync<ApiResponse<ItemListDto<PromissoryNoteCardDto>>>(Paths.ApiPath_PromissoryNotes);

        if (result.StatusCode == HttpStatusCode.OK)
            PromissoryNotes = mapper.Map<List<PromissoryNoteCardModel>>(result.Response!.Data!.Items!);
    }

    private static async Task LoadBreakthroughCardsAsync(ITwilightImperiumApiHttpClient httpClient, IMapper mapper)
    {
        var result = await httpClient.GetAsync<ApiResponse<ItemListDto<BreakthroughCardDto>>>(Paths.ApiPath_BreakthroughCards);

        if (result.StatusCode == HttpStatusCode.OK)
        {
            var excludedBreakthroughs = new List<BreakthroughName>
            {
                BreakthroughName.Corsair,
                BreakthroughName.EidolonMaximum,
            };

            var filteredBreakthroughCards = result.Response!.Data!.Items!
                .Where(x => !excludedBreakthroughs.Contains(x.BreakthroughName))
                .ToList();

            BreakthroughCards = mapper.Map<List<BreakthroughCardModel>>(filteredBreakthroughCards);
        }
    }

    private static async Task LoadFlagshipCardsAsync(ITwilightImperiumApiHttpClient httpClient, IMapper mapper)
    {
        var result = await httpClient.GetAsync<ApiResponse<ItemListDto<FlagshipCardDto>>>(Paths.ApiPath_FlagshipCards);

        if (result.StatusCode == HttpStatusCode.OK)
            FlagshipCards = mapper.Map<List<FlagshipCardModel>>(result.Response!.Data!.Items!);
    }

    private static async Task LoadSpecialComponentCardsAsync(ITwilightImperiumApiHttpClient httpClient, IMapper mapper)
    {
        var result = await httpClient.GetAsync<ApiResponse<ItemListDto<SpecialComponentCardDto>>>(Paths.ApiPath_SpecialComponentCards);

        if (result.StatusCode == HttpStatusCode.OK)
            SpecialComponentCards = mapper.Map<List<SpecialComponentCardModel>>(result.Response!.Data!.Items!);
    }
}
