using TwilightImperiumUltimate.Contracts.ApiContracts;

namespace TwilightImperiumUltimate.Business.Logic.MapArchive;

public class GetUserMapRatingQueryHandler(
    IMapArchiveRepository mapArchiveRepository,
    IMapper mapper)
    : IRequestHandler<GetUserMapRatingQuery, ApiResponse<MapRatingDto>>
{
    private readonly IMapArchiveRepository _mapArchiveRepository = mapArchiveRepository;
    private readonly IMapper _mapper = mapper;

    public async Task<ApiResponse<MapRatingDto>> Handle(GetUserMapRatingQuery request, CancellationToken cancellationToken)
    {
        var userRating = await _mapArchiveRepository.GetMapRatingFromUser(request.UserId, request.MapId, cancellationToken);

        if (userRating is null)
        {
            return new ApiResponse<MapRatingDto>() { Success = false, Data = null, ProblemDetails = new ProblemDetailsDto() { Title = "Not Found", Detail = "This user have't rate this map yet" } };
        }

        var mapRatingDto = _mapper.Map<MapRatingDto>(userRating);

        return new ApiResponse<MapRatingDto>() { Success = true, Data = mapRatingDto };
    }
}
