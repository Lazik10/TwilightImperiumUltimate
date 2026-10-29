using TwilightImperiumUltimate.Contracts.DraftRooms;

namespace TwilightImperiumUltimate.Business.Logic.DraftRooms;

internal static class DraftRoomCallerValidator
{
    public static bool IsValid(DraftRoomCaller caller)
    {
        ArgumentNullException.ThrowIfNull(caller);

        if (caller.UserId is not null)
        {
            return !string.IsNullOrWhiteSpace(caller.UserId)
                && !string.IsNullOrWhiteSpace(caller.UserName)
                && caller.UserName.Length <= DraftRoomLimits.MaxAccountUserNameLength
                && caller.GuestParticipantId is null
                && caller.GuestSecret is null;
        }

        return caller.UserName is null
            && (caller.GuestParticipantId is null) == (caller.GuestSecret is null)
            && (caller.GuestParticipantId is null || !string.IsNullOrWhiteSpace(caller.GuestSecret));
    }
}
