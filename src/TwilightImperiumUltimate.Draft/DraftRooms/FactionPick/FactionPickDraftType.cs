using System.Text.Json;
using TwilightImperiumUltimate.Contracts.DTOs.DraftRooms;
using TwilightImperiumUltimate.Contracts.Enums;
using TwilightImperiumUltimate.Draft.DraftRooms.Engine;

namespace TwilightImperiumUltimate.Draft.DraftRooms.FactionPick;

/// <summary>
/// Validates Faction Pick rules and initializes its phase and auditable seat order without I/O.
/// </summary>
public sealed partial class FactionPickDraftType : IDraftType<FactionPickSettingsDto, FactionPickDraftState>
{
    /// <inheritdoc />
    public DraftTypeId Id => DraftTypeId.FactionPick;

    /// <inheritdoc />
    public int MinSeats => 2;

    /// <inheritdoc />
    public int MaxSeats => 8;

    /// <summary>
    /// Verifies a host-defined order contains exactly the room's current seats.
    /// </summary>
    public static bool IsSeatOrderValid(FactionPickSettingsDto settings, IReadOnlyList<Guid> seats)
    {
        ArgumentNullException.ThrowIfNull(settings);
        ArgumentNullException.ThrowIfNull(seats);
        return HasValidOrderShape(settings, seats.Count)
            && (settings.PickOrderMode != FactionPickOrderMode.HostDefined || settings.SeatOrder.ToHashSet().SetEquals(seats));
    }

    /// <inheritdoc />
    public FactionPickSettingsDto CreateDefaultSettings() => new();

    /// <inheritdoc />
    public DraftValidationResult ValidateSettings(FactionPickSettingsDto settings, int seatCount, int poolSize)
    {
        ArgumentNullException.ThrowIfNull(settings);
        var errors = new List<string>();
        if (seatCount < this.MinSeats || seatCount > this.MaxSeats)
        {
            errors.Add("InvalidSeatCount");
        }

        if (settings.GameVersions is null || settings.GameVersions.Count == 0)
        {
            errors.Add("GameVersionsRequired");
        }
        else if (settings.GameVersions.Any(version =>
            version is not (GameVersion.BaseGame or GameVersion.ProphecyOfKings or GameVersion.ThundersEdge)))
        {
            errors.Add("UnsupportedGameVersion");
        }

        if (!Enum.IsDefined(settings.BansMode))
        {
            errors.Add("InvalidBansMode");
        }

        if (settings.BansPerPlayer is < 1 or > 3)
        {
            errors.Add("InvalidBansPerPlayer");
        }

        if (settings.HostBanCount < 0)
        {
            errors.Add("InvalidHostBanCount");
        }

        if (settings.PicksPerPlayer is < 1 or > 5)
        {
            errors.Add("InvalidPicksPerPlayer");
        }

        if (!Enum.IsDefined(settings.PickOrderMode))
        {
            errors.Add("InvalidPickOrderMode");
        }
        else if (!HasValidOrderShape(settings, seatCount))
        {
            errors.Add("InvalidSeatOrder");
        }

        var maximumBans = settings.BansMode switch
        {
            FactionPickBansMode.HostBans => (long)settings.HostBanCount,
            FactionPickBansMode.SimultaneousPlayerBans => (long)settings.BansPerPlayer * seatCount,
            _ => 0L,
        };
        if (poolSize < 0 || poolSize - maximumBans < (long)seatCount * settings.PicksPerPlayer)
        {
            errors.Add("InsufficientFactionPool");
        }

        return new DraftValidationResult(errors);
    }

    /// <inheritdoc />
    public DraftTransition Start(
        FactionPickSettingsDto settings,
        FactionPickDraftState state,
        IRandomnessSource randomness)
    {
        ArgumentNullException.ThrowIfNull(state);
        ArgumentNullException.ThrowIfNull(randomness);
        var validation = this.ValidateSettings(settings, state.SeatIds.Count, state.Pool.Distinct().Count());
        if (!validation.IsValid)
        {
            return new DraftTransition(state, validation, [], []);
        }

        if (state.IsStarted)
        {
            return Refuse(state, "DraftAlreadyStarted");
        }

        if (state.SeatIds.Contains(Guid.Empty) || state.SeatIds.Distinct().Count() != state.SeatIds.Count)
        {
            return Refuse(state, "InvalidSeats");
        }

        if (!IsSeatOrderValid(settings, state.SeatIds))
        {
            return Refuse(state, "InvalidSeatOrder");
        }

        var order = settings.PickOrderMode == FactionPickOrderMode.HostDefined ? settings.SeatOrder.ToList() : state.SeatIds.ToList();
        if (settings.PickOrderMode == FactionPickOrderMode.Random)
        {
            randomness.Shuffle(order);
        }

        var next = state with
        {
            IsStarted = true,
            BansMode = settings.BansMode,
            Phase = settings.BansMode == FactionPickBansMode.None ? FactionPickPhase.Picks : FactionPickPhase.Bans,
            TurnIndex = 0,
            BansRevealed = settings.BansMode == FactionPickBansMode.None,
            SeatIds = Array.AsReadOnly(state.SeatIds.ToArray()),
            Pool = Array.AsReadOnly(state.Pool.Distinct().ToArray()),
            TurnOrder = order.AsReadOnly(),
        };
        return new DraftTransition(
            next,
            DraftValidationResult.Valid,
            [new DraftLogEvent(settings.PickOrderMode == FactionPickOrderMode.Random ? "PickOrderRandomized" : "PickOrderConfigured", JsonSerializer.SerializeToElement(new { SeatIds = order }))],
            []);
    }

