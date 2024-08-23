using TwilightImperiumUltimate.Contracts.DTOs.SliceGeneration;
using TwilightImperiumUltimate.Draft.Drafts.SliceDraft;

namespace TwilightImperiumUltimate.Business.Logic.Drafts.Slices;

internal class GenerateSlicesCommandHandler(
    IDraftSlicesService draftSlicesService,
    IMapper mapper)
    : IRequestHandler<GenerateSlicesCommand, ItemListDto<SliceDto>>
{
    private readonly IDraftSlicesService _draftSlicesService = draftSlicesService;
    private readonly IMapper _mapper = mapper;

    public async Task<ItemListDto<SliceDto>> Handle(GenerateSlicesCommand request, CancellationToken cancellationToken)
    {
        var slices = await _draftSlicesService.DraftSlices(request.SliceDraftRequest, cancellationToken);

        var newSlices = _mapper.Map<List<SliceDto>>(slices);

        return new ItemListDto<SliceDto>(newSlices);
    }
}
