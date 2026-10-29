using System.Text.Json;
using TwilightImperiumUltimate.Contracts.DTOs.DraftRooms;
using TwilightImperiumUltimate.Contracts.Enums;

namespace TwilightImperiumUltimate.Draft.DraftRooms.Engine;

/// <summary>
/// Defines pure draft rules. The application loads input state and persists transitions; engines do no I/O.
/// </summary>
/// <typeparam name="TSettings">The type-specific settings contract.</typeparam>
/// <typeparam name="TState">The plain server-only state, including any preloaded pool.</typeparam>
public interface IDraftType<TSettings, in TState>
    where TSettings : DraftSettingsDto
    where TState : IDraftState
{
    /// <summary>
    /// Gets the engine identifier.
    /// </summary>
    DraftTypeId Id { get; }

    /// <summary>
    /// Gets the minimum supported number of seats.
    /// </summary>
    int MinSeats { get; }

    /// <summary>
    /// Gets the maximum supported number of seats.
    /// </summary>
    int MaxSeats { get; }

    /// <summary>
    /// Creates independent default settings.
    /// </summary>
    TSettings CreateDefaultSettings();

    /// <summary>
    /// Validates settings against seat count and the size of the pool loaded by the application.
    /// </summary>
    DraftValidationResult ValidateSettings(TSettings settings, int seatCount, int poolSize);

    /// <summary>
    /// Starts a draft from preloaded state, returning the actual random decisions for logging.
    /// </summary>
    DraftTransition Start(TSettings settings, TState state, IRandomnessSource randomness);

    /// <summary>
    /// Applies an action without mutating the input state.
    /// </summary>
    DraftTransition Apply(TSettings settings, TState state, DraftActor actor, DraftActionDto action);

    /// <summary>
    /// Discloses a target seat's pending choice before applying a manager's replacement choice.
    /// </summary>
    DraftTransition DecideFor(
        TSettings settings,
        TState state,
        DraftActor actor,
        Guid seatId,
        DraftActionDto action);

    /// <summary>
    /// Undoes the last eligible public decision without mutating the input state.
    /// </summary>
    DraftTransition Undo(TSettings settings, TState state, DraftActor actor);

    /// <summary>
    /// Produces the only state representation permitted to leave the server for this viewer.
    /// </summary>
    DraftViewDto Project(TSettings settings, TState state, DraftViewer viewer);

    /// <summary>
    /// Builds an explicitly public result payload for durable snapshot storage.
    /// </summary>
    JsonElement BuildResult(TSettings settings, TState state);

    /// <summary>
    /// Formats a public result payload for copying as text.
    /// </summary>
    string FormatResultAsText(JsonElement result);
}