    /// <inheritdoc />
    public DraftTransition Apply(
        FactionPickSettingsDto settings,
        FactionPickDraftState state,
        DraftActor actor,
        DraftActionDto action)
    {
        ArgumentNullException.ThrowIfNull(settings);
        ArgumentNullException.ThrowIfNull(state);
        ArgumentNullException.ThrowIfNull(actor);
        return state.Phase == FactionPickPhase.Picks
            ? ApplyPicks(settings, state, actor, action)
            : ApplyBans(settings, state, actor, action);
    }

    /// <inheritdoc />
    public DraftTransition DecideFor(
        FactionPickSettingsDto settings,
        FactionPickDraftState state,
        DraftActor actor,
        Guid seatId,
        DraftActionDto action)
    {
        ArgumentNullException.ThrowIfNull(settings);
        ArgumentNullException.ThrowIfNull(state);
        ArgumentNullException.ThrowIfNull(actor);
        return state.Phase == FactionPickPhase.Picks
            ? DecidePick(settings, state, actor, seatId, action)
            : DecideBans(settings, state, actor, seatId, action);
    }

    /// <inheritdoc />
    public DraftTransition Undo(FactionPickSettingsDto settings, FactionPickDraftState state, DraftActor actor)
    {
        ArgumentNullException.ThrowIfNull(state);
        ArgumentNullException.ThrowIfNull(actor);
        return UndoPick(settings, state, actor);
    }

    /// <inheritdoc />
    public DraftViewDto Project(FactionPickSettingsDto settings, FactionPickDraftState state, DraftViewer viewer)
    {
        ArgumentNullException.ThrowIfNull(settings);
        ArgumentNullException.ThrowIfNull(state);
        ArgumentNullException.ThrowIfNull(viewer);
        return new FactionPickViewDto
        {
            Phase = state.Phase,
            TurnOrder = state.TurnOrder.ToArray(),
            Pool = state.Pool.ToArray(),
            PublicBans = state.BansRevealed || settings.BansMode == FactionPickBansMode.HostBans
                ? state.Choices.Where(choice => choice.Kind == FactionPickSelectionKind.Ban)
                    .Select(choice => choice.Faction).Distinct().Order().ToArray()
                : [],
            MyBans = state.Choices.Where(choice => choice.Kind == FactionPickSelectionKind.Ban
                    && viewer.CanSee(DraftVisibility.SeatPrivate, choice.SeatId))
                .Select(choice => choice.Faction).Order().ToArray(),
            LockedBanSeatIds = state.LockedBanSeatIds.ToArray(),
            BansRevealed = state.BansRevealed,
            TurnIndex = state.TurnIndex,
            ActiveSeatId = state.IsStarted && state.Phase == FactionPickPhase.Picks ? SeatAt(settings, state, state.TurnIndex) : null,
            MyPendingPick = state.Choices.FirstOrDefault(choice => choice.Kind == FactionPickSelectionKind.Pick
                && choice.State == FactionPickSelectionState.Pending
                && viewer.CanSee(DraftVisibility.SeatPrivate, choice.SeatId))?.Faction,
            LockedPicks = OrderedPicks(settings, state)
                .Select(choice => new FactionPickLockedPickDto(choice.SeatId!.Value, choice.Faction, choice.Round, choice.DecidedByParticipantId is not null))
                .ToArray(),
            CanUndo = state.IsStarted && state.Phase == FactionPickPhase.Picks && state.TurnIndex > 0
                && viewer.Role is DraftRoomRole.Host or DraftRoomRole.CoHost,
            MyActionRequired = RequiresAction(settings, state, viewer),
        };
    }

    /// <inheritdoc />
    public JsonElement BuildResult(FactionPickSettingsDto settings, FactionPickDraftState state)
    {
        ArgumentNullException.ThrowIfNull(settings);
        ArgumentNullException.ThrowIfNull(state);
        return BuildFactionResult(settings, state);
    }

    /// <inheritdoc />
    public string FormatResultAsText(JsonElement result) =>
        FormatFactionResult(result);

    private static DraftTransition Refuse(FactionPickDraftState state, string code) =>
        new(state, new DraftValidationResult([code]), [], []);

    private static bool HasValidOrderShape(FactionPickSettingsDto settings, int seatCount) =>
        settings.SeatOrder is not null
        && (settings.PickOrderMode != FactionPickOrderMode.HostDefined
            || (settings.SeatOrder.Count == seatCount && !settings.SeatOrder.Contains(Guid.Empty)
                && settings.SeatOrder.Distinct().Count() == seatCount));
}
