namespace TwilightImperiumUltimate.Draft.DraftRooms.Engine;

/// <summary>
/// Base for plain server-only engine state. State is never returned directly from an endpoint.
/// </summary>
public interface IDraftState
{
    /// <summary>
    /// Gets the interaction model of the current phase.
    /// </summary>
    DraftPhaseKind PhaseKind { get; }
}
