using TwilightImperiumUltimate.Draft.Drafts.MapDraft.Interfaces;

namespace TwilightImperiumUltimate.Business.Logic.Drafts.Map;

public class GenerateMapCommandHandler(
    IGenerateMapService factionDraftService,
    IMapper mapper)
    : IRequestHandler<GenerateMapCommand, GeneratedMapLayoutDto>
{
    private readonly IGenerateMapService _mapDraftService = factionDraftService;
    private readonly IMapper _mapper = mapper;

    public async Task<GeneratedMapLayoutDto> Handle(GenerateMapCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var generatedMapLayout = await _mapDraftService.GenerateMapLayout(request.DraftRequest, cancellationToken);

        var generatedMapLayoutDto = _mapper.Map<GeneratedMapLayoutDto>(generatedMapLayout);

        return generatedMapLayoutDto;
    }
}
