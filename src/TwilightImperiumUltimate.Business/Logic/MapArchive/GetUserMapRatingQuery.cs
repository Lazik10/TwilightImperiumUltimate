using TwilightImperiumUltimate.Contracts.ApiContracts;

namespace TwilightImperiumUltimate.Business.Logic.MapArchive;

public class GetUserMapRatingQuery(
    string userId,
    int mapId)
    : IRequest<ApiResponse<MapRatingDto>>
{
    public string UserId { get; set; } = userId;

    public int MapId { get; set; } = mapId;
}
