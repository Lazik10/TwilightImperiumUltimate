using TwilightImperiumUltimate.Core.Entities.DraftRooms;
using TwilightImperiumUltimate.Draft.DraftRooms.FactionPick;

namespace TwilightImperiumUltimate.Business.Services.DraftRooms;

internal static class FactionPickChoiceMapping
{
    internal static FactionPickChoice ToChoice(FactionPickSelection selection) =>
        new(selection.SeatId, selection.Kind, selection.Faction, selection.State, selection.Round, selection.DecidedByParticipantId);

    internal static FactionPickSelection ToEntity(FactionPickChoice choice, Guid roomId) => new()
    {
        RoomId = roomId,
        SeatId = choice.SeatId,
        Kind = choice.Kind,
        Faction = choice.Faction,
        State = choice.State,
        Round = choice.Round,
        DecidedByParticipantId = choice.DecidedByParticipantId,
    };
}
