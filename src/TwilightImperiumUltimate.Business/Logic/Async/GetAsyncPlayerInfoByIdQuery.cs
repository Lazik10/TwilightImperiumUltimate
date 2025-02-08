using TwilightImperiumUltimate.Contracts.DTOs.Async.Responses;

namespace TwilightImperiumUltimate.Business.Logic.Async;

public class GetAsyncPlayerInfoByIdQuery(long asyncGameId) : IRequest<AsyncPlayerProfileSummaryStatsDto>
{
    public long DiscordUserId { get; set; } = asyncGameId;
}
