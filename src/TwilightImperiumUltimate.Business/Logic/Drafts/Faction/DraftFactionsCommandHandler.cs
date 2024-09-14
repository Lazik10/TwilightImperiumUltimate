namespace TwilightImperiumUltimate.Business.Logic.Drafts.Faction;

public class DraftFactionsCommandHandler(
    IFactionDraftService factionDraftService,
    IGameStatisticsRepository gameStatisticsRepository,
    IMapper mapper)
    : IRequestHandler<DraftFactionsCommand, FactionDraftResultDto>
{
    private readonly IFactionDraftService _factionDraftService = factionDraftService;
    private readonly IGameStatisticsRepository _gameStatisticsRepository = gameStatisticsRepository;
    private readonly IMapper _mapper = mapper;

    public async Task<FactionDraftResultDto> Handle(DraftFactionsCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        await _gameStatisticsRepository.UpdateWebsiteStatistics(StatisticsType.FactionsDrafted, cancellationToken);

        var factionDraftResult = await _factionDraftService.DraftFactions(request.FactionDraftRequest, cancellationToken);

        var factionDraftResultDto = _mapper.Map<FactionDraftResultDto>(factionDraftResult);

        return factionDraftResultDto;
    }
}
