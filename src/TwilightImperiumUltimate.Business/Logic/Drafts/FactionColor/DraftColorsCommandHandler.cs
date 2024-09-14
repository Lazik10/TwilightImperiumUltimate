namespace TwilightImperiumUltimate.Business.Logic.Drafts.FactionColor;

public class DraftColorsCommandHandler(
    IColorDraftService colorDraftService,
    IGameStatisticsRepository gameStatisticsRepository,
    IMapper mapper)
    : IRequestHandler<DraftColorsCommand, FactionColorDraftResultDto>
{
    private readonly IColorDraftService _colorDraftService = colorDraftService;
    private readonly IGameStatisticsRepository _gameStatisticsRepository = gameStatisticsRepository;
    private readonly IMapper _mapper = mapper;

    public async Task<FactionColorDraftResultDto> Handle(DraftColorsCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        await _gameStatisticsRepository.UpdateWebsiteStatistics(StatisticsType.ColorsDrafted, cancellationToken);

        var results = await _colorDraftService.DraftColors(request.ColorDraftRequest, cancellationToken);

        var factionColorDraftResultsDto = _mapper.Map<FactionColorDraftResultDto>(results);

        return factionColorDraftResultsDto;
    }
}
