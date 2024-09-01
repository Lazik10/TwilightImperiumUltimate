namespace TwilightImperiumUltimate.Business.Logic.MapArchive;

public class GetAllMapsQueryHandler(
    IMapArchiveRepository mapArchiveRepository,
    IMapper mapper)
    : IRequestHandler<GetAllMapsQuery, ItemListDto<MapDto>>
{
    private readonly IMapArchiveRepository _mapArchiveRepository = mapArchiveRepository;
    private readonly IMapper _mapper = mapper;

    public async Task<ItemListDto<MapDto>> Handle(GetAllMapsQuery request, CancellationToken cancellationToken)
    {
        var maps = await _mapArchiveRepository.GetAllMaps(cancellationToken);
        var mapsList = maps.ToList();
        var mapsDto = _mapper.Map<List<MapDto>>(mapsList);
        return new ItemListDto<MapDto>(mapsDto);
    }
}
