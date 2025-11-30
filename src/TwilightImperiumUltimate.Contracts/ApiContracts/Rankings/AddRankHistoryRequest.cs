using TwilightImperiumUltimate.Contracts.Enums;

namespace TwilightImperiumUltimate.Contracts.ApiContracts.Rankings;

public class AddRankHistoryRequest
{
    public int TiglUserId { get; set; }

    public TiglLeague League { get; set; }

    public TiglRankName Rank { get; set; }

    public long AchievedAt { get; set; }
}
