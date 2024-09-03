using TwilightImperiumUltimate.Contracts.ApiContracts;

namespace TwilightImperiumUltimate.Business.Logic.SlicesArchive;

public class GetUserSliceDraftRatingQueryHandler(
    ISlicesArchiveRepository slicesArchiveRepository,
    IMapper mapper)
    : IRequestHandler<GetUserSliceDraftRatingQuery, ApiResponse<SliceDraftRatingDto>>
{
    private readonly ISlicesArchiveRepository _slicesArchiveRepository = slicesArchiveRepository;
    private readonly IMapper _mapper = mapper;

    public async Task<ApiResponse<SliceDraftRatingDto>> Handle(GetUserSliceDraftRatingQuery request, CancellationToken cancellationToken)
    {
        var userRating = await _slicesArchiveRepository.GetSliceDraftRatingFromUser(request.UserId, request.SliceDraftId, cancellationToken);

        if (userRating is null)
        {
            return new ApiResponse<SliceDraftRatingDto>() { Success = false, Data = null, ProblemDetails = new ProblemDetailsDto() { Title = "Not Found", Detail = "This user have't rate this slice draft yet" } };
        }

        var sliceDraftRatingDto = _mapper.Map<SliceDraftRatingDto>(userRating);

        return new ApiResponse<SliceDraftRatingDto>() { Success = true, Data = sliceDraftRatingDto };
    }
}
