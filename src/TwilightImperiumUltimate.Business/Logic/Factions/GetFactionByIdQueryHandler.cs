namespace TwilightImperiumUltimate.Business.Logic.Factions;

public class GetFactionByIdQueryHandler(
    IFactionRepository factionRepository,
    IMapper mapper)
    : IRequestHandler<GetFactionByIdQuery, FactionDto>
{
    private readonly IFactionRepository _factionRepository = factionRepository;
    private readonly IMapper _mapper = mapper;

    public async Task<FactionDto> Handle(GetFactionByIdQuery request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var faction = await _factionRepository.GetFactionById(request.Id, cancellationToken);

        var factionDto = _mapper.Map<FactionDto>(faction);

        return factionDto;
    }
}
