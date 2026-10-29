using FluentResults;
using TwilightImperiumUltimate.Contracts.DTOs.DraftRooms;
using TwilightImperiumUltimate.Contracts.Enums;
using TwilightImperiumUltimate.DataAccess.Repositories;

namespace TwilightImperiumUltimate.Business.Logic.DraftRooms;

/// <summary>
/// Loads a bounded list of rooms where the account has an active participant record.
/// </summary>
public sealed class GetMyRoomsQueryHandler(IDraftRoomRepository draftRoomRepository)
    : IRequestHandler<GetMyRoomsQuery, Result<IReadOnlyList<MyDraftRoomDto>>>
{
    private const int MaximumRoomCount = 50;

    private readonly IDraftRoomRepository _draftRoomRepository = draftRoomRepository
        ?? throw new ArgumentNullException(nameof(draftRoomRepository));

    /// <inheritdoc />
    public async Task<Result<IReadOnlyList<MyDraftRoomDto>>> Handle(
        GetMyRoomsQuery request,
        CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);
        if (string.IsNullOrWhiteSpace(request.UserId))
        {
            return Result.Fail<IReadOnlyList<MyDraftRoomDto>>(
                new DraftRoomError(DraftRoomErrorCode.InvalidCaller));
        }

        var participants = await this._draftRoomRepository
            .GetActiveParticipantsForUserAsync(
                request.UserId,
                MaximumRoomCount,
                cancellationToken)
            .ConfigureAwait(false);
        var rooms = participants
            .Select(participant => new MyDraftRoomDto
            {
                Code = participant.Room.Code,
                Name = participant.Room.Name,
                Status = participant.Room.Status,
                Role = participant.Role,
                SeatIndex = participant.Room.Seats
                    .Where(seat => seat.ParticipantId == participant.Id)
                    .Select(seat => (int?)seat.Index)
                    .FirstOrDefault(),
                LastActivityUtc = participant.Room.LastActivityUtc,
            })
            .ToArray();

        return Result.Ok<IReadOnlyList<MyDraftRoomDto>>(rooms);
    }
}
