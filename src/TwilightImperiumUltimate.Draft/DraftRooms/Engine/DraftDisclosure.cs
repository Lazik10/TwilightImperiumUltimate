using System.Text.Json;

namespace TwilightImperiumUltimate.Draft.DraftRooms.Engine;

/// <summary>
/// Records previously private values intentionally made public by a decide-for action.
/// </summary>
/// <param name="SeatId">The affected seat.</param>
/// <param name="Data">The values disclosed to every participant before the replacement decision.</param>
public sealed record DraftDisclosure(Guid SeatId, JsonElement Data);
