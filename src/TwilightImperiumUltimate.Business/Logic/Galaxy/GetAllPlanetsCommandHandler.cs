namespace TwilightImperiumUltimate.Business.Logic.Galaxy;

public class GetAllPlanetsCommandHandler(
    IGalaxyRepository systemTileRepository,
    IMapper mapper)
    : IRequestHandler<GetAllPlanetsCommand, ItemListDto<PlanetDto>>
{
    private readonly IGalaxyRepository _systemTileRepository = systemTileRepository;
    private readonly IMapper _mapper = mapper;

    public async Task<ItemListDto<PlanetDto>> Handle(GetAllPlanetsCommand request, CancellationToken cancellationToken)
    {
        var planets = await _systemTileRepository.GetAllPlanets(cancellationToken);

        var planetsDto = _mapper.Map<List<PlanetDto>>(planets);

        return new ItemListDto<PlanetDto>(planetsDto);
    }
}
