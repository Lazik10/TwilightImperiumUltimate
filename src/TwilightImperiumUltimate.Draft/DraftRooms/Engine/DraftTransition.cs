namespace TwilightImperiumUltimate.Draft.DraftRooms.Engine;

/// <summary>
/// Returns a new state plus explicit public effects. The application persists successful transitions atomically.
/// On refusal the input state is preserved and no effects may be published.
/// </summary>
/// <param name="State">The resulting server-only state.</param>
/// <param name="Validation">The rule outcome.</param>
/// <param name="LogEvents">Public events in application order.</param>
/// <param name="Disclosures">Intentional disclosures that must be logged before other events.</param>
public sealed record DraftTransition(
    IDraftState State,
    DraftValidationResult Validation,
    IReadOnlyList<DraftLogEvent> LogEvents,
    IReadOnlyList<DraftDisclosure> Disclosures)
{
    /// <summary>Gets whether this successful transition finishes the room draft.</summary>
    public bool IsFinished { get; init; }
}
