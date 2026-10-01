using Microsoft.Extensions.Caching.Memory;

namespace TwilightImperiumUltimate.Business.Logic.Technologies;

public class GetAllTechnologiesQueryHandler(
    ITechnologyRepository technologyRepository,
    IMapper mapper,
    IMemoryCache memoryCache)
    : IRequestHandler<GetAllTechnologiesQuery, ItemListDto<TechnologyDto>>
{
    private const string CacheKey = "technologies:all";
    private static readonly TimeSpan CacheDuration = TimeSpan.FromHours(1);

    private readonly ITechnologyRepository _technologyRepository = technologyRepository;
    private readonly IMapper _mapper = mapper;
    private readonly IMemoryCache _memoryCache = memoryCache;

    public async Task<ItemListDto<TechnologyDto>> Handle(GetAllTechnologiesQuery request, CancellationToken cancellationToken)
    {
        if (_memoryCache.TryGetValue(CacheKey, out ItemListDto<TechnologyDto>? cached) && cached is not null)
            return cached;

        var technologies = await _technologyRepository.GetAllTechnologies(cancellationToken);

        var technologiesDto = _mapper.Map<List<TechnologyDto>>(technologies);

        var result = new ItemListDto<TechnologyDto>(technologiesDto);
        _memoryCache.Set(CacheKey, result, CacheDuration);

        return result;
    }
}
