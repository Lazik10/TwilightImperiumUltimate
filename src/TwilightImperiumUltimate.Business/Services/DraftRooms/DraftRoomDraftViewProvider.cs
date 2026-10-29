using TwilightImperiumUltimate.Contracts.Enums;
using TwilightImperiumUltimate.Draft.DraftRooms.Engine;
using TwilightImperiumUltimate.Draft.DraftRooms.FactionPick;

namespace TwilightImperiumUltimate.Business.Services.DraftRooms;

/// <inheritdoc />
public sealed class DraftRoomDraftViewProvider(
    IDraftTypeRegistry registry,
    IFactionPickPoolProvider poolProvider,
    IDraftRoomAuthorizer authorizer) : IDraftRoomDraftViewProvider
{
    private readonly IDraftTypeRegistry registry = registry ?? throw new ArgumentNullException(nameof(registry));
    private readonly IFactionPickPoolProvider poolProvider = poolProvider ?? throw new ArgumentNullException(nameof(poolProvider));
    private readonly IDraftRoomAuthorizer authorizer = authorizer ?? throw new ArgumentNullException(nameof(authorizer));

    /// <inheritdoc />
    public async Task<DraftRoomDraftView> GetAsync(DraftRoomActor actor, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(actor);
        var room = actor.Room;
        var type = Enum.Parse<DraftTypeId>(room.DraftType);
        var module = this.registry.Get(type);
        if (type != DraftTypeId.FactionPick)
        {
            throw new InvalidOperationException("No view adapter is registered for the room's draft type.");
        }

        var storedSettings = room.FactionPickSettings
            ?? throw new InvalidOperationException("The room is missing its required Faction Pick settings.");
        var settings = FactionPickSettingsMapping.ToDto(storedSettings);
        var pool = await this.poolProvider.GetPoolAsync(storedSettings.GameVersions, cancellationToken);
        var validation = module.ValidateSettings(settings, room.Seats.Count, pool.Count);
        if (validation.IsValid && !FactionPickDraftType.IsSeatOrderValid(settings, room.Seats.Select(seat => seat.Id).ToArray()))
        {
            validation = new DraftValidationResult(["InvalidSeatOrder"]);
        }

        var blockers = DraftRoomStartRules.GetBlockers(room, module, validation).Select(code => code.ToString()).ToArray();
        var canManage = this.authorizer.CanManageDraft(actor);
        var storedState = room.FactionPickState;
        if (room.Status != DraftRoomStatus.Lobby && storedState is null)
        {
            throw new InvalidOperationException("The running room is missing its required Faction Pick state.");
        }

        var state = new FactionPickDraftState
        {
            IsStarted = storedState is not null,
            Phase = storedState?.Phase ?? FactionPickPhase.Bans,
            TurnIndex = storedState?.TurnIndex ?? 0,
            BansRevealed = storedState?.BansRevealed ?? false,
            BansMode = settings.BansMode,
            SeatIds = room.Seats.OrderBy(seat => seat.Index).Select(seat => seat.Id).ToArray(),
            Pool = pool,
            TurnOrder = room.FactionPickTurnOrder.OrderBy(entry => entry.Position).Select(entry => entry.SeatId).ToArray(),
            LockedBanSeatIds = storedState?.LockedBanSeatIds.ToArray() ?? [],
            Choices = room.FactionPickSelections.Select(FactionPickChoiceMapping.ToChoice).ToArray(),
        };
        var projected = room.Status == DraftRoomStatus.Lobby
            ? null
            : module.Project(settings, state, new DraftViewer(actor.Participant.Id, actor.Seat?.Id, actor.Role));
        return new DraftRoomDraftView(
            type,
            settings,
            projected,
            canManage && blockers.Length == 0,
            blockers,
            canManage && room.Status == DraftRoomStatus.InProgress);
    }
}
