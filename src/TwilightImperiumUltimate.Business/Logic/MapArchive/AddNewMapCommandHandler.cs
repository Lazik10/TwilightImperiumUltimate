namespace TwilightImperiumUltimate.Business.Logic.MapArchive;

public class AddNewMapCommandHandler(
    IMapArchiveRepository mapArchiveRepository,
    IMapper mapper)
    : IRequestHandler<AddNewMapCommand, bool>
{
    private readonly IMapArchiveRepository _mapArchiveRepository = mapArchiveRepository;
    private readonly IMapper _mapper = mapper;

    public async Task<bool> Handle(AddNewMapCommand request, CancellationToken cancellationToken)
    {
        var map = _mapper.Map<Map>(request.Map);
        return await _mapArchiveRepository.AddNewMap(map, cancellationToken);
    }
}
