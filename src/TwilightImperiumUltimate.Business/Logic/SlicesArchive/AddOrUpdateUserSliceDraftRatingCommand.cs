using TwilightImperiumUltimate.Contracts.ApiContracts;

namespace TwilightImperiumUltimate.Business.Logic.SlicesArchive;

public class AddOrUpdateUserSliceDraftRatingCommand(
    string userId,
    int sliceDraftId,
    float rating)
    : IRequest<ApiResponse<SliceDraftRatingDto>>
{
    public string UserId { get; set; } = userId;

    public int SliceDraftId { get; set; } = sliceDraftId;

    public float Rating { get; set; } = rating;
}