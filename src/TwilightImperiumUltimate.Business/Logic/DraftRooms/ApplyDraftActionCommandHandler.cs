using System.Text.Json;
using FluentResults;
using TwilightImperiumUltimate.Business.Services.DraftRooms;
using TwilightImperiumUltimate.Contracts.DTOs.DraftRooms;
using TwilightImperiumUltimate.Contracts.Enums;
using TwilightImperiumUltimate.DataAccess.UnitOfWork;
using TwilightImperiumUltimate.Draft.DraftRooms.Engine;

namespace TwilightImperiumUltimate.Business.Logic.DraftRooms;

/// <summary>Persists successful engine actions and their ordered public effects in one room transaction.</summary>
public sealed class ApplyDraftActionCommandHandler(
    IDraftRoomUnitOfWork unitOfWork,
    IDraftRoomAuthorizer authorizer,
    IDraftTypeRegistry registry,
    IDraftRoomDraftPersistence persistence,
    IDraftRoomLog log,
    DraftRoomResultWriter resultWriter) : IRequestHandler<ApplyDraftActionCommand, Result>, IRequestHandler<DecideForCommand, Result>, IRequestHandler<UndoLastPickCommand, Result>
{
    /// <inheritdoc />
    public Task<Result> Handle(ApplyDraftActionCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);
        ArgumentNullException.ThrowIfNull(request.Action);
        return this.ExecuteAsync(
            request.Code,
            request.Caller,
            false,
            (module, settings, state, actor) => module.Apply(settings, state, actor, request.Action),
            cancellationToken);
    }

    /// <inheritdoc />
    public Task<Result> Handle(DecideForCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);
        ArgumentNullException.ThrowIfNull(request.Action);
        return this.ExecuteAsync(
            request.Code,
            request.Caller,
            true,
            (module, settings, state, actor) => module.DecideFor(settings, state, actor, request.SeatId, request.Action),
            cancellationToken);
    }

    /// <inheritdoc />
    public Task<Result> Handle(UndoLastPickCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);
        return this.ExecuteAsync(
            request.Code,
            request.Caller,
            true,
            (module, settings, state, actor) => module.Undo(settings, state, actor),
            cancellationToken);
    }

    private static DraftLogEntryType LogType(string code) => code switch
    {
        "HostBansChanged" => DraftLogEntryType.HostBansChanged,
        "SeatBansLocked" => DraftLogEntryType.SeatBansLocked,
        "BansRevealed" => DraftLogEntryType.BansRevealed,
        "HostDecidedForSeat" => DraftLogEntryType.HostDecidedForSeat,
        "FactionPicked" => DraftLogEntryType.FactionPicked,
        "PickUndone" => DraftLogEntryType.PickUndone,
        "DraftFinished" => DraftLogEntryType.DraftFinished,
        _ => throw new InvalidOperationException($"No public log type is registered for engine event '{code}'."),
    };

    private async Task<Result> ExecuteAsync(
        string code,
        DraftRoomCaller caller,
        bool managerOnly,
        Func<IDraftTypeModule, DraftSettingsDto, IDraftState, DraftActor, DraftTransition> apply,
        CancellationToken cancellationToken)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(code);
        ArgumentNullException.ThrowIfNull(caller);
        try
        {
            return await unitOfWork.ExecuteAsync(
                code,
                async scope =>
                {
                    var actor = authorizer.Resolve(scope.Room, caller);
                    if (actor is null)
                    {
                        return Result.Fail(new DraftRoomError(DraftRoomErrorCode.NotFoundOrWrongPassword));
                    }

                    if (managerOnly && !authorizer.CanManageDraft(actor))
                    {
                        return Result.Fail(new DraftRoomError(DraftRoomErrorCode.Forbidden));
                    }

                    if (scope.Room.Status != DraftRoomStatus.InProgress)
                    {
                        return Result.Fail(new DraftRoomError(DraftRoomErrorCode.RoomChanged));
                    }

                    var module = registry.Get(Enum.Parse<DraftTypeId>(scope.Room.DraftType));
                    var loaded = await persistence.LoadAsync(scope, cancellationToken);
                    var engineActor = new DraftActor(actor.Participant.Id, actor.Seat?.Id, actor.Role);
                    var transition = apply(module, loaded.Settings, loaded.State, engineActor);
                    if (!transition.Validation.IsValid)
                    {
                        var error = DraftRoomErrorCode.InvalidDraftAction;
                        if (transition.Validation.ErrorCodes.Any(item => item is "Forbidden" or "NotYourTurn"))
                        {
                            error = DraftRoomErrorCode.Forbidden;
                        }
                        else if (transition.Validation.ErrorCodes.Any(item => item is "AlreadyLocked" or "WrongPhase" or "StaleTurn" or "MissingSelection" or "NothingToUndo"))
                        {
                            error = DraftRoomErrorCode.RoomChanged;
                        }

                        return Result.Fail(new DraftRoomError(error, transition.Validation.ErrorCodes));
                    }

                    await persistence.SaveRuntimeStateAsync(scope, transition.State, cancellationToken);
                    if (transition.IsFinished)
                    {
                        resultWriter.Create(scope, module, loaded.Settings, transition.State);
                        scope.Room.Status = DraftRoomStatus.Finished;
                    }

                    foreach (var entry in transition.LogEvents)
                    {
                        var publicData = entry.Data.EnumerateObject().ToDictionary(property => property.Name, property => property.Value.Clone());
                        publicData["ActorDisplayName"] = JsonSerializer.SerializeToElement(actor.Participant.DisplayName);
                        if (entry.Data.TryGetProperty("SeatId", out var loggedSeatId))
                        {
                            var loggedSeat = scope.Room.Seats.Single(seat => seat.Id == loggedSeatId.GetGuid());
                            publicData["SeatIndex"] = JsonSerializer.SerializeToElement(loggedSeat.Index);
                        }

                        log.Append(scope, LogType(entry.Code), actor.Participant.Id, publicData);
                    }

                    return Result.Ok();
                },
                cancellationToken);
        }
        catch (DraftRoomNotFoundException)
        {
            return Result.Fail(new DraftRoomError(DraftRoomErrorCode.NotFoundOrWrongPassword));
        }
        catch (DraftRoomConflictException)
        {
            return Result.Fail(new DraftRoomError(DraftRoomErrorCode.RoomChanged));
        }
    }
}
