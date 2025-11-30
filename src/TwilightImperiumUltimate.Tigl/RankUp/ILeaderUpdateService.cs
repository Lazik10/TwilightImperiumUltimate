using TwilightImperiumUltimate.Contracts.Enums;
using TwilightImperiumUltimate.Core.Entities.Tigl;

namespace TwilightImperiumUltimate.Tigl.RankUp;

internal interface ILeaderUpdateService
{
    Task<bool> UpdateLeader(TiglUser player, MatchReport matchReport, TiglFactionName faction, CancellationToken cancellationToken);
}
