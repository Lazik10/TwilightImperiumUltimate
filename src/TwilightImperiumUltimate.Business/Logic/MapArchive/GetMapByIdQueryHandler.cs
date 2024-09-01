using TwilightImperiumUltimate.Contracts.ApiContracts;

namespace TwilightImperiumUltimate.Business.Logic.MapArchive;

public class GetMapByIdQueryHandler(
    IMapArchiveRepository mapArchiveRepository,
    IMapper mapper)
    : IRequestHandler<GetMapByIdQuery, ApiResponse<MapDto>>
{
    private readonly IMapArchiveRepository _mapArchiveRepository = mapArchiveRepository;
    private readonly IMapper _mapper = mapper;

    public async Task<ApiResponse<MapDto>> Handle(GetMapByIdQuery request, CancellationToken cancellationToken)
    {
        var map = await _mapArchiveRepository.GetMapById(request.Id, cancellationToken);
        if (map == null)
        {
            return new ApiResponse<MapDto>() { Success = false, ProblemDetails = new ProblemDetailsDto() { Title = "Map not found" } };
        }

        var mapDto = _mapper.Map<MapDto>(map);

        return new ApiResponse<MapDto>() { Success = true, Data = mapDto };
    }
}
