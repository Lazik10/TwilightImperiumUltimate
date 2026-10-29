using FluentResults;
using TwilightImperiumUltimate.Business.Services.DraftRooms;
using TwilightImperiumUltimate.Contracts.Enums;
using TwilightImperiumUltimate.DataAccess.UnitOfWork;

namespace TwilightImperiumUltimate.Business.Logic.DraftRooms;

/// <summary>
/// Changes a participant's role when the active host requests it.
/// </summary>
public sealed class SetParticipantRoleCommandHandler(
    IDraftRoomUnitOfWork unitOfWork,
    IDraftRoomAuthorizer authorizer,
    IDraftRoomLog draftRoomLog)
    : IRequestHandler<SetParticipantRoleCommand, Result>
{
    private readonly IDraftRoomUnitOfWork _unitOfWork = unitOfWork
        ?? throw new ArgumentNullException(nameof(unitOfWork));

    private readonly IDraftRoomAuthorizer _authorizer = authorizer
        ?? throw new ArgumentNullException(nameof(authorizer));

    private readonly IDraftRoomLog _draftRoomLog = draftRoomLog
        ?? throw new ArgumentNullException(nameof(draftRoomLog));

    /// <inheritdoc />
    public async Task<Result> Handle(SetParticipantRoleCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);
        ArgumentException.ThrowIfNullOrWhiteSpace(request.Code);
        ArgumentNullException.ThrowIfNull(request.Caller);

        if (request.Role is not (DraftRoomRole.Member or DraftRoomRole.CoHost))
        {
            return Failure(DraftRoomErrorCode.InvalidRole);
        }

        try
        {
            return await this._unitOfWork.ExecuteAsync(
                request.Code,
                scope =>
                {
                    var actor = this._authorizer.Resolve(scope.Room, request.Caller);
                    if (actor is null)
                    {
                        return Task.FromResult(Failure(DraftRoomErrorCode.NotFoundOrWrongPassword));
                    }

                    var target = scope.Room.Participants
                        .SingleOrDefault(participant => participant.Id == request.ParticipantId);
                    if (target is null)
                    {
                        return Task.FromResult(Failure(DraftRoomErrorCode.ParticipantNotFound));
                    }

                    if (!this._authorizer.CanChangeRoles(actor, target))
                    {
                        return Task.FromResult(Failure(DraftRoomErrorCode.Forbidden));
                    }

                    var previousRole = target.Role;
                    if (previousRole != request.Role)
                    {
                        target.Role = request.Role;
                        this._draftRoomLog.Append(
                            scope,
                            DraftLogEntryType.RoleChanged,
                            actor.Participant.Id,
                            new
                            {
                                ParticipantId = target.Id,
                                target.DisplayName,
                                PreviousRole = previousRole.ToString(),
                                NewRole = request.Role.ToString(),
                            });
                    }

                    return Task.FromResult(Result.Ok());
                },
                cancellationToken).ConfigureAwait(false);
        }
        catch (DraftRoomNotFoundException)
        {
            return Failure(DraftRoomErrorCode.NotFoundOrWrongPassword);
        }
        catch (DraftRoomConflictException)
        {
            return Failure(DraftRoomErrorCode.RoomChanged);
        }
    }

    private static Result Failure(DraftRoomErrorCode code)
    {
        return Result.Fail(new DraftRoomError(code));
    }
}
