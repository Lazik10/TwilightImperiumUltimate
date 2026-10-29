using System.Text.Json;
using TwilightImperiumUltimate.Contracts.DTOs.DraftRooms;
using TwilightImperiumUltimate.Contracts.Enums;

namespace TwilightImperiumUltimate.Draft.DraftRooms.Engine;

/// <summary>
/// Provides checked dispatch from registry contracts to a strongly typed pure engine.
/// </summary>
/// <typeparam name="TSettings">The engine's settings.</typeparam>
/// <typeparam name="TState">The engine's server-only state.</typeparam>
public sealed class DraftTypeModule<TSettings, TState>(IDraftType<TSettings, TState> engine) : IDraftTypeModule
    where TSettings : DraftSettingsDto
    where TState : IDraftState
{
    private readonly IDraftType<TSettings, TState> _engine = engine
        ?? throw new ArgumentNullException(nameof(engine));

    /// <inheritdoc />
    public DraftTypeId Id => _engine.Id;

    /// <inheritdoc />
    public int MinSeats => _engine.MinSeats;

    /// <inheritdoc />
    public int MaxSeats => _engine.MaxSeats;

    /// <inheritdoc />
    public DraftSettingsDto CreateDefaultSettings() => _engine.CreateDefaultSettings();

    /// <inheritdoc />
    public DraftValidationResult ValidateSettings(DraftSettingsDto settings, int seatCount, int poolSize) =>
        _engine.ValidateSettings(RequireSettings(settings), seatCount, poolSize);

    /// <inheritdoc />
    public DraftTransition Start(DraftSettingsDto settings, IDraftState state, IRandomnessSource randomness) =>
        _engine.Start(RequireSettings(settings), RequireState(state), randomness);

    /// <inheritdoc />
    public DraftTransition Apply(DraftSettingsDto settings, IDraftState state, DraftActor actor, DraftActionDto action) =>
        _engine.Apply(RequireSettings(settings), RequireState(state), actor, action);

    /// <inheritdoc />
    public DraftTransition DecideFor(
        DraftSettingsDto settings,
        IDraftState state,
        DraftActor actor,
        Guid seatId,
        DraftActionDto action) =>
        _engine.DecideFor(RequireSettings(settings), RequireState(state), actor, seatId, action);

    /// <inheritdoc />
    public DraftTransition Undo(DraftSettingsDto settings, IDraftState state, DraftActor actor) =>
        _engine.Undo(RequireSettings(settings), RequireState(state), actor);

    /// <inheritdoc />
    public DraftViewDto Project(DraftSettingsDto settings, IDraftState state, DraftViewer viewer) =>
        _engine.Project(RequireSettings(settings), RequireState(state), viewer);

    /// <inheritdoc />
    public JsonElement BuildResult(DraftSettingsDto settings, IDraftState state) =>
        _engine.BuildResult(RequireSettings(settings), RequireState(state));

    /// <inheritdoc />
    public string FormatResultAsText(JsonElement result) => _engine.FormatResultAsText(result);

    private static TSettings RequireSettings(DraftSettingsDto settings)
    {
        ArgumentNullException.ThrowIfNull(settings);

        return settings is TSettings typed
            ? typed
            : throw new ArgumentException("The settings do not match the selected draft engine.", nameof(settings));
    }

    private static TState RequireState(IDraftState state)
    {
        ArgumentNullException.ThrowIfNull(state);

        return state is TState typed
            ? typed
            : throw new ArgumentException("The state does not match the selected draft engine.", nameof(state));
    }
}
