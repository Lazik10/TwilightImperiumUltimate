using FluentResults;
using TwilightImperiumUltimate.Business.Services.DraftRooms;
using TwilightImperiumUltimate.Contracts.Enums;
using TwilightImperiumUltimate.DataAccess.UnitOfWork;
using TwilightImperiumUltimate.Draft.DraftRooms.Engine;

namespace TwilightImperiumUltimate.Business.Logic.DraftRooms;

/// <summary>Changes type-specific storage atomically with the authorized lobby update and log entry.</summary>
public sealed class ChangeDraftTypeCommandHandler(
    IDraftRoomUnitOfWork unitOfWork,
    IDraftRoomAuthorizer authorizer,
    IDraftTypeRegistry registry,
    IDraftRoomDraftPersistence persistence,
    IDraftRoomLog log) : IRequestHandler<ChangeDraftTypeCommand, Result>
{
    private readonly IDraftRoomUnitOfWork unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
    private readonly IDraftRoomAuthorizer authorizer = authorizer ?? throw new ArgumentNullException(nameof(authorizer));
    private readonly IDraftTypeRegistry registry = registry ?? throw new ArgumentNullException(nameof(registry));
    private readonly IDraftRoomDraftPersistence persistence = persistence ?? throw new ArgumentNullException(nameof(persistence));
    private readonly IDraftRoomLog log = log ?? throw new ArgumentNullException(nameof(log));

    /// <inheritdoc />
    public async Task<Result> Handle(ChangeDraftTypeCommand request, CancellationToken cancellationToken)
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

                    if (scope.Room.Status != DraftRoomStatus.Lobby)
                    {
                        return Result.Fail(new DraftRoomError(DraftRoomErrorCode.RoomChanged));
                    }

                    if (!this.authorizer.CanChangeSettings(actor))
                    {
                        return Result.Fail(new DraftRoomError(DraftRoomErrorCode.Forbidden));
                    }

                    return await DraftRoomTypeChange.ApplyAsync(
                        scope, actor, request.Type, this.registry, this.persistence, this.log, cancellationToken);
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
