using TwilightImperiumUltimate.Contracts.ApiContracts;

namespace TwilightImperiumUltimate.Business.Logic.SlicesArchive;

public class GetUserSliceDraftRatingQuery(
    string userId,
    int sliceDraftId)
    : IRequest<ApiResponse<SliceDraftRatingDto>>
{
    public string UserId { get; set; } = userId;

    public int SliceDraftId { get; set; } = sliceDraftId;
}
