using TwilightImperiumUltimate.Contracts.ApiContracts;

namespace TwilightImperiumUltimate.Business.Logic.SlicesArchive;

public class AddOrUpdateUserSliceDraftRatingCommandHandler(
    ISlicesArchiveRepository slicesArchiveRepository,
    IMapper mapper)
    : IRequestHandler<AddOrUpdateUserSliceDraftRatingCommand, ApiResponse<SliceDraftRatingDto>>
{
    private readonly ISlicesArchiveRepository _slicesArchiveRepository = slicesArchiveRepository;
    private readonly IMapper _mapper = mapper;

    public async Task<ApiResponse<SliceDraftRatingDto>> Handle(AddOrUpdateUserSliceDraftRatingCommand request, CancellationToken cancellationToken)
    {
        var userRating = await _slicesArchiveRepository.AddOrUpdateUserSliceDraftRating(request.UserId, request.SliceDraftId, request.Rating, cancellationToken);

        var sliceDraftRatingDto = _mapper.Map<SliceDraftRatingDto>(userRating);

        return new ApiResponse<SliceDraftRatingDto>() { Success = true, Data = sliceDraftRatingDto };
    }
}
