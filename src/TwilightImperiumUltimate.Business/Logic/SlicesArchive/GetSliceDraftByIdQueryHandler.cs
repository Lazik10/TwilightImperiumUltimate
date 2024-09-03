using TwilightImperiumUltimate.Contracts.ApiContracts;

namespace TwilightImperiumUltimate.Business.Logic.SlicesArchive;

public class GetSliceDraftByIdQueryHandler(
    ISlicesArchiveRepository slicesArchiveRepository,
    IMapper mapper)
    : IRequestHandler<GetSliceDraftByIdQuery, ApiResponse<SliceDraftDto>>
{
    private readonly ISlicesArchiveRepository _slicesArchiveRepository = slicesArchiveRepository;
    private readonly IMapper _mapper = mapper;

    public async Task<ApiResponse<SliceDraftDto>> Handle(GetSliceDraftByIdQuery request, CancellationToken cancellationToken)
    {
        var map = await _slicesArchiveRepository.GetSliceDraftById(request.Id, cancellationToken);
        if (map == null)
        {
            return new ApiResponse<SliceDraftDto>() { Success = false, ProblemDetails = new ProblemDetailsDto() { Title = "Slice draft not found" } };
        }

        var sliceDraftDto = _mapper.Map<SliceDraftDto>(map);

        return new ApiResponse<SliceDraftDto>() { Success = true, Data = sliceDraftDto };
    }
}
