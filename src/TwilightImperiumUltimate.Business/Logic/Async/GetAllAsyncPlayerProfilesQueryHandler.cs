using TwilightImperiumUltimate.Contracts.DTOs.Async;
using TwilightImperiumUltimate.Contracts.DTOs.Async.Responses;

namespace TwilightImperiumUltimate.Business.Logic.Async;

public class GetAllAsyncPlayerProfilesQueryHandler(
    IAsyncStatsRepository asyncPlayerProfileRepository,
    IMapper mapper)
    : IRequestHandler<GetAllAsyncPlayerProfilesQuery, AsyncPlayerProfileListDto>
{
    private readonly IAsyncStatsRepository _asyncPlayerProfileRepository = asyncPlayerProfileRepository;
    private readonly IMapper _mapper = mapper;

    public async Task<AsyncPlayerProfileListDto> Handle(GetAllAsyncPlayerProfilesQuery request, CancellationToken cancellationToken)
    {
        var playerProfiles = await _asyncPlayerProfileRepository.GetAllAsyncPlayerProfiles(false, cancellationToken);
        var playerProfilesDto = _mapper.Map<IReadOnlyCollection<AsyncPlayerProfileDto>>(playerProfiles);
        return new AsyncPlayerProfileListDto(playerProfilesDto);
    }
}
