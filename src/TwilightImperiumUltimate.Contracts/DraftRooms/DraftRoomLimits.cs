namespace TwilightImperiumUltimate.Contracts.DraftRooms;

public static class DraftRoomLimits
{
    public const int MinRoomNameLength = 3;
    public const int MaxRoomNameLength = 60;
    public const int MinPasswordLength = 6;
    public const int MaxPasswordLength = 64;
    public const int MinDisplayNameLength = 2;
    public const int MaxDisplayNameLength = 24;
    public const int MaxAccountUserNameLength = 256;
    public const int MaxStoredDisplayNameLength = MaxAccountUserNameLength + 2;
    public const int CodeLength = 6;
    public const string CodeAlphabet = "ABCDEFGHJKMNPQRSTUVWXYZ23456789";
    public const int MinSeatCount = 2;
    public const int MaxSeatCount = 8;
    public const int DefaultSeatCount = 6;
    public const int MaxActiveParticipants = 32;
    public const int MaxChatMessageLength = 300;
    public const int MaxChatMessagesPerWindow = 5;
    public const int MaxHistoryPageSize = 200;
    public const int DefaultHistoryPageSize = MaxHistoryPageSize;
    public const int MaxOpenRoomsPerUser = 5;
    public const int MaxJoinFailuresPerIpAndCode = 5;
    public const int MaxRoomCreationsPerIpPerHour = 10;

    public static readonly TimeSpan JoinFailureWindow = TimeSpan.FromMinutes(15);
    public static readonly TimeSpan ChatMessageWindow = TimeSpan.FromSeconds(5);
    public static readonly TimeSpan PresenceGracePeriod = TimeSpan.FromSeconds(30);
}
