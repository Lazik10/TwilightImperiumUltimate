using TwilightImperiumUltimate.Contracts.ApiContracts.AsyncTI4;
using TwilightImperiumUltimate.Contracts.DTOs.Async.Responses;

namespace TwilightImperiumUltimate.Business.Logic.Async;

public class GetAsyncPlayerInfoByPlayerProfileQuery(
    AsyncPlayerProfileRequest request)
    : IRequest<AsyncPlayerProfileSummaryStatsDto>
{
    public long DiscordId { get; set; } = request.DiscordId;

    public string Name { get; set; } = request.Name;

    public int PlayerId { get; set; } = request.PlayerId;
}
