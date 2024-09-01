using TwilightImperiumUltimate.Contracts.ApiContracts;

namespace TwilightImperiumUltimate.Business.Logic.MapArchive;

public class AddOrUpdateUserMapRatingCommand(
    string userId,
    int mapId,
    float rating)
    : IRequest<ApiResponse<MapRatingDto>>
{
    public string UserId { get; set; } = userId;

    public int MapId { get; set; } = mapId;

    public float Rating { get; set; } = rating;
}