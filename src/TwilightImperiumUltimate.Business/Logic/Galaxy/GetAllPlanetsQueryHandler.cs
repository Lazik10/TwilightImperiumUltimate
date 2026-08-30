using Microsoft.Extensions.Caching.Memory;

namespace TwilightImperiumUltimate.Business.Logic.Galaxy;

public class GetAllPlanetsQueryHandler(
    IGalaxyRepository systemTileRepository,
    IMapper mapper,
    IMemoryCache memoryCache)
    : IRequestHandler<GetAllPlanetsQuery, ItemListDto<PlanetDto>>
{
    private const string CacheKey = "galaxy:planets:all";
    private static readonly TimeSpan CacheDuration = TimeSpan.FromHours(1);

    private readonly IGalaxyRepository _systemTileRepository = systemTileRepository;
    private readonly IMapper _mapper = mapper;
    private readonly IMemoryCache _memoryCache = memoryCache;

    public async Task<ItemListDto<PlanetDto>> Handle(GetAllPlanetsQuery request, CancellationToken cancellationToken)
    {
        if (_memoryCache.TryGetValue(CacheKey, out ItemListDto<PlanetDto>? cached) && cached is not null)
            return cached;

        var planets = await _systemTileRepository.GetAllPlanets(cancellationToken);

        var planetsDto = _mapper.Map<List<PlanetDto>>(planets);

        var result = new ItemListDto<PlanetDto>(planetsDto);
        _memoryCache.Set(CacheKey, result, CacheDuration);

        return result;
    }
}
