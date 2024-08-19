namespace TwilightImperiumUltimate.Business.Logic.Drafts.Faction;

public class DraftFactionsCommandHandler(
    IFactionDraftService factionDraftService,
    IMapper mapper)
    : IRequestHandler<DraftFactionsCommand, FactionDraftResultDto>
{
    private readonly IFactionDraftService _factionDraftService = factionDraftService;
    private readonly IMapper _mapper = mapper;

    public async Task<FactionDraftResultDto> Handle(DraftFactionsCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var factionDraftResult = await _factionDraftService.DraftFactions(request.FactionDraftRequest, cancellationToken);

        var factionDraftResultDto = _mapper.Map<FactionDraftResultDto>(factionDraftResult);

        return factionDraftResultDto;
    }
}
