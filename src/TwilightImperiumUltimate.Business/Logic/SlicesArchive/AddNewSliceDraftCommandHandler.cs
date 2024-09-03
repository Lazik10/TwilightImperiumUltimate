namespace TwilightImperiumUltimate.Business.Logic.SlicesArchive;

public class AddNewSliceDraftCommandHandler(
    ISlicesArchiveRepository slicesArchiveRepository,
    IMapper mapper)
    : IRequestHandler<AddNewSliceDraftCommand, bool>
{
    private readonly ISlicesArchiveRepository _slicesArchiveRepository = slicesArchiveRepository;
    private readonly IMapper _mapper = mapper;

    public async Task<bool> Handle(AddNewSliceDraftCommand request, CancellationToken cancellationToken)
    {
        var sliceDraft = _mapper.Map<SliceDraft>(request.SliceDraft);
        return await _slicesArchiveRepository.AddNewSliceDraft(sliceDraft, cancellationToken);
    }
}
