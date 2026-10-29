using TwilightImperiumUltimate.Contracts.DTOs.DraftRooms;
using TwilightImperiumUltimate.Contracts.Enums;
using TwilightImperiumUltimate.Core.Entities.DraftRooms;
using TwilightImperiumUltimate.DataAccess.Repositories;
using TwilightImperiumUltimate.DataAccess.UnitOfWork;
using TwilightImperiumUltimate.Draft.DraftRooms.Engine;
using TwilightImperiumUltimate.Draft.DraftRooms.FactionPick;

namespace TwilightImperiumUltimate.Business.Services.DraftRooms;

/// <inheritdoc />
public sealed class DraftRoomDraftPersistence(
    IFactionPickRepository factionPick,
    IFactionPickPoolProvider poolProvider) : IDraftRoomDraftPersistence
{
    private readonly IFactionPickRepository factionPick = factionPick ?? throw new ArgumentNullException(nameof(factionPick));
    private readonly IFactionPickPoolProvider poolProvider = poolProvider ?? throw new ArgumentNullException(nameof(poolProvider));

    /// <inheritdoc />
    public void InitializeDefaults(DraftRoomScope scope, DraftTypeId type)
    {
        ArgumentNullException.ThrowIfNull(scope);
        switch (type)
        {
            case DraftTypeId.FactionPick:
                scope.Context.FactionPickSettings.Add(new FactionPickSettings { RoomId = scope.Room.Id });
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(type), type, "No persistence adapter is registered for this draft type.");
        }
    }

    /// <inheritdoc />
    public Task DeleteAllAsync(DraftRoomScope scope, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(scope);
        return scope.Room.DraftType switch
        {
            nameof(DraftTypeId.FactionPick) => this.factionPick.DeleteAllAsync(scope, cancellationToken),
            _ => throw new InvalidOperationException("The room's draft type has no registered persistence adapter."),
        };
    }

    /// <inheritdoc />
    public async Task<(DraftSettingsDto Settings, IDraftState State, int PoolSize)> LoadAsync(
        DraftRoomScope scope,
        CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(scope);
        EnsureFactionPick(scope);
        var settings = await this.factionPick.LoadSettingsAsync(scope, cancellationToken)
            ?? throw new InvalidOperationException("The room is missing its required Faction Pick settings.");
        var pool = await this.poolProvider.GetPoolAsync(settings.GameVersions, cancellationToken);
        var persisted = await this.factionPick.LoadStateAsync(scope, cancellationToken);
        if (scope.Room.Status != DraftRoomStatus.Lobby && persisted is null)
        {
            throw new InvalidOperationException("The running room is missing its required Faction Pick state.");
        }

        var order = await this.factionPick.LoadTurnOrderAsync(scope, cancellationToken);
        var choices = await this.factionPick.LoadSelectionsAsync(scope, cancellationToken);
        var state = new FactionPickDraftState
        {
            SeatIds = scope.Room.Seats.OrderBy(seat => seat.Index).Select(seat => seat.Id).ToArray(),
            Pool = pool,
            IsStarted = persisted is not null,
            Phase = persisted?.Phase ?? FactionPickPhase.Bans,
            TurnIndex = persisted?.TurnIndex ?? 0,
            BansRevealed = persisted?.BansRevealed ?? false,
            BansMode = settings.BansMode,
            TurnOrder = order.Select(entry => entry.SeatId).ToArray(),
            LockedBanSeatIds = persisted?.LockedBanSeatIds.ToArray() ?? [],
            Choices = choices.Select(FactionPickChoiceMapping.ToChoice).ToArray(),
            ResultSeats = scope.Room.Seats.OrderBy(seat => seat.Index)
                .Select(seat => new FactionPickResultSeat(
                    seat.Id,
                    seat.Index,
                    scope.Room.Participants.SingleOrDefault(participant => participant.Id == seat.ParticipantId)?.DisplayName ?? string.Empty))
                .ToArray(),
            ParticipantNames = scope.Room.Participants.ToDictionary(participant => participant.Id, participant => participant.DisplayName),
        };
        return (FactionPickSettingsMapping.ToDto(settings), state, pool.Count);
    }

    /// <inheritdoc />
    public async Task SaveStartedStateAsync(DraftRoomScope scope, IDraftState state, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(state);
        await this.SaveRuntimeStateAsync(scope, state, cancellationToken);
        this.factionPick.AddTurnOrder(scope, ((FactionPickDraftState)state).TurnOrder);
    }

    /// <inheritdoc />
    public async Task SaveRuntimeStateAsync(DraftRoomScope scope, IDraftState state, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(scope);
        EnsureFactionPick(scope);
        if (state is not FactionPickDraftState { IsStarted: true } factionState)
        {
            throw new ArgumentException("An initialized Faction Pick state is required.", nameof(state));
        }

        await this.factionPick.SaveStateAsync(
            scope,
            new FactionPickState
            {
                RoomId = scope.Room.Id,
                Phase = factionState.Phase,
                TurnIndex = factionState.TurnIndex,
                BansRevealed = factionState.BansRevealed,
                LockedBanSeatIds = factionState.LockedBanSeatIds.ToList(),
            },
            cancellationToken);
        await this.factionPick.SaveSelectionsAsync(
            scope,
            factionState.Choices.Select(choice => FactionPickChoiceMapping.ToEntity(choice, scope.Room.Id)).ToArray(),
            cancellationToken);
    }

    /// <inheritdoc />
    public Task DeleteStateAsync(DraftRoomScope scope, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(scope);
        EnsureFactionPick(scope);
        return this.factionPick.DeleteStateAsync(scope, cancellationToken);
    }

    /// <inheritdoc />
    public async Task<DraftValidationResult> ValidateSettingsAsync(
        DraftRoomScope scope,
        DraftSettingsDto settings,
        IDraftTypeModule module,
        CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(scope);
        ArgumentNullException.ThrowIfNull(module);
        EnsureFactionPick(scope);
        if (settings is not FactionPickSettingsDto factionSettings)
        {
            throw new ArgumentException("Faction Pick settings are required.", nameof(settings));
        }

        var preliminary = module.ValidateSettings(settings, scope.Room.Seats.Count, int.MaxValue);
        if (!preliminary.IsValid)
        {
            return preliminary;
        }

        if (!FactionPickDraftType.IsSeatOrderValid(factionSettings, scope.Room.Seats.Select(seat => seat.Id).ToArray()))
        {
            return new DraftValidationResult(["InvalidSeatOrder"]);
        }

        var pool = await this.poolProvider.GetPoolAsync(factionSettings.GameVersions.ToArray(), cancellationToken);
        return module.ValidateSettings(settings, scope.Room.Seats.Count, pool.Count);
    }

    /// <inheritdoc />
    public Task SaveSettingsAsync(DraftRoomScope scope, DraftSettingsDto settings, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(scope);
        EnsureFactionPick(scope);
        if (settings is not FactionPickSettingsDto factionSettings)
        {
            throw new ArgumentException("Faction Pick settings are required.", nameof(settings));
        }

        return this.factionPick.SaveSettingsAsync(
            scope,
            new FactionPickSettings
            {
                RoomId = scope.Room.Id,
                GameVersions = factionSettings.GameVersions.ToList(),
                BansMode = factionSettings.BansMode,
                BansPerPlayer = factionSettings.BansPerPlayer,
                HostBanCount = factionSettings.HostBanCount,
                PickOrderMode = factionSettings.PickOrderMode,
                SeatOrder = factionSettings.SeatOrder.ToList(),
                PicksPerPlayer = factionSettings.PicksPerPlayer,
                Snake = factionSettings.Snake,
            },
            cancellationToken);
    }

    private static void EnsureFactionPick(DraftRoomScope scope)
    {
        if (!string.Equals(scope.Room.DraftType, nameof(DraftTypeId.FactionPick), StringComparison.Ordinal))
        {
            throw new InvalidOperationException("The room's draft type has no registered persistence adapter.");
        }
    }
}
