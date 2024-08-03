using TwilightImperiumUltimate.Draft.Drafts.MapDraft.Interfaces;
using System.Linq;

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

        var newMapLayoutDto = new List<HexDto>();

        newMapLayoutDto.AddRange(from kvp in generatedMapLayout.MapLayout
                                 let hexDto = new HexDto() { Name = kvp.Value.Name, X = kvp.Value.X, Y = kvp.Value.Y, SystemTile = _mapper.Map<SystemTileDto>(kvp.Value.SystemTile) }
                                 select hexDto);

        return new GeneratedMapLayoutDto(newMapLayoutDto);
    }
}
