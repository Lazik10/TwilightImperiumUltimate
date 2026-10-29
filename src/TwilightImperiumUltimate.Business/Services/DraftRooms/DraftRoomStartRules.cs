using TwilightImperiumUltimate.Contracts.Enums;
using TwilightImperiumUltimate.Core.Entities.DraftRooms;
using TwilightImperiumUltimate.Draft.DraftRooms.Engine;

namespace TwilightImperiumUltimate.Business.Services.DraftRooms;

/// <summary>Evaluates the shared lifecycle blockers used by Start and the personalized room view.</summary>
public static class DraftRoomStartRules
{
    /// <summary>Returns stable blocker codes without changing the room.</summary>
    public static IReadOnlyList<DraftRoomErrorCode> GetBlockers(
        DraftRoom room,
        IDraftTypeModule module,
        DraftValidationResult settingsValidation)
    {
        ArgumentNullException.ThrowIfNull(room);
        ArgumentNullException.ThrowIfNull(module);
        ArgumentNullException.ThrowIfNull(settingsValidation);
        var blockers = new List<DraftRoomErrorCode>();
        if (room.Status != DraftRoomStatus.Lobby)
        {
            blockers.Add(DraftRoomErrorCode.RoomChanged);
        }

        if (room.Seats.Count < module.MinSeats)
        {
            blockers.Add(DraftRoomErrorCode.NotEnoughSeats);
        }

        if (room.Seats.Any(seat => seat.ParticipantId is null
            || !room.Participants.Any(participant =>
                participant.Id == seat.ParticipantId && participant.Status == DraftParticipantStatus.Active)))
        {
            blockers.Add(DraftRoomErrorCode.SeatsNotFull);
        }

        if (!settingsValidation.IsValid)
        {
            blockers.Add(DraftRoomErrorCode.SettingsInvalid);
        }

        return blockers;
    }
}
