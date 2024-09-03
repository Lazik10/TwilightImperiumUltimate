namespace TwilightImperiumUltimate.Business.Logic.SlicesArchive;

public class GetAllSliceDraftsQueryHandler(
    ISlicesArchiveRepository slicesArchiveRepository,
    IMapper mapper)
    : IRequestHandler<GetAllSliceDraftsQuery, ItemListDto<SliceDraftDto>>
{
    private readonly ISlicesArchiveRepository _slicesArchiveRepository = slicesArchiveRepository;
    private readonly IMapper _mapper = mapper;

    public async Task<ItemListDto<SliceDraftDto>> Handle(GetAllSliceDraftsQuery request, CancellationToken cancellationToken)
    {
        var sliceDrafts = await _slicesArchiveRepository.GetAllSliceDrafts(cancellationToken);
        var sliceDraftList = sliceDrafts.ToList();
        var sliceDraftDto = _mapper.Map<List<SliceDraftDto>>(sliceDraftList);
        return new ItemListDto<SliceDraftDto>(sliceDraftDto);
    }
}
