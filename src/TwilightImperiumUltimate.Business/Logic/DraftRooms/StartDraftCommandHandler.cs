using FluentResults;
using TwilightImperiumUltimate.Business.Services.DraftRooms;
using TwilightImperiumUltimate.Contracts.Enums;
using TwilightImperiumUltimate.DataAccess.UnitOfWork;
using TwilightImperiumUltimate.Draft.DraftRooms.Engine;

namespace TwilightImperiumUltimate.Business.Logic.DraftRooms;

/// <summary>Validates and starts a draft within one authorized room transaction.</summary>
public sealed class StartDraftCommandHandler(
    IDraftRoomUnitOfWork unitOfWork,
    IDraftRoomAuthorizer authorizer,
    IDraftTypeRegistry registry,
    IDraftRoomDraftPersistence persistence,
    IRandomnessSource randomness,
    IDraftRoomLog log) : IRequestHandler<StartDraftCommand, Result>
{
    private readonly IDraftRoomUnitOfWork unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
    private readonly IDraftRoomAuthorizer authorizer = authorizer ?? throw new ArgumentNullException(nameof(authorizer));
    private readonly IDraftTypeRegistry registry = registry ?? throw new ArgumentNullException(nameof(registry));
    private readonly IDraftRoomDraftPersistence persistence = persistence ?? throw new ArgumentNullException(nameof(persistence));
    private readonly IRandomnessSource randomness = randomness ?? throw new ArgumentNullException(nameof(randomness));
    private readonly IDraftRoomLog log = log ?? throw new ArgumentNullException(nameof(log));

    /// <inheritdoc />
    public async Task<Result> Handle(StartDraftCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);
        ArgumentException.ThrowIfNullOrWhiteSpace(request.Code);
        ArgumentNullException.ThrowIfNull(request.Caller);
        try
        {
            return await this.unitOfWork.ExecuteAsync(
                request.Code,
                async scope =>
                {
                    var actor = this.authorizer.Resolve(scope.Room, request.Caller);
                    if (actor is null)
                    {
                        return Result.Fail(new DraftRoomError(DraftRoomErrorCode.NotFoundOrWrongPassword));
                    }

                    if (!this.authorizer.CanManageDraft(actor))
                    {
                        return Result.Fail(new DraftRoomError(DraftRoomErrorCode.Forbidden));
                    }

                    if (scope.Room.Status != DraftRoomStatus.Lobby)
                    {
                        return Result.Fail(new DraftRoomError(DraftRoomErrorCode.RoomChanged));
                    }

                    var module = this.registry.Get(Enum.Parse<DraftTypeId>(scope.Room.DraftType));
                    var loaded = await this.persistence.LoadAsync(scope, cancellationToken);
                    var validation = module.ValidateSettings(loaded.Settings, scope.Room.Seats.Count, loaded.PoolSize);
                    var blockers = DraftRoomStartRules.GetBlockers(scope.Room, module, validation);
                    if (blockers.Count > 0)
                    {
                        return Result.Fail(blockers.Select(code => new DraftRoomError(code, validation.ErrorCodes)));
                    }

                    var transition = module.Start(loaded.Settings, loaded.State, this.randomness);
                    if (!transition.Validation.IsValid)
                    {
                        return Result.Fail(new DraftRoomError(DraftRoomErrorCode.SettingsInvalid, transition.Validation.ErrorCodes));
                    }

                    await this.persistence.SaveStartedStateAsync(scope, transition.State, cancellationToken);
                    foreach (var seat in scope.Room.Seats)
                    {
                        var participant = scope.Room.Participants.Single(item => item.Id == seat.ParticipantId);
                        participant.DraftSeatId = seat.Id;
                        participant.CanReclaimDraftSeat = false;
                        seat.HasBeenReleased = false;
                    }

                    scope.Room.Status = DraftRoomStatus.InProgress;
                    foreach (var entry in transition.LogEvents)
                    {
                        var type = entry.Code == "PickOrderConfigured" ? DraftLogEntryType.DraftOrderConfigured : DraftLogEntryType.DraftRandomized;
                        this.log.Append(scope, type, actor.Participant.Id, new { entry.Code, entry.Data });
                    }

                    this.log.Append(scope, DraftLogEntryType.DraftStarted, actor.Participant.Id, new { scope.Room.DraftType });
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
