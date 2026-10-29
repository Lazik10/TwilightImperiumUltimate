using TwilightImperiumUltimate.Contracts.Enums;
using TwilightImperiumUltimate.Draft.DraftRooms.Engine;

namespace TwilightImperiumUltimate.Draft.DraftRooms.FactionPick;

/// <summary>
/// Contains preloaded, server-only Faction Pick inputs and the current runtime phase.
/// </summary>
public sealed record FactionPickDraftState : IDraftState
{
    public IReadOnlyList<Guid> SeatIds { get; init; } = [];

    public IReadOnlyList<FactionName> Pool { get; init; } = [];

    public IReadOnlyList<Guid> TurnOrder { get; init; } = [];

    public bool IsStarted { get; init; }

    public FactionPickPhase Phase { get; init; }

    public FactionPickBansMode BansMode { get; init; }

    public int TurnIndex { get; init; }

    public bool BansRevealed { get; init; }

    public IReadOnlyList<Guid> LockedBanSeatIds { get; init; } = [];

    public IReadOnlyList<FactionPickChoice> Choices { get; init; } = [];

    public IReadOnlyList<FactionPickResultSeat> ResultSeats { get; init; } = [];

    public IReadOnlyDictionary<Guid, string> ParticipantNames { get; init; } = new Dictionary<Guid, string>();

    /// <inheritdoc />
    public DraftPhaseKind PhaseKind => (IsStarted, Phase, BansMode) switch
    {
        (false, _, _) or (_, FactionPickPhase.Finished, _) => DraftPhaseKind.Automatic,
        (_, FactionPickPhase.Picks, _) => DraftPhaseKind.Turn,
        (_, _, FactionPickBansMode.HostBans) => DraftPhaseKind.HostOnly,
        _ => DraftPhaseKind.Simultaneous,
    };
}
