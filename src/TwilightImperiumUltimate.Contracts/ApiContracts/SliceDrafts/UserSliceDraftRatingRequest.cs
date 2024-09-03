namespace TwilightImperiumUltimate.Contracts.ApiContracts.SliceDrafts;

public class UserSliceDraftRatingRequest
{
    public string UserId { get; set; } = string.Empty;

    public int SliceDraftId { get; set; }

    public float Rating { get; set; }
}
