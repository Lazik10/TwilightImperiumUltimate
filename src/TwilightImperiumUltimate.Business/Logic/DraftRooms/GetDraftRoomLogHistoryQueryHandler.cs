using FluentResults;
using TwilightImperiumUltimate.Business.Services.DraftRooms;
using TwilightImperiumUltimate.Contracts.DraftRooms;
using TwilightImperiumUltimate.Contracts.DTOs.DraftRooms;
using TwilightImperiumUltimate.Contracts.Enums;
using TwilightImperiumUltimate.DataAccess.Repositories;

namespace TwilightImperiumUltimate.Business.Logic.DraftRooms;

/// <summary>
/// Loads a bounded page of activity entries for an active room participant.
/// </summary>
public sealed class GetDraftRoomLogHistoryQueryHandler(
    IDraftRoomRepository draftRoomRepository,
    IDraftRoomAuthorizer authorizer)
    : IRequestHandler<GetDraftRoomLogHistoryQuery, Result<IReadOnlyList<DraftLogEntryDto>>>
{
    private readonly IDraftRoomRepository _draftRoomRepository = draftRoomRepository
        ?? throw new ArgumentNullException(nameof(draftRoomRepository));

    private readonly IDraftRoomAuthorizer _authorizer = authorizer
        ?? throw new ArgumentNullException(nameof(authorizer));

    /// <inheritdoc />
    public async Task<Result<IReadOnlyList<DraftLogEntryDto>>> Handle(
        GetDraftRoomLogHistoryQuery request,
        CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);
        ArgumentNullException.ThrowIfNull(request.Caller);

        if (request.AfterId is < 0)
        {
            return Failure(DraftRoomErrorCode.InvalidHistoryCursor);
        }

        if (request.Limit is < 1 or > DraftRoomLimits.MaxHistoryPageSize)
        {
            return Failure(DraftRoomErrorCode.InvalidHistoryLimit);
        }

        if (string.IsNullOrWhiteSpace(request.Code))
        {
            return Failure(DraftRoomErrorCode.NotFoundOrWrongPassword);
        }

        var room = await this._draftRoomRepository
            .GetRoomByCodeAsync(request.Code.Trim().ToUpperInvariant(), cancellationToken)
            .ConfigureAwait(false);
        if (room is null || this._authorizer.Resolve(room, request.Caller) is null)
        {
            return Failure(DraftRoomErrorCode.NotFoundOrWrongPassword);
        }

        var entries = await this._draftRoomRepository
            .GetLogHistoryAsync(room.Id, request.AfterId, request.Limit, cancellationToken)
            .ConfigureAwait(false);

        return Result.Ok<IReadOnlyList<DraftLogEntryDto>>(entries
            .Select(entry => new DraftLogEntryDto
            {
                Id = entry.Id,
                ActorParticipantId = entry.ActorParticipantId,
                Type = entry.Type,
                DataJson = entry.DataJson,
                CreatedAtUtc = entry.CreatedAtUtc,
            })
            .ToArray());
    }

    private static Result<IReadOnlyList<DraftLogEntryDto>> Failure(DraftRoomErrorCode code)
    {
        return Result.Fail<IReadOnlyList<DraftLogEntryDto>>(new DraftRoomError(code));
    }
}
