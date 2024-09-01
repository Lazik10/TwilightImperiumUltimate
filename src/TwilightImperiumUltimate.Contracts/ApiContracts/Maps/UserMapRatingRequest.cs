namespace TwilightImperiumUltimate.Contracts.ApiContracts.Maps;

public class UserMapRatingRequest
{
    public string UserId { get; set; } = string.Empty;

    public int MapId { get; set; }

    public float Rating { get; set; }
}
