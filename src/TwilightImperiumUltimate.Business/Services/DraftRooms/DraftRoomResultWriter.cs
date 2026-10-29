using System.Security.Cryptography;
using TwilightImperiumUltimate.Contracts.DTOs.DraftRooms;
using TwilightImperiumUltimate.Contracts.Enums;
using TwilightImperiumUltimate.Core.Entities.DraftRooms;
using TwilightImperiumUltimate.DataAccess.Repositories;
using TwilightImperiumUltimate.DataAccess.UnitOfWork;
using TwilightImperiumUltimate.Draft.DraftRooms.Engine;

namespace TwilightImperiumUltimate.Business.Services.DraftRooms;

/// <summary>Stages a public result in the exact transaction that locks the final pick.</summary>
public sealed class DraftRoomResultWriter(IDraftResultRepository repository)
{
    /// <summary>Builds and stages the immutable snapshot without committing independently.</summary>
    public void Create(DraftRoomScope scope, IDraftTypeModule module, DraftSettingsDto settings, IDraftState state)
    {
        ArgumentNullException.ThrowIfNull(scope);
        ArgumentNullException.ThrowIfNull(module);
        if (scope.Room.ResultId is not null)
        {
            throw new InvalidOperationException("A finished room cannot create a second result.");
        }

        var result = new DraftResult
        {
            Id = Guid.NewGuid(),
            ShareToken = Convert.ToBase64String(RandomNumberGenerator.GetBytes(16)).TrimEnd('=').Replace('+', '-').Replace('/', '_'),
            DraftType = module.Id,
            RoomName = scope.Room.Name,
            SourceRoomId = scope.Room.Id,
            OwnerUserId = scope.Room.Participants.Single(participant => participant.Role == DraftRoomRole.Host).UserId,
            FinishedAtUtc = scope.Now,
            CreatedAtUtc = scope.Now,
            PayloadVersion = 1,
            PayloadJson = module.BuildResult(settings, state).GetRawText(),
        };
        foreach (var seat in scope.Room.Seats.OrderBy(seat => seat.Index))
        {
            var participant = scope.Room.Participants.SingleOrDefault(item => item.Id == seat.ParticipantId);
            result.Players.Add(new DraftResultPlayer
            {
                ResultId = result.Id,
                SeatIndex = seat.Index,
                DisplayName = participant?.DisplayName ?? string.Empty,
                UserId = participant?.UserId,
            });
        }

        repository.Add(scope, result);
        scope.Room.ResultId = result.Id;
    }
}
