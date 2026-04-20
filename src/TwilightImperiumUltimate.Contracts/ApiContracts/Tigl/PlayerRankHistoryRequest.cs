using System.Collections.ObjectModel;

namespace TwilightImperiumUltimate.Contracts.ApiContracts.Tigl;

public class PlayerRankHistoryRequest
{
    public Collection<long> DiscordUserIds { get; init; } = new();
}
