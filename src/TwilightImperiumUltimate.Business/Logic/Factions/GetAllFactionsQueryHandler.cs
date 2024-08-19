namespace TwilightImperiumUltimate.Business.Logic.Factions;

public class GetAllFactionsQueryHandler(
    IFactionRepository factionRepository,
    IMapper mapper)
    : IRequestHandler<GetAllFactionsQuery, ItemListDto<FactionDto>>
{
    private readonly IFactionRepository _factionRepository = factionRepository;
    private readonly IMapper _mapper = mapper;

    public async Task<ItemListDto<FactionDto>> Handle(GetAllFactionsQuery request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var factions = await _factionRepository.GetAllFactions(cancellationToken);

        var factionsDto = _mapper.Map<List<FactionDto>>(factions);

        return new ItemListDto<FactionDto>(factionsDto);
    }
}
