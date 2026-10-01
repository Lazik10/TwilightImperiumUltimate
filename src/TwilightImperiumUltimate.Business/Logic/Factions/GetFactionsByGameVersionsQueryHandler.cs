namespace TwilightImperiumUltimate.Business.Logic.Factions;

public class GetFactionsByGameVersionsQueryHandler(
    IFactionRepository factionRepository,
    IMapper mapper)
    : IRequestHandler<GetFactionsByGameVersionsQuery, ItemListDto<FactionDto>>
{
    private readonly IFactionRepository _factionRepository = factionRepository;
    private readonly IMapper _mapper = mapper;

    public async Task<ItemListDto<FactionDto>> Handle(GetFactionsByGameVersionsQuery request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        if (request.GameVersions.Count == 0)
            throw new ArgumentException("At least one game version must be provided.", nameof(request));

        var factions = await _factionRepository.GetFactionsByGameVersions(request.GameVersions, cancellationToken);

        var factionsDto = _mapper.Map<List<FactionDto>>(factions);

        return new ItemListDto<FactionDto>(factionsDto);
    }
}
