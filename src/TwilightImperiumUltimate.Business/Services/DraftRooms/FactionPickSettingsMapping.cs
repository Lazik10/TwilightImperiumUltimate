using TwilightImperiumUltimate.Contracts.DTOs.DraftRooms;
using TwilightImperiumUltimate.Core.Entities.DraftRooms;

namespace TwilightImperiumUltimate.Business.Services.DraftRooms;

internal static class FactionPickSettingsMapping
{
    internal static FactionPickSettingsDto ToDto(FactionPickSettings settings) => new()
    {
        GameVersions = settings.GameVersions.ToArray(),
        BansMode = settings.BansMode,
        BansPerPlayer = settings.BansPerPlayer,
        HostBanCount = settings.HostBanCount,
        PickOrderMode = settings.PickOrderMode,
        SeatOrder = settings.SeatOrder.ToArray(),
        PicksPerPlayer = settings.PicksPerPlayer,
        Snake = settings.Snake,
    };
}
