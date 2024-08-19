namespace TwilightImperiumUltimate.Business.Logic.Galaxy;

public class GetAllSystemTilesQueryHandler(
    IGalaxyRepository systemTileRepository,
    IMapper mapper)
    : IRequestHandler<GetAllSystemTilesQuery, ItemListDto<SystemTileDto>>
{
    private readonly IGalaxyRepository _galaxyRepository = systemTileRepository;
    private readonly IMapper _mapper = mapper;

    public async Task<ItemListDto<SystemTileDto>> Handle(GetAllSystemTilesQuery request, CancellationToken cancellationToken)
    {
        var systemTiles = await _galaxyRepository.GetAllSystemTiles(cancellationToken);

        var systemTilesDto = _mapper.Map<List<SystemTileDto>>(systemTiles);

        return new ItemListDto<SystemTileDto>(systemTilesDto);
    }
}
