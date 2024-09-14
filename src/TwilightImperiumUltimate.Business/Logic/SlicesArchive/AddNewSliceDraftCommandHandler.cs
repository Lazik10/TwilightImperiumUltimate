namespace TwilightImperiumUltimate.Business.Logic.SlicesArchive;

public class AddNewSliceDraftCommandHandler(
    ISlicesArchiveRepository slicesArchiveRepository,
    IGameStatisticsRepository gameStatisticsRepository,
    IMapper mapper)
    : IRequestHandler<AddNewSliceDraftCommand, bool>
{
    private readonly ISlicesArchiveRepository _slicesArchiveRepository = slicesArchiveRepository;
    private readonly IGameStatisticsRepository _gameStatisticsRepository = gameStatisticsRepository;
    private readonly IMapper _mapper = mapper;

    public async Task<bool> Handle(AddNewSliceDraftCommand request, CancellationToken cancellationToken)
    {
        await _gameStatisticsRepository.UpdateWebsiteStatistics(StatisticsType.SlicesArchived, cancellationToken);
        var sliceDraft = _mapper.Map<SliceDraft>(request.SliceDraft);
        return await _slicesArchiveRepository.AddNewSliceDraft(sliceDraft, cancellationToken);
    }
}
