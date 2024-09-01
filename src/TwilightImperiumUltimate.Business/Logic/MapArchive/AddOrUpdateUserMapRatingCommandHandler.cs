using TwilightImperiumUltimate.Contracts.ApiContracts;

namespace TwilightImperiumUltimate.Business.Logic.MapArchive;

public class AddOrUpdateUserMapRatingCommandHandler(
    IMapArchiveRepository mapArchiveRepository,
    IMapper mapper)
    : IRequestHandler<AddOrUpdateUserMapRatingCommand, ApiResponse<MapRatingDto>>
{
    private readonly IMapArchiveRepository _mapArchiveRepository = mapArchiveRepository;
    private readonly IMapper _mapper = mapper;

    public async Task<ApiResponse<MapRatingDto>> Handle(AddOrUpdateUserMapRatingCommand request, CancellationToken cancellationToken)
    {
        var userRating = await _mapArchiveRepository.AddOrUpdateUserMapRating(request.UserId, request.MapId, request.Rating, cancellationToken);

        var mapRatingDto = _mapper.Map<MapRatingDto>(userRating);

        return new ApiResponse<MapRatingDto>() { Success = true, Data = mapRatingDto };
    }
}
