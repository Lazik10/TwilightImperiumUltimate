using TwilightImperiumUltimate.Draft.DraftRooms.Engine;

namespace TwilightImperiumUltimate.Tests.DraftRooms.Engine;

internal sealed record FakeDraftState(Guid OwnerSeatId, string PrivateValue) : IDraftState
{
    public DraftPhaseKind PhaseKind => DraftPhaseKind.Simultaneous;

    public bool Started { get; init; }

    public int ActionCount { get; init; }
}
