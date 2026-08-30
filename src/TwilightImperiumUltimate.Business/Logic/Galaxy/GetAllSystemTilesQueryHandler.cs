using Microsoft.Extensions.Caching.Memory;

namespace TwilightImperiumUltimate.Business.Logic.Galaxy;

public class GetAllSystemTilesQueryHandler(
    IGalaxyRepository systemTileRepository,
    IMapper mapper,
    IMemoryCache memoryCache)
    : IRequestHandler<GetAllSystemTilesQuery, ItemListDto<SystemTileDto>>
{
    private const string CacheKey = "galaxy:system-tiles:all";
    private static readonly TimeSpan CacheDuration = TimeSpan.FromHours(1);

    private readonly IGalaxyRepository _galaxyRepository = systemTileRepository;
    private readonly IMapper _mapper = mapper;
    private readonly IMemoryCache _memoryCache = memoryCache;

    public async Task<ItemListDto<SystemTileDto>> Handle(GetAllSystemTilesQuery request, CancellationToken cancellationToken)
    {
        if (_memoryCache.TryGetValue(CacheKey, out ItemListDto<SystemTileDto>? cached) && cached is not null)
            return cached;

        var systemTiles = await _galaxyRepository.GetAllSystemTiles(cancellationToken);

        var systemTilesDto = _mapper.Map<List<SystemTileDto>>(systemTiles);

        var result = new ItemListDto<SystemTileDto>(systemTilesDto);
        _memoryCache.Set(CacheKey, result, CacheDuration);

        return result;
    }
}
