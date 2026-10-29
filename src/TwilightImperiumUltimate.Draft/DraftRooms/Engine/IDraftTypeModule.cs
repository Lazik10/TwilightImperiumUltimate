using System.Text.Json;
using TwilightImperiumUltimate.Contracts.DTOs.DraftRooms;
using TwilightImperiumUltimate.Contracts.Enums;

namespace TwilightImperiumUltimate.Draft.DraftRooms.Engine;

/// <summary>
/// Adapts a typed engine to a registry without exposing raw objects or adding persistence to engine rules.
/// </summary>
public interface IDraftTypeModule
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
    DraftSettingsDto CreateDefaultSettings();

    /// <summary>
    /// Validates settings against the loaded pool and seat count.
    /// </summary>
    DraftValidationResult ValidateSettings(DraftSettingsDto settings, int seatCount, int poolSize);

    /// <summary>
    /// Starts the draft using preloaded server-only state.
    /// </summary>
    DraftTransition Start(DraftSettingsDto settings, IDraftState state, IRandomnessSource randomness);

    /// <summary>
    /// Applies a verified actor's action.
    /// </summary>
    DraftTransition Apply(DraftSettingsDto settings, IDraftState state, DraftActor actor, DraftActionDto action);

    /// <summary>
    /// Applies a manager's disclosed decision for a target seat.
    /// </summary>
    DraftTransition DecideFor(
        DraftSettingsDto settings,
        IDraftState state,
        DraftActor actor,
        Guid seatId,
        DraftActionDto action);

    /// <summary>
    /// Undoes the last eligible decision.
    /// </summary>
    DraftTransition Undo(DraftSettingsDto settings, IDraftState state, DraftActor actor);

    /// <summary>
    /// Produces the viewer-specific projection.
    /// </summary>
    DraftViewDto Project(DraftSettingsDto settings, IDraftState state, DraftViewer viewer);

    /// <summary>
    /// Builds the public result payload.
    /// </summary>
    JsonElement BuildResult(DraftSettingsDto settings, IDraftState state);

    /// <summary>
    /// Formats the public result payload as text.
    /// </summary>
    string FormatResultAsText(JsonElement result);
}
