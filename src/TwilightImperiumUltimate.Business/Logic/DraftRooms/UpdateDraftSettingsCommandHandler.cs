using FluentResults;
using TwilightImperiumUltimate.Business.Services.DraftRooms;
using TwilightImperiumUltimate.Contracts.Enums;
using TwilightImperiumUltimate.DataAccess.UnitOfWork;
using TwilightImperiumUltimate.Draft.DraftRooms.Engine;

namespace TwilightImperiumUltimate.Business.Logic.DraftRooms;

/// <summary>Validates and saves engine settings in the room's existing transaction.</summary>
public sealed class UpdateDraftSettingsCommandHandler(
    IDraftRoomUnitOfWork unitOfWork,
    IDraftRoomAuthorizer authorizer,
    IDraftTypeRegistry registry,
    IDraftRoomDraftPersistence persistence,
    IDraftRoomLog log) : IRequestHandler<UpdateDraftSettingsCommand, Result>
{
    private readonly IDraftRoomUnitOfWork unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
    private readonly IDraftRoomAuthorizer authorizer = authorizer ?? throw new ArgumentNullException(nameof(authorizer));
    private readonly IDraftTypeRegistry registry = registry ?? throw new ArgumentNullException(nameof(registry));
    private readonly IDraftRoomDraftPersistence persistence = persistence ?? throw new ArgumentNullException(nameof(persistence));
    private readonly IDraftRoomLog log = log ?? throw new ArgumentNullException(nameof(log));

    /// <inheritdoc />
    public async Task<Result> Handle(UpdateDraftSettingsCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);
        ArgumentException.ThrowIfNullOrWhiteSpace(request.Code);
        ArgumentNullException.ThrowIfNull(request.Settings);
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

                    if (scope.Room.Status != DraftRoomStatus.Lobby)
                    {
                        return Result.Fail(new DraftRoomError(DraftRoomErrorCode.RoomChanged));
                    }

                    if (!this.authorizer.CanChangeSettings(actor))
                    {
                        return Result.Fail(new DraftRoomError(DraftRoomErrorCode.Forbidden));
                    }

                    var module = this.registry.Get(Enum.Parse<DraftTypeId>(scope.Room.DraftType));
                    if (request.Settings.GetType() != module.CreateDefaultSettings().GetType())
                    {
                        return Result.Fail(new DraftRoomError(DraftRoomErrorCode.SettingsInvalid, ["SettingsTypeMismatch"]));
                    }

                    var validation = await this.persistence.ValidateSettingsAsync(scope, request.Settings, module, cancellationToken);
                    if (!validation.IsValid)
                    {
                        return Result.Fail(new DraftRoomError(DraftRoomErrorCode.SettingsInvalid, validation.ErrorCodes));
                    }

                    await this.persistence.SaveSettingsAsync(scope, request.Settings, cancellationToken);
                    this.log.Append(
                        scope,
                        DraftLogEntryType.DraftSettingsChanged,
                        actor.Participant.Id,
                        new { scope.Room.DraftType });
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
