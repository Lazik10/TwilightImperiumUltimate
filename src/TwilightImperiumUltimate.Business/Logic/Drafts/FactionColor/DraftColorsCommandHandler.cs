namespace TwilightImperiumUltimate.Business.Logic.Drafts.FactionColor;

public class DraftColorsCommandHandler(
    IColorDraftService colorDraftService,
    IMapper mapper)
    : IRequestHandler<DraftColorsCommand, FactionColorDraftResultDto>
{
    private readonly IColorDraftService _colorDraftService = colorDraftService;
    private readonly IMapper _mapper = mapper;

    public async Task<FactionColorDraftResultDto> Handle(DraftColorsCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var results = await _colorDraftService.DraftColors(request.ColorDraftRequest, cancellationToken);

        var factionColorDraftResultsDto = _mapper.Map<FactionColorDraftResultDto>(results);

        return factionColorDraftResultsDto;
    }
}
